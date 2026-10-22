using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using DG.Tweening;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class EquipButtonView : UIView
	{
		public static readonly string FrameOrangeSprite = "frame_orange";

		public static readonly string FrameIndigoSprite = "frame_indigo";

		public static readonly string ButtonOrangeSprite = "btn_orange";

		public static readonly string ButtonIndigoSprite = "btn_indigo";

		public static readonly Color CaptionOrangeSprite = new Color32(byte.MaxValue, 179, 0, byte.MaxValue);

		public static readonly Color CaptionIndigoSprite = new Color32(0, 207, 236, byte.MaxValue);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnDoubleTapEvent__BackingField;

		[SerializeField]
		private tk2dBaseSprite[] _frames;

		[SerializeField]
		private tk2dUIToggleButton _button;

		private EquipButtonStyle _style;

		private bool _show;

		private Tweener _transformTweener;

		public bool IsShowed
		{
			get
			{
				return _show;
			}
		}

		public event Action OnDoubleTapEvent
		{
			add
			{
				Action action = OnDoubleTapEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDoubleTapEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnDoubleTapEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDoubleTapEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public void SetCaption(string keyOnCaption, string keyOffCaption)
		{
		}

		public void SetStyle(EquipButtonStyle style)
		{
			if (_style == style)
			{
				return;
			}
			_style = style;
			if (_style == EquipButtonStyle.Orange)
			{
				tk2dBaseSprite[] frames = _frames;
				foreach (tk2dBaseSprite tk2dBaseSprite2 in frames)
				{
					tk2dBaseSprite2.SetSprite(FrameOrangeSprite);
				}
			}
			else if (_style == EquipButtonStyle.Indigo)
			{
				tk2dBaseSprite[] frames2 = _frames;
				foreach (tk2dBaseSprite tk2dBaseSprite3 in frames2)
				{
					tk2dBaseSprite3.SetSprite(FrameIndigoSprite);
				}
			}
		}

		private void OnTap()
		{
			if (OnDoubleTapEvent__BackingField != null && _show)
			{
				OnDoubleTapEvent__BackingField();
			}
		}

		public void Show(bool value)
		{
			if (gameObject.activeSelf != value)
			{
				gameObject.SetActive(value);
			}
		}
	}
}
