using CBBL.src.Pieces;

namespace CBBL.src.Events.Callbacks;

public interface IBitboardMutationCallback
{
    void BeforePieceRemoved(PieceType piece, int square);
    void OnPieceRemoved(PieceType piece, int square);
    void BeforePieceMoved(PieceType piece, int fromSquare, int toSquare);
    void OnPieceMoved(PieceType piece, int fromSquare, int toSquare);
    void AfterPieceMoved(PieceType piece, int fromSquare, int toSquare);
    void BeforePiecePromoted(PieceType piece, int fromSquare, int toSquare);
    void OnPiecePromoted(PieceType piece, int fromSquare, int toSquare, PieceType promotedTo);

    void OnBitboardMutation(string mutationType, PieceType? piece, int? fromSquare, int? toSquare, PieceType? promotedTo = null);
}

public class BitboardMutationCallback : IBitboardMutationCallback
{
    public virtual void BeforePieceRemoved(PieceType piece, int square) { }
    public virtual void OnPieceRemoved(PieceType piece, int square) { }

    public virtual void BeforePieceMoved(PieceType piece, int fromSquare, int toSquare) { }
    public virtual void OnPieceMoved(PieceType piece, int fromSquare, int toSquare) { }
    public virtual void AfterPieceMoved(PieceType piece, int fromSquare, int toSquare) { }

    public virtual void BeforePiecePromoted(PieceType piece, int fromSquare, int toSquare) { }
    public virtual void OnPiecePromoted(PieceType piece, int fromSquare, int toSquare, PieceType promotedTo) { }

    public virtual void OnBitboardMutation(string mutationType, PieceType? piece, int? fromSquare, int? toSquare, PieceType? promotedTo = null) { }
}