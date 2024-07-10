using EksiSozluk.Api.Application.Repositories;
using EksiSozluk.Common.ViewModels.Queries;
using EksİSozluk.Domain.Models;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Cache
{
    public class RedisCacheService : IRedisCacheService
    {
        readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;

        public RedisCacheService(IConfiguration configuration, IUserRepository userRepository)
        {
            this.configuration = configuration;
            this.userRepository = userRepository;
        }

        public async Task<User> GetByIdAsync(Guid key, CancellationToken cancellationToken)
        {
            //var HOST_NAME = "redis-12985.c267.us-east-1-4.ec2.redns.redis-cloud.com";
            //var PORT_NUMBER = "12985";
            //var PASSWORD = "DrVMZPwGC1NJElCt5czA66czyEDCZUh4";
            IConfigurationSection info =configuration.GetSection("Redis"); 
            ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect(info.ToString());

            IDatabase db = _redis.GetDatabase();
            var user = await db.StringGetAsync(key.ToString());

            if (!user.IsNullOrEmpty)
            {
                // Deserialize the data to the User object (assuming JSON serialization)
                var userData = JsonSerializer.Deserialize<User>(user);
                return userData;
            }

            return null;
        }
        public Task DeleteAsync(Guid key, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }



        public async Task SetAsync(UserDetailViewModel user, CancellationToken cancellationToken)
        {
            var Control = await GetByIdAsync(user.Id, cancellationToken);
            if (Control != null)
            {
                Console.WriteLine("veri sistemde kayıtlı");
            }

            var HOST_NAME = "redis-12985.c267.us-east-1-4.ec2.redns.redis-cloud.com";
            var PORT_NUMBER = "12985";
            var PASSWORD = "DrVMZPwGC1NJElCt5czA66czyEDCZUh4";
            ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect($"{HOST_NAME}:{PORT_NUMBER},password={PASSWORD}");
            IDatabase db = _redis.GetDatabase();
            await db.StringSetAsync(user.Id.ToString(), JsonSerializer.Serialize(user));
        }

        public Task UpdatedAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
