using System;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Shops;
using Slugterra.UI.Popup;

namespace Slugterra.UI.Shops
{
	public class ArenaItemController : UIController<ArenaItemView>
	{
		private PopupGuideAnnouncementView _popupContainer;

		private EnemyType _modelValue;

		private FoeItem _model;

		private RestoreHealthConfiguration healthRestoreManager;

		private ShopModel _shopModel;

		public EnemyType ModelValue
		{
			get
			{
				return _modelValue;
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			healthRestoreManager = MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
			_shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
		}

		private void OnEnable()
		{
			ArenaItemView view = View;
			view.OnBtnEquipItem = (Action)Delegate.Combine(view.OnBtnEquipItem, new Action(OnBtnEquip));
		}

		private void OnDisable()
		{
			ArenaItemView view = View;
			view.OnBtnEquipItem = (Action)Delegate.Remove(view.OnBtnEquipItem, new Action(OnBtnEquip));
		}

		public void ShowEnemy(EnemyType type, FoeItem item)
		{
			_modelValue = type;
			_model = item;
			View.Item = item;
			View.ShowEnemy(type);
		}

		private void Update()
		{
		}

		private void OnBtnEquip()
		{
			DuelSetting duelSetting = MonoSingleton<AppController>.Instance.Model.DuelSetting;
			if (_model != null && duelSetting.Foe != null)
			{
				duelSetting.Foe = _model;
			}
		}
	}
}
