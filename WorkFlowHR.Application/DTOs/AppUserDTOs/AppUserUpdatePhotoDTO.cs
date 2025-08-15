using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowHR.Application.DTOs.AppUserDTOs
{
    public class AppUserUpdatePhotoDTO
    {
        public Guid Id { get; set; }
        public byte[] Image { get; set; } = default!;
    }
}
