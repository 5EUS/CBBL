using System.ComponentModel;
using System.Runtime.CompilerServices;
using CBBL.src.Board;
using CBBL.src.Interfaces;
using CBBL.src.Pieces;
using static CBBL.src.Board.BoardUtils;
using static CBBL.src.Board.BoardOps;

namespace CBBL.src.Implementation.Movement;

public class CBBLPieceMoveGenerator(BoardState boardState, IAttackTables attackTables) : IPieceMoveGenerator
{
    private readonly BoardState _boardState = boardState;
    private readonly IAttackTables _attackTables = attackTables;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<Move> GenerateMoves(PieceType pieceType)
    {
        return pieceType switch
        {
            PieceType.WhiteQueen =>  GenerateQueenMoves(_boardState.Bitboards[(int)PieceType.WhiteQueen], TargetsFor(PlayerColor.White)),
            PieceType.BlackQueen =>  GenerateQueenMoves(_boardState.Bitboards[(int)PieceType.BlackQueen], TargetsFor(PlayerColor.Black)),
            PieceType.WhiteKnight => GenerateKnightMoves(_boardState.Bitboards[(int)PieceType.WhiteKnight], TargetsFor(PlayerColor.White)),
            PieceType.BlackKnight => GenerateKnightMoves(_boardState.Bitboards[(int)PieceType.BlackKnight], TargetsFor(PlayerColor.Black)),
            PieceType.WhiteBishop => GenerateBishopMoves(_boardState.Bitboards[(int)PieceType.WhiteBishop], TargetsFor(PlayerColor.White)),
            PieceType.BlackBishop => GenerateBishopMoves(_boardState.Bitboards[(int)PieceType.BlackBishop], TargetsFor(PlayerColor.Black)),
            PieceType.WhiteKing =>   GenerateKingMoves(_boardState.Bitboards[(int)PieceType.WhiteKing], TargetsFor(PlayerColor.White)),
            PieceType.BlackKing =>   GenerateKingMoves(_boardState.Bitboards[(int)PieceType.BlackKing], TargetsFor(PlayerColor.Black)),
            PieceType.WhiteRook =>   GenerateRookMoves(_boardState.Bitboards[(int)PieceType.WhiteRook], TargetsFor(PlayerColor.White)),
            PieceType.BlackRook =>   GenerateRookMoves(_boardState.Bitboards[(int)PieceType.BlackRook], TargetsFor(PlayerColor.Black)),
            PieceType.WhitePawn =>   GeneratePawnMoves(_boardState.Bitboards[(int)PieceType.WhitePawn], TargetsFor(PlayerColor.White), true),
            PieceType.BlackPawn =>   GeneratePawnMoves(_boardState.Bitboards[(int)PieceType.BlackPawn], TargetsFor(PlayerColor.Black), false),
            _ => throw new InvalidEnumArgumentException("Invalid PieceType")
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ulong TargetsFor(PlayerColor color)
    {
        return color switch
        {
            PlayerColor.White => BlackPieces(_boardState) | Empties(_boardState),
            PlayerColor.Black => WhitePieces(_boardState) | Empties(_boardState),
            _ => throw new NotImplementedException()
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private List<Move> GenerateQueenMoves(ulong pieces, ulong targets)
    {
        List<Move> result = [];
        while (pieces != 0UL)
        {
            int from = PopLsb(ref pieces);

            ulong attacks = _attackTables.MagicGenerator.GetQueenAttacks(from, AllPieces(_boardState)) & targets;

            while (attacks != 0UL) result.Add(new Move(from, PopLsb(ref attacks)));
        }
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private List<Move> GenerateRookMoves(ulong pieces, ulong targets)
    {
        List<Move> result = [];
        while (pieces != 0UL)
        {
            int from = PopLsb(ref pieces);

            ulong attacks = _attackTables.MagicGenerator.GetRookAttacks(from, AllPieces(_boardState)) & targets;

            while (attacks != 0UL) result.Add(new Move(from, PopLsb(ref attacks)));
        }

        if (_boardState.ColorToMove == PlayerColor.White)
        {
            if (_boardState.CanCastleKingside(true))
                result.Add(new Move(4, 6, MoveFlag.Castling));
            if (_boardState.CanCastleQueenside(true))
                result.Add(new Move(4, 2, MoveFlag.Castling));
        }
        else
        {
            if (_boardState.CanCastleKingside(false))
                result.Add(new Move(60, 62, MoveFlag.Castling));
            if (_boardState.CanCastleQueenside(false))
                result.Add(new Move(60, 58, MoveFlag.Castling));
        }

        return result; 
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private List<Move> GenerateBishopMoves(ulong pieces, ulong targets)
    {
        List<Move> result = [];
        while (pieces != 0UL)
        {
            int from = PopLsb(ref pieces);

            ulong attacks = _attackTables.MagicGenerator.GetBishopAttacks(from, AllPieces(_boardState)) & targets;

            while (attacks != 0UL) result.Add(new Move(from, PopLsb(ref attacks)));
        }
        return result; 
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private List<Move> GenerateKnightMoves(ulong pieces, ulong targets)
    {
        List<Move> result = [];
        while (pieces != 0UL)
        {
            int from = PopLsb(ref pieces);

            ulong attacks = _attackTables.LeapingAttackTables.GetAttacksForPiece(PieceType.WhiteKnight, from) & targets;

            while (attacks != 0UL) result.Add(new Move(from, PopLsb(ref attacks)));
        }
        return result; 
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private List<Move> GenerateKingMoves(ulong pieces, ulong targets)
    {
        List<Move> result = [];
        while (pieces != 0UL)
        {
            int from = PopLsb(ref pieces);

            ulong attacks = _attackTables.LeapingAttackTables.GetAttacksForPiece(PieceType.WhiteKing, from) & targets;

            while (attacks != 0UL) result.Add(new Move(from, PopLsb(ref attacks)));
        }
        return result; 
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static List<Move> PawnPushFor(ulong to, ulong from)
    {
        List<Move> result = [];
        while (to != 0UL && from != 0UL)
        {
            int to_square = PopLsb(ref to);
            int from_square = PopLsb(ref from);
            result.Add(new Move(from_square, to_square));
        }
        return result;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private List<Move> GeneratePawnMoves(ulong pieces, ulong targets, bool isWhite)
    {
        List<Move> result = [];
        PieceType pieceType = _boardState.ColorToMove == PlayerColor.White ? PieceType.WhitePawn : PieceType.BlackPawn;
        ulong empties = Empties(_boardState);
        if (isWhite)
        {
            ulong promotions = pieces & RANK_7;
            ulong non_promotions = pieces & ~RANK_7;

            ulong singles = North(non_promotions) & empties;
            result.AddRange(PawnPushFor(singles, non_promotions));
            ulong doubles = North(singles & RANK_3) & empties;
            result.AddRange(PawnPushFor(doubles, non_promotions & RANK_2));

            if (promotions != 0UL)
            {
                ulong westPromotions = NorthWest(promotions) & targets;
                result.AddRange(PawnPushFor(westPromotions, promotions & RANK_7));
                ulong eastPromotions = NorthEast(promotions) & targets;
                result.AddRange(PawnPushFor(eastPromotions, promotions & RANK_7));
            }

            ulong westCaptures = NorthWest(promotions) & targets;
            result.AddRange(PawnPushFor(westCaptures, SouthEast(promotions) & pieces));
            ulong eastCaptures = NorthEast(promotions) & targets;
            result.AddRange(PawnPushFor(eastCaptures, SouthWest(promotions) & pieces));

            if (_boardState.EnPassantSquare != null)
            {
                var enpassants = non_promotions & _attackTables.LeapingAttackTables.GetAttacksForPiece(pieceType, GetLSB(_boardState.EnPassantSquare.Value));
                var enpassant_from = GetLSB(East(enpassants) | West(enpassants));
                result.Add(new Move(enpassant_from, GetLSB(enpassants), MoveFlag.EnPassant));
            }
        }
        else
        {
            var promotions = pieces & RANK_2;
            var non_promotions = pieces & ~RANK_2;

            var singles = South(non_promotions) & Empties(_boardState);
            result.AddRange(PawnPushFor(singles, non_promotions));
            var doubles = South(singles & RANK_6) & Empties(_boardState);
            result.AddRange(PawnPushFor(doubles, non_promotions & RANK_5));

            if (promotions != 0UL)
            {
                var westPromotions = SouthWest(promotions) & targets;
                result.AddRange(PawnPushFor(westPromotions, promotions & RANK_2));
                var eastPromotions = SouthEast(promotions) & targets;
                result.AddRange(PawnPushFor(eastPromotions, promotions & RANK_2));
            }      

            var westCaptures = SouthWest(promotions) & targets;
            result.AddRange(PawnPushFor(westCaptures, NorthEast(promotions) & pieces));
            var eastCaptures = SouthEast(promotions) & targets;
            result.AddRange(PawnPushFor(eastCaptures, NorthWest(promotions) & pieces));

            if (_boardState.EnPassantSquare != null)
            {
                var enpassants = non_promotions & _attackTables.LeapingAttackTables.GetAttacksForPiece(pieceType, GetLSB(_boardState.EnPassantSquare.Value));
                var enpassant_from = GetLSB(East(enpassants) | West(enpassants));
                result.Add(new Move(enpassant_from, GetLSB(enpassants), MoveFlag.EnPassant));
            }
        }
        return result;
    }
}