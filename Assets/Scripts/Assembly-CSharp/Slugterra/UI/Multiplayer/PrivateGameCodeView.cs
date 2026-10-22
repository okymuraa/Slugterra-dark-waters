using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class PrivateGameCodeView : PopupView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCloseEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnInstructionEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<string> OnStartPrivateGameEvent__BackingField;

		[SerializeField]
		private GameObject _enterCode;

		[SerializeField]
		private GameObject _instructionCode;

		[SerializeField]
		private GameObject _btnNotReady;

		[SerializeField]
		private GameObject _btnReady;

		[SerializeField]
		private tk2dSlicedSprite[] _readyLights;

		[SerializeField]
		private TextMesh[] _nameCells;

		private int _textIndex;

		public GameObject EnterCode
		{
			get
			{
				return _enterCode;
			}
		}

		public GameObject InstructionCode
		{
			get
			{
				return _instructionCode;
			}
		}

		public event Action OnCloseEvent
		{
			add
			{
				Action action = OnCloseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCloseEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnCloseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCloseEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnInstructionEvent
		{
			add
			{
				Action action = OnInstructionEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnInstructionEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnInstructionEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnInstructionEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<string> OnStartPrivateGameEvent
		{
			add
			{
				Action<string> action = OnStartPrivateGameEvent__BackingField;
				Action<string> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnStartPrivateGameEvent__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<string> action = OnStartPrivateGameEvent__BackingField;
				Action<string> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnStartPrivateGameEvent__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			gameObject.SetActive(false);
		}

		private void OnBtnCloseClick()
		{
			if (OnCloseEvent__BackingField != null)
			{
				OnCloseEvent__BackingField();
			}
		}

		private void OnBtnInstructionClick()
		{
			if (OnInstructionEvent__BackingField != null)
			{
				OnInstructionEvent__BackingField();
			}
		}

		private void OnBtnStartPrivateGameClick()
		{
			string text = string.Empty;
			for (int i = 0; i < _nameCells.Length; i++)
			{
				text += _nameCells[i].text;
			}
			if (OnStartPrivateGameEvent__BackingField != null)
			{
				OnStartPrivateGameEvent__BackingField(text);
			}
		}

		public override void Close()
		{
			if (OnCloseEvent__BackingField != null)
			{
				OnCloseEvent__BackingField();
			}
			OnCloseEvent__BackingField = null;
		}

		private void OnBtn0Click()
		{
			if (_textIndex < _nameCells.Length)
			{
				_nameCells[_textIndex++].text = "0";
			}
			CheckReady();
		}

		private void OnBtn1Click()
		{
			if (_textIndex < _nameCells.Length)
			{
				_nameCells[_textIndex++].text = "1";
			}
			CheckReady();
		}

		private void OnBtn2Click()
		{
			if (_textIndex < _nameCells.Length)
			{
				_nameCells[_textIndex++].text = "2";
			}
			CheckReady();
		}

		private void OnBtn3Click()
		{
			if (_textIndex < _nameCells.Length)
			{
				_nameCells[_textIndex++].text = "3";
			}
			CheckReady();
		}

		private void OnBtn4Click()
		{
			if (_textIndex < _nameCells.Length)
			{
				_nameCells[_textIndex++].text = "4";
			}
			CheckReady();
		}

		private void OnBtn5Click()
		{
			if (_textIndex < _nameCells.Length)
			{
				_nameCells[_textIndex++].text = "5";
			}
			CheckReady();
		}

		private void OnBtn6Click()
		{
			if (_textIndex < _nameCells.Length)
			{
				_nameCells[_textIndex++].text = "6";
			}
			CheckReady();
		}

		private void OnBtn7Click()
		{
			if (_textIndex < _nameCells.Length)
			{
				_nameCells[_textIndex++].text = "7";
			}
			CheckReady();
		}

		private void OnBtn8Click()
		{
			if (_textIndex < _nameCells.Length)
			{
				_nameCells[_textIndex++].text = "8";
			}
			CheckReady();
		}

		private void OnBtn9Click()
		{
			if (_textIndex < _nameCells.Length)
			{
				_nameCells[_textIndex++].text = "9";
			}
			CheckReady();
		}

		private void OnBtnClearClick()
		{
			if (_textIndex > 0)
			{
				_nameCells[_textIndex-- - 1].text = "_";
			}
			_btnReady.SetActive(false);
			for (int i = 0; i < _readyLights.Length; i++)
			{
				_readyLights[i].gameObject.SetActive(false);
			}
			_btnNotReady.SetActive(true);
		}

		private void CheckReady()
		{
			if (_textIndex == _nameCells.Length)
			{
				_btnReady.SetActive(true);
				for (int i = 0; i < _readyLights.Length; i++)
				{
					_readyLights[i].gameObject.SetActive(true);
				}
				_btnNotReady.SetActive(false);
			}
		}

		private void OnBtnRndClick()
		{
		}
	}
}
