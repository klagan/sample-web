﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sample.OpenApi.Extensions.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter" />
    /// <autogeneratedoc />
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="context">The context.</param>
        /// <autogeneratedoc />
        public void Apply(Operation operation, OperationFilterContext context)
        {
            //https://skalinets.github.io/swagger-authorization.html
            if (!context
                    .MethodInfo
                    .GetCustomAttributes(true)
                    .Any(x => x is AllowAnonymousAttribute) 
                && !context
                    .MethodInfo
                    .DeclaringType
                    .GetCustomAttributes(true)
                    .Any(x => x is AllowAnonymousAttribute))
            {
                operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                {
                    new Dictionary<string, IEnumerable<string>>
                    {
                        {"Bearer", Array.Empty<string>()}
                    }
                };
            }
        }
    }
}