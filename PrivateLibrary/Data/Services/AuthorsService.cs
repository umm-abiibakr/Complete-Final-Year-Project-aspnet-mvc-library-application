using Microsoft.EntityFrameworkCore;
using PrivateLibrary.Data.Base;
using PrivateLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrivateLibrary.Data.Services
{
    public class AuthorsService : EntityBaseRepository<Author>, IAuthorsService
    {
        public AuthorsService(AppDbContext context) : base(context) { } 
    }

}
