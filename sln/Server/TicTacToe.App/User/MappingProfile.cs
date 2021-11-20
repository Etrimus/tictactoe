using AutoMapper;
using TicTacToe.Domain;

namespace TicTacToe.App.User
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserEntity, UserModel>();
        }
    }
}