using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene.Game;
using UnityEngine;

namespace Slugterra.UI.MapMenu
{
	public class MapScenePathView : MonoBehaviour
	{
		[SerializeField]
		private Material defaultPathMaterial;

		[SerializeField]
		private Material completePathMatetial;

		[SerializeField]
		private Material unlockDailyMissionPathMatetial;

		[SerializeField]
		private Material unlockPathMaterial;

		[SerializeField]
		private Material unlockArenaPathMatetial;

		[SerializeField]
		private GameObject path_1_1_to_2_1;

		[SerializeField]
		private GameObject path_2_1_to_2_2;

		[SerializeField]
		private GameObject path_2_2_to_Arena;

		[SerializeField]
		private GameObject path_2_2_to_2_3;

		[SerializeField]
		private GameObject path_2_3_to_Arena;

		[SerializeField]
		private GameObject path_2_3_to_2_4;

		[SerializeField]
		private GameObject path_2_4_to_2_5;

		[SerializeField]
		private GameObject path_2_5_to_Arena;

		[SerializeField]
		private GameObject path_2_5_to_3_1;

		[SerializeField]
		private GameObject path_3_1_to_Daily;

		[SerializeField]
		private GameObject path_3_1_to_3_2;

		[SerializeField]
		private GameObject path_3_2_to_Daily;

		[SerializeField]
		private GameObject path_3_2_to_3_3;

		[SerializeField]
		private GameObject path_3_3_to_Daily0;

		[SerializeField]
		private GameObject path_3_3_to_Daily1;

		[SerializeField]
		private GameObject path_3_3_to_Arena;

		[SerializeField]
		private GameObject path_3_3_to_4_1;

		[SerializeField]
		private GameObject path_4_1_to_Daily;

		[SerializeField]
		private GameObject path_4_1_to_4_2;

		[SerializeField]
		private GameObject path_4_2_to_Daily;

		[SerializeField]
		private GameObject path_4_2_to_4_3;

		[SerializeField]
		private GameObject path_4_3_to_Daily0;

		[SerializeField]
		private GameObject path_4_3_to_Daily1;

		[SerializeField]
		private GameObject path_4_3_to_Arena;

		[SerializeField]
		private GameObject path_4_3_to_5_1;

		[SerializeField]
		private GameObject path_5_1_to_Daily0;

		[SerializeField]
		private GameObject path_5_1_to_Daily1;

		[SerializeField]
		private GameObject path_5_1_to_5_2;

		[SerializeField]
		private GameObject path_5_2_to_Daily0;

		[SerializeField]
		private GameObject path_5_2_to_Daily1;

		[SerializeField]
		private GameObject path_5_2_to_6_1;

		[SerializeField]
		private GameObject path_6_1_to_6_2;

		[SerializeField]
		private GameObject path_6_2_to_6_3;

		[SerializeField]
		private GameObject path_6_3_to_Arena;

		[SerializeField]
		private GameObject obj_3_3_daily;

		[SerializeField]
		private GameObject obj_4_3_daily;

