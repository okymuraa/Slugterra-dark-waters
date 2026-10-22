using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppsMinistry.Core.Input.Window
{
	[Serializable]
	public class InputWindowTransition
	{
		[SerializeField]
		private InputWindowView _view;

		[SerializeField]
		private List<InputEvent> _transitionConditions;

		[SerializeField]
		private int _priority;

		public InputWindowView View
		{
			get
			{
				return _view;
			}
		}

		public List<InputEvent> TransitionConditions
		{
			get
			{
				return _transitionConditions;
			}
		}

		public int Priority
		{
			get
			{
				return _priority;
			}
		}

		public InputWindowTransition()
		{
		}

		public InputWindowTransition(InputWindowView view, List<InputEvent> _conditions)
		{
			_view = view;
			_transitionConditions = new List<InputEvent>(_conditions);
		}
	}
}
