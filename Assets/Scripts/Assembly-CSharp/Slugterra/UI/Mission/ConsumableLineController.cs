using System.Collections.Generic;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Mission
{
	public class ConsumableLineController : UIController<UIView>
	{
		[SerializeField]
		private GameObject _itemPrefab;

		[SerializeField]
		private int _count = 1;

		[SerializeField]
		private float _itemWidth;

		private List<ConsumableItemController> _items;

		protected override void OnCreate()
		{
			if (!(_itemPrefab == null))
			{
				_items = new List<ConsumableItemController>();
				for (int i = 0; i < _count; i++)
				{
					GameObject gameObject = Object.Instantiate(_itemPrefab);
					gameObject.transform.parent = transform;
					gameObject.transform.localPosition = new Vector3(_itemWidth * ((float)i - (float)_count / 2f + 0.5f), 0f, 0f);
					_items.Add(gameObject.GetComponent<ConsumableItemController>());
				}
			}
		}

		public ConsumableItemController GetItem(int index)
		{
			return _items[index];
		}
	}
}
