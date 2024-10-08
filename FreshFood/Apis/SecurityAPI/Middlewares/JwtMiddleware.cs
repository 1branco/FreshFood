﻿using SecurityAPI.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace SecurityAPI.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.DecodeJwt(token) as JwtSecurityToken;
            if (userId != null && DateTimeOffset.FromUnixTimeSeconds(int.Parse(userId.Payload["exp"].ToString())).ToLocalTime() > DateTime.Now)
            {
                var claimValue = userId.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;

                // attach user to context on successful jwt validation
                //context.Items["User"] = service.GetCustomerByEmail(claimValue);
            }

            await _next(context);
        }
    }
}
