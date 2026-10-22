using Slugterra.UI.MapMenu;
using UnityEngine;

namespace AppsMinistry.Core.Input.Window
{
	[RequireComponent(typeof(LevelItemView))]
	public class InputWindowLevelElement : InputWindowElement
	{
		[SerializeField]
		private tk2dButton _button;

		[SerializeField]
		private LevelItemView _levelItemView;

		public override void SetState(bool isSelected)
		{
			if (isSelected)
			{
				_levelItemView.OnBtnLevelClick(_button);
				_levelItemView.MoveCamera();
			}
		}

		public void SetOrder(int orderId)
		{
			_orderID = orderId;
		}
	}
}
