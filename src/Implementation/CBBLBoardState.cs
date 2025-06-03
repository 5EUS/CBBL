using System.Runtime.CompilerServices;
using System.Text;
using CBBL.src.Board;
using CBBL.src.Events;
using CBBL.src.Interfaces;
using CBBL.src.Pieces;
using CBBL.src.Util;
using static CBBL.src.Pieces.Pieces;

namespace CBBL.src.Implementation;

public class CBBLBoardState : BoardState
{
    private ulong[] _bitboards = new ulong[BoardGlobals.Instance.NumPieces];
    internal override ulong[] Bitboards { get => _bitboards; }

    public override PlayerColor ActivePlayer { get; } = PlayerColor.White;

    public override int HalfMoveClock { get; protected set; } = 0;

    public override int FullMoveNumber { get; protected set; } = 1;

    public override int? EnPassantSquare { get; protected set; } = null;

    public override PlayerColor ColorToMove { get; internal set; } = PlayerColor.White;

    private bool _isWhiteInCheck = false;
    private bool _isBlackInCheck = false;

    private bool _blackQSRookMoved = false;
    private bool _blackKSRookMoved = false;
    private bool _whiteQSRookMoved = false;
    private bool _whiteKSRookMoved = false;
    private bool _blackKingMoved = false;
    private bool _whiteKingMoved = false;

    private List<Move> _moveCache = [];
    private readonly Stack<(string, List<Move>)> _history = [];

    public CBBLBoardState(string? fen = null)
    {
        if (fen != null)
        {
            FromFen(fen);
        }
        else
        {
            Init();
        }
    }

    public CBBLBoardState(PlayerColor color)
    {
        Init();
        ColorToMove = color;
    }

    public override bool CanCastleKingside(bool isWhite)
    {
        if (isWhite)
        {
            return !_whiteKSRookMoved && !_whiteKingMoved;
        }
        else
        {
            return !_blackKSRookMoved && !_blackKingMoved;
        }
    }

    public override bool CanCastleQueenside(bool isWhite)
    {
        if (isWhite)
        {
            return !_whiteQSRookMoved && !_whiteKingMoved;
        }
        else
        {
            return !_blackQSRookMoved && !_blackKingMoved;
        }
    }

    public override ulong GetBitboardFor(PieceType type)
    {
        return Bitboards[(int)type];
    }

    public override bool IsKingInCheck(PlayerColor playerColor)
    {
        if (playerColor == PlayerColor.White)
        {
            return _isWhiteInCheck;
        }
        else if (playerColor == PlayerColor.Black)
        {
            return _isBlackInCheck;
        }
        throw new ArgumentException("Invalid player color", nameof(playerColor));
    }

    private void PushStateToHistory()
    {
        _history.Push(new(GetFen(), [.. _moveCache]));
    }

