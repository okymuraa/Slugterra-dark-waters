using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core.Sound;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Units.Player;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class SlugButtonController : UIController<SlugButtonView>, IObserver
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<SlugType> OnUseEvent__BackingField;

		[SerializeField]
		private SoundContainer _callDownEndSound;

		private Slug _model;

		private PlayerController _playerController;

		private SlugButtonState _state = SlugButtonState.Enabled;

		private SlugButtonCounter _counter;

		private GameController _gameController;

		public event Action<SlugType> OnUseEvent
		{
			add
			{
				Action<SlugType> action = OnUseEvent__BackingField;
				Action<SlugType> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUseEvent__BackingField, (Action<SlugType>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<SlugType> action = OnUseEvent__BackingField;
				Action<SlugType> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUseEvent__BackingField, (Action<SlugType>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			PlayerManager.OnAddLocalPlayer((PlayerController p) =>
			{
				_playerController = p;
			});
			if (_callDownEndSound != null)
			{
				_callDownEndSound.RegisterInSoundManager();
			}
			_gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			_gameController.Model.Eli.AddObserver(this);
		}

		protected override void _OnEnable()
		{
			View.OnUseEvent += OnUse;
		}

		protected override void _OnDisable()
		{
			View.OnUseEvent -= OnUse;
		}

		private void OnUse()
		{
			if (_model != null && OnUseEvent__BackingField != null)
			{
				OnUseEvent__BackingField(_model.Type);
			}
		}

		private void OnCallDownEnd()
		{
			PlayCallDownEndSound();
		}

		public void ApplyModel(Slug slug)
		{
			_model = slug;
			View.ApplyModel(_model);
		}

		public void OnObjectChanged(object observable)
		{
			if (!(observable is Humanoid))
			{
				return;
			}
			Humanoid humanoid = observable as Humanoid;
			if (humanoid != _gameController.Model.Eli || _model == null)
			{
				return;
			}
			SlugGroup slugGroup = SlugTypeHelper.GetGroup(_model.Type);
			foreach (SlugType key in humanoid.Slugs.Keys)
			{
				if (key != SlugType.Flopper && slugGroup == SlugTypeHelper.GetGroup(key) && _model != humanoid.GetSlug(key))
				{
					_model = humanoid.GetSlug(key);
					View.ApplyModel(_model);
					break;
				}
			}
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (_model != null && _playerController != null)
			{
				ReloadCommand reloadCommand = _playerController.ReloadCommand;
				Slug nextSlug = _playerController.NextSlug;
				if (_playerController.DisabledSpecialSlug)
				{
					SetButtonState(SlugButtonState.Disabled);
					SetButtonCounter(SlugButtonCounter.None);
				}
				else if (nextSlug != null && nextSlug.Equals(_model))
				{
					SetButtonState(SlugButtonState.Active);
					SetButtonCounter(SlugButtonCounter.None);
				}
				else if (!_model.Available || _model.NextFire > Time.time)
				{
					SetButtonState(SlugButtonState.Disabled);
					SetButtonCounter(SlugButtonCounter.SlugCallDown);
					float calldownProgress = 1f - (_model.NextFire - Time.time) / (_model.CallDown * PlayerManager.LocalPlayer.Model.CurrentAttackSpeedCoeficient());
					View.SetCalldownProgress(calldownProgress);
				}
				else if (reloadCommand != null && reloadCommand.SlugType == _model.Type)
				{
					SetButtonState(SlugButtonState.Disabled);
					SetButtonCounter(SlugButtonCounter.SlugReaload);
					float realoadProgress = 1f - (reloadCommand.ReloadEndTime - Time.time) / reloadCommand.ReloadTime;
					View.SetRealoadProgress(realoadProgress);
				}
				else
				{
					SetButtonState(SlugButtonState.Enabled);
					SetButtonCounter(SlugButtonCounter.None);
				}
			}
			else
			{
				SetButtonState(SlugButtonState.None);
			}
		}

		private void SetButtonState(SlugButtonState state)
		{
			if (_state != state)
			{
				_state = state;
				View.SetButtonState(_state);
			}
		}

		private void SetButtonCounter(SlugButtonCounter counter)
		{
			if (_counter != counter)
			{
				if (_counter == SlugButtonCounter.SlugCallDown)
				{
					OnCallDownEnd();
				}
				_counter = counter;
				View.EnableCounter(counter, 1f);
			}
		}

		private SoundItem PlayCallDownEndSound()
		{
			return (!(_callDownEndSound != null)) ? null : _callDownEndSound.Play();
		}
	}
}
