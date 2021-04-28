﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using Microsoft.AspNetCore.Identity;

namespace invMed.Services
{
    public class AdminService
    {
        private readonly ApplicationDbContext _db;

        public AdminService(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<List<IdentityUser>> GetUsers()
        {
            List<IdentityUser> identityUsers = _db.Users.ToList();
            return Task.FromResult(identityUsers);
        }

        public Task<List<IdentityRole>> GetRoles()
        {
            List<IdentityRole> identityRoles = _db.Roles.ToList();
            return Task.FromResult(identityRoles);
        }
    }
}