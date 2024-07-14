public abstract class FiniteStateMachineState<T>
    where T : FiniteStateMachineState<T>
{
    private readonly FiniteStateMachine<T> _machine;

    public FiniteStateMachineState(FiniteStateMachine<T> machine)
    {
        _machine = machine;
    }

    public void SetState<TU>()
        where TU : T
    {
        _machine.SetState(typeof(TU));
    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void Update()
    {
    }
}