using CBBL.src.Interfaces;
using CBBL.src.Pieces;

namespace CBBL.src.Implementation.Movement;

public class CBBLMoveGenerator(BoardState state, IAttackTables attackTables) : IMoveGenerator
{
    private readonly BoardState _state = state;
    public IPieceMoveGenerator Generator { get; } = new CBBLPieceMoveGenerator(state, attackTables);

    public IEnumerable<Move> GenerateMoves(MoveType moveType)
    {
        return moveType switch
        {
            MoveType.LEGAL => GenerateLegalMoves(),
            MoveType.PSEUDO_LEGAL => GeneratePseudoLegalMoves(),
            MoveType.EVASION => GenerateEvasionMoves(),
            _ => throw new ArgumentOutOfRangeException(nameof(moveType), moveType, "Invalid MoveType")
        };
    }

    private IEnumerable<Move> GenerateLegalMoves()
    {
        var psuedo = _state.IsKingInCheck(_state.ColorToMove) ? GenerateEvasionMoves() : GeneratePseudoLegalMoves();

        IEnumerable<Move> legal = [];

        foreach(var move in psuedo)
        {
            if (IsMoveLegal(move)) _ = legal.Append(move);
        }
        return legal;
    }

    private List<Move> GeneratePseudoLegalMoves()
    {
        List<Move> results = [];
        if (_state.ColorToMove == Board.PlayerColor.White)
        {
            foreach (var type in Pieces.Pieces.WhitePieces)
            {
                results.AddRange(Generator.GenerateMoves(type));
            }
        }
        else
        {
            foreach (var type in Pieces.Pieces.WhitePieces)
            {
                results.AddRange(Generator.GenerateMoves(type));
            }
        }
        return results;
    }

    private List<Move> GenerateEvasionMoves()
    {
        throw new NotImplementedException();
    }

    private bool IsMoveLegal(Move move)
    {
        return true; // TODO
    }
    
}