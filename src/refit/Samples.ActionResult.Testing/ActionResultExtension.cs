using System;
using Microsoft.AspNetCore.Mvc;

namespace Samples.ActionResult.Testing
{
    public static class ActionResultExtension
    {
        /// <summary>
        /// Extracts the response value from the actionresult
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionResult"></param>
        /// <returns></returns>
        public static T Convert<T>(this ActionResult<T> actionResult) where T : class
        {
            var response = (actionResult.Result as ObjectResult);

            switch (response.StatusCode)
            {
                // server error response
                case int n when n >= 500:
                    break;

                // client error response
                case int n when n < 500 && n >= 400:
                    break;

                // redirection
                case int n when n < 400 && n >= 300:
                    break;

                // successful response
                case int n when n < 300 && n >= 200:
                    break;

                // informational
                case int n when n < 200 && n >= 0:
                    return response as T;

                default:
                    throw new NotSupportedException($"Result status {response.StatusCode} is not supported");
            }

            return response.Value as T;
        }
    }
}
