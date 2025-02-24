using Microsoft.AspNetCore.Mvc;
using Talbat.APIs.Errors;
using Talbat.APIs.Helpers;
using Talbat.Core;
using Talbat.Core.Repositories;
using Talbat.Repository;
using Talbat.Services;

namespace Talbat.APIs.Extensions
{
    public static class ApplicationServerExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection Services)
        {
            Services.AddScoped<IPaymentService, PaymentService>();
            Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            Services.AddScoped(typeof(IOrderServices),typeof(OrderServices));
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            Services.AddAutoMapper(typeof(MappingProfiles));
            Services.Configure<ApiBehaviorOptions>(Options => {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                            .SelectMany(P => P.Value.Errors)
                                            .Select(p => p.ErrorMessage)
                                            .ToList();

                    var validtionErrorResponse = new ApiValidtionResponseError()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validtionErrorResponse);
                };
            });
            return Services;
        }
    }
}
