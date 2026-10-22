using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class EliPlatformView : UIView
	{
		[SerializeField]
		private Transform _eliAnchor;

		private EliShopView _eliShopView;

		public EliShopView Eli
		{
			get
			{
				return _eliShopView;
			}
			set
			{
				SetEli(value);
			}
		}

		private void SetEli(EliShopView eliShopView)
		{
			if (_eliShopView != null)
			{
				Object.DestroyImmediate(_eliShopView.gameObject);
			}
			_eliShopView = eliShopView;
			if (!(_eliShopView == null))
			{
				_eliShopView.transform.parent = _eliAnchor;
				_eliShopView.transform.position = Vector3.zero;
				_eliShopView.transform.localPosition = Vector3.zero;
			}
		}
	}
}
