using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.BusinessLogic.Graph;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Dal;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using Olma = Dpl.B2b.Dal.Models;
using User = Dpl.B2b.Contracts.Models.User;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class UserService : BaseService, IUserService
    {
        private readonly IRepository<Olma.User> _userRepo;
        private readonly IRepository<Olma.Customer> _customerRepo;
        private readonly IRepository<Olma.UserCustomerRelation> _userCustomerRelationRepo;
        private readonly ICustomersService _customersService;
        private readonly IPostingAccountsService _postingAccountsService;
        private readonly IPermissionsService _permissionsService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGraphFactory _graphFactory;
        private readonly OlmaDbContext _olmaDbContext;
        private readonly IMapper _mapper;
        private readonly IAuthorizationDataService _authData;
        

        public UserService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.User> userRepo,
            ICustomersService customersService,
            IPostingAccountsService postingAccountsService,
            IPermissionsService permissionsService,
            IServiceProvider serviceProvider,
            IGraphFactory graphFactory,
            OlmaDbContext olmaDbContext, IRepository<Olma.Customer> customerRepo, IRepository<Olma.UserCustomerRelation> userCustomerRelationRepo) : base(authData, mapper)
        {
            _authData = authData;
            _userRepo = userRepo;
            _customersService = customersService;
            _postingAccountsService = postingAccountsService;
            _permissionsService = permissionsService;
            _serviceProvider = serviceProvider;
            _graphFactory = graphFactory;
            _olmaDbContext = olmaDbContext;
            _customerRepo = customerRepo;
            _userCustomerRelationRepo = userCustomerRelationRepo;
            _mapper = mapper;
        }

        public async Task<IWrappedResponse> Create(UserCreateRequest request)
        {
            var cmd = ServiceCommand<User, Rules.User.Create.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.User.Create.MainRule(request))
                .Then(CreateAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CreateAction(Rules.User.Create.MainRule mainRule)
        {
            var strategy = _olmaDbContext.Database.CreateExecutionStrategy();

            var result = await strategy.Execute(async () =>
            {
                var request = mainRule.Context.Parent;
                
                await using var ctxTransaction = await _olmaDbContext.Database.BeginTransactionAsync();
                //scope macht probleme mit UpdateCache
                //using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                try
                {
                    var person = Mapper.Map<Olma.Person>(request);
                    var customer = _customerRepo.FindAll().IgnoreQueryFilters().Single(c => !c.IsDeleted && c.Id == request.CustomerId);
                    var user = new Olma.User()
                    {
                        Upn = mainRule.Context.Upn, //TODO: Maybe the upn should be generated 
                        Customers = new List<Olma.UserCustomerRelation>() { new Olma.UserCustomerRelation() {CustomerId = customer.Id }},
                        OrganizationId = customer.OrganizationId,
                        Person = person,
                        Role = request.Role,
                        Locked = true // User is locked because of no Permissions
                    };
                
                    _userRepo.Create(user);
                    _userRepo.Save();
                    
                    // Patch this user values
                    await CreateGraphUser(new Microsoft.Graph.User()
                    {
                        AccountEnabled = false,
                        DisplayName = mainRule.Context.DisplayName,
                        UserPrincipalName = user.Upn,
                        Surname = person.LastName,
                        GivenName = person.FirstName,
                        MailNickname = mainRule.Context.EmailNickname,
                        PasswordPolicies = "DisableStrongPassword",
                        PasswordProfile = new PasswordProfile
                        {
                            ForceChangePasswordNextSignIn = true,
                            Password = mainRule.Context.FirstLoginPassword
                        }
                    });
                
                    //scope.Complete();
                    await ctxTransaction.CommitAsync();
                    
                    // Auf Dirty muss nicht unbedingt geprüft werden, in jedem fall muss der Cache zurückgesetzt werden! 
                    await _permissionsService.UpdateCache();
                    
                    var userDto = Mapper.Map<UserListItem>(user);
                    userDto.FirstLoginPassword = mainRule.Context.FirstLoginPassword;
                
                    return userDto;
                }
                catch(Exception)
                {
                    await ctxTransaction.RollbackAsync();
                    throw;
                }
            });
            return await Task.FromResult(Ok(result));
        }

        public async Task<IWrappedResponse> AddToCustomer(UserAddToCustomerRequest request)
        {
            var user = _userRepo.FindAll().IgnoreQueryFilters().Include(u => u.Customers)
                .SingleOrDefault(u => !u.IsDeleted && u.Id == request.UserId);
            var customer = _customerRepo.FindAll().IgnoreQueryFilters().SingleOrDefault(c => !c.IsDeleted && c.Id == request.CustomerId);
           
            if (user != null && customer != null)
            {
                //Check if Relation already exists
                if (user.Customers.Any(c => c.CustomerId == request.CustomerId))
                    return Ok(_mapper.Map<User>(user));
                //Else add new Relation
                user.Customers.Add(new Olma.UserCustomerRelation(){User = user,Customer = customer});
                _userRepo.Save();
                
                // Eventuelle ungültiger Permission Cache 
                await _permissionsService.UpdateCache();
                
                return Ok(_mapper.Map<User>(user));
            }
            return Failed<User>();
        }
        public async Task<IWrappedResponse> RemoveFromCustomer(RemoveFromCustomerRequest request)
        {
            var user = _userRepo.FindAll().IgnoreQueryFilters().Include(u => u.Customers)
                .SingleOrDefault(u => !u.IsDeleted && u.Id == request.UserId);
            var relation = _userCustomerRelationRepo.FindAll().IgnoreQueryFilters().FirstOrDefault(c => c.UserId == request.UserId && c.CustomerId == request.CustomerId);

            if (user != null && relation != null)
            {
                _userCustomerRelationRepo.Delete(relation);
                _userCustomerRelationRepo.Save();
                
                // Eventuelle ungültiger Permission Cache 
                await _permissionsService.UpdateCache();
            }
            return Ok(_mapper.Map<User>(user));
        }

        public async Task<IWrappedResponse> Get()
        {
            var user = _userRepo.FindAll()
                .ProjectTo<User>(Mapper.ConfigurationProvider)
                .SingleOrDefault();

            var isNotDplEmployee = AuthData.GetUserRole() != Common.Enumerations.UserRole.DplEmployee;
            if (!isNotDplEmployee)
            {
                user.Role = Common.Enumerations.UserRole.DplEmployee;
            }

            var customersServiceResponse = (IWrappedResponse<IEnumerable<Customer>>) await _customersService.GetAll();
            var customers = isNotDplEmployee ? customersServiceResponse.Data : new Customer[] { };
            var postingAccountsServiceResponse = (IWrappedResponse<IEnumerable<PostingAccount>>) await _postingAccountsService.GetAll();

            var postingAccounts = isNotDplEmployee ? postingAccountsServiceResponse.Data : new PostingAccount[] { };
            var permssions = isNotDplEmployee ? await _permissionsService.GetPermissions() : new UserPermission[] { };

            user.Customers = customers;
            user.PostingAccounts = postingAccounts;
            user.Permissions = permssions;

            return Ok(user);
        }

        public async Task<IWrappedResponse> GetByCustomerId(int customerId)
        {
            var user = _userRepo.FindAll()
                .ProjectTo<User>(Mapper.ConfigurationProvider)
                .SingleOrDefault();

            user.Role = AuthData.GetUserRole();

            var customerResult = (IWrappedResponse<Customer>) await _customersService.GetById(customerId);
            if (customerResult.ResultType == ResultType.NotFound)
            {
                return NotFound<User>(customerId);
            }

            var customers = new[] {customerResult.Data};
            var postingAccountsServiceResponse = (IWrappedResponse<IEnumerable<PostingAccount>>) await _postingAccountsService.GetByCustomerId(customerId);
            var postingAccounts = postingAccountsServiceResponse.Data;
            var permissions = Array.Empty<UserPermission>();

            user.Customers = customers;
            user.PostingAccounts = postingAccounts;
            user.Permissions = permissions;

            return Ok(user);
        }

        public async Task<IWrappedResponse> UpdateSettings(Newtonsoft.Json.Linq.JObject settings)
        {
            var user = _userRepo.FindAll()
                .Include(i => i.UserSettings)
                .SingleOrDefault();

            if (user.UserSettings == null)
            {
                user.UserSettings = new Olma.UserSetting();
            }

            user.UserSettings.Data = settings;

            _userRepo.Save();

            // möglicherweise ungültiger Permission Cache
            _permissionsService.UpdateCache();

            return new WrappedResponse()
            {
                ResultType = ResultType.Ok
            };
        }

        public IEnumerable<UserListItem> Search(UsersSearchRequest request)
        {
            // Returns IEnumerable instead IQueryable because
            // filter and Sorting on User Role cannot be translated to SQL  

            //Build Query
            var query = _userRepo.FindAll()
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Where(q => !q.IsDeleted);

            if (request.OrganizationId != null)
                query = query.Where(q => q.OrganizationId == request.OrganizationId);

            if (request.CustomerId != null)
                query = query.Where(q => q.Customers.Any(c => c.CustomerId == (int) request.CustomerId));

            if (request.DivisionId != null) //TODO: check if query by DivisionId is correct
                query = query.Where(q => q.Customers.Any(c => c.Customer.Divisions.Any(d => d.Id == (int) request.DivisionId)));

            return query.ProjectTo<UserListItem>(Mapper.ConfigurationProvider).ToList(); 
        }

        public async Task<IWrappedResponse> All(UsersSearchRequest request)
        {
            var query = Search(request);
            return Ok(query.AsEnumerable<UserListItem>());
        }

        public async Task<IWrappedResponse> AllByOrganization(UsersByOrganizationRequest request)
        {
            var query = _userRepo.FindAll()
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Where(q => !q.IsDeleted);
                //Filter by Organization
                query = query.Where(q => q.OrganizationId == request.OrganizationId);
                //Exclude User by CustomerId
                if (request.ExceptCustomerId != null)
                    query = query.Where(q => q.Customers.All(c => c.CustomerId != (int) request.ExceptCustomerId));


            var projectedResult =  query.ProjectTo<UserListItem>(Mapper.ConfigurationProvider);
            return Ok(projectedResult.AsEnumerable< UserListItem>());
        }
        public async Task<IWrappedResponse> UpdateLocked(UsersLockRequest request)
        {
            var cmd = ServiceCommand<User, Rules.User.UpdateLock.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.User.UpdateLock.MainRule(request))
                .Then(UpdateLockedAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> UpdateLockedAction(Rules.User.UpdateLock.MainRule mainRule)
        {
            var strategy = _olmaDbContext.Database.CreateExecutionStrategy();

            var result = await strategy.Execute(async () =>
            {
                //using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                await using var ctxTransaction = await _olmaDbContext.Database.BeginTransactionAsync();

                try
                {
                    var newLockedState = mainRule.Context.Parent.Locked.GetValueOrDefault();
                    var user = mainRule.Context.User;

                    if (user.Locked != newLockedState)
                    {
                        user.Locked = newLockedState;
                        _userRepo.Update(user);
                        _userRepo.Save();
                    }

                    var userDto = _mapper.Map<User>(user);

                    var accessToken = await _authData.GetAccessToken();
                    //var userAssertion = new UserAssertion(accessToken);
                
                    // Patch this user values
                    await UpdateGraphUser(
                        user.Upn,
                        new Microsoft.Graph.User
                        {
                            AccountEnabled = !newLockedState
                        });

                    //scope.Complete();
                    await ctxTransaction.CommitAsync();
                    
                    // Eventuelle ungültiger Permission Cache 
                    await _permissionsService.UpdateCache();

                    return userDto;
                }
                catch (Exception)
                {
                    await ctxTransaction.RollbackAsync();
                    throw;
                }
               
            });

            return await Task.FromResult(Ok(result));
        }

        private async Task<Microsoft.Graph.User> UpdateGraphUser([NotNull] string upn, Microsoft.Graph.User user, UserAssertion withUserAssertion=null)
        {
            if (upn == null) throw new ArgumentNullException(nameof(upn));
            
            // Berechtigungen
            // Benutzer abrufen (Anwendung)	--> User.ReadWrite.All, User.ManageIdentities.All, Directory.ReadWrite.All
            // https://docs.microsoft.com/de-de/graph/api/user-update?view=graph-rest-1.0&tabs=http
            // Zum Updaten von PasswordProfile wird Mitgliedschaft in passender Rolle benötigt (z.B. Helpdeskadministrator)

            var authProvider = withUserAssertion == null
                ? (IAuthenticationProvider) _graphFactory.CreateAuthProvider()
                : _graphFactory.CreateOnBehalfOfProvider(new[] {"User.ReadWrite.All"});
            
            var client= new GraphServiceClient(authProvider);
            
            var request = client.Users[$"{upn}"].Request();

            if (withUserAssertion != null)
                request = request.WithUserAssertion(withUserAssertion);

            return await request.UpdateAsync(user);
        }
        
        private async Task<Microsoft.Graph.User> GetUserViaDelegate([NotNull] string upn)
        {
            if (upn == null) throw new ArgumentNullException(nameof(upn));

            var authProvider = _graphFactory.CreateOnBehalfOfProvider(new[] {"User.ReadWrite.All"});
            var client = new GraphServiceClient(authProvider);
            
            //var accessToken = _authData.GetPrincipal()?.Claims.FirstOrDefault(c => c.Type == "access_token")?.Value;
            //var accessToken = await Request.HttpContext.GetTokenAsync("access_token");
            var accessToken = await _authData.GetAccessToken();
            
            var userAssert = new UserAssertion(accessToken);
            
            return await client
                .Users[$"{upn}"]
                .Request()
                .WithUserAssertion(userAssert)
                .GetAsync();
        }
        
        private async Task<Microsoft.Graph.User> CreateGraphUser(Microsoft.Graph.User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            
            // Berechtigungen
            // Benutzer erstellen (Anwendung)	--> User.ReadWrite.All, Directory.ReadWrite.All
            // https://docs.microsoft.com/de-de/graph/api/user-post-users?view=graph-rest-1.0&tabs=http
            
            var authProvider = _graphFactory.CreateAuthProvider();
            var client = new GraphServiceClient(authProvider);

            return await client.Users
                .Request()
                .AddAsync(user);
        }
        
        public async Task<IWrappedResponse> Update(UserUpdateRequest request)
        {
            var cmd = ServiceCommand<User, Rules.User.Update.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.User.Update.MainRule(request))
                .Then(UpdateAction);
            return await cmd.Execute();
        }
        
        private async Task<IWrappedResponse> UpdateAction(Rules.User.Update.MainRule mainRule)
        {
            var strategy = _olmaDbContext.Database.CreateExecutionStrategy();

            var result = await strategy.Execute(async () =>
            {
                //using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                await using var ctxTransaction = await _olmaDbContext.Database.BeginTransactionAsync();

                try
                {
                    mainRule.PatchUser();
                
                    _userRepo.Save();
                
                    //scope.Complete();
                    await ctxTransaction.CommitAsync();
                    
                    // Eventuelle ungültiger Permission Cache 
                    await _permissionsService.UpdateCache();

                    return _mapper.Map<UserListItem>(mainRule.Context.User); 
                }
                catch(Exception)
                {
                    await ctxTransaction.RollbackAsync();
                    throw;
                }
              
            });
            
            
            return await Task.FromResult(Ok(result));
        }
        
        public async Task<IWrappedResponse> ResetPassword(UserResetPasswordRequest request)
        {
            var cmd = ServiceCommand<User, Rules.User.ResetPassword.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.User.ResetPassword.MainRule(request))
                .Then(ResetPasswordAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> ResetPasswordAction(Rules.User.ResetPassword.MainRule mainRule)
        {
            var userId = mainRule.Context.User.Upn;
            var newPasswort = mainRule.Context.NewPassword;

            await UpdateGraphUser(userId, new Microsoft.Graph.User
            {
                PasswordPolicies = "DisableStrongPassword",
                PasswordProfile = new PasswordProfile
                {
                    ForceChangePasswordNextSignIn = true,
                    Password = newPasswort
                }
            });
            
            var result = Created(new UserResetPasswordResult()
            {
                NewPassword = newPasswort
            });
            
            return await Task.FromResult(result);
        }
    }
}