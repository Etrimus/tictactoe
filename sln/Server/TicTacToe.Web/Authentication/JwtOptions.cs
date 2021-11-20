using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TicTacToe.Web.Authentication
{
    internal class JwtOptions
    {
        private const string KEY = "B72163EDE81641F4BCD82D0B57725280";

        public const string ISSUER = "TicTacToeServer";

        public const string AUDIENCE = "TicTacToeClient";

        public const int LIFETIME_IN_MINUTES = 10000;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}