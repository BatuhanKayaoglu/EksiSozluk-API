using EksiSozluk.Api.Application.Cache;
using EksiSozluk.Api.Application.Features.Commands.User.ConfirmEmail;
using EksiSozluk.Api.Application.Features.Queries.GetUserDetail;
using EksiSozluk.Common.Events.User;
using EksiSozluk.Common.ViewModels.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text;

namespace EksiSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {

        private readonly IMediator mediator;
        private readonly IRedisCacheService redisCacheService;

        public UserController(IMediator mediator, IRedisCacheService redisCacheService)
        {
            this.mediator = mediator;
            this.redisCacheService = redisCacheService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await mediator.Send(new GetUserDetailQuery(id));

            await redisCacheService.SetAsync(user, default);

            //await distributedCache.SetStringAsync("User", System.Text.Json.JsonSerializer.Serialize(user));

            return Ok(user);
        }

        [HttpGet]
        [Route("UserName/{userName}")]
        public async Task<IActionResult> GetByUserName(string userName)
        {
            var user = await mediator.Send(new GetUserDetailQuery(Guid.Empty, userName));

            var data = await redisCacheService.GetByIdAsync(user.Id, default);

            return Ok(user);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var res = await mediator.Send(command);

            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var res = await mediator.Send(command);

            return Ok(res);
        }


        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            var res = await mediator.Send(command);

            return Ok(res);
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> Confirm(Guid id)
        {
            var res = await mediator.Send(new ConfirmEmailCommand() { ConfirmationId = id });

            return Ok(res);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangeUserPasswordCommand command)
        {
            if (command?.UserId is null) // eğer ki dışarıdan bu id'yi göndermedikleri bir senaryo olursa jwtToken'dan gelen UserId bilgisi CreatedById bilgisine atansın.
                command.UserId = UserId;
            var res = await mediator.Send(command);

            return Ok(res);
        }

    }
}
