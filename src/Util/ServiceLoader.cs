using CBBL.src.Abstraction;
using CBBL.src.Debugging;
using CBBL.src.Exceptions;
using CBBL.src.Implementation.AttackTables;
using CBBL.src.Implementation.Movement;
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
    /// Precomputed attack tables for all pieces
    /// </summary>
    private IAttackTables? _attackTables;
    public IAttackTables AttackTables
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

    /// <summary>
    /// Utility class for generating moves given a board position
    /// </summary>
    private IMoveGenerator? _moveGenerator;
    public IMoveGenerator MoveGenerator
    {
        get
        {
            if (!IsInitialized || _moveGenerator == null)
                throw new UninitializedContextException();
            return _moveGenerator;    
        }
        protected set
        {
            _moveGenerator = value;
        }
    }


    // Methods
    /// <summary>
    /// Initialize a context and board
    /// </summary>
    /// <param name="board">The board to register</param>
    public virtual void Init(int debug = 0, IAttackTables? attackTables = null, IMoveGenerator? moveGenerator = null)
    {
        // if (IsInitialized)
        //     throw new InvalidOperationException("ServiceLoader already initialized!");

        Logger.Init();
        if (debug == 0)
            Logger.SetAltLogLevel(LogLevel.None);

        Logger.DualLogLine();
        Logger.DualLogLine("Starting engine using CBBL...");
        Logger.DualLogLine();

        if (attackTables == null)
            AttackTables = new CBBLAttackTables(debug);
        else
            AttackTables = attackTables;

        if (moveGenerator == null)
            MoveGenerator = new CBBLMoveGenerator(_attackTables);
        else
            MoveGenerator = moveGenerator;

        IsInitialized = true;

        Logger.DualLogLine();
        Logger.DualLogLine("Successfully initialized engine");
        Logger.DualLogLine();
    }
}