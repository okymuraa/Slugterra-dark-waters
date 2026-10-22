using System;
using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using Slugterra.UI.Game;
using Slugterra.UI.Utils;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class EliDashboardView : ItemDashboardView
	{
		private enum ViewState
		{
			Empty = 0,
			Equiped = 1,
			Recover = 2
		}

		[SerializeField]
		private GameObject _assistantFoeView;

		[SerializeField]
		private tk2dSprite _assistantIconEquiped;

		[SerializeField]
		private tk2dSprite _assistantIconRecover;

		[SerializeField]
		private TextMesh _assistantNameEquiped;

		[SerializeField]
		private TextMesh _assistantNameRecover;

		[SerializeField]
		private tk2dButton _assistantEmptyBtn;

		[SerializeField]
		private tk2dButton _assistantBtn;

		[SerializeField]
		private tk2dButton _assistantRecoverBtn;

		[SerializeField]
		private TextMesh _assistantForceRate;

		[SerializeField]
		private TextMesh _assistantRecoveryForceRate;

		private RestoreHealthConfiguration healthRestoreManager
		{
			get
			{
				if (MonoSingleton<GameConfiguration>.Instance != null)
				{
					return MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
				}
				return null;
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			MonoSingleton<Localization>.Instance.ConfigTextMesh(_assistantNameEquiped);
			MonoSingleton<Localization>.Instance.ConfigTextMesh(_assistantNameRecover);
			if (IsArena())
			{
				_title.text = MonoSingleton<Localization>.Instance.GetValueByKey("rivals_section_title");
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (missionSetting != null && SurvivalController.IsSurvival(missionSetting.Mission))
			{
				_assistantFoeView.SetActive(false);
			}
		}

		private void OnEnable()
		{
			healthRestoreManager.OnUpdateValues += OnHealthValuesUpdate;
		}

		private void OnDisable()
		{
			if (healthRestoreManager != null)
			{
				healthRestoreManager.OnUpdateValues -= OnHealthValuesUpdate;
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (model == null)
			{
				return;
			}
			if (model is MissionSetting)
			{
				MissionSetting missionSetting = model as MissionSetting;
				if (!IsArena())
				{
					SetAssistant(missionSetting.Assistant);
				}
			}
			if (model is DuelSetting)
			{
				DuelSetting duelSetting = model as DuelSetting;
				if (IsArena())
				{
					SetFoe(duelSetting.Foe);
				}
			}
		}

		private void SetFoe(FoeItem item)
		{
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			EnemyType enemyType = EnemyType.BossPronto;
			if (item != null)
			{
				enemyType = (EnemyType)Enum.Parse(typeof(EnemyType), item.Type);
			}
			if (item == null || (item != null && enemyType == EnemyType.None))
			{
				SetAssistantViewState(ViewState.Empty);
				return;
			}
			FoeConfig foesConfig = instance.UnitConfigurationManager.GetFoesConfig(enemyType);
			SetAssistantViewState(ViewState.Equiped);
			_assistantNameEquiped.text = Localization.GetValue(foesConfig.title, string.Empty);
			_assistantIconEquiped.SetSprite(SpriteNamesHelper.GetFoeSpriteName(enemyType));
		}

		private void SetAssistant(AssistantItem item)
		{
			if (item == null || (item != null && item.Type == AllyType.None))
			{
				SetAssistantViewState(ViewState.Empty);
				SetColorScheme(DashboardColorScheme.Default);
				return;
			}
			tk2dBaseSprite tk2dBaseSprite2 = _assistantIconEquiped;
			TextMesh textMesh = _assistantNameEquiped;
			if (healthRestoreManager.GetHealthConfiguration(item.Type).IsFullyRestored)
			{
				SetAssistantViewState(ViewState.Equiped);
				SetColorScheme(DashboardColorScheme.Upgrade);
				HumanoidLevels alliesConfigurationLevel = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfigurationLevel(item.Type, item.Level);
				_assistantForceRate.text = alliesConfigurationLevel.forceRate.ToString();
			}
			else
			{
				SetAssistantViewState(ViewState.Recover);
				SetColorScheme(DashboardColorScheme.Recovery);
				tk2dBaseSprite2 = _assistantIconRecover;
				textMesh = _assistantNameRecover;
				HumanoidLevels alliesConfigurationLevel2 = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfigurationLevel(item.Type, item.Level);
				_assistantRecoveryForceRate.text = string.Format("<b><color=#FF001D>0</color></b><b><color=#B1B1B1>/</color></b><b><color=#B1B1B1>{0}</color></b>", ForceRate());
			}
			tk2dBaseSprite2.SetSprite(SpriteNamesHelper.GetAssistantSpriteName(item.Type));
			string key = string.Format("companion_{0}_title", item.Type.ToString().ToLower());
			textMesh.text = MonoSingleton<Localization>.Instance.GetValueByKey(key, item.Type.ToString().ToUpper());
		}

		private void SetAssistantViewState(ViewState state)
		{
			switch (state)
			{
			case ViewState.Empty:
				_assistantEmptyBtn.gameObject.SetActive(true);
				_assistantBtn.gameObject.SetActive(false);
				_assistantRecoverBtn.gameObject.SetActive(false);
				break;
			case ViewState.Equiped:
				_assistantEmptyBtn.gameObject.SetActive(false);
				_assistantBtn.gameObject.SetActive(true);
				_assistantRecoverBtn.gameObject.SetActive(false);
				break;
			case ViewState.Recover:
				_assistantEmptyBtn.gameObject.SetActive(false);
				_assistantBtn.gameObject.SetActive(false);
				_assistantRecoverBtn.gameObject.SetActive(true);
				break;
			}
		}

		private void OnHealthValuesUpdate()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			OnApplyModel(missionSetting);
		}

		private bool IsArena()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			return MonoSingleton<AppController>.Instance.Model.GetLevelItem(missionSetting.Mission).Type == LevelType.Arena;
		}
	}
}
