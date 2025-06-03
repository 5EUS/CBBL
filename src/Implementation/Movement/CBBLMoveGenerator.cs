using CBBL.src.Board;
using CBBL.src.Interfaces;
using CBBL.src.Pieces;

namespace CBBL.src.Implementation.Movement;

public class CBBLMoveGenerator(IAttackTables attackTables) : IMoveGenerator
{
    public IPieceMoveGenerator Generator { get; } = new CBBLPieceMoveGenerator(attackTables);

    public IEnumerable<Move> GenerateMoves(BoardState boardState, MoveType moveType, PlayerColor? color = null)
    {
        color ??= boardState.ColorToMove;
        return moveType switch
        {
            MoveType.LEGAL => GenerateLegalMoves(boardState, color),
            MoveType.PSEUDO_LEGAL => GeneratePseudoLegalMoves(boardState, color),
            MoveType.EVASION => GenerateEvasionMoves(boardState, color),
            _ => throw new ArgumentOutOfRangeException(nameof(moveType), moveType, "Invalid MoveType")
        };
    }
    
    private List<Move> GenerateLegalMoves(BoardState boardState, PlayerColor? color = null)
    {
        color ??= boardState.ColorToMove;
        var psuedo = boardState.IsKingInCheck(color.Value) 
            ? GenerateEvasionMoves(boardState, color) : GeneratePseudoLegalMoves(boardState, color);

        List<Move> legal = [];
        List<int> checks = [];
        for (int i = 0; i < psuedo.Count; i++)
        {
            var move = psuedo[i];
            var result = IsMoveLegal(boardState, move);
            if (result.Item1) legal.Add(move);
            if (result.Item2) checks.Add(legal.IndexOf(move));
        }

        for (int i = 0; i < checks.Count; i++)
        {
            var index = checks[i];
            var check = legal[index];
            check.Flag |= MoveFlag.Check;
            legal[index] = check;
        }

        return legal;
    }

    private List<Move> GeneratePseudoLegalMoves(BoardState boardState, PlayerColor? color = null)
    {
        List<Move> results = [];

        color ??= boardState.ColorToMove;
        var pieces = color == PlayerColor.White
            ? Pieces.Pieces.WhitePieces : Pieces.Pieces.BlackPieces;

        foreach (var pieceType in pieces)
            Generator.GenerateMoves(ref results, boardState, pieceType);

        return results;
    }

    private List<Move> GenerateEvasionMoves(BoardState boardState, PlayerColor? color = null)
    {
        List<Move> results = [];

        color ??= boardState.ColorToMove;
        var kingType = color == PlayerColor.White ? PieceType.WhiteKing : PieceType.BlackKing;
        ulong kingBB = boardState.GetBitboardFor(kingType);
        int kingSquare = BoardOps.GetLSB(kingBB);
        var enemyColor = color == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
        var threats = GeneratePseudoLegalMoves(boardState, enemyColor);

        // 1. Find checking pieces
        var checkers = 0UL;
        threats.Where(m => m.ToSquare == kingSquare)
            .ToList().ForEach(m => checkers |= BoardOps.SetBit(0UL, m.FromSquare));

        // 2. King moves (that do not move into check)
        List<Move> kingMoves = [];
        Generator.GenerateMoves(ref kingMoves, boardState, kingType);
        foreach (var move in kingMoves)
        {
            if (IsMoveLegal(boardState, move).Item1)
                results.Add(move);
        }

        // If there is more than one checker, only king moves are legal
        if (BoardOps.PopulationCount(checkers) > 1)
            return results;

        // 3. Capture the checking piece
        int checkerSquare = BoardOps.GetLSB(checkers);
        var allMoves = GenerateMoves(boardState, MoveType.PSEUDO_LEGAL, color.Value);
        foreach (var move in allMoves)
        {
            if (move.ToSquare == checkerSquare && move.FromSquare != kingSquare)
            {
                results.Add(move);
            }
        }

        // 4. Block the check (only applies to sliding piece checks)
        var checkerPieceType = (PieceType)boardState.GetPieceAtSquare(checkerSquare);
        if (Pieces.Pieces.SlidingPieces.Contains(checkerPieceType))
        {
            ulong blockSquares = BoardOps.Between(kingSquare, checkerSquare);
            foreach (var move in allMoves)
            {
                if (((1UL << move.ToSquare) & blockSquares) != 0 && move.FromSquare != kingSquare)
                {
                    results.Add(move);
                }
            }
        }

        return results;
    }


    private (bool,bool) IsMoveLegal(BoardState boardState, Move move)
    {
        if (!boardState.ExecuteMove(move))
            return new(false, false);

        bool check = false;    

        // check for legality
        var kingType = boardState.ColorToMove == PlayerColor.Black // move's king
            ? PieceType.WhiteKing : PieceType.BlackKing;

        var moves = GeneratePseudoLegalMoves(boardState);    

        bool isLegal = !moves
            .Any(m => m.ToSquare == BoardOps.GetLSB(boardState.GetBitboardFor(kingType)));

        // also check for check on opponent's king
        if (isLegal)
        {
            kingType = boardState.ColorToMove == PlayerColor.Black // opponent's king
                ? PieceType.BlackKing : PieceType.WhiteKing;

            var blockers = BoardUtils.AllPiecesNoKings(boardState);
            var pieceType = (PieceType)boardState.GetPieceAtSquare(move.ToSquare);
            int kingSquare = BoardOps.GetLSB(boardState.GetBitboardFor(kingType));

            _ = GeneratePseudoLegalMoves(boardState, boardState.ColorToMove == PlayerColor.White
                ? PlayerColor.Black : PlayerColor.White)
                .ToList().Where(m => m.ToSquare == kingSquare) // filter by moves that target the king
                .ToList().Any(m => check = true);
        }
        boardState.UndoMove();

        return new(isLegal, check);
    }
}