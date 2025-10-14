using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Application.Auth.DTOs
{
    public record RefreshTokenRequest
    {
        public string RefreshToken { get; init; } 
    }
}
