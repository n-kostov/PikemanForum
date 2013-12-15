using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.ViewModels
{
    public class UserRolesViewModel
    {
        public string Id { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}