using Abp.Authorization;
using BackendTest.Authorization.Roles;
using BackendTest.Authorization.Users;

namespace BackendTest.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
