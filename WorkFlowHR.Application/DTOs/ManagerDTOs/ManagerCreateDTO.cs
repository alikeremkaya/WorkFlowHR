using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.Application.DTOs.ManagerDTOs
{
    public class ManagerCreateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Roles Roles { get; set; }
    }
}
