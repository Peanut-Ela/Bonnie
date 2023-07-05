using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public bool logStateTransfer = false;
    public virtual BaseState StartState => new BaseState(this);
    public virtual BaseState DefaultState => new BaseState(this);
    public BaseState currentState;
    public BaseState nextState;
    public BaseState bufferedState;

    //public event System.Action deathEvent;

    protected virtual void Awake()
    {
    }
    protected virtual void OnDestroy()
    {
    }
    protected virtual void Start()
    {
        Init();
    }
    protected virtual void Update()
    {
        currentState.Update();
    }
    protected virtual void FixedUpdate()
    {
        currentState.FixedUpdate();
        if (nextState != null)
        {
            ChangeState(nextState);
            nextState = null;
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter2D(collision);
    }
    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        currentState.OnCollisionExit2D(collision);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OnTriggerEnter2D(collision);
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        currentState.OnTriggerExit2D(collision);
    }
    public virtual void ChangeState(BaseState newState)
    {
        currentState.OnExit();
        if (logStateTransfer)
            print($"Before: {currentState.stateName}, after: {newState.stateName}");
        newState.OnEnter();
        currentState = newState;
    }
    /// <summary>
    /// <para>Only the first queued state will be used, any consequent queuestates will be discarded.</para>
    /// <para>This exists to ensure that multiple state changes do not occur in a single fixed Update, especially using inheritance.</para>
    /// </summary>
    /// <param name="newState"></param>
    public virtual void QueueState(BaseState newState)
    {
        if (nextState != null)
        {
            if (logStateTransfer)
                print($"DISCARDED {newState.stateName} for {nextState.stateName}");
            return;
        }
        nextState = newState;
    }

    public virtual void Init()
    {
        currentState = StartState;
        currentState.OnEnter();
    }
}