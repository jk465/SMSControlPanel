using SMSControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.ViewModel
{
    public class UserViewModel
    {
        public UserDetails User { get; set; }
        public string ErrorType { get; set; }
    }
}
