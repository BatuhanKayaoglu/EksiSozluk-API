using AutoMapper;
using EksiSozluk.Api.Application.Cache;
using EksiSozluk.Api.Application.Repositories;
using EksiSozluk.Common.ViewModels.Queries;
using EksİSozluk.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Queries.GetUserDetail
{
    public class GetUserDetailQueryHandler : IRequestHandler<GetUserDetailQuery, UserDetailViewModel>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IRedisCacheService redisCacheService;

        public GetUserDetailQueryHandler(IUserRepository userRepository, IMapper mapper, IRedisCacheService redisCacheService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.redisCacheService = redisCacheService;
        }

        public async Task<UserDetailViewModel> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
        {
            var data = await userRepository.GetByIdAsync(request.UserId);

            if (data != null)
                return mapper.Map<UserDetailViewModel>(data);

            // if redis cache is empty, get data from db        
            User? dbUser = null;
            if (request.UserId != Guid.Empty)
            {
                dbUser = await userRepository.GetByIdAsync(request.UserId);
                // user not found in Redis, add it  
                await redisCacheService.SetAsync(dbUser, default);
            }

            else if (!string.IsNullOrEmpty(request.UserName))
            {
                dbUser = await userRepository.GetSingleAsync(i => i.Username == request.UserName);
                await redisCacheService.SetAsync(dbUser, default);
            }
            return mapper.Map<UserDetailViewModel>(dbUser);
        }
    }
}
