using CBBL.src.Events.Callbacks;
using CBBL.src.Pieces;

namespace CBBL.src.Events;

public static class BitboardEventManager
{
    private static readonly List<IBitboardMutationCallback> _callbacks = [];

    public static void RegisterCallback(IBitboardMutationCallback callback)
        => _callbacks.Add(callback);

    public static void UnregisterCallback(IBitboardMutationCallback callback)
        => _callbacks.Remove(callback);

    public static void NotifyBeforePieceRemoved(PieceType piece, int square)
    {
        foreach (var callback in _callbacks)
            callback.BeforePieceRemoved(piece, square);
    }

    public static void NotifyPieceRemoved(PieceType piece, int square)
    {
        foreach (var callback in _callbacks)
            callback.OnPieceRemoved(piece, square);
    }

    public static void NotifyBeforePieceMoved(PieceType piece, int fromSquare, int toSquare)
    {
        foreach (var callback in _callbacks)
            callback.BeforePieceMoved(piece, fromSquare, toSquare);
    }

    public static void NotifyPieceMoved(PieceType piece, int fromSquare, int toSquare)
    {
        foreach (var callback in _callbacks)
            callback.OnPieceMoved(piece, fromSquare, toSquare);
    }

    public static void NotifyAfterPieceMoved(PieceType piece, int fromSquare, int toSquare)
    {
        foreach (var callback in _callbacks)
            callback.AfterPieceMoved(piece, fromSquare, toSquare);
    }

    public static void NotifyBeforePiecePromoted(PieceType piece, int fromSquare, int toSquare)
    {
        foreach (var callback in _callbacks)
            callback.BeforePiecePromoted(piece, fromSquare, toSquare);
    }

    public static void NotifyPiecePromoted(PieceType piece, int fromSquare, int toSquare, PieceType promotedTo)
    {
        foreach (var callback in _callbacks)
            callback.OnPiecePromoted(piece, fromSquare, toSquare, promotedTo);
    }
    
    public static void NotifyBitboardMutation(string mutationType, PieceType? piece = null, int? fromSquare = null, int? toSquare = null, PieceType? promotedTo = null)
    {
        foreach (var callback in _callbacks)
            callback.OnBitboardMutation(mutationType, piece, fromSquare, toSquare, promotedTo);
    }
}
