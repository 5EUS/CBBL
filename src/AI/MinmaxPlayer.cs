using CBBL.src.Board;
using CBBL.src.Implementation;
using CBBL.src.Interfaces;
using CBBL.src.Pieces;
using CBBL.src.Util;

namespace CBBL.src.AI;

public class MinmaxPlayer(BoardState boardState, PlayerColor color, int maxDepth = 3) : IAIPlayer
{
    public PlayerColor Color { get; private set; } = color;
    public int MaxDepth { get; private set; } = maxDepth;
    private BoardState _boardState = boardState;

    public async Task<Move> GetBestMoveAsync()
    {
        return await Task.Run(GetBestMove);
    }

    public Move GetBestMove()
    {
        float alpha = float.MinValue;
        float beta = float.MaxValue;
        string fen = _boardState.GetFen();
        Move? result = null;

        foreach (var move in _boardState.GetMoves())
        {
            var copy = new CBBLBoardState(fen);
            if (!copy.ExecuteMove(move))
                continue;
            copy.UpdateMoves();
            float score = Minmax(copy, MaxDepth - 1, alpha, beta);
            if (Color == PlayerColor.White)
            {
                if (score > alpha)
                {
                    alpha = score;
                    result = move;
                }
            }
            else
            {
                if (score < beta)
                {
                    beta = score;
                    result = move;
                }
            }
            if (beta <= alpha)
                break; // Alpha-Beta pruning
        }

        if (result == null)
            throw new InvalidOperationException("No valid moves found.");

        return result.Value;
    }

    private float Minmax(CBBLBoardState copy, int depth, float alpha, float beta)
    {
        if (depth == 0 || copy.IsKingInCheckmate(copy.ColorToMove))
        {
            return EvaluateBoard(copy);
        }

        bool isWhite = copy.ColorToMove == PlayerColor.White;

        foreach (var move in copy.GetMoves())
        {
            if (!copy.ExecuteMove(move))
                continue;
            copy.UpdateMoves();
            float score = Minmax(copy, depth - 1, alpha, beta);
            copy.UndoMove();

            if (isWhite)
            {
                alpha = Math.Max(alpha, score);
                if (beta <= alpha)
                    break; // Beta cut-off
            }
            else
            {
                beta = Math.Min(beta, score);
                if (beta <= alpha)
                    break; // Alpha cut-off
            }
        }

        return isWhite ? alpha : beta;
    }

    private float EvaluateBoard(CBBLBoardState copy)
    {
        float score = 0f;

        for (int i = 0; i < BoardGlobals.Instance.NumPieces; i++)
        {
            var pieceType = (PieceType)i;
            ulong bitboard = copy.GetBitboardFor(pieceType);

            var pieceValue = GetPieceValue(pieceType, bitboard);

            var positionalValue = GetPositionalValue(pieceType, bitboard);

            // var captureValue = GetCaptureValue(pieceType, copy, bitboard);

            if (copy.ColorToMove == PlayerColor.White)
            {
                score += pieceValue + positionalValue; 
            }
            else
            {
                score -= pieceValue + positionalValue;
            }
        }

        if (copy.IsKingInCheckmate(Color == PlayerColor.White ? PlayerColor.Black : PlayerColor.White))
            return float.NegativeInfinity;
        else if (copy.IsKingInCheckmate(Color))
            return float.PositiveInfinity;
        else if (copy.IsStalemate())
            return 0f;

        return score;
    }

    private float GetPieceValue(PieceType pieceType, ulong occupancy)
    {
        float score = 0f;
        while (occupancy != 0)
        {
            BoardOps.PopLsb(ref occupancy);
            score += GetPieceValue(pieceType);
        }
        return score;
    }

    private float GetPositionalValue(PieceType pieceType, ulong occupancy)
    {
        float score = 0f;
        while (occupancy != 0)
        {
            int square = BoardOps.PopLsb(ref occupancy);
            score += PositionTable.GetValue(pieceType, square);
        }
        return score;
    }

    private float GetCaptureValue(PieceType pieceType, BoardState copy, ulong occupancy)
    {
        float score = 0f;
        var enemyColor = copy.ColorToMove == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
        var enemyMoves = Context.Instance.MoveGenerator.GenerateMoves(copy, MoveType.LEGAL, enemyColor);
        var moves = copy.GetMoves();

        List<int> squares = [];
        while (occupancy != 0)
        {
            int square = BoardOps.PopLsb(ref occupancy);
            squares.Add(square);
        }

        foreach (var move in enemyMoves)
        {
            if (squares.Contains(move.ToSquare))
            {
                score -= GetPieceValue(pieceType);
            }
        }

        foreach (var move in moves)
        {
            var enemyType = copy.GetPieceAtSquare(move.ToSquare);
            if (enemyType != -1)
            {
                score += GetPieceValue((PieceType)enemyType);
            }
        }
        return score;
    }

    private float GetPieceValue(PieceType pieceType)
    {
        return pieceType switch
        {
            PieceType.WhitePawn => 1f,
            PieceType.BlackPawn => 1f,
            PieceType.WhiteKnight => 3f,
            PieceType.BlackKnight => 3f,
            PieceType.WhiteBishop => 3f,
            PieceType.BlackBishop => 3f,
            PieceType.WhiteRook => 5f,
            PieceType.BlackRook => 5f,
            PieceType.WhiteQueen => 9f,
            PieceType.BlackQueen => 9f,
            PieceType.WhiteKing => 1000f,
            PieceType.BlackKing => 1000f,
            _ => throw new ArgumentOutOfRangeException(nameof(pieceType), pieceType, null)
        };
    }
}