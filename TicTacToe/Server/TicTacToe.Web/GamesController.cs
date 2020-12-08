using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicTacToe.App.Games;
using TicTacToe.Web.Authentication;

namespace TicTacToe.Web
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly GameService _gameService;
        private readonly AuthService _authService;

        public GamesController(GameService gameService, AuthService authService)
        {
            _gameService = gameService;
            _authService = authService;
        }

        [HttpGet]
        public Task<Game[]> GetAll()
        {
            return _gameService.GetAllAsync().ToArrayAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public Task<Game> Get([FromRoute] Guid id)
        {
            return _gameService.GetAsync(id);
        }

        [HttpPut("{id}/crossPlayer")]
        public async Task<Guid> SetCrossPlayer([FromRoute] Guid id)
        {
            var playerId = await _gameService.SetCrossPlayer(id);

            await HttpContext.SignInAsync(TicTacToeAuthDefaults.AuthenticationScheme, _authService.CreateClaimsPrincipal(id, playerId));

            return playerId;
        }

        [HttpPut("{id}/zeroPlayer")]
        public async Task<Guid> SetZeroPlayer([FromRoute] Guid id)
        {
            var playerId = await _gameService.SetZeroPlayer(id);

            await HttpContext.SignInAsync(TicTacToeAuthDefaults.AuthenticationScheme, _authService.CreateClaimsPrincipal(id, playerId));

            return playerId;
        }

        [HttpPost]
        public Task<Guid> Add()
        {
            return _gameService.AddAsync(new Game())
                .ContinueWith(x => x.Result.Id);
        }
    }
}