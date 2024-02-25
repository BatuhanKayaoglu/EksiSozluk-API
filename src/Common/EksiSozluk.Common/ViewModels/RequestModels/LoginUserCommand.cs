using EksiSozluk.Common.ViewModels.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Common.ViewModels.RequestModels
{
    // Dışarıdan alacagim bilgiler için
    public class LoginUserCommand:IRequest<LoginUserViewModel> // Dışarıya 'LoginUserViewModel' dönmek istiyorum. 
    {
        // Dışarıdan alacagım bilgiler
        public string EmailAdress { get; set; }
        public string Password { get; set; }


        public LoginUserCommand(string emailAdress, string password)
        {
            EmailAdress = emailAdress;
            Password = password;
        }

        public LoginUserCommand()
        {
                
        }


    }
}
