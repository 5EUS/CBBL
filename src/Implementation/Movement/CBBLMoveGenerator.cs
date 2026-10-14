using CBBL.src.Board;
using CBBL.src.Interfaces;
using CBBL.src.Pieces;

namespace CBBL.src.Implementation.Movement;

public class CBBLMoveGenerator(IAttackTables attackTables) : IMoveGenerator
{
    public IPieceMoveGenerator Generator { get; } = new CBBLPieceMoveGenerator(attackTables);
    private IAttackTables _attackTables = attackTables;

    // TODO Span instead of IEnumerable
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
            var result = CheckLegalityAndCheck(boardState, move);
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
        var threats = boardState.GetMoves();

        // 1. Find checking pieces
        var checkers = 0UL;
        threats.Where(m => m.Flag.HasFlag(MoveFlag.Check))
            .ToList().ForEach(m => checkers = BoardOps.SetBit(checkers, m.ToSquare));

        // 2. King moves (that do not move into check)
        List<Move> kingMoves = [];
        Generator.GenerateMoves(ref kingMoves, boardState, kingType);
        foreach (var move in kingMoves)
        {
            if (CheckLegalityAndCheck(boardState, move).Item1)
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


    private (bool,bool) CheckLegalityAndCheck(BoardState boardState, Move move)
    {
        // legality

        // get some info
        var move_color = boardState.ColorToMove;
        var move_king_type = move_color == PlayerColor.White ? PieceType.WhiteKing : PieceType.BlackKing;
        ulong move_king_square = boardState.GetBitboardFor(move_king_type);
        var move_piece_type = (PieceType)boardState.GetPieceAtSquare(move.FromSquare);
        if (move_piece_type == PieceType.WhiteKing || move_piece_type == PieceType.BlackKing)
            move_king_square = BoardOps.SetBit(0UL, move.ToSquare);
        int move_king_square_int = BoardOps.GetLSB(move_king_square);
        ulong opponent_pieces = move_color == PlayerColor.White ? BoardUtils.BlackPieces(boardState) : BoardUtils.WhitePieces(boardState);
        opponent_pieces &= ~BoardOps.SetBit(0UL, move.ToSquare); // simulate capture 

        // filter piece squares based on precalculated danger mask
        ulong move_legality_mask = _attackTables.KingDangerMask[move_king_square_int];
        ulong legality_occupancy = move_legality_mask & opponent_pieces;
        ulong blockers = BoardUtils.AllPiecesNoKings(boardState) 
            & (~BoardOps.SetBit(0UL, move.FromSquare) | BoardOps.SetBit(0UL, move.ToSquare)); // remove potential blockers

        bool is_legal = true;
        while (legality_occupancy != 0)
        {
            int square = BoardOps.GetLSB(legality_occupancy);
            var type = (PieceType)boardState.GetPieceAtSquare(square);
            if (square == move.ToSquare)
                type = (PieceType)boardState.GetPieceAtSquare(move.FromSquare);
            BoardOps.PopLsb(ref legality_occupancy);

            ulong attacks;
            if (Pieces.Pieces.LeapingPieces.Contains(type))
            {
                attacks = _attackTables.LeapingAttackTables.GetAttacksForPiece(type, square);
            }
            else
            {
                attacks = _attackTables.MagicGenerator.GetAttacksForPiece(type, square, blockers);
            }

            if ((attacks & move_king_square) != 0) is_legal = false;
        }

        // check
        bool is_check = false;
        if (!is_legal)
            return new(is_legal, is_check); // cannot be a check if illegal

        var opponent_king_type = move_color == PlayerColor.White
            ? PieceType.BlackKing : PieceType.WhiteKing;
        var opponent_king_square = boardState.GetBitboardFor(opponent_king_type);
        var opponent_king_square_int = BoardOps.GetLSB(opponent_king_square);
        var move_pieces = move_color == PlayerColor.White ? BoardUtils.WhitePieces(boardState) : BoardUtils.BlackPieces(boardState);
        move_pieces &= ~BoardOps.SetBit(0UL, move.FromSquare);
        move_pieces = BoardOps.SetBit(move_pieces, move.ToSquare);

        // filter
        var check_mask = _attackTables.KingDangerMask[opponent_king_square_int];
        ulong check_occupancy = check_mask & move_pieces;

        while (check_occupancy != 0)
        {
            int square = BoardOps.GetLSB(check_occupancy);
            var type = (PieceType)boardState.GetPieceAtSquare(square);
            if (square == move.ToSquare)
                type = (PieceType)boardState.GetPieceAtSquare(move.FromSquare);
            BoardOps.PopLsb(ref check_occupancy);

            ulong attacks;
            if (Pieces.Pieces.LeapingPieces.Contains(type))
            {
                attacks = _attackTables.LeapingAttackTables.GetAttacksForPiece(type, square);
            }
            else
            {
                attacks = _attackTables.MagicGenerator.GetAttacksForPiece(type, square, blockers);
            }
            if ((attacks & opponent_king_square) != 0) is_check = true;
        }

        return new(is_legal, is_check); // TODO GC overhead
    }
}