using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using Slugterra.Game.Duel;
using Slugterra.UI.Utils;
using UnityEngine;

namespace Slugterra.UI.Game.SlugFight
{
	public class SlugFightView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPauseEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnAttackEvent__BackingField;

		[SerializeField]
		private tk2dBaseSprite _playerIcon;

		[SerializeField]
		private tk2dBaseSprite _enemyIcon;

		[SerializeField]
		private ProgressBarController _playerProgressBar;

		[SerializeField]
		private ProgressBarController _enemyProgressBar;

		[SerializeField]
		private TextMesh _countdown;

		[SerializeField]
		private tk2dButton _attackButton;

		private SlugFightState _state;

		public event Action OnPauseEvent
		{
			add
			{
				Action action = OnPauseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPauseEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnPauseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPauseEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnAttackEvent
		{
			add
			{
				Action action = OnAttackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAttackEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnAttackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAttackEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
		}

		private void OnEnable()
		{
			_attackButton.ButtonDownEvent += OnAttackDown;
		}

		private void OnDisable()
		{
			_attackButton.ButtonDownEvent -= OnAttackDown;
		}

		protected override void OnApplyModel(object model)
		{
			if (model is SlugFightState)
			{
				_state = model as SlugFightState;
				SetSlugIcons(_state);
				_playerProgressBar.Progress = 1f;
				_enemyProgressBar.Progress = 1f;
			}
		}

		private void SetSlugIcons(SlugFightState state)
		{
			_playerIcon.SetSprite(SpriteNamesHelper.GetSlugSpriteName(state.PlayerSlug.Model.Type));
			_enemyIcon.SetSprite(SpriteNamesHelper.GetSlugSpriteName(state.EnemySlug.Model.Type));
		}

		private void LateUpdate()
		{
			if (_state != null)
			{
				_playerProgressBar.Progress = _state.PlayerHealthProgress;
				_enemyProgressBar.Progress = _state.EnemyHealthProgress;
				float num = _state.FinishBattleTime - Time.time;
				_countdown.text = string.Format("00:{0:D2}", (int)num);
			}
		}

		private void OnBtnPauseClick()
		{
			if (OnPauseEvent__BackingField != null)
			{
				OnPauseEvent__BackingField();
			}
		}

		private void OnAttackDown(tk2dButton source)
		{
			if (OnAttackEvent__BackingField != null)
			{
				OnAttackEvent__BackingField();
			}
		}
	}
}
