using DiCho.Core.Custom;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.Core.Extension
{
    public static class FilterExtension
    {
        public static void ConfigureFilter<TErrorHandlingFilter>(this IServiceCollection services)
            where TErrorHandlingFilter : IExceptionFilter
        {
            services.AddMvc(ops =>
            {
                ops.ValueProviderFactories.Add(new SnakeCaseQueryValueProviderFactory());
            });
            services.AddControllers(options =>
            {
                options.Filters.Add<TErrorHandlingFilter>();
            });
        }
    }
}
