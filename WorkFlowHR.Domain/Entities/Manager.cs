using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Core.Base;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.Domain.Entities
{
    public class Manager:BaseUser
    {
        public string? Password { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsFirstLogin { get; set; } = true;
        public Roles Roles { get; set; }
        public byte[]? Image { get; set; }
    }
}
