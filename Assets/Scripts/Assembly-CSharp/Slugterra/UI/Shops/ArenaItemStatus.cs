using System;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	[Serializable]
	public struct ArenaItemStatus
	{
		public GameObject ReadyStatus;

		public GameObject LockedStatus;

		public GameObject NewStatus;

		public GameObject EquipStatus;

		public GameObject BackgroundNormal;

		public GameObject BackgroundNew;

		public void SetStatus(ArenaItemViewType status)
		{
			LockedStatus.SetActive(false);
			ReadyStatus.SetActive(false);
			EquipStatus.SetActive(false);
			NewStatus.SetActive(false);
			BackgroundNormal.SetActive(false);
			BackgroundNew.SetActive(false);
			switch (status)
			{
			case ArenaItemViewType.Acquired:
				ReadyStatus.SetActive(true);
				BackgroundNormal.SetActive(true);
				break;
			case ArenaItemViewType.NeedEquip:
				EquipStatus.SetActive(true);
				BackgroundNormal.SetActive(true);
				break;
			case ArenaItemViewType.Locked:
				LockedStatus.SetActive(true);
				BackgroundNormal.SetActive(true);
				break;
			case ArenaItemViewType.New:
				NewStatus.SetActive(true);
				EquipStatus.SetActive(true);
				BackgroundNew.SetActive(true);
				break;
			}
		}
	}
}
