using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicTacToe.App.Games;

namespace TicTacToe.Web
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly GameService _gameService;

        public GamesController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public Task<Game[]> GetAll()
        {
            return _gameService.GetAllAsync().ToArrayAsync();
        }

        [HttpGet("{id}")]
        public Task<Game> Get([FromRoute] Guid id)
        {
            return _gameService.GetAsync(id);
        }

        [HttpPut("{id}/crossPlayer")]
        public Task<Guid> SetCrossPlayer([FromRoute] Guid id)
        {
            return _gameService.SetCrossPlayer(id);
        }

        [HttpPut("{id}/zeroPlayer")]
        public Task<Guid> SetZeroPlayer([FromRoute] Guid id)
        {
            return _gameService.SetZeroPlayer(id);
        }

        [HttpPost]
        public Task<Guid> Add()
        {
            return _gameService.AddAsync(new Game()).ContinueWith(x => x.Result.Id.Value);
        }
    }
}