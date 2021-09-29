using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace invMed.Data
{
    public class RolesConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Name = RoleName.Admin,
                    NormalizedName = RoleNormalizedName.Admin
                },
                new IdentityRole
                {
                    Name = RoleName.Manager,
                    NormalizedName = RoleNormalizedName.Manager
                },
                new IdentityRole
                {
                    Name = RoleName.Warehouseman,
                    NormalizedName = RoleNormalizedName.Warehouseman
                }
            );
        }
    }
}
