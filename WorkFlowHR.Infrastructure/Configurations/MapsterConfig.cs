using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Core.Base;
using WorkFlowHR.Domain.Entities;

namespace WorkFlowHR.Infrastructure.Configurations
{
    public class MapsterConfig
    {
        public static void RegisterMappings(TypeAdapterConfig config)
        {
            config.NewConfig<ClaimsPrincipal, AppUser>()
                .Map(d => d.Email, s => s.FindFirst(ClaimTypes.Email)!.Value)
                .Map(d => d.FirstName, s => s.FindFirst(ClaimTypes.GivenName)!.Value)
                .Map(d => d.LastName, s => s.FindFirst(ClaimTypes.Surname)!.Value)
                .Map(d => d.AzureAdObjectId, s => s.FindFirst("oid")!.Value)
                .Map(d => d.DisplayName, s => s.Identity != null ? s.Identity.Name : string.Empty)
                // Role mapping is handled in service
                .Inherits<BaseUser, AppUser>();
        }
    }
}
