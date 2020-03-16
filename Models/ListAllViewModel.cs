using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JeromeCore.Models
{
    public class ListAllViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
