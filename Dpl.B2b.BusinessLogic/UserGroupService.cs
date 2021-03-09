using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExpress.Office.Utils;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class UserGroupService : BaseService, IUserGroupService
    {
        private readonly IRepository<Olma.UserGroup> _userGroupRepo;
        private readonly IRepository<Olma.OrganizationUserGroup> _organizationUserGroupRepo;
        private readonly IRepository<Olma.CustomerUserGroup> _customerUserGroupRepo;
        private readonly IRepository<Olma.CustomerDivisionUserGroup> _divisionUserGroupRepo;
        private readonly IRepository<Olma.UserUserGroupRelation> _userGroupRelationRepo;
        private readonly IRepository<Olma.User> _userRepo;
        private readonly IRepository<Olma.Permission> _permissionRepo;
        private readonly IUserService _userService;
        private readonly IPermissionsService _permissionsService;

        public UserGroupService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.UserGroup> userGroupRepo, 
            IPermissionsService permissionsService, 
            IRepository<Olma.User> userRepo, 
            IRepository<Olma.UserUserGroupRelation> userGroupRelationRepo, 
            IRepository<Olma.OrganizationUserGroup> organizationUserGroupRepo, 
            IRepository<Olma.CustomerUserGroup> customerUserGroupRepo, 
            IRepository<Olma.CustomerDivisionUserGroup> divisionUserGroupRepo, 
            IRepository<Olma.Permission> permissionRepo, 
            IUserService userService) : base(authData, mapper)
        {
            _userGroupRepo = userGroupRepo;
            _userRepo = userRepo;
            _userGroupRelationRepo = userGroupRelationRepo;
            _organizationUserGroupRepo = organizationUserGroupRepo;
            _customerUserGroupRepo = customerUserGroupRepo;
            _divisionUserGroupRepo = divisionUserGroupRepo;
            _permissionRepo = permissionRepo;
            _permissionsService = permissionsService;
            _userService = userService;
        }
        
        public async Task<IWrappedResponse> Create(UserGroupCreateRequest request)
        {
            if (request.OrganizationId != null)
            {
                var group = Mapper.Map<Olma.OrganizationUserGroup>(request);
                _organizationUserGroupRepo.Create(group);
                _organizationUserGroupRepo.Save();
                return Ok(Mapper.Map<UserGroupListItem>(group));
            }
            if (request.CustomerId != null)
            {
                var group = Mapper.Map<Olma.CustomerUserGroup>(request);
                _customerUserGroupRepo.Create(group);
                _customerUserGroupRepo.Save();
                return Ok(Mapper.Map<UserGroupListItem>(group));
            }
            if (request.DivisionId != null)
            {
                var group = Mapper.Map<Olma.CustomerDivisionUserGroup>(request);
                _divisionUserGroupRepo.Create(group);
                _divisionUserGroupRepo.Save();
                return Ok(Mapper.Map<UserGroupListItem>(group));
            }
            return Failed<UserGroupListItem>();
        }
      

        public async Task<IWrappedResponse<UserGroupListItem>> UpdatePermissions(int groupId, UserGroupUpdatePermissionsRequest request)
        {
            var userGroup = _userGroupRepo.FindAll().IgnoreQueryFilters()
                .Include(i => i.Permissions)
                .Include(i => i.Users)
                .SingleOrDefault(g => g.Id == groupId && !g.IsDeleted);
            if (userGroup == null)
                return new WrappedResponse<UserGroupListItem>()
                {
                    ResultType = ResultType.NotFound,
                    Data = null,
                    Errors = new[] { $"There was no Group with the id: {groupId}" }
                };
            if (request.GroupName != null &&  userGroup.Name != request.GroupName)
                userGroup.Name = request.GroupName;

            // Remove Permissions witch are not in the List anymore
            var existingPermissions = userGroup.Permissions;
            //List<Olma.Permission> permissionsToRemove = new List<Olma.Permission>();
            foreach (var existingPermission in existingPermissions.ToList())
            {
                if (!request.Permissions.Exists(p => 
                        p.Action == existingPermission.Action && 
                        p.Resource == existingPermission.Resource && 
                        p.ReferenceId == existingPermission.ReferenceId)
                )
                    userGroup.Permissions.Remove(existingPermission);
            }
            // Add new Permissions
            foreach (var permission in request.Permissions.ToList())
            {
                if (!existingPermissions.Any(e => 
                    e.Action == permission.Action &&
                    e.Resource == permission.Resource &&
                    e.ReferenceId == permission.ReferenceId)
                )
                {
                    var newPermission = new Olma.Permission()
                    {
                        Action = permission.Action,
                        Type = Olma.PermissionType.Allow,
                        Scope = Olma.PermissionScope.Group,
                        Resource = permission.Resource,
                        ReferenceId = permission.ReferenceId
                    };
                    userGroup.Permissions.Add(newPermission);
                }
            }

            try
            {
                _userGroupRepo.Save();
                
                // Möglicher ungültiger Permission Cache
                await _permissionsService.UpdateCache();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return new WrappedResponse<UserGroupListItem>()
            {
                ResultType = ResultType.Updated,
                Data = Mapper.Map<Olma.UserGroup, UserGroupListItem>(userGroup)
            };
        }
        public async Task<IWrappedResponse<UserGroupListItem>> UpdateMembers(int groupId, UserGroupUpdateMembershipRequest request)
        {
            var userGroup = _userGroupRepo.FindAll()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Include(i => i.Users)
                .SingleOrDefault(g => g.Id == groupId && !g.IsDeleted);
            if (userGroup == null)
                return new WrappedResponse<UserGroupListItem>()
                {
                    ResultType = ResultType.NotFound,
                    Data = null,
                    Errors = new[] { $"There was no Group with the id: {groupId}" }
                };
            userGroup.Users ??= new List<Olma.UserUserGroupRelation>();

            // Remove Members witch are not in the List anymore
            var existingMembers = userGroup.Users;
            foreach (var existingMember in existingMembers.ToList().Where(existingMember => !request.Users.Select(u => u.Id).Contains(existingMember.UserId)))
            {
                userGroup.Users.Remove(existingMember); 
                var relationToDelete = _userGroupRelationRepo.GetByKey(existingMember.Id);
                _userGroupRelationRepo.Delete(relationToDelete);

                //Lock user if no other group
                var user = _userRepo.FindAll().IgnoreQueryFilters().Include(u => u.UserGroups)
                    .SingleOrDefault(u => u.Id == existingMember.UserId);
                if (user != null && !user.Locked && (user.UserGroups.All(g => g.UserGroupId == existingMember.UserGroupId) || user.UserGroups.Count == 0))
                {
                    user.Locked = true;
                    var lockResult = await _userService.UpdateLocked(new UsersLockRequest()
                        {Id = user.Id, Locked = true});
                    if (lockResult.ResultType != ResultType.Ok)
                    {
                        return new WrappedResponse<UserGroupListItem>()
                        {
                            ResultType = ResultType.Failed,
                            Errors = new[] { $"Error disabling User {user.Upn} on Azure Active Directory." },
                            Data = Mapper.Map<Olma.UserGroup, UserGroupListItem>(userGroup)
                        };
                    }
                }
            }
            
            // Add new Members
            foreach (var user in request.Users.ToList())
            {
                if (!existingMembers.Select(e => e.UserId).Contains(user.Id))
                {
                    var newUser = _userRepo.FindAll().IgnoreQueryFilters()
                        .SingleOrDefault(g => g.Id == user.Id && !g.IsDeleted);
                    // Unlock if locked
                    if (newUser.Locked)
                    {
                        newUser.Locked = false;
                        var unlockResult = await _userService.UpdateLocked(new UsersLockRequest() {Id = newUser.Id, Locked = false});
                        if (unlockResult.ResultType != ResultType.Ok)
                        {
                            return new WrappedResponse<UserGroupListItem>()
                            {
                                ResultType = ResultType.Failed,
                                Errors = new[] {$"Error unlocking User {newUser.Upn} on Azure Active Directory.." },
                                Data = Mapper.Map<Olma.UserGroup, UserGroupListItem>(userGroup)
                            };
                        }
                    }
                    
                    userGroup.Users.Add(new Olma.UserUserGroupRelation() { UserGroup = userGroup, User = newUser });
                    _userGroupRelationRepo.Create(new Olma.UserUserGroupRelation(){UserGroupId =  userGroup.Id,UserId = user.Id});
                }
            }
            
            _userGroupRelationRepo.Save();
            
            //  Ändern der "Members" macht den Permission Cache ungültig 
            await _permissionsService.UpdateCache();
            
            return new WrappedResponse<UserGroupListItem>()
            {
                ResultType = ResultType.Updated,
                Data = Mapper.Map<Olma.UserGroup, UserGroupListItem>(userGroup)
            };
        }

        public async Task<IWrappedResponse<UserGroupListItem>> AddUser(int groupId, int userId)
        {
            var userGroup = _userGroupRepo.FindAll().IgnoreQueryFilters()
                .Include(i => i.Users).ThenInclude(i => i.User)
                .SingleOrDefault(g => g.Id == groupId && !g.IsDeleted);
            var user = _userRepo.FindAll().IgnoreQueryFilters().SingleOrDefault(g => g.Id == userId && !g.IsDeleted);
            if (userGroup == null)
                return new WrappedResponse<UserGroupListItem>()
                {
                    ResultType = ResultType.NotFound,
                    Data = null,
                    Errors = new[] { $"There was no Group with the id: {groupId}" }
                };  
            if (user == null)
                return new WrappedResponse<UserGroupListItem>()
                {
                    ResultType = ResultType.NotFound,
                    Data = null,
                    Errors = new[] { $"There was no User with the id: {userId}" }
                }; 
            if (!userGroup.Users.Select(u => u.UserId).Contains(userId))
            {
                userGroup.Users.Add(new Olma.UserUserGroupRelation() { UserGroup = userGroup, User = user });
                _userGroupRepo.Update(userGroup);
                try
                {
                    _userGroupRepo.Save();
                    
                    //  Hinzufügen eines User macht den Permission Cache ungültig 
                    await _permissionsService.UpdateCache();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
               
            }
            return new WrappedResponse<UserGroupListItem>()
            {
                ResultType = ResultType.Updated,
                Data = Mapper.Map<Olma.UserGroup, UserGroupListItem>(userGroup)
            };
        }

        public async Task<IWrappedResponse<UserGroupListItem>> RemoveUser(int groupId, int userId)
        {
            var userGroup = _userGroupRepo.FindAll().IgnoreQueryFilters()
                .Include(i => i.Users).ThenInclude(i => i.User)
                .SingleOrDefault(g => g.Id == groupId && !g.IsDeleted);
            var user = _userRepo.FindAll().IgnoreQueryFilters().SingleOrDefault(g => g.Id == userId && !g.IsDeleted);
            var userGroupRelation = userGroup.Users.SingleOrDefault(u => u.UserId == userId);
            if (userGroup == null || userGroupRelation == null)
                return new WrappedResponse<UserGroupListItem>()
                {
                    ResultType = ResultType.NotFound,
                    Data = null,
                    Errors = new[] { $"There was no Group with the id: {groupId}" }
                };
            if (user == null)
                return new WrappedResponse<UserGroupListItem>()
                {
                    ResultType = ResultType.NotFound,
                    Data = null,
                    Errors = new[] { $"There was no User with the id: {userId}" }
                };
            if (userGroup.Users.Select(u => u.UserId).Contains(userId))
            {
                _userGroupRelationRepo.Delete(userGroupRelation);
                try
                {
                    _userGroupRelationRepo.Save();

                    //  Entfernen eines User macht den Permission Cache ungültig 
                    await _permissionsService.UpdateCache();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return new WrappedResponse<UserGroupListItem>()
            {
                ResultType = ResultType.Updated,
                Data = Mapper.Map<Olma.UserGroup, UserGroupListItem>(userGroup)
            };
        }

        public IEnumerable<UserGroupListItem> Search(UserGroupsSearchRequest request)
        {
            //Build Query
            if (request.OrganizationId.HasValue)
            {
                var query = _userGroupRepo.FindAll().OfType<Olma.OrganizationUserGroup>()
                    .Include(i => i.Permissions)
                    .AsNoTracking()
                    .IgnoreQueryFilters()
                    .Where(q => !q.IsDeleted && q.OrganizationId == request.OrganizationId);
                return query.ProjectTo<UserGroupListItem>(Mapper.ConfigurationProvider).ToList();
            }

            if (request.CustomerId.HasValue)
            {
                var query = _userGroupRepo.FindAll().OfType<Olma.CustomerUserGroup>()
                    .Include(i => i.Permissions)
                    .AsNoTracking()
                    .IgnoreQueryFilters()
                    .Where(q => !q.IsDeleted && q.CustomerId == request.CustomerId);
                return query.ProjectTo<UserGroupListItem>(Mapper.ConfigurationProvider).ToList();
            }
            if (request.CustomerDivisionId.HasValue)
            {
                var query = _userGroupRepo.FindAll().OfType<Olma.CustomerDivisionUserGroup>()
                    .Include(i => i.Permissions)
                    .AsNoTracking()
                    .IgnoreQueryFilters()
                    .Where(q => !q.IsDeleted && q.CustomerDivisionId == request.CustomerDivisionId);

                return query.ProjectTo<UserGroupListItem>(Mapper.ConfigurationProvider).ToList();
            }
            if (request.PostingAccountId.HasValue)
            {
                throw new NotImplementedException(); //TODO: Get Usergroups by PostingAccountId
            }
            return null;
        }

        public IEnumerable<UserGroupMembership> GetMembers(UserGroupsSearchRequest request)
        {
            var result = new List<UserGroupMembership>();
            //Build Query
            if (request.OrganizationId.HasValue)
            {
                var query = _userGroupRepo.FindAll().OfType<Olma.OrganizationUserGroup>()
                    .Include(i => i.Users)
                    .AsNoTracking()
                    .IgnoreQueryFilters()
                    .Where(q => !q.IsDeleted && q.OrganizationId == request.OrganizationId);
                result = query.ProjectTo<UserGroupMembership>(Mapper.ConfigurationProvider).ToList();
            }
            if (request.CustomerId.HasValue)
            {
                var query = _userGroupRepo.FindAll().OfType<Olma.CustomerUserGroup>()
                    .Include(i => i.Users)
                    .AsNoTracking()
                    .IgnoreQueryFilters()
                    .Where(q => !q.IsDeleted && q.CustomerId == request.CustomerId);
                result =  query.ProjectTo<UserGroupMembership>(Mapper.ConfigurationProvider).ToList();
            }
            if (request.CustomerDivisionId.HasValue)
            {
                var query = _userGroupRepo.FindAll().OfType<Olma.CustomerDivisionUserGroup>()
                    .Include(i => i.Users)
                    .AsNoTracking()
                    .IgnoreQueryFilters()
                    .Where(q => !q.IsDeleted && q.CustomerDivisionId == request.CustomerDivisionId);
                result = query.ProjectTo<UserGroupMembership>(Mapper.ConfigurationProvider).ToList();
            }
            return result;
        }

        #region Helper
        #endregion
        }
}
