using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core.Command;
using UnityEngine;

namespace AppsMinistry.Core.State
{
	public class StateMachine : MonoBehaviour
	{
		private bool isStopped;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<MonoState> OnStateChanged__BackingField;

		private Dictionary<Type, StateFlow> _stateFlows;

		private IStateMachineController _controller;

		private MonoState _currentState;

		private Type _nextState;

		private object[] _nextStateArgs;

		public IStateMachineController Controller
		{
			get
			{
				return _controller;
			}
			set
			{
				_controller = value;
			}
		}

		public MonoState CurrentState
		{
			get
			{
				return _currentState;
			}
		}

		public Type GetNextState
		{
			get
			{
				return _nextState;
			}
		}

		public event Action<MonoState> OnStateChanged
		{
			add
			{
				Action<MonoState> action = OnStateChanged__BackingField;
				Action<MonoState> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnStateChanged__BackingField, (Action<MonoState>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<MonoState> action = OnStateChanged__BackingField;
				Action<MonoState> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnStateChanged__BackingField, (Action<MonoState>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private void Awake()
		{
			_stateFlows = new Dictionary<Type, StateFlow>();
		}

		public void AddStateFlow(StateFlow stateFlow)
		{
			if (_stateFlows.ContainsKey(stateFlow.State))
			{
				UnityEngine.Debug.LogWarning(string.Format("Please check your StateMachine: StateFlow with {0} already exist", stateFlow.State.Name));
			}
			_stateFlows[stateFlow.State] = stateFlow;
		}

		public void NextState<T>(params object[] args) where T : MonoState
		{
			NextState(typeof(T), args);
		}

		public void NextState(Type type, params object[] args)
		{
			_nextState = type;
			_nextStateArgs = args;
		}

		public void ChangeState<T>(params object[] args) where T : MonoState
		{
			ChangeState(typeof(T), args);
		}

		public void ChangeState(Type type, params object[] args)
		{
			_nextState = type;
			_nextStateArgs = args;
			Terminate();
		}

		private void ApplyState()
		{
			_currentState = MonoCommand.ExecuteOn(_nextState, _controller.GameObject, _nextStateArgs) as MonoState;
			_currentState.StateMachine = this;
			_currentState.AsyncToken.AddResponder(new Responder<MonoState>(OnStateSuccess, OnStateFailed));
			if (OnStateChanged__BackingField != null)
			{
				OnStateChanged__BackingField(_currentState);
			}
			_nextState = null;
			_nextStateArgs = null;
		}

		private void OnStateSuccess(MonoState state)
		{
			_controller.Next(state);
			if (_stateFlows.ContainsKey(state.GetType()))
			{
				NextState(_stateFlows[state.GetType()].NextState);
			}
			_currentState = null;
		}

		private void OnStateFailed(MonoState state)
		{
			if (_stateFlows.ContainsKey(state.GetType()))
			{
				NextState(_stateFlows[state.GetType()].FailState);
			}
			_currentState = null;
		}

		public void Terminate()
		{
			if (_currentState != null)
			{
				_currentState.Finish(false);
			}
		}

		public void STopStateMashine()
		{
			isStopped = true;
		}

		private void Update()
		{
			if (!isStopped && _currentState == null && _nextState != null)
			{
				ApplyState();
			}
		}
	}
}
