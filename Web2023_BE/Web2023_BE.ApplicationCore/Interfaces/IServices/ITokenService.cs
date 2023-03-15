
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.ApplicationCore.Interfaces.IServices
{
    public interface ITokenService
    {
        TokenResponse GetToken(TokenRequest request);
    }
}
