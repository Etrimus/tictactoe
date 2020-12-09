using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using JetBrains.Annotations;
using TicTacToe.Core;
using TicTacToe.Core.Models;
using TicTacToe.Domain;

namespace TicTacToe.App.Games
{
    [UsedImplicitly]
    public class MappingProfile : Profile
    {
        private static ReadOnlyTwoDimensionalCollection<Cell> _mapCells(byte[] source)
        {
            return new ReadOnlyTwoDimensionalCollection<Cell>(new Cell[2, 2]);
            //return source.Select(x => new Cell(x)).ToList();
        }

        public MappingProfile()
        {
            CreateMap<GameEntity, Game>()
                .ForMember(x => x.Cells, opt => opt.MapFrom(entry => new ReadOnlyTwoDimensionalCollection<Cell>(new Cell[2, 2])))
                //.ForMember(x => x.Cells, opt => opt.MapFrom(entity => _mapCells(entity.Cells)))
                .ReverseMap();
        }
    }
}