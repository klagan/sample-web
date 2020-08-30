﻿namespace Sample.OpenApi.Extensions
{
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <inheritdoc />
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <autogeneratedoc />
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info
            {
                Title       = "Sample title for the API",
                Version     = description.ApiVersion.ToString(),
                Description = "Write some blurb here",
                Contact     = new Contact
                {
                    Name = "Kam Lagan"
                    , Email = "help@help.com"
                },
                TermsOfService = "Shareware",
                License        = new License
                {
                    Name = "MIT"
                    , Url = "https://opensource.org/licenses/MIT"
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += "<br/><p><font size='6' face='verdana' color='red'>This API version has been deprecated.</font></p>";
            }

            return info;
        }
    }
}