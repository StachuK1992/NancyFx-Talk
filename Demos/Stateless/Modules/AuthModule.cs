﻿using Nancy;
using Stateless.UserData;

namespace Stateless.Modules
{
    public class AuthModule : NancyModule
    {
        public AuthModule()
            : base("/auth/")
        {
            //the Post["/login"] method is used mainly to fetch the api key for subsequent calls
            Post["/"] = x =>
            {
                string apiKey = UserDatabase.ValidateUser((string)this.Request.Form.Username,
                                                          (string)this.Request.Form.Password);

                return string.IsNullOrEmpty(apiKey)
                           ? new Response { StatusCode = HttpStatusCode.Unauthorized }
                           : this.Response.AsJson(new { ApiKey = apiKey });
            };

            //do something to destroy the api key, maybe?                    
            Delete["/"] = x =>
            {
                var apiKey = (string)this.Request.Form.ApiKey;
                UserDatabase.RemoveApiKey(apiKey);
                return new Response { StatusCode = HttpStatusCode.OK };
            };
        }
    }
}