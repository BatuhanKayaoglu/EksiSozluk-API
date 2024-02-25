using AutoMapper;
using EksiSozluk.Api.Application.Repositories;
using EksiSozluk.Common.Infrastructure.Exceptions;
using EksiSozluk.Common.ViewModels.RequestModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EksİSozluk.Domain.Models;
using EksiSozluk.Common.Infrastructure;
using EksiSozluk.Common.Events.User;
using EksiSozluk.Common;

namespace EksiSozluk.Api.Application.Features.Commands.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existUser = await userRepository.GetSingleAsync(i => i.EmailAddress == request.EmailAddress);

            if (existUser is not null)
                throw new DatabaseValidationException("User already exist");

            var dbUser = mapper.Map<EksİSozluk.Domain.Models.User>(request);

            var rows = await userRepository.AddAsync(dbUser);

            // Email changed/created
            if (rows > 0)
            {
                var @event = new UserEmailChangedEvent()
                {
                    OldEmailAddress = null,
                    NewEmailAddress = dbUser.EmailAddress
                };

                QueueFactory.SendMessageToExchange(exchangeName:SozlukConstants.UserExchangeName,exchangeType:SozlukConstants.DefaultExchangeType,queueName:SozlukConstants.UserEmailChangedQueueName,obj:@event);

            }

            return dbUser.Id;


        }


    }
}
