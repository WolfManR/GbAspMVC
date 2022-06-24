﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace TemplateMailSender.Core
{
    public class AuthorizationHelper
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public AuthorizationHelper() => _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        public bool TryGetUserIdFromToken(string token, out Guid userId)
        {
            JwtSecurityToken tokenData = _jwtSecurityTokenHandler.ReadJwtToken(token);

            var userIdClaim = tokenData.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.NameId);

            if (userIdClaim is not null && Guid.TryParse(userIdClaim.Value, out userId)) return true;

            userId = Guid.Empty;
            return false;

        }
    }
}