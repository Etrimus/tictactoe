using AutoMapper;
using TicTacToe.Domain;

namespace TicTacToe.App.Games
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GameEntity, Game>()
                .ReverseMap();
        }
    }
}