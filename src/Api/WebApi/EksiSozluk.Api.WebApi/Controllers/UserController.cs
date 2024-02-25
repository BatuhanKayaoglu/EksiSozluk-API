using EksiSozluk.Api.Application.Features.Commands.User.ConfirmEmail;
using EksiSozluk.Common.Events.User;
using EksiSozluk.Common.ViewModels.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EksiSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
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
            var res = await mediator.Send(new ConfirmEmailCommand() { ConfirmationId=id});

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
