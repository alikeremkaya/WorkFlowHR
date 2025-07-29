using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkFlowHR.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace WorkFlowHR.Infrastructure.AppContext
{
    public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public const string DevConnectionString = "AppConnectionDev";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
    }
}
