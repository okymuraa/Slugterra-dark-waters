using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using DG.Tweening;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.UI.Utils;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class SlugButtonView : UIView
	{
		public static readonly Vector3 SlugButtonSelectedScale = new Vector3(0.8f, 0.8f, 0.8f);

		public const float ScaleAnimationTime = 0.35f;

		public const float SlugDisabledButtonAlpha = 0.334f;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnUseEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnEndEvent__BackingField;

		[SerializeField]
		private tk2dUIItem _slugButton;

		[SerializeField]
		private tk2dSprite[] _slugIcon;

		[SerializeField]
		private tk2dUIProgressBar _realoadProgress;

		[SerializeField]
		private tk2dUIProgressBar _calldownProgress;

		[SerializeField]
		private GameObject _frame;

		[SerializeField]
		private tk2dSprite[] _buttonElements;

		[SerializeField]
		private GameObject _StateEmpty;

		[SerializeField]
		private GameObject _StateNormal;

		[SerializeField]
		private GameObject _Statecharging;

		[SerializeField]
		private GameObject _StateCooldown;

		[SerializeField]
		private GameObject _StateReady;

		[SerializeField]
		private TextMesh[] _badge;

		private bool _selected;

		private Tweener _transformTweener;

		private bool stopUpdate;

		public event Action OnUseEvent
		{
			add
			{
				Action action = OnUseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUseEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnUseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUseEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnEndEvent
		{
			add
			{
				Action action = OnEndEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnEndEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnEndEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnEndEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			Slug slug = model as Slug;
			if (slug != null && slug.Type != SlugType.None)
			{
				for (int i = 0; i < _slugIcon.Length; i++)
				{
					_slugIcon[i].gameObject.SetActive(true);
					_slugIcon[i].SetSprite(SpriteNamesHelper.GetSlugSpriteName(slug.Type));
				}
				for (int j = 0; j < _badge.Length; j++)
				{
					_badge[j].text = TextUtils.GetSlugLevel(slug.Level);
				}
				if (!stopUpdate)
				{
					ReplaceState(SlugButtonState.Enabled);
					stopUpdate = true;
				}
			}
			else
			{
				for (int k = 0; k < _slugIcon.Length; k++)
				{
					_slugIcon[k].gameObject.SetActive(false);
				}
				for (int l = 0; l < _badge.Length; l++)
				{
					_badge[l].text = string.Empty;
				}
			}
		}

		public void ReplaceState(SlugButtonState state, SlugButtonCounter counter = SlugButtonCounter.None)
		{
			if (counter == SlugButtonCounter.None)
			{
				if (state == SlugButtonState.None)
				{
					_StateEmpty.SetActive(true);
				}
				else
				{
					_StateEmpty.SetActive(false);
				}
				if (state == SlugButtonState.Enabled)
				{
					_StateNormal.SetActive(true);
				}
				else
				{
					_StateNormal.SetActive(false);
				}
				if (state == SlugButtonState.Active)
				{
					_StateReady.SetActive(true);
				}
				else
				{
					_StateReady.SetActive(false);
				}
			}
			else
			{
				_StateEmpty.SetActive(false);
				_StateNormal.SetActive(false);
				_StateReady.SetActive(false);
			}
			if (counter == SlugButtonCounter.SlugReaload)
			{
				_Statecharging.SetActive(true);
			}
			else
			{
				_Statecharging.SetActive(false);
			}
			if (counter == SlugButtonCounter.SlugCallDown)
			{
				_StateCooldown.SetActive(true);
			}
			else
			{
				_StateCooldown.SetActive(false);
			}
		}

		public void SetButtonState(SlugButtonState state)
		{
			switch (state)
			{
			case SlugButtonState.None:
				ReplaceState(state);
				Select(false);
				_slugButton.enabled = false;
				SetButtonTransparency(1f);
				break;
			case SlugButtonState.Enabled:
				ReplaceState(state);
				Select(false);
				_slugButton.enabled = true;
				SetButtonTransparency(1f);
				break;
			case SlugButtonState.Disabled:
				ReplaceState(state);
				Select(false);
				_slugButton.enabled = false;
				SetButtonTransparency(0.334f);
				break;
			case SlugButtonState.Active:
				ReplaceState(state);
				Select(true);
				_slugButton.enabled = false;
				SetButtonTransparency(1f);
				break;
			}
		}

		public void EnableCounter(SlugButtonCounter counter, float startValue)
		{
			switch (counter)
			{
			case SlugButtonCounter.SlugCallDown:
				ReplaceState(SlugButtonState.None, counter);
				SetCalldownProgress(startValue);
				break;
			case SlugButtonCounter.SlugReaload:
				ReplaceState(SlugButtonState.None, counter);
				SetRealoadProgress(startValue);
				break;
			}
		}

		public void SetRealoadProgress(float value)
		{
			_realoadProgress.Value = value;
		}

		public void SetCalldownProgress(float value)
		{
			_calldownProgress.Value = value;
		}

		private void SetButtonTransparency(float value)
		{
			tk2dSprite[] buttonElements = _buttonElements;
			foreach (tk2dSprite tk2dSprite2 in buttonElements)
			{
				Color color = tk2dSprite2.color;
				color.a = value;
				tk2dSprite2.color = color;
			}
		}

		private void Select(bool value)
		{
			if (_selected != value)
			{
				if (_transformTweener != null && !_transformTweener.IsComplete())
				{
					_transformTweener.Kill();
				}
				if (!value && OnEndEvent__BackingField != null)
				{
					OnEndEvent__BackingField();
				}
				_selected = value;
			}
		}

		private void OnBtnUseClick()
		{
			if (OnUseEvent__BackingField != null)
			{
				OnUseEvent__BackingField();
			}
		}
	}
}
