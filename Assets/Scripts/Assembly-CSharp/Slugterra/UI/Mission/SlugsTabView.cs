using System;
using System.Diagnostics;
using System.Threading;
using Slugterra.Configuration;
using UnityEngine;

namespace Slugterra.UI.Mission
{
	public class SlugsTabView : MissionTabView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<SlugGroup> OnSlugGroupChangedEvent__BackingField;

		[SerializeField]
		private tk2dButton _attackButton;

		[SerializeField]
		private TextMesh _attackButtonText;

		[SerializeField]
		private tk2dButton _defenceButton;

		[SerializeField]
		private TextMesh _defenceButtonText;

		[SerializeField]
		private tk2dButton _specialButton;

		[SerializeField]
		private TextMesh _specialButtonText;

		private tk2dButton _activeButton;

		private TextMesh _activeText;

		public event Action<SlugGroup> OnSlugGroupChangedEvent
		{
			add
			{
				Action<SlugGroup> action = OnSlugGroupChangedEvent__BackingField;
				Action<SlugGroup> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSlugGroupChangedEvent__BackingField, (Action<SlugGroup>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<SlugGroup> action = OnSlugGroupChangedEvent__BackingField;
				Action<SlugGroup> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSlugGroupChangedEvent__BackingField, (Action<SlugGroup>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private void OnEnable()
		{
			_attackButton.ButtonPressedEvent += OnAttack;
			_defenceButton.ButtonPressedEvent += OnDefence;
			_specialButton.ButtonPressedEvent += OnSpecial;
		}

		private void OnDisable()
		{
			_attackButton.ButtonPressedEvent -= OnAttack;
			_defenceButton.ButtonPressedEvent -= OnDefence;
			_specialButton.ButtonPressedEvent -= OnSpecial;
		}

		private void OnAttack(tk2dButton source)
		{
			if (OnSlugGroupChangedEvent__BackingField != null)
			{
				OnSlugGroupChangedEvent__BackingField(SlugGroup.Attack);
			}
		}

		private void OnDefence(tk2dButton source)
		{
			if (OnSlugGroupChangedEvent__BackingField != null)
			{
				OnSlugGroupChangedEvent__BackingField(SlugGroup.Control);
			}
		}

		private void OnSpecial(tk2dButton source)
		{
			if (OnSlugGroupChangedEvent__BackingField != null)
			{
				OnSlugGroupChangedEvent__BackingField(SlugGroup.Special);
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (_activeButton != null)
			{
				_activeButton.GetComponent<tk2dSlicedSprite>().SetSprite("btn_subcategory_normal");
				_activeText.color = new Color32(0, 239, byte.MaxValue, byte.MaxValue);
			}
			if (model is SlugGroup)
			{
				switch ((SlugGroup)model)
				{
				case SlugGroup.Attack:
					_activeButton = _attackButton;
					_activeText = _attackButtonText;
					break;
				case SlugGroup.Control:
					_activeButton = _defenceButton;
					_activeText = _defenceButtonText;
					break;
				case SlugGroup.Special:
					_activeButton = _specialButton;
					_activeText = _specialButtonText;
					break;
				}
				_activeButton.GetComponent<tk2dSlicedSprite>().SetSprite("btn_subcategory_selected");
				_activeText.color = new Color32(byte.MaxValue, 222, 77, byte.MaxValue);
			}
		}
	}
}
