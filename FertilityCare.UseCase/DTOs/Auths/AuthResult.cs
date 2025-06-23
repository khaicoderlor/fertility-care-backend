using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Auths
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; }

        public string? ErrorMessage { get; set; }

        public AuthResponse? Data { get; set; }

        public static AuthResult Success(AuthResponse data) => new() { IsSuccess = true, Data = data };

        public static AuthResult Failed(string error) => new() { IsSuccess = false, ErrorMessage = error };
    }
}
