using CBBL.src.Board;
using CBBL.src.Debugging;
using CBBL.src.Interfaces;

namespace CBBL.src.Testing;

public class BoardUnitTester(ISlidingAttackHelper helper)
{
    private readonly ISlidingAttackHelper _helper = helper;

    private bool PerformTest(string piece, string square, ulong blockers, ulong expected)
    {
        Logger.DualLogLine($"Performing test on {piece} at {square} with blockers {blockers} expecting {expected}", LogLevel.Test);
        int squareIndex = BoardUtils.SquareToIndex(square);
        if (squareIndex < 0)
            return false;

        ulong actual = piece switch
        {
            "rook" => _helper.GetRookAttacks(squareIndex, blockers),
            "bishop" => _helper.GetBishopAttacks(squareIndex, blockers),
            "queen" => _helper.GetQueenAttacks(squareIndex, blockers),
            _ => throw new ArgumentException("Invalid piece type")
        };

        if (actual != expected)
        {
            Logger.DualLogLine($"Test failed for {piece} on {square} with blockers {blockers:X16}. Expected: {expected:X16}, Actual: {actual:X16}", LogLevel.Test);
            Logger.DualLogLine($"Blockers: {blockers:X16}", LogLevel.Test);
            BoardUtils.PrintBitboard(blockers, LogLevel.Test);
            Logger.DualLogLine($"Expected: {expected:X16}", LogLevel.Test);
            BoardUtils.PrintBitboard(expected, LogLevel.Test);
            Logger.DualLogLine($"Actual: {actual:X16}", LogLevel.Test);
            BoardUtils.PrintBitboard(actual, LogLevel.Test);
            return false;
        }
        else
        {
            Logger.DualLogLine($"Test passed for {piece} on {square} with blockers {blockers:X16}. Expected: {expected:X16}, Actual: {actual:X16}", LogLevel.Test);
            return true;
        }
    }
}