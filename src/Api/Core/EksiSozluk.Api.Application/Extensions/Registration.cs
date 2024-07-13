using EksiSozluk.Api.Application.Cache;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddApplicationRegistration(this IServiceCollection services)
        {
            var asm= Assembly.GetExecutingAssembly();   

            // MediatorR ve AutoMapper'in DI kütüphanalerini bu yüzden yükledik.
            services.AddMediatR(asm);
            services.AddAutoMapper(asm);    
            services.AddValidatorsFromAssembly(asm);  
            services.AddTransient<IRedisCacheService,RedisCacheService>();
            services.AddTransient(typeof(IGenericRedisService<>), typeof(GenericRedisService<>));

            return services;    
        }
    }
}
