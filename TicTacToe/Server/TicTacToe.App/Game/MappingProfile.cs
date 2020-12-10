using System;
using AutoMapper;
using JetBrains.Annotations;
using TicTacToe.Core.Models;
using TicTacToe.Domain;

namespace TicTacToe.App.Game
{
    [UsedImplicitly]
    public class MappingProfile : Profile
    {
        private static Cell[,] _mapCells(byte[] source)
        {
            var dimensionLength = Math.Sqrt(source.Length);
            //var result = new Cell[dimensionLength, dimensionLength];
            return new Cell[2, 2];
        }

        public MappingProfile()
        {
            CreateMap<GameEntity, GameModel>()
                .ForMember(x => x.Cells, opt => opt.Ignore())
                //.ForMember(x => x.Cells, opt => opt.MapFrom(entity => _mapCells(entity.Cells)))
                .ReverseMap();
        }
    }
}