using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Olma = Dpl.B2b.Dal.Models;
using Z.EntityFramework.Plus;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Dpl.B2b.BusinessLogic
{
    //HACK this service ignores isDeleted of auditable entities in some places
    //Ether use an special context or remove security query filters from user and permissions to fix this
    //TODO Discuss Get Permission on Feature level?
    // TODO Handle user having NO permissions or being removed
    // TODO Create scoped user permission service that gets a single user permissions from distr cache and caches them for the duration of the request
    public class PermissionsService : IPermissionsService
    {
        private readonly IAuthorizationDataService _authData;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly IServiceProvider _serviceProvider;

        private ConcurrentDictionary<int, ImmutableHashSet<string>> _permissionsCache;


        public PermissionsService(IAuthorizationDataService authData, IMapper mapper, IServiceProvider serviceProvider, IDistributedCache cache)
        {
            _serviceProvider = serviceProvider;
            this._authData = authData;
            this._mapper = mapper;
            _cache = cache;
            _permissionsCache = new ConcurrentDictionary<int, ImmutableHashSet<string>>();

            this.PopulateCache();
        }

        private void PopulateCache()
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var usersPermissions = this.GetAll().Result;

            var cacheTasks = new List<Task>(
                usersPermissions
                    .Select(user => _cache.SetStringAsync($"Upn:{user.Upn}", JsonConvert.SerializeObject(user, jsonSettings)))
                    .ToArray()
            );

            var permissionKeys = usersPermissions.SelectMany(user => user.Permissions
                .Select(p => new { UserId = user.Id, Key = GetPermissionKey(user.Id, p.Resource, p.Action, p.ReferenceId) }))
                .ToList();

            var attributePermissionKeys = usersPermissions
                .SelectMany(user => user.Permissions.GroupBy(p => new { UserId = user.Id, p.Action }))
                .Select(g => new { UserId = g.Key.UserId, Key = GetAttributePermissionKey(g.Key.UserId, g.Key.Action) })
                .ToList();

            var allPermissionKeys = permissionKeys.Concat(attributePermissionKeys).ToList();

            foreach (var permissions in allPermissionKeys.GroupBy(i => i.UserId))
            {
                var userPermissions = permissions.Select(i => i.Key).ToArray();

                // update distributed cache
                cacheTasks.Add(SetUserPermissionsInCache(permissions.Key, userPermissions));

                // update local cache
                var hashSet = ImmutableHashSet.Create(userPermissions);
                _permissionsCache.AddOrUpdate(permissions.Key, hashSet, (a, b) => hashSet);
            }

            Task.WaitAll(cacheTasks.ToArray());
        }

        private string GetPermissionsCacheKey(int userId)
        {
            return $"Permission:{userId}";
        }

        private Task SetUserPermissionsInCache(int userId, IEnumerable<string> userPermissions)
        {
            return _cache.SetStringAsync(GetPermissionsCacheKey(userId), JsonConvert.SerializeObject(userPermissions));
        }

        async private Task<ImmutableHashSet<string>> GetUserPermissionsFromCache(int userId)
        {
            var cacheValue = await _cache.GetStringAsync(GetPermissionsCacheKey(userId));
            return ImmutableHashSet.Create(JsonConvert.DeserializeObject<string[]>(cacheValue));
        }

        private ImmutableHashSet<string> GetUserPermissionsFromCacheValue(string userPermissionsCacheValue)
        {
            return ImmutableHashSet.Create(userPermissionsCacheValue.Split("|"));
        }

        private string GetPermissionKey(int userId, PermissionResourceType resource, ResourceAction action, int referenceId)
        {
            return $"{userId}|{resource}|{action}|{referenceId}";
        }

        private string GetAttributePermissionKey(int userId, ResourceAction action)
        {
            return $"{userId}|{action}";
        }

        private async Task<IEnumerable<CachedUser>> GetInternal(int? userId)
        {
            var scope = _serviceProvider.CreateScope();
            var userRepo = scope.ServiceProvider.GetService<IRepository<Olma.User>>();
            var query = userRepo.FindAll()
                .IgnoreQueryFilters()
                .IncludeOptimized(u => u.Permissions)
                .IncludeOptimized(u => u.UserGroups)
                .IncludeOptimized(u => u.UserGroups.Select(i => i.UserGroup))
                .IncludeOptimized(r => r.UserGroups.SelectMany(i => i.UserGroup.Permissions));

            if (userId.HasValue)
            {
                query = query.Where(i => i.Id == userId.Value);
            }

            var olmaUsers = query.ToList();

            var users = olmaUsers.Select(u =>
            {
                var userPermissions = u.Permissions;
                var userGroupPermissions = u.UserGroups.SelectMany(r => r.UserGroup.Permissions);

                // union all permissions (flat)
                var permissionsFlat = userPermissions
                    .Union(userGroupPermissions);

                // seperate allow / deny permissions
                var permissionsByType = permissionsFlat
                    .ToLookup(i => i.Type);

                // Subtract the deny-permissions from the allow-permissions
                var permissionsAllow = _mapper.Map<HashSet<UserPermission>>(permissionsByType.Contains(Olma.PermissionType.Allow)
                    ? permissionsByType[Olma.PermissionType.Allow]
                    : new Olma.Permission[] { }
                );

                var permissionsDeny = _mapper.Map<HashSet<UserPermission>>(permissionsByType.Contains(Olma.PermissionType.Allow)
                    ? permissionsByType[Olma.PermissionType.Deny]
                    : new Olma.Permission[] { }
                );

                var permissions = permissionsAllow
                    .Except(permissionsDeny)
                    .ToList();

                var readReferenceIdsByResource = permissions
                    .Where(i => i.Action == ResourceAction.Read)
                    .ToLookup(i => i.Resource, i => i.ReferenceId);

                var user = _mapper.Map<CachedUser>(u);
                user.Permissions = permissions;

                // in addition to chaching permissions, also cache all relevant ids for reading (used by AuthDataProvider)
                user.OrganizationIds = readReferenceIdsByResource.Contains(PermissionResourceType.Organization)
                    ? readReferenceIdsByResource[PermissionResourceType.Organization].Distinct().ToArray()
                    : new int[] { };

                user.CustomerIds = readReferenceIdsByResource.Contains(PermissionResourceType.Customer)
                    ? readReferenceIdsByResource[PermissionResourceType.Customer].Distinct().ToArray()
                    : new int[] { };

                user.CustomerDivisionIds = readReferenceIdsByResource.Contains(PermissionResourceType.Division)
                    ? readReferenceIdsByResource[PermissionResourceType.Division].Distinct().ToArray()
                    : new int[] { };
                return user;
            }).ToList();

            #region Cache Posting AccountIds

            var divisionRepo = scope.ServiceProvider.GetService<IRepository<Olma.CustomerDivision>>();
            var postingAccountLookupQuery = divisionRepo.FindAll()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(i => i.PostingAccountId.HasValue);

            if (userId.HasValue && users.Count == 1)
            {
                postingAccountLookupQuery = postingAccountLookupQuery.Where(i => users[0].CustomerDivisionIds.Contains(i.Id));
            }

            var postingAccountLookup = postingAccountLookupQuery
                .Select(i => new { i.Id, PostingAccountId = i.PostingAccountId.Value })
                .ToLookup(i => i.Id, i => i.PostingAccountId);

            foreach (var user in users)
            {
                user.PostingAccountIds = user.CustomerDivisionIds.SelectMany(divisionId => postingAccountLookup[divisionId])
                    .Distinct()
                    .ToList();
            }

            #endregion

            return users;
        }

        public async Task UpdateCache()
        {
            this.PopulateCache();
        }

        public Task<IEnumerable<CachedUser>> GetAll()
        {
            return this.GetInternal(null);
        }

        public async Task<CachedUser> GetUser(string upn)
        {
            var userSerialized = await _cache.GetStringAsync($"Upn:{upn}");
            if (string.IsNullOrEmpty(userSerialized))
            {
                return null;
            }

            var user = JsonConvert.DeserializeObject<CachedUser>(userSerialized);
            return user;
        }

        public Task<CachedUser> GetUser()
        {
            return this.GetUser(_authData.GetUserUpn());
        }

        public async Task<IEnumerable<UserPermission>> GetPermissions()
        {
            var upn = _authData.GetUserUpn();
            var user = JsonConvert.DeserializeObject<User>(await _cache.GetStringAsync($"Upn:{upn}"));
            return user.Permissions;
        }

        public bool HasPermission(PermissionResourceType resource, ResourceAction action, int referenceId)
        {
            // skip checking permissions for dpl employees
            if (_authData.GetUserRole() == UserRole.DplEmployee)
            {
                return true;
            }

            // TODO decide if this method should be sync or async
            // if async we can use the permissions in the distributed cache
            // they are already inserted individually
            var userId = _authData.GetUserId();
            ImmutableHashSet<string> userPermissions;
            if (_permissionsCache.TryGetValue(userId, out userPermissions))
            {
                var permissionsKey = GetPermissionKey(userId, resource, action, referenceId);
                var hasPermission = userPermissions.Contains(permissionsKey);
                return hasPermission;
            }

            return false;
        }

        public bool HasAttributePermission(ResourceAction action)
        {
            // TODO decide if this method should be sync or async
            // if async we can use the permissions in the distributed cache
            // they are already inserted individually
            var userId = _authData.GetUserId();
            ImmutableHashSet<string> userPermissions;
            if (_permissionsCache.TryGetValue(userId, out userPermissions))
            {
                var permissionsKey = GetAttributePermissionKey(userId, action);
                var hasPermission = userPermissions.Contains(permissionsKey);
                return hasPermission;
            }

            return false;
        }
    }
}
