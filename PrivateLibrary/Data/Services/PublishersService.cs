using PrivateLibrary.Data.Base;
using PrivateLibrary.Models;

namespace PrivateLibrary.Data.Services
{
    public class PublishersService : EntityBaseRepository<Publisher>, IPublishersService
    {
        public PublishersService(AppDbContext context) : base(context)
        {
            
        }
    }
}
