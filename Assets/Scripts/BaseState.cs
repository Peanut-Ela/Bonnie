using UnityEngine;

public class BaseState
{
    [HideInInspector] public StateMachine sm;
    [SerializeField] public string stateName = "Base State";
    public float duration = 0;
    public float age = float.MaxValue;
    //public bool bufferPoint = true;
    //public bool forceExit = false;
    //protected bool isDirty = false; // Use this only to return early after base FixedUpdate, set isDirty to true after directly changing states
    protected float _elapsedTime;

    public BaseState(StateMachine sm)
    {
        this.sm = sm;
        stateName = GetType().Name;
    }
    public virtual void Awake()
    {
    }
    public virtual void OnEnter()
    {
        if (duration != 0)
            age = duration;
    }
    public virtual void Update()
    {
    }
    /// <summary>
    /// <para>Counts down the age of the state if a duration is set.</para>
    /// This will <see cref="StateMachine.QueueState(BaseState)">QueueState</see> to buffered or default state after this state's duration expires.
    /// </summary>
    public virtual void FixedUpdate()
    {
        _elapsedTime += Time.fixedDeltaTime;
        StateUpdate();
        if (duration == 0) return;
        age -= Time.fixedDeltaTime;
        if (age <= 0)
        {
            OnStateExpired();
            if (sm.bufferedState == null)
                sm.QueueState(sm.DefaultState);
            else
            {
                BaseState tempState = sm.bufferedState;
                sm.bufferedState = null;
                sm.QueueState(tempState);
            }
        }
    }
    /// <summary>
    /// <para>FixedUpdate will queue <see cref="StateMachine.DefaultState">DefaultState</see> or <see cref="StateMachine.bufferedState">bufferedState</see> on low priority.</para>
    /// Should only handle state management. Calling <see cref="StateMachine.QueueState(BaseState)">QueueState</see> should only be done here, <see cref="StateMachine.bufferedState">bufferedState</see> can be set anywhere in the state.
    /// </summary>
    public virtual void StateUpdate()
    {

    }
    public virtual void OnExit()
    {
    }
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
    }
    public virtual void OnCollisionExit2D(Collision2D collision)
    {
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
    }
    public virtual void OnTriggerExit2D(Collider2D collision)
    {
    }
    public virtual void OnStateExpired()
    {
    }
}