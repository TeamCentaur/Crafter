using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamCentaur_LiveChat.Areas.Admin.ViewModels
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }
        public string UserName { get; set; }
    }
}