using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using Slugterra.Game.Units.Ally;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class MinimapController : UIController<MinimapView>, IObserver
	{
		private PackItem _model;

		public void OnObjectChanged(object observable)
		{
			if (observable is PackItem)
			{
				View.ApplyModel(observable as PackItem);
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			ApplyModel(MonoSingleton<AppController>.Instance.Model.ShopModel.GetPack(PackType.First));
		}

		protected override void _OnEnable()
		{
			EliController eliController = Object.FindObjectOfType<EliController>();
			if (eliController != null && eliController.InputStrategy != null)
			{
				eliController.DisableControl(GetType());
			}
			View.OnBuyIntelPackEvent += OnBuyIntelPack;
		}

		protected override void _OnDisable()
		{
			EliController eliController = Object.FindObjectOfType<EliController>();
			if (eliController != null && eliController.InputStrategy != null)
			{
				eliController.EnableControl(GetType());
			}
			View.OnBuyIntelPackEvent -= OnBuyIntelPack;
		}

		private void OnBuyIntelPack()
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			if (currentGameLevel != null)
			{
				shopModel.OnBuyPack(_model, currentGameLevel.Name);
			}
		}

		public void ApplyModel(PackItem item)
		{
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
			_model = item;
			_model.AddObserver(this);
			View.ApplyModel(_model);
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
		}
	}
}
