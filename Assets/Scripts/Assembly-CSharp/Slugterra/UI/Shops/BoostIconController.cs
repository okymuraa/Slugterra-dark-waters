using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class BoostIconController : UIController<BoostIconView>, IObserver
	{
		[SerializeField]
		private ConsumableType _type;

		private ConsumableItem _model;

		public void OnObjectChanged(object observable)
		{
			if (observable is ConsumableItem)
			{
				View.ApplyModel(observable as ConsumableItem);
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			_model = MonoSingleton<AppController>.Instance.Model.ShopModel.GetConsumable(_type);
		}

		protected override void _OnEnable()
		{
			if (_model != null)
			{
				_model.AddObserver(this);
				View.ApplyModel(_model);
			}
		}

		protected override void _OnDisable()
		{
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
		}
	}
}
