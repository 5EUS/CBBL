using System.Runtime.CompilerServices;
using CBBL.src.Board;
using CBBL.src.Interfaces;
using CBBL.src.Pieces;
using static CBBL.src.Board.BoardUtils;
using static CBBL.src.Board.BoardOps;
using static CBBL.src.Pieces.Pieces;

namespace CBBL.src.Implementation.Movement;

public class CBBLPieceMoveGenerator(IAttackTables attackTables) : IPieceMoveGenerator
{
    private readonly IAttackTables _attackTables = attackTables;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void GenerateMoves(ref List<Move> result, BoardState boardState, PieceType pieceType)
    {
        var bitboard = boardState.GetBitboardFor(pieceType);

        switch (pieceType)
        {
            case PieceType.WhiteKnight:
            case PieceType.BlackKnight:
                GenerateKnightMoves(ref result, bitboard, TargetsFor(boardState, boardState.ColorToMove));
                break;
            case PieceType.WhiteKing:
            case PieceType.BlackKing:
                GenerateKingMoves(ref result, boardState, bitboard, TargetsFor(boardState, boardState.ColorToMove));
                break;
            case PieceType.WhiteQueen:
            case PieceType.BlackQueen:
                GenerateQueenMoves(ref result, boardState, bitboard, TargetsFor(boardState, boardState.ColorToMove));
                break;
            case PieceType.WhiteRook:
            case PieceType.BlackRook:
                GenerateRookMoves(ref result, boardState, bitboard, TargetsFor(boardState, boardState.ColorToMove));
                break;
            case PieceType.WhiteBishop:
            case PieceType.BlackBishop:
                GenerateBishopMoves(ref result, boardState, bitboard, TargetsFor(boardState, boardState.ColorToMove));
                break;
            case PieceType.WhitePawn:
            case PieceType.BlackPawn:
                bool isWhite = pieceType == PieceType.WhitePawn;
                var targets = isWhite ? BlackPieces(boardState) : WhitePieces(boardState);
                GeneratePawnMoves(ref result, boardState, bitboard, targets, isWhite);
                break;      
        }   
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong TargetsFor(BoardState boardState, PlayerColor color)
    {
        return color switch
        {
            PlayerColor.White => BlackPieces(boardState) | Empties(boardState),
            PlayerColor.Black => WhitePieces(boardState) | Empties(boardState),
            _ => throw new NotImplementedException()
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GenerateQueenMoves(ref List<Move> result, BoardState boardState, ulong pieces, ulong targets)
    {
        while (pieces != 0UL)
        {
            int from = PopLsb(ref pieces);

            ulong attacks = _attackTables.MagicGenerator.GetQueenAttacks(from, AllPieces(boardState)) & targets;

            while (attacks != 0UL) result.Add(new Move(from, PopLsb(ref attacks)));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GenerateRookMoves(ref List<Move> result, BoardState boardState, ulong pieces, ulong targets)
    {
        while (pieces != 0UL)
        {
            int from = PopLsb(ref pieces);

            ulong attacks = _attackTables.MagicGenerator.GetRookAttacks(from, AllPieces(boardState)) & targets;

            while (attacks != 0UL) result.Add(new Move(from, PopLsb(ref attacks)));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GenerateBishopMoves(ref List<Move> result, BoardState boardState, ulong pieces, ulong targets)
    {
        while (pieces != 0UL)
        {
            int from = PopLsb(ref pieces);

            ulong attacks = _attackTables.MagicGenerator.GetBishopAttacks(from, AllPieces(boardState)) & targets;

            while (attacks != 0UL) result.Add(new Move(from, PopLsb(ref attacks)));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GenerateKnightMoves(ref List<Move> result, ulong pieces, ulong targets)
    {
        while (pieces != 0UL)
        {
            int from = PopLsb(ref pieces);

            ulong attacks = _attackTables.LeapingAttackTables.GetAttacksForPiece(PieceType.WhiteKnight, from) & targets;

            while (attacks != 0UL) result.Add(new Move(from, PopLsb(ref attacks)));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GenerateKingMoves(ref List<Move> result, BoardState boardState, ulong pieces, ulong targets)
    {
        while (pieces != 0UL)
        {
            int from = PopLsb(ref pieces);

            ulong attacks = _attackTables.LeapingAttackTables.GetAttacksForPiece(PieceType.WhiteKing, from) & targets;

            while (attacks != 0UL) result.Add(new Move(from, PopLsb(ref attacks)));
        }

        var types = boardState.ColorToMove == PlayerColor.White
            ? Pieces.Pieces.BlackPieces : Pieces.Pieces.WhitePieces;

        var attack = _attackTables.Aggregate(boardState, types);

        if (boardState.ColorToMove == PlayerColor.White)
        {
            if (!boardState.IsKingInCheck(PlayerColor.White) && boardState.CanCastleKingside(true)
                && !GetBit(boardState, 5) && !GetBit(boardState, 6)
                && !IsSquareAttackedBy(attack, 5)
                && !IsSquareAttackedBy(attack, 6))
                result.Add(new Move(4, 6, MoveFlag.Castling));
            if (!boardState.IsKingInCheck(PlayerColor.White) && boardState.CanCastleQueenside(true)
                && !GetBit(boardState, 1) && !GetBit(boardState, 2) && !GetBit(boardState, 3)
                && !IsSquareAttackedBy(attack, 2)
                && !IsSquareAttackedBy(attack, 3))
                result.Add(new Move(4, 2, MoveFlag.Castling));
        }
        else
        {
            if (!boardState.IsKingInCheck(PlayerColor.Black) && boardState.CanCastleKingside(false)
                && !GetBit(boardState, 61) && !GetBit(boardState, 62)
                && !IsSquareAttackedBy(attack, 61)
                && !IsSquareAttackedBy(attack, 62))
                result.Add(new Move(60, 62, MoveFlag.Castling));
            if (!boardState.IsKingInCheck(PlayerColor.Black) && boardState.CanCastleQueenside(false)
                && !GetBit(boardState, 57) && !GetBit(boardState, 58) && !GetBit(boardState, 59)
                && !IsSquareAttackedBy(attack, 58)
                && !IsSquareAttackedBy(attack, 59))
                result.Add(new Move(60, 58, MoveFlag.Castling));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void PawnPushFor(ref List<Move> result, ulong to, ulong from, MoveFlag flags = MoveFlag.None, PieceType[]? promotionPieces = null)
    {
        while (to != 0UL && from != 0UL)
        {
            int to_square;

            if (flags.HasFlag(MoveFlag.EnPassant))
                to_square = GetLSB(to);
            else
                to_square = PopLsb(ref to);

            int from_square = PopLsb(ref from);

            if (from_square == -1 || to_square == -1)
                break;
                
            if (flags.HasFlag(MoveFlag.Promotion))
                foreach (var promotionPiece in promotionPieces)
                    result.Add(new Move(from_square, to_square, flags, promotionPiece));
            else
                result.Add(new Move(from_square, to_square, flags));

            continue;
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GeneratePawnMoves(ref List<Move>result, BoardState boardState, ulong pieces, ulong targets, bool isWhite)
    {
        PieceType pieceType = boardState.ColorToMove == PlayerColor.White ? PieceType.WhitePawn : PieceType.BlackPawn;
        ulong empties = Empties(boardState);
        if (isWhite)
        {
            ulong promotions = pieces & RANK_7;
            ulong non_promotions = pieces & ~RANK_7;

            ulong singles = North(non_promotions) & empties;
            PawnPushFor(ref result, singles, South(singles));
            ulong doubles = North(singles & RANK_3) & empties;
            PawnPushFor(ref result, doubles, South(South(doubles)), MoveFlag.DoublePush);

            if (promotions != 0UL)
            {
                ulong northPromotions = North(promotions) & empties;
                PawnPushFor(ref result, northPromotions, promotions & RANK_7, MoveFlag.Promotion, WhitePromotionPieces);
                ulong westPromotions = NorthWest(promotions) & targets;
                PawnPushFor(ref result, westPromotions, promotions & RANK_7, MoveFlag.Promotion | MoveFlag.Capture, WhitePromotionPieces);
                ulong eastPromotions = NorthEast(promotions) & targets;
                PawnPushFor(ref result, eastPromotions, promotions & RANK_7, MoveFlag.Promotion | MoveFlag.Capture, WhitePromotionPieces);
            }

            ulong westCaptures = NorthWest(non_promotions) & targets;
            PawnPushFor(ref result, westCaptures, SouthEast(westCaptures), MoveFlag.Capture);
            ulong eastCaptures = NorthEast(non_promotions) & targets;
            PawnPushFor(ref result, eastCaptures, SouthWest(eastCaptures), MoveFlag.Capture);

            if (boardState.EnPassantSquare != null)
            {
                var enpassants = SetBit(0UL, boardState.EnPassantSquare.Value) & (_attackTables.LeapingAttackTables.GetAttacksForPiece(pieceType, boardState.EnPassantSquare.Value - 7) |
                                                   _attackTables.LeapingAttackTables.GetAttacksForPiece(pieceType, boardState.EnPassantSquare.Value - 9));
                var enpassant_from = non_promotions & (SouthEast(enpassants) | SouthWest(enpassants));
                PawnPushFor(ref result, enpassants, enpassant_from, MoveFlag.EnPassant);
            }
        }
        else
        {
            var promotions = pieces & RANK_2;
            var non_promotions = pieces & ~RANK_2;

            var singles = South(non_promotions) & empties;
            PawnPushFor(ref result, singles, North(singles));
            var doubles = South(singles & RANK_6) & empties;
            PawnPushFor(ref result, doubles, North(North(doubles)), MoveFlag.DoublePush);

            if (promotions != 0UL)
            {
                var southPromotions = South(promotions) & empties;
                PawnPushFor(ref result, southPromotions, promotions & RANK_2, MoveFlag.Promotion, BlackPromotionPieces);
                var westPromotions = SouthWest(promotions) & targets;
                PawnPushFor(ref result, westPromotions, promotions & RANK_2, MoveFlag.Promotion | MoveFlag.Capture, BlackPromotionPieces);
                var eastPromotions = SouthEast(promotions) & targets;
                PawnPushFor(ref result, eastPromotions, promotions & RANK_2, MoveFlag.Promotion | MoveFlag.Capture, BlackPromotionPieces);
            }

            var westCaptures = SouthWest(non_promotions) & targets; 
            PawnPushFor(ref result, westCaptures, NorthEast(westCaptures), MoveFlag.Capture);
            var eastCaptures = SouthEast(non_promotions) & targets;
            PawnPushFor(ref result, eastCaptures, NorthWest(eastCaptures), MoveFlag.Capture);

            if (boardState.EnPassantSquare != null)
            {
                var enpassants = SetBit(0UL, boardState.EnPassantSquare.Value) & (_attackTables.LeapingAttackTables.GetAttacksForPiece(pieceType, boardState.EnPassantSquare.Value + 7) |
                                                   _attackTables.LeapingAttackTables.GetAttacksForPiece(pieceType, boardState.EnPassantSquare.Value + 9));
                var enpassant_from = non_promotions & (NorthEast(enpassants) | NorthWest(enpassants));
                PawnPushFor(ref result, enpassants, enpassant_from, MoveFlag.EnPassant);
            }
        }
    }
}