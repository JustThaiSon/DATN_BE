using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Helpers.Common.interfaces
{
    public interface IUltil
    {
        //string GenerateJwt(LoginDTO user);

        string GenerateToken(Guid id);
        string GenerateRefreshToken(Guid id);
        string? GenerateTokenFromRefreshToken(string refreshToken);
        Guid? ValidateToken(string token);
        bool IsAccessTokenExpired(string accessToken);
    }
}
