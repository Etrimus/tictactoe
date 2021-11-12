using AutoMapper;
using JetBrains.Annotations;
using TicTacToe.Core;
using TicTacToe.Core.Models;
using TicTacToe.Core.Services;
using TicTacToe.Domain;

namespace TicTacToe.App.Game
{
    [UsedImplicitly]
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GameEntity, GameModel>()
                .ForMember(x => x.Board, opt => opt.MapFrom(entity => _getBoard(entity)))
                .ReverseMap()
                .ForMember(x => x.NextTurn, opt => opt.MapFrom(model => model.Board.NextTurn))
                .ForMember(x => x.Winner, opt => opt.MapFrom(model => model.Board.Winner))
                .ForMember(x => x.Cells, opt => opt.MapFrom(model => _getCellsBytes(model.Board.Cells)));
        }

        private static Board _getBoard(GameEntity source)
        {
            if (source.Cells.Length < 4)
            {
                throw new ArgumentException("Count of board cells must be greater than 4.", $"{nameof(source)}.{nameof(source.Cells)}");
            }

            var dimensionLength = Math.Sqrt(source.Cells.Length);

            if (dimensionLength % 1 != 0)
            {
                throw new ArgumentException($"{nameof(source)}.{nameof(source.Cells)} can't represent a valid board.");
            }

            var cells = new CellType[(int)dimensionLength, (int)dimensionLength];
            var flatCounter = 0;

            for (var i = 0; i < dimensionLength; i++)
            {
                for (var j = 0; j < dimensionLength; j++)
                {
                    if (!Enum.IsDefined(typeof(CellType), source.Cells[flatCounter]))
                    {
                        throw new ArgumentException($"The value {source.Cells[flatCounter]} can not be converted to enum {nameof(CellType)}.");
                    }

                    cells[i, j] = (CellType)source.Cells[flatCounter];
                    flatCounter++;
                }
            }

            return new BoardManager().CreateBoard(source.NextTurn, source.Winner, cells);
        }

        private static byte[] _getCellsBytes(IEnumerable<Cell> source)
        {
            return source.Select(x => (byte)x.State).ToArray();
        }
    }
}