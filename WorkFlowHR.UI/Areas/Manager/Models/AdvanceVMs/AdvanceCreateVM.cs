using Microsoft.AspNetCore.Mvc.Rendering;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.UI.Areas.Manager.Models.AdvanceVMs
{

    public class AdvanceCreateVM
    {
        public double Amount { get; set; }
        public DateTime AdvanceDate { get; set; } =DateTime.Now;
        public string Description { get; set; } = null!;

        public IFormFile? ImageFile { get; set; }          // yüklenen dosya

        public Guid? ManagerAppUserId { get; set; }        // opsiyonel
        public SelectList? Managers { get; set; }          // controller dolduracak
         

       
        public IFormFile NewImage { get; set; }
        public Roles Roles { get; set; }
        public Guid ManagerId { get; set; }
                
        public AdvanceStatus AdvanceStatus { get; set; } = AdvanceStatus.Pending;
    }

}
