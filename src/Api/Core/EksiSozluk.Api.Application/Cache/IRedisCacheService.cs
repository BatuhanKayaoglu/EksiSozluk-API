using EksiSozluk.Common.ViewModels.Queries;
using EksİSozluk.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Cache
{
    public interface IRedisCacheService
    {
        Task<User> GetByIdAsync(Guid key,CancellationToken cancellationToken);
        Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
        Task SetAsync(UserDetailViewModel user, CancellationToken cancellationToken);
        Task DeleteAsync(Guid key, CancellationToken cancellationToken);
        Task UpdatedAsync(User user, CancellationToken cancellationToken);
    }
}
