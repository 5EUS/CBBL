using CBBL.src.Abstraction;
using CBBL.src.Debugging;
using CBBL.src.Exceptions;
using CBBL.src.Implementation;
using CBBL.src.Interfaces;

namespace CBBL.src.Util;

/// <summary>
/// Functions as a context for CBBL engines. 
/// From the requied initialization step, attacks and magic bitboards are generated.
/// </summary>
public abstract class ServiceLoader : BitboardSingleton
{
    // Fields
    /// <summary>
    /// Whether or not Init() has been called
    /// </summary>
    public bool IsInitialized { get; protected set; } = false;

    /// <summary>
    /// The board that the context deals with
    /// </summary>
    private IBoard? _board;
    public IBoard? Board
    {
        get
        {
            if (!IsInitialized || _board == null)
                throw new UninitializedContextException();
            return _board;
        }
        protected set
        {
            _board = value;
        }
    }

    /// <summary>
    /// Precomputed attack tables for all pieces
    /// </summary>
    private IAttackTables? _attackTables;
    public IAttackTables? AttackTables
    {
        get
        {
            if (!IsInitialized || _attackTables == null)
                throw new UninitializedContextException();
            return _attackTables;
        }
        protected set
        {
            _attackTables = value;
        }
    }

    // Instance
    /// <summary>
    /// The context instance
    /// </summary>
    private static ServiceLoader? _instance;
    public static ServiceLoader Instance => _instance ??= new CBBLServiceLoader();

    public static void SetInstance(ServiceLoader serviceLoader)
    {
        _instance = serviceLoader;
    }

    // Methods
    /// <summary>
    /// Initialize a context and board
    /// </summary>
    /// <param name="board">The board to register</param>
    public virtual void Init(IBoard board)
    {
        Logger.Init();
        Logger.DualLogLine();
        Logger.DualLogLine("Starting engine using CBBL...");
        Logger.DualLogLine();
        Board = board;
        AttackTables = new CBBLAttackTables();
        IsInitialized = true;
        Logger.DualLogLine();
        Logger.DualLogLine("Successfully initialized engine");
        Logger.DualLogLine();
    }
}