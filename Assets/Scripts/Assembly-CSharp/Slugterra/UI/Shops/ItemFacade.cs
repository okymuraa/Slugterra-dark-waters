using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public abstract class ItemFacade : MonoBehaviour, IObserver
	{
		[SerializeField]
		private ItemDashboardController _dashboard;

		protected MissionSetting _missionSetting;

		protected DuelSetting _duelSetting;

		protected virtual void OnCreate()
		{
		}

		private void Awake()
		{
			_missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			_duelSetting = MonoSingleton<AppController>.Instance.Model.DuelSetting;
			OnCreate();
		}

		protected virtual void OnMissionSettingChanged(MissionSetting settings)
		{
		}

		protected virtual void OnDuelSettingChanged(DuelSetting settings)
		{
		}

		public virtual void OnObjectChanged(object observable)
		{
			if (observable is MissionSetting)
			{
				OnMissionSettingChanged(observable as MissionSetting);
			}
			if (observable is DuelSetting)
			{
				OnDuelSettingChanged(observable as DuelSetting);
			}
		}

		protected virtual void OnApplyModel(object model)
		{
		}

		public void ApplyModel(object model)
		{
			if (_dashboard != null)
			{
				_dashboard.ApplyModel(model);
			}
			OnApplyModel(model);
		}

		protected virtual void OnEnable()
		{
			_missionSetting.AddObserver(this);
			_duelSetting.AddObserver(this);
			if (_dashboard != null)
			{
				_dashboard.View.gameObject.SetActive(true);
			}
		}

		protected virtual void OnDisable()
		{
			_missionSetting.RemoveObserver(this);
			_duelSetting.RemoveObserver(this);
			if (_dashboard != null)
			{
				_dashboard.View.gameObject.SetActive(false);
			}
		}
	}
}