    public override bool ExecuteMove(Move move)
    {
        int movingPieceIndex = GetPieceAtSquare(move.FromSquare);
        BitboardEventManager.NotifyBeforePieceMoved((PieceType)movingPieceIndex, move.FromSquare, move.ToSquare);
        if (movingPieceIndex == -1)
            return false;

        if ((ColorToMove == PlayerColor.White && movingPieceIndex >= BoardGlobals.Instance.HalfPieces) ||
            (ColorToMove == PlayerColor.Black && movingPieceIndex < BoardGlobals.Instance.HalfPieces))
            return false;

        BitboardEventManager.NotifyPieceMoved((PieceType)movingPieceIndex, move.FromSquare, move.ToSquare);
        PushStateToHistory();

        ulong fromMask = 1UL << move.FromSquare;
        ulong toMask = 1UL << move.ToSquare;

        Bitboards[movingPieceIndex] &= ~fromMask;
        Bitboards[movingPieceIndex] |= toMask;

        int capturedPieceIndex = GetPieceAtSquare(move.ToSquare, skipMovingPiece: movingPieceIndex);
        if (capturedPieceIndex != -1)
        {
            BitboardEventManager.NotifyBeforePieceRemoved((PieceType)capturedPieceIndex, move.ToSquare);
            Bitboards[capturedPieceIndex] &= ~toMask;
            BitboardEventManager.NotifyPieceRemoved((PieceType)capturedPieceIndex, move.ToSquare);
        }

        if (move.Flag.HasFlag(MoveFlag.Castling))
        {
            if (ColorToMove == PlayerColor.White)
            {
                if (move.ToSquare == 6) // Kingside castling
                {
                    _whiteKSRookMoved = true;
                    Bitboards[(int)PieceType.WhiteRook] &= ~(1UL << 7);
                    Bitboards[(int)PieceType.WhiteRook] |= 1UL << 5;
                }
                else if (move.ToSquare == 2) // Queenside castling
                {
                    _whiteQSRookMoved = true;
                    Bitboards[(int)PieceType.WhiteRook] &= ~(1UL << 0);
                    Bitboards[(int)PieceType.WhiteRook] |= 1UL << 3;
                }
            }
            else
            {
                if (move.ToSquare == 62) // Kingside castling
                {
                    _blackKSRookMoved = true;
                    Bitboards[(int)PieceType.BlackRook] &= ~(1UL << 63);
                    Bitboards[(int)PieceType.BlackRook] |= 1UL << 61;
                }
                else if (move.ToSquare == 58) // Queenside castling
                {
                    _blackQSRookMoved = true;
                    Bitboards[(int)PieceType.BlackRook] &= ~(1UL << 56);
                    Bitboards[(int)PieceType.BlackRook] |= 1UL << 59;
                }
            }
        }

        if (movingPieceIndex == (int)PieceType.WhiteKing)
        {
            _whiteKingMoved = true;
        }
        else if (movingPieceIndex == (int)PieceType.BlackKing)
        {
            _blackKingMoved = true;
        }

        if (movingPieceIndex == (int)PieceType.WhiteRook)
        {
            if (move.FromSquare == 0)
            {
                _whiteQSRookMoved = true;
            }
            else if (move.FromSquare == 7)
            {
                _whiteKSRookMoved = true;
            }
        }
        else if (movingPieceIndex == (int)PieceType.BlackRook)
        {
            if (move.FromSquare == 56)
            {
                _blackQSRookMoved = true;
            }
            else if (move.FromSquare == 63)
            {
                _blackKSRookMoved = true;
            }
        }

        if (move.Flag.HasFlag(MoveFlag.DoublePush))
        {
            if (ColorToMove == PlayerColor.White)
            {
                EnPassantSquare = move.ToSquare - 8;
            }
            else
            {
                EnPassantSquare = move.ToSquare + 8;
            }
        }
        else
        {
            EnPassantSquare = null;
        }

        if (move.Flag.HasFlag(MoveFlag.EnPassant))
        {
            if (ColorToMove == PlayerColor.White)
            {
                Bitboards[(int)PieceType.BlackPawn] &= ~(1UL << (move.ToSquare - 8));
            }
            else
            {
                Bitboards[(int)PieceType.WhitePawn] &= ~(1UL << (move.ToSquare + 8));
            }
        }

        if (move.Flag.HasFlag(MoveFlag.Promotion))
        {
            BitboardEventManager.NotifyBeforePiecePromoted((PieceType)movingPieceIndex, move.FromSquare, move.ToSquare);
            if (ColorToMove == PlayerColor.White)
            {
                Bitboards[(int)PieceType.WhitePawn] &= ~toMask;
                Bitboards[(int)move.PromotionPieceType!.Value] |= toMask;
            }
            else
            {
                Bitboards[(int)PieceType.BlackPawn] &= ~toMask;
                Bitboards[(int)move.PromotionPieceType!.Value] |= toMask;
            }
            BitboardEventManager.NotifyPiecePromoted((PieceType)movingPieceIndex, move.FromSquare, move.ToSquare, move.PromotionPieceType!.Value);
        }

        if (move.Flag.HasFlag(MoveFlag.Check))
        {
            if (ColorToMove == PlayerColor.Black)
            {
                _isWhiteInCheck = true;
            }
            else
            {
                _isBlackInCheck = true;
            }
        }
        else
        {
            if (ColorToMove == PlayerColor.Black)
            {
                _isWhiteInCheck = false;
            }
            else
            {
                _isBlackInCheck = false;
            }
        }

        ColorToMove = (ColorToMove == PlayerColor.White) ? PlayerColor.Black : PlayerColor.White;
        BitboardEventManager.NotifyAfterPieceMoved((PieceType)movingPieceIndex, move.FromSquare, move.ToSquare);
        BitboardEventManager.NotifyBitboardMutation("movement", (PieceType)movingPieceIndex, move.FromSquare, move.ToSquare);
        return true;
    }

