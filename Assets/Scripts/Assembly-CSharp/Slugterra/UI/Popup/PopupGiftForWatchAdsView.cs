using System;
using AppsMinistry.Core;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupGiftForWatchAdsView : PopupView
	{
		[SerializeField]
		private TextMesh _rewardText;

		public Action closeEvent;

		public TextMesh RewardText
		{
			get
			{
				return _rewardText;
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			MonoSingleton<Localization>.Instance.ConfigTextMesh(_rewardText);
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
		}

		public override void Close()
		{
			if (closeEvent != null)
			{
				closeEvent();
			}
		}
	}
}