		public void UpdateView(LevelItem levelItem)
		{
			bool flag = levelItem.Status == LevelStatus.Unlocked;
			bool flag2 = levelItem.Status == LevelStatus.Completed;
			bool flag3 = flag || flag2;
			switch (levelItem.Name)
			{
			case "1.1":
				path_1_1_to_2_1.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_1_1_to_2_1.GetComponent<MeshRenderer>().material = ((!flag3) ? defaultPathMaterial : unlockPathMaterial);
				break;
			case "2.1":
				path_1_1_to_2_1.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_1_1_to_2_1.GetComponent<MeshRenderer>().material = ((!flag2) ? unlockPathMaterial : completePathMatetial);
				path_2_1_to_2_2.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_2_1_to_2_2.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockPathMaterial);
				break;
			case "2.2":
				path_2_1_to_2_2.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_2_1_to_2_2.GetComponent<MeshRenderer>().material = ((!flag2) ? unlockPathMaterial : completePathMatetial);
				path_2_2_to_2_3.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_2_2_to_2_3.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockPathMaterial);
				path_2_2_to_Arena.GetComponent<AnimateMoveShader>().enabled = flag || flag2;
				path_2_2_to_Arena.GetComponent<MeshRenderer>().material = (flag ? defaultPathMaterial : unlockArenaPathMatetial);
				break;
			case "2.3":
				path_2_2_to_2_3.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_2_2_to_2_3.GetComponent<MeshRenderer>().material = ((!flag2) ? unlockPathMaterial : completePathMatetial);
				path_2_3_to_2_4.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_2_3_to_2_4.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockPathMaterial);
				path_2_3_to_Arena.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_2_3_to_Arena.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockArenaPathMatetial);
				break;
			case "2.4":
				path_2_3_to_2_4.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_2_3_to_2_4.GetComponent<MeshRenderer>().material = ((!flag2) ? unlockPathMaterial : completePathMatetial);
				path_2_4_to_2_5.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_2_4_to_2_5.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockPathMaterial);
				break;
			case "2.5":
				path_2_4_to_2_5.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_2_4_to_2_5.GetComponent<MeshRenderer>().material = ((!flag2) ? unlockPathMaterial : completePathMatetial);
				path_2_5_to_3_1.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_2_5_to_3_1.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockPathMaterial);
				path_2_5_to_Arena.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_2_5_to_Arena.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockArenaPathMatetial);
				break;
			case "3.1":
				path_2_5_to_3_1.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_2_5_to_3_1.GetComponent<MeshRenderer>().material = ((!flag2) ? unlockPathMaterial : completePathMatetial);
				path_3_1_to_3_2.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_3_1_to_3_2.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockPathMaterial);
				break;
			case "3.2":
			{
				path_3_1_to_3_2.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_3_1_to_3_2.GetComponent<MeshRenderer>().material = ((!flag2) ? unlockPathMaterial : completePathMatetial);
				path_3_2_to_3_3.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_3_2_to_3_3.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockPathMaterial);
				bool flag7 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DailyMission_01_Assault_01)).Status == LevelStatus.Completed;
				path_3_1_to_Daily.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_3_1_to_Daily.GetComponent<MeshRenderer>().material = ((!flag7) ? unlockDailyMissionPathMatetial : completePathMatetial);
				break;
			}
			case "3.3":
			{
				path_3_2_to_3_3.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_3_2_to_3_3.GetComponent<MeshRenderer>().material = ((!flag2) ? unlockPathMaterial : completePathMatetial);
				path_3_3_to_4_1.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_3_3_to_4_1.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockPathMaterial);
				path_3_3_to_Arena.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_3_3_to_Arena.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockArenaPathMatetial);
				bool flag10 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DailyMission_02_Defence)).Status == LevelStatus.Completed;
				path_3_2_to_Daily.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_3_2_to_Daily.GetComponent<MeshRenderer>().material = ((!flag10) ? unlockDailyMissionPathMatetial : completePathMatetial);
				break;
			}
			case "4.1":
			{
				path_3_3_to_4_1.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_3_3_to_4_1.GetComponent<MeshRenderer>().material = ((!flag2) ? unlockPathMaterial : completePathMatetial);
				path_4_1_to_4_2.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_4_1_to_4_2.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockPathMaterial);
				bool flag6 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DailyMission_03_Assault_01)).Status == LevelStatus.Completed;
				path_3_3_to_Daily0.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_3_3_to_Daily0.GetComponent<MeshRenderer>().material = ((!flag6) ? unlockDailyMissionPathMatetial : completePathMatetial);
				flag6 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DailyMission_04_Escort)).Status == LevelStatus.Completed;
				path_3_3_to_Daily1.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_3_3_to_Daily1.GetComponent<MeshRenderer>().material = ((!flag6) ? unlockDailyMissionPathMatetial : completePathMatetial);
				break;
			}
			case "4.2":
			{
				path_4_1_to_4_2.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_4_1_to_4_2.GetComponent<MeshRenderer>().material = ((!flag2) ? unlockPathMaterial : completePathMatetial);
				path_4_2_to_4_3.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_4_2_to_4_3.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockPathMaterial);
				bool flag12 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DailyMission_01_Defence_01)).Status == LevelStatus.Completed;
				path_4_1_to_Daily.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_4_1_to_Daily.GetComponent<MeshRenderer>().material = ((!flag12) ? unlockDailyMissionPathMatetial : completePathMatetial);
				break;
			}
			case "4.3":
			{
				path_4_2_to_4_3.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_4_2_to_4_3.GetComponent<MeshRenderer>().material = ((!flag2) ? unlockPathMaterial : completePathMatetial);
				path_4_3_to_5_1.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_4_3_to_5_1.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockPathMaterial);
				path_4_3_to_Arena.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_4_3_to_Arena.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockArenaPathMatetial);
				bool flag4 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DailyMission_02_Escort)).Status == LevelStatus.Completed;
				path_4_2_to_Daily.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_4_2_to_Daily.GetComponent<MeshRenderer>().material = ((!flag4) ? unlockDailyMissionPathMatetial : completePathMatetial);
				break;
			}
			case "5.1":
			{
				path_4_3_to_5_1.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_4_3_to_5_1.GetComponent<MeshRenderer>().material = ((!flag2) ? unlockPathMaterial : completePathMatetial);
				path_5_1_to_5_2.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_5_1_to_5_2.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockPathMaterial);
				bool flag8 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DailyMission_03_Assault_02)).Status == LevelStatus.Completed;
				path_4_3_to_Daily0.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_4_3_to_Daily0.GetComponent<MeshRenderer>().material = ((!flag8) ? unlockDailyMissionPathMatetial : completePathMatetial);
				flag8 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DailyMission_04_Defence)).Status == LevelStatus.Completed;
				path_4_3_to_Daily1.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_4_3_to_Daily1.GetComponent<MeshRenderer>().material = ((!flag8) ? unlockDailyMissionPathMatetial : completePathMatetial);
				break;
			}
			case "5.2":
			{
				path_5_1_to_5_2.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_5_1_to_5_2.GetComponent<MeshRenderer>().material = ((!flag2) ? unlockPathMaterial : completePathMatetial);
				path_5_2_to_6_1.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_5_2_to_6_1.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockPathMaterial);
				bool flag9 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DailyMission_01_Escort_01)).Status == LevelStatus.Completed;
				path_5_1_to_Daily0.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_5_1_to_Daily0.GetComponent<MeshRenderer>().material = ((!flag9) ? unlockDailyMissionPathMatetial : completePathMatetial);
				flag9 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DailyMission_02_Assault)).Status == LevelStatus.Completed;
				path_5_1_to_Daily1.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_5_1_to_Daily1.GetComponent<MeshRenderer>().material = ((!flag9) ? unlockDailyMissionPathMatetial : completePathMatetial);
				break;
			}
			case "6.1":
			{
				path_5_2_to_6_1.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_5_2_to_6_1.GetComponent<MeshRenderer>().material = ((!flag2) ? unlockPathMaterial : completePathMatetial);
				path_6_1_to_6_2.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_6_1_to_6_2.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockPathMaterial);
				bool flag11 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DailyMission_03_Defence)).Status == LevelStatus.Completed;
				path_5_2_to_Daily0.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_5_2_to_Daily0.GetComponent<MeshRenderer>().material = ((!flag11) ? unlockDailyMissionPathMatetial : completePathMatetial);
				flag11 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DailyMission_04_Assault)).Status == LevelStatus.Completed;
				path_5_2_to_Daily1.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_5_2_to_Daily1.GetComponent<MeshRenderer>().material = ((!flag11) ? unlockDailyMissionPathMatetial : completePathMatetial);
				break;
			}
			case "6.2":
			{
				path_6_1_to_6_2.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_6_1_to_6_2.GetComponent<MeshRenderer>().material = ((!flag2) ? unlockPathMaterial : completePathMatetial);
				path_6_2_to_6_3.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_6_2_to_6_3.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockPathMaterial);
				path_6_3_to_Arena.GetComponent<AnimateMoveShader>().enabled = flag2;
				path_6_3_to_Arena.GetComponent<MeshRenderer>().material = ((!flag2) ? defaultPathMaterial : unlockArenaPathMatetial);
				bool flag5 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DailyMission_03_Defence)).Status == LevelStatus.Completed;
				break;
			}
			case "6.3":
				path_6_2_to_6_3.GetComponent<AnimateMoveShader>().enabled = flag3;
				path_6_2_to_6_3.GetComponent<MeshRenderer>().material = ((!flag2) ? unlockPathMaterial : completePathMatetial);
				break;
			}
		}
	}
}
