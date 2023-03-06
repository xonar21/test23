using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Identity.Razor.Users.ViewModels.UserViewModels
{
    public class UpdateUserRoleViewModel
    {
        public string UserId { get; set; }

        public List<string>RoleIds { get; set; }
    }
}
