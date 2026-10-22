using AppsMinistry.Core;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class ObjectiveCheckboxView : UIView
	{
		[SerializeField]
		private TextMesh _label;

		[SerializeField]
		private tk2dSlicedSprite _checkbox;

		[SerializeField]
		private GameObject _completeState;

		[SerializeField]
		private GameObject _failedState;

		public TextMesh Label
		{
			get
			{
				return _label;
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			MonoSingleton<Localization>.Instance.ConfigTextMesh(_label);
		}

		public void ApplyModel(ObjectiveCheckboxState state)
		{
			_checkbox.color = GetCheckBoxColor(state);
			switch (state)
			{
			case ObjectiveCheckboxState.Unset:
				_completeState.SetActive(false);
				_failedState.SetActive(false);
				break;
			case ObjectiveCheckboxState.Success:
				_completeState.SetActive(true);
				_failedState.SetActive(false);
				break;
			case ObjectiveCheckboxState.Fail:
				_completeState.SetActive(false);
				_failedState.SetActive(true);
				break;
			}
		}

		protected string GetIconSpriteName(ObjectiveCheckboxState state)
		{
			switch (state)
			{
			case ObjectiveCheckboxState.Success:
				return "icon_yes";
			case ObjectiveCheckboxState.Fail:
				return "icon_no";
			default:
				return string.Empty;
			}
		}

		protected Color GetColor(ObjectiveCheckboxState state)
		{
			switch (state)
			{
			case ObjectiveCheckboxState.Success:
				return new Color32(0, 226, 50, byte.MaxValue);
			case ObjectiveCheckboxState.Fail:
				return new Color32(byte.MaxValue, 0, 30, byte.MaxValue);
			default:
				return new Color32(42, 139, 145, byte.MaxValue);
			}
		}

		protected Color GetCheckBoxColor(ObjectiveCheckboxState state)
		{
			switch (state)
			{
			case ObjectiveCheckboxState.Success:
				return new Color32(0, byte.MaxValue, 47, byte.MaxValue);
			case ObjectiveCheckboxState.Fail:
				return new Color32(239, 0, 15, byte.MaxValue);
			default:
				return new Color32(53, 101, 109, byte.MaxValue);
			}
		}

		public void SetMessage(string message)
		{
			_label.text = message;
		}
	}
}
