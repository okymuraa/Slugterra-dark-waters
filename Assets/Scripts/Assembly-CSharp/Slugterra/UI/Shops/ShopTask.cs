using Slugterra.Scene;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class ShopTask : MonoBehaviour
	{
		public object ItemValue;

		private void Start()
		{
			SceneLoader.LoadScene<ShopSceneController>();
		}

		public static void ShowItem(object itemValue)
		{
			EquipStoreController equipStoreController = Object.FindObjectOfType<EquipStoreController>();
			if (equipStoreController != null)
			{
				equipStoreController.OpenTabAtItem(itemValue);
				return;
			}
			ShopTask shopTask = new GameObject("ShopTask").AddComponent<ShopTask>();
			Object.DontDestroyOnLoad(shopTask.gameObject);
			shopTask.ItemValue = itemValue;
		}
	}
}
