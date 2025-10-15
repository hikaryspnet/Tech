using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Core.Auth.Entities;

namespace Tech.Application.Auth.DTOs
{
    public record ServiceResultTokens
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
        public RefreshToken RefreshTokenEntity { get; init; }

    }
}
