using CBBL.src.Board;
using CBBL.src.Pieces;

namespace CBBL.src.Interfaces;

public interface IAIPlayer
{
    public PlayerColor Color { get; }

    public Task<Move> GetBestMoveAsync();
    public Move GetBestMove();
}