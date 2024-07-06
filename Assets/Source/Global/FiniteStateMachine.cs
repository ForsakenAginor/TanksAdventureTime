using System;
using System.Collections.Generic;

public class FiniteStateMachine<T>
    where T : FiniteStateMachineState<T>
{
    private Dictionary<Type, FiniteStateMachineState<T>> _states;
    private FiniteStateMachineState<T> _current;

    public void AddStates(Dictionary<Type, FiniteStateMachineState<T>> states)
    {
        _states = states;
    }

    public void SetState(Type stateType)
    {
        if (_states.TryGetValue(stateType, out FiniteStateMachineState<T> state) == false)
            return;

        _current?.Exit();
        _current = state;
        _current.Enter();
    }

    public void Update()
    {
        _current?.Update();
    }

    public void Exit()
    {
        _current?.Exit();
    }
}