    public override void UndoMove()
    {
        if (_history.Count > 0)
        {
            var hist = _history.Pop();
            FromFen(hist.Item1);
            _moveCache = hist.Item2;
            BitboardEventManager.NotifyBitboardMutation("undo");
        }
        else
        {
            throw new InvalidOperationException("No previous state to undo to.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HistoryContains(Move move)
    {
        return _history.Any(h => h.Equals(move));
    }

    public override string GetFen()
    {
        var sb = new StringBuilder();

        for (int rank = 7; rank >= 0; rank--)
        {
            int empty = 0;
            for (int file = 0; file < 8; file++)
            {
                int sq = rank * 8 + file;
                int idx = GetPieceAtSquare(sq);
                if (idx == -1)
                {
                    empty++;
                }
                else
                {
                    if (empty > 0)
                    {
                        sb.Append(empty);
                        empty = 0;
                    }
                    sb.Append(PieceTypeToChar((PieceType)idx));
                }
            }
            if (empty > 0) sb.Append(empty);
            if (rank > 0) sb.Append('/');
        }

        sb.Append(' ');
        sb.Append(ColorToMove == PlayerColor.White ? 'w' : 'b');

        sb.Append(' ');
        var rights = new StringBuilder();
        if (CanCastleKingside(true)) rights.Append('K');
        if (CanCastleQueenside(true)) rights.Append('Q');
        if (CanCastleKingside(false)) rights.Append('k');
        if (CanCastleQueenside(false)) rights.Append('q');
        sb.Append(rights.Length > 0 ? rights.ToString() : "-");

        sb.Append(' ');
        if (EnPassantSquare.HasValue)
        {
            int sq = ColorToMove == PlayerColor.White ? EnPassantSquare.Value - 8 : EnPassantSquare.Value + 8;
            char fileChar = (char)('a' + (sq % 8));
            char rankChar = (char)('1' + (sq / 8));
            sb.Append(fileChar).Append(rankChar);
        }
        else sb.Append('-');

        sb.Append(' ').Append(HalfMoveClock);

        sb.Append(' ').Append(FullMoveNumber);

        return sb.ToString();
    }

    private void FromFen(string fen)
    {
        var parts = fen.Split(' ');
        if (parts.Length != 6)
            throw new ArgumentException("FEN must have 6 fields", nameof(fen));

        for (int i = 0; i < _bitboards.Length; i++)
            _bitboards[i] = 0UL;

        string[] ranks = parts[0].Split('/');
        if (ranks.Length != 8)
            throw new ArgumentException("Invalid piece placement", nameof(fen));

        for (int r = 0; r < 8; r++)
        {
            int rank = 7 - r;
            int file = 0;
            foreach (char c in ranks[r])
            {
                if (char.IsDigit(c))
                {
                    file += c - '0';
                }
                else
                {
                    var pt = CharToPieceType(c);
                    int idx = (int)pt;
                    int sq = rank * 8 + file;
                    _bitboards[idx] |= 1UL << sq;
                    file++;
                }
            }
            if (file != 8)
                throw new ArgumentException($"Rank {rank} is malformed in FEN", nameof(fen));
        }

        ColorToMove = parts[1] == "w" ? PlayerColor.White : PlayerColor.Black;

        string cr = parts[2];
        _whiteKingMoved = !cr.Contains('K') && !cr.Contains('Q');
        _whiteKSRookMoved = !cr.Contains('K');
        _whiteQSRookMoved = !cr.Contains('Q');
        _blackKingMoved = !cr.Contains('k') && !cr.Contains('q');
        _blackKSRookMoved = !cr.Contains('k');
        _blackQSRookMoved = !cr.Contains('q');

        if (parts[3] != "-")
        {
            int file = parts[3][0] - 'a';
            int rank = parts[3][1] - '1';
            EnPassantSquare = file + rank * 8;
            EnPassantSquare = ColorToMove == PlayerColor.White ? EnPassantSquare + 8 : EnPassantSquare - 8;
        }
        else
        {
            EnPassantSquare = null;
        }

        if (!int.TryParse(parts[4], out int hm))
            throw new ArgumentException("Invalid halfmove clock", nameof(fen));
        HalfMoveClock = hm;

        if (!int.TryParse(parts[5], out int fm) || fm < 1)
            throw new ArgumentException("Invalid fullmove number", nameof(fen));
        FullMoveNumber = fm;
    }

    public override void UpdateMoves()
    {
        _moveCache = [.. Context.Instance.MoveGenerator.GenerateMoves(this, MoveType.LEGAL, ColorToMove)];
    }

    public override List<Move> GetMoves()
    {
        return _moveCache;
    }

    public override bool IsKingInCheckmate(PlayerColor playerColor)
    {
        if (playerColor == PlayerColor.White)
        {
            return _isWhiteInCheck && !_moveCache.Any(m => m.Flag.HasFlag(MoveFlag.Check));
        }
        else if (playerColor == PlayerColor.Black)
        {
            return _isBlackInCheck && !_moveCache.Any(m => m.Flag.HasFlag(MoveFlag.Check));
        }
        return false;
    }

    public override bool IsStalemate()
    {
        return !_moveCache.Any() && !_isWhiteInCheck && !_isBlackInCheck;
    }
}