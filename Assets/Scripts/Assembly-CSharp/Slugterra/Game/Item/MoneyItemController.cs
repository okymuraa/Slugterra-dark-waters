using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Domain.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Item
{
	public class MoneyItemController : ItemController
	{
		[SerializeField]
		private int _itemAmount = 1;

		[SerializeField]
		public GameObject _EventObject;

		protected override void OnPickUp(Unit unit)
		{
			GameObject gameObject = Object.Instantiate(_EventObject, BaseTransform.position, BaseTransform.rotation);
			TextMesh textMesh = gameObject.GetComponent<ItemShowAmountEvent>()._TextMesh;
			if (textMesh != null)
			{
				textMesh.text = string.Empty;
				textMesh.text = "+" + _itemAmount;
			}
			else
			{
				Debug.Log("Text mesh not found on pickup event, check prefab!");
			}
			MonoSingleton<AppController>.Instance.Model.Wallet.Gold += _itemAmount;
		}
	}
}
