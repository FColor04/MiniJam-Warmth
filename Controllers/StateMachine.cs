using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MainGameFramework;

namespace MiniJam_Warmth.Controllers;

/// <summary>
/// State machine, contains list of transitions between states and manages current state based on these transitions and their conditions.
/// </summary>
public class StateMachine
{
    private IState _currentState;
    private readonly Dictionary<Type, List<Transition>> _transitions = new ();
    private readonly List<Transition> _anyTransitions = new();

    /// <summary>
    /// Creates an empty state machine.
    /// </summary>
    /// <param name="defaultState">Initial state machine state</param>
    public StateMachine(IState defaultState)
    {
        _currentState = defaultState;
        defaultState.OnEnter();
        MainGame.OnUpdate += Tick;
    }

    ~StateMachine()
    {
        MainGame.OnUpdate -= Tick;
    }
    
    /// <summary>
    /// Updates state machine each frame and checks if any transitions should happen
    /// This is called internally by <see cref="MainGame.Update"/>
    /// </summary>
    /// <param name="deltaTime">Delta time</param>
    private void Tick(float deltaTime)
    {
        var transition = GetActiveTransition(_currentState);
        if (transition != null)
            SetState(transition.To);
        _currentState?.Tick();
    }

    /// <summary>
    /// Gets current state available transitions and evaluates each condition,
    /// if any returns true, the transition into new state begins.
    /// </summary>
    /// <param name="state">State to check</param>
    /// <returns>Transition if any or null</returns>
    private Transition GetActiveTransition(IState state)
    {
        for (int i = 0; i < _anyTransitions.Count; i++)
        {
            if (_anyTransitions[i].Condition())
                return _anyTransitions[i];
        }

        if (state == null) return null;
        
        if (_transitions.TryGetValue(state.GetType(), out var transitions))
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].Condition())
                    return transitions[i];
            }
        }

        return null;
    }

    /// <summary>
    /// Set state to new state, this also calls <see cref="IState.OnExit"/> and <see cref="IState.OnEnter"/> methods on previous and next state respectively.
    /// </summary>
    /// <param name="state">New state</param>
    public void SetState(IState state)
    {
        if (_currentState == state) return;
        //TODO: Find a way to wait for this to finish before moving onto new state, maybe some custom coroutines?
        _currentState?.OnExit();
        _currentState = state;
        _currentState?.OnEnter();
    }

    /// <summary>
    /// Adds regular transition between two <see cref="IState"/>
    /// </summary>
    /// <param name="from">Not null, from state</param>
    /// <param name="to">Not null, target state</param>
    /// <param name="condition">Condition to be met</param>
    public void AddTransition([NotNull] IState from, [NotNull] IState to, Func<bool> condition)
    {
        var newTransition = new Transition(to, condition);
        if(_transitions.ContainsKey(from.GetType()))
            _transitions[from.GetType()].Add(newTransition);
        else
            _transitions.Add(from.GetType(), new (){newTransition});
    }
    
    /// <summary>
    /// Adds any transition, any transitions are checked by every <see cref="IState"/> in state machine before other transitions
    /// </summary>
    /// <param name="to">Not null,Transition target state</param>
    /// <param name="condition">Condition to be met to transition to target state</param>
    public void AddAnyTransition([NotNull] IState to, Func<bool> condition) => _anyTransitions.Add(new Transition(to, condition));
    
    /// <summary>
    /// State machine transition, specifies target state and condition
    /// </summary>
    private class Transition
    {
        public readonly IState To;
        public readonly Func<bool> Condition;

        public Transition([NotNull] IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }
}