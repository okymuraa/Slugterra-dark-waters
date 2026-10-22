using UnityEngine;

namespace Slugterra.UI.Game
{
	public class ObjectiveCompleteMessageView : MessageViewBase
	{
		public GameObject[] CheckBox_Complete;

		public GameObject[] CheckBox_Fail;

		public const string FrameSpriteComplete = "frame_green";

		public const string FrameSpriteFail = "frame_red";

		public const string IconSpriteComplete = "icon_yes";

		public const string IconSpriteFail = "icon_no";

		public static readonly Color DescColorComplete = new Color32(0, 226, 50, byte.MaxValue);

		public static readonly Color DescColorFail = new Color32(byte.MaxValue, 0, 30, byte.MaxValue);

		[SerializeField]
		private tk2dBaseSprite[] _frames;

		[SerializeField]
		private tk2dBaseSprite _icon;

		private ObjectiveMessageState _state;

		private void OnDisable()
		{
		}

		public void SetState(ObjectiveMessageState state)
		{
			bool flag = state == ObjectiveMessageState.Complete;
			if (flag)
			{
				SetColorDescription(DescColorComplete);
			}
			else
			{
				SetColorDescription(DescColorFail);
			}
			for (int i = 0; i < CheckBox_Complete.Length; i++)
			{
				CheckBox_Complete[i].SetActive(flag);
				CheckBox_Fail[i].SetActive(!flag);
			}
		}
	}
}
