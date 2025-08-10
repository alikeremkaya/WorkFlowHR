using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Core.Base;

namespace WorkFlowHR.Domain.Entities
{
    public class AppUser:BaseUser
    {
        [StringLength(200)]
        public string DisplayName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = null!; // "Manager" veya "Employee"

        [Required]
        [StringLength(100)]
        public string AzureAdObjectId { get; set; } = null!;

      

        // Navigation - Onayladığı kayıtlar
        public virtual IEnumerable<Advance> Advances { get; set; } = new HashSet<Advance>();
        public virtual IEnumerable<Expense> Expenses { get; set; } = new HashSet<Expense>();
        public virtual IEnumerable<Leave> Leaves { get; set; } = new HashSet<Leave>();

    }
}
