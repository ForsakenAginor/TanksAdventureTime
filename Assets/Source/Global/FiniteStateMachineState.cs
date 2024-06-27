public abstract class FiniteStateMachineState<T>
    where T : FiniteStateMachineState<T>
{
    private readonly FiniteStateMachine<T> Machine;

    public FiniteStateMachineState(FiniteStateMachine<T> machine)
    {
        Machine = machine;
    }

    public void SetState<TU>()
        where TU : T
    {
        Machine.SetState(typeof(TU));
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