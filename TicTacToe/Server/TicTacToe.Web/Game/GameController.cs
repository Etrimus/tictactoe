using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.App.Game;
using TicTacToe.Web.Authentication;

namespace TicTacToe.Web.Game
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;
        private readonly AuthService _authService;

        public GameController(GameService gameService, AuthService authService)
        {
            _gameService = gameService;
            _authService = authService;
        }

        [HttpGet]
        public Task<GameModel[]> GetAll()
        {
            return _gameService.GetAllAsync();
        }

        [HttpGet("free")]
        public Task<GameModel[]> GetFree()
        {
            return _gameService.GetFreeAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public Task<GameModel> Get([FromRoute] Guid id)
        {
            return _gameService.GetAsync(id);
        }

        [HttpPost]
        public Task<Guid> Add()
        {
            return _gameService.CreateNewAsync()
                .ContinueWith(x => x.Result.Id);
        }

        [HttpPut("{id}/crossPlayer")]
        public async Task<Guid> SetCrossPlayer([FromRoute] Guid id)
        {
            var playerId = await _gameService.SetCrossPlayer(id);

            await HttpContext.SignInAsync(TicTacToeAuthDefaults.AUTHENTICATION_SCHEME, _authService.CreateClaimsPrincipal(id, playerId));

            return playerId;
        }

        [HttpPut("{id}/zeroPlayer")]
        public async Task<Guid> SetZeroPlayer([FromRoute] Guid id)
        {
            var playerId = await _gameService.SetZeroPlayer(id);

            await HttpContext.SignInAsync(TicTacToeAuthDefaults.AUTHENTICATION_SCHEME, _authService.CreateClaimsPrincipal(id, playerId));

            return playerId;
        }

        [HttpPut("{id}/turn/{cellNumber}")]
        [Authorize]
        public Task Turn([FromRoute] Guid id, [FromRoute] ushort cellNumber)
        {
            return _gameService.MakeTurn(id, Guid.Parse(HttpContext.User.Identity.Name), cellNumber);
        }
    }
}