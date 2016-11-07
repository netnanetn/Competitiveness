using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Falcon.Data.Domain;

namespace Falcon.Security
{
    public interface IAuthorizationService
    {
        bool isAllowed(User user, Resource resource, string privilege);
        bool isAllowed(User user, string resourceName, string privilege);
        bool isAllowed(int userId, string resourceName, string privilege);

        IEnumerable<Role> GetAllRoles();        
        void AddRole(Role role);
        void UpdateRole(Role role);
        Role GetRole(int roleId);
        Role GetRole(string roleName);

        IEnumerable<Role> GetAllRolesByUser(int userId);
        IEnumerable<User> GetAllUsersByRole(int roleId);
        void AddUserRole(int userId, int roleId);
        void RemoveUserRole(int userId, int roleId);
        void RemoveUserRoleByUserId(int userId);

        IEnumerable<Resource> GetAllResources();
        void AddResource(Resource resource);
        void UpdateResource(Resource resource);
        Resource GetResource(int resourceId);
        Resource GetResource(string resourceName);

        IEnumerable<Permission> GetAllPermissions();
        void AddPermission(Permission permission);
        void RemovePermission(Permission permission);
        void UpdatePermission(Permission permission);
        void UpdateRolePermission(int roleId, IList<Permission> permissions);
        Permission GetPermission(int roleId, int resourceId, string privilege);

        IEnumerable<Permission_GetByRoleIdResult> GetPermissionsByRoleId(int roleId);
        
    }
}