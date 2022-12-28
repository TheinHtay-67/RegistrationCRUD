using Microsoft.EntityFrameworkCore;
using RegistrationTest20221221.Models.Student;
using RegistrationTest20221221.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationTest20221221.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<RegistrationTest20221221.Models.User.UserViewModel> UserViewModel { get; set; }
        public DbSet<StudentModel> Student { get; set; }
    }
}
