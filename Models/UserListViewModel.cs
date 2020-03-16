using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JeromeCore.Models
{
    public class UserListViewModel
    {
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
