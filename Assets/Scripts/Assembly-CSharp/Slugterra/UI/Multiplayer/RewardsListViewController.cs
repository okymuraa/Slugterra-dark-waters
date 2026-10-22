using System.Collections.Generic;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class RewardsListViewController : UIView
	{
		[SerializeField]
		private tk2dUIScrollableArea _scrollArea;

		[SerializeField]
		private RewardItemController _rewardItemPrefab;

		protected override void OnStart()
		{
			base.OnStart();
			List<RewardItemController> list = new List<RewardItemController>();
			list.Add(Object.Instantiate(_rewardItemPrefab));
			list.Add(Object.Instantiate(_rewardItemPrefab));
			list.Add(Object.Instantiate(_rewardItemPrefab));
			list.Add(Object.Instantiate(_rewardItemPrefab));
			AddItems(list);
		}

		private void AddItems(List<RewardItemController> items)
		{
			for (int i = 0; i < items.Count; i++)
			{
				RewardItemController rewardItemController = items[i];
				rewardItemController.transform.parent = _scrollArea.contentContainer.transform;
				rewardItemController.transform.localPosition = new Vector3(0f, (float)i * -1f, 0f);
			}
			_scrollArea.ContentLength = (float)items.Count * 1f;
		}
	}
}
