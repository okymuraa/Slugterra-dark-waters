using System;
using EasyInput.Core;
using UnityEngine;
using UnityEngine.Events;

namespace EasyInput.StandardControllers
{
	[AddComponentMenu("EasyInputHelper/Custom Controllers/CustomButtonVoidController")]
	public class CustomButtonVoidController : MonoBehaviour
	{
		[Serializable]
		public class ButtonHandler : UnityEvent
		{
		}

		public EasyInputConstants.PLAYER_NUMBER player;

		public EasyInputConstants.CONTROLLER_BUTTON myButton;

		[SerializeField]
		public ButtonHandler onClickStart;

		[SerializeField]
		public ButtonHandler onClickEnd;

		[SerializeField]
		public ButtonHandler onClick;

		[SerializeField]
		public ButtonHandler onQuickClickEnd;

		[SerializeField]
		public ButtonHandler onDoubleClickEnd;

		[SerializeField]
		public ButtonHandler onLongClickStart;

		[SerializeField]
		public ButtonHandler onLongClickEnd;

		[SerializeField]
		public ButtonHandler onLongClick;

		private void OnEnable()
		{
			EasyInputHelper.On_Click += localButtonClick;
			EasyInputHelper.On_ClickEnd += localButtonClickEnd;
			EasyInputHelper.On_ClickStart += localButtonClickStart;
			EasyInputHelper.On_DoubleClickEnd += localButtonDoubleClickEnd;
			EasyInputHelper.On_LongClick += localButtonLongClick;
			EasyInputHelper.On_LongClickEnd += localButtonLongClickEnd;
			EasyInputHelper.On_LongClickStart += localButtonLongClickStart;
			EasyInputHelper.On_QuickClickEnd += localButtonQuickClickEnd;
		}

		private void OnDestroy()
		{
			EasyInputHelper.On_Click -= localButtonClick;
			EasyInputHelper.On_ClickEnd -= localButtonClickEnd;
			EasyInputHelper.On_ClickStart -= localButtonClickStart;
			EasyInputHelper.On_DoubleClickEnd -= localButtonDoubleClickEnd;
			EasyInputHelper.On_LongClick -= localButtonLongClick;
			EasyInputHelper.On_LongClickEnd -= localButtonLongClickEnd;
			EasyInputHelper.On_LongClickStart -= localButtonLongClickStart;
			EasyInputHelper.On_QuickClickEnd -= localButtonQuickClickEnd;
		}

		private void Update()
		{
		}

		private void localButtonClick(ButtonClick button)
		{
			if (button.button == myButton && (button.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any))
			{
				onClick.Invoke();
			}
		}

		private void localButtonClickStart(ButtonClick button)
		{
			if (button.button == myButton && (button.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any))
			{
				onClickStart.Invoke();
			}
		}

		private void localButtonClickEnd(ButtonClick button)
		{
			if (button.button == myButton && (button.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any))
			{
				onClickEnd.Invoke();
			}
		}

		private void localButtonQuickClickEnd(ButtonClick button)
		{
			if (button.button == myButton && (button.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any))
			{
				onQuickClickEnd.Invoke();
			}
		}

		private void localButtonDoubleClickEnd(ButtonClick button)
		{
			if (button.button == myButton && (button.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any))
			{
				onDoubleClickEnd.Invoke();
			}
		}

		private void localButtonLongClick(ButtonClick button)
		{
			if (button.button == myButton && (button.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any))
			{
				onLongClick.Invoke();
			}
		}

		private void localButtonLongClickStart(ButtonClick button)
		{
			if (button.button == myButton && (button.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any))
			{
				onLongClickStart.Invoke();
			}
		}

		private void localButtonLongClickEnd(ButtonClick button)
		{
			if (button.button == myButton && (button.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any))
			{
				onLongClickEnd.Invoke();
			}
		}
	}
}
