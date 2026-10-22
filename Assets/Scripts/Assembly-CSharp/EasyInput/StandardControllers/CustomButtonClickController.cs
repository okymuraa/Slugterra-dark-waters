using System;
using EasyInput.Core;
using UnityEngine;
using UnityEngine.Events;

namespace EasyInput.StandardControllers
{
	[AddComponentMenu("EasyInputHelper/Custom Controllers/CustomButtonClickController")]
	public class CustomButtonClickController : MonoBehaviour
	{
		[Serializable]
		public class ButtonHandler : UnityEvent<ButtonClick>
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
				onClick.Invoke(button);
			}
		}

		private void localButtonClickStart(ButtonClick button)
		{
			if (button.button == myButton && (button.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any))
			{
				onClickStart.Invoke(button);
			}
		}

		private void localButtonClickEnd(ButtonClick button)
		{
			if (button.button == myButton && (button.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any))
			{
				onClickEnd.Invoke(button);
			}
		}

		private void localButtonQuickClickEnd(ButtonClick button)
		{
			if (button.button == myButton && (button.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any))
			{
				onQuickClickEnd.Invoke(button);
			}
		}

		private void localButtonDoubleClickEnd(ButtonClick button)
		{
			if (button.button == myButton && (button.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any))
			{
				onDoubleClickEnd.Invoke(button);
			}
		}

		private void localButtonLongClick(ButtonClick button)
		{
			if (button.button == myButton && (button.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any))
			{
				onLongClick.Invoke(button);
			}
		}

		private void localButtonLongClickStart(ButtonClick button)
		{
			if (button.button == myButton && (button.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any))
			{
				onLongClickStart.Invoke(button);
			}
		}

		private void localButtonLongClickEnd(ButtonClick button)
		{
			if (button.button == myButton && (button.player == player || player == EasyInputConstants.PLAYER_NUMBER.Any))
			{
				onLongClickEnd.Invoke(button);
			}
		}
	}
}
