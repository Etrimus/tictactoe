using Microsoft.Extensions.Options;

namespace TicTacToe.Web.Authentication
{
    internal class TicTacToePostConfigureOptions : IPostConfigureOptions<TicTacToeAutSchemeOptions>
    {
        public void PostConfigure(string name, TicTacToeAutSchemeOptions options)
        { }
    }
}