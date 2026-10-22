using System;
using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.Scene;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene.Game;
using UnityEngine;

namespace Slugterra.UI.Survival
{
	public class SurvivalMapItemView : UIView, IObserver
	{
		public Action<Type> OnSurvivalSelectBtnClickEvent;

		[SerializeField]
		private TextMesh _mapName;

		[SerializeField]
		private TextMesh _bestScore;

		[SerializeField]
		private GameObject _lockedObject;

		[SerializeField]
		private tk2dSprite _preview;

		[SerializeField]
		private GameObject _unlockedObject;

		[SerializeField]
		private GameObject _needDownload;

		[SerializeField]
		private GameObject _readyState;

		[SerializeField]
		private GameObject _notReadyState;

		private LevelItem _levelItem;

		private int _challengeCount;

		private string leaderBoardID = string.Empty;

		private bool startLoading;

		private int score;

		[SerializeField]
		private tk2dButton[] _curButton;

		public tk2dButton CurButton(int Index)
		{
			return _curButton[Index];
		}

		public bool PlayLevel()
		{
			if (score > 0)
			{
				return true;
			}
			return false;
		}

		private void OnEnable()
		{
			MonoSingleton<AppController>.Instance.Model.MissionSetting.AddObserver(this);
		}

		private void OnDisable()
		{
			if (MonoSingleton<AppController>.IsAlive)
			{
				MonoSingleton<AppController>.Instance.Model.MissionSetting.RemoveObserver(this);
			}
		}

		protected override void OnStart()
		{
			DownloadManager instance = DonDestroyMonoSingleton<DownloadManager>.Instance;
			instance.finishedBundleLoading = (Action<string>)Delegate.Combine(instance.finishedBundleLoading, new Action<string>(BundleDownloaded));
		}

		protected override void OnReleaseResource()
		{
			if (DonDestroyMonoSingleton<DownloadManager>.IsAlive)
			{
				DownloadManager instance = DonDestroyMonoSingleton<DownloadManager>.Instance;
				instance.finishedBundleLoading = (Action<string>)Delegate.Remove(instance.finishedBundleLoading, new Action<string>(BundleDownloaded));
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (model is LevelItem)
			{
				_levelItem = model as LevelItem;
				SurvivalDescriptionAttribute survivalDescriptionAttribute = Attribute.GetCustomAttribute(_levelItem.Scene, typeof(SurvivalDescriptionAttribute)) as SurvivalDescriptionAttribute;
				LeaderBoardIDAttribute leaderBoardIDAttribute = Attribute.GetCustomAttribute(_levelItem.Scene, typeof(LeaderBoardIDAttribute)) as LeaderBoardIDAttribute;
				if (leaderBoardID != null && !string.IsNullOrEmpty(leaderBoardIDAttribute.LeaderBoardId))
				{
					leaderBoardID = leaderBoardIDAttribute.LeaderBoardId;
				}
				if (DonDestroyMonoSingleton<GameServiceContext>.Instance.IsPlayerAuthenticated())
				{
					DonDestroyMonoSingleton<GameServiceContext>.Instance.RetrieveScoresForPlayerId(DonDestroyMonoSingleton<GameServiceContext>.Instance.PlayerIdentifier(), leaderBoardID);
				}
				string key = "survivalBestScore" + leaderBoardID;
				int num = (score = PlayerPrefs.GetInt(key, 0));
				float x = -0.085f * (float)(num.ToString().Length - 1);
				_bestScore.transform.parent.localPosition = new Vector3(x, 0f, 0f);
				_bestScore.text = num.ToString();
				if (survivalDescriptionAttribute != null)
				{
					_mapName.text = MonoSingleton<Localization>.Instance.GetValueByKey(survivalDescriptionAttribute.TitleKey, survivalDescriptionAttribute.TitleKey);
					_preview.SetSprite(survivalDescriptionAttribute.PreviewKey);
				}
				if (_levelItem.Scene == typeof(EastSurvival_1))
				{
					_lockedObject.SetActive(!PlayerPrefs.HasKey(_levelItem.Scene.Name));
				}
				if (_levelItem.Status != LevelStatus.Locked)
				{
				}
			}
		}

		private void OnSelectBtnClick()
		{
			_notReadyState.SetActive(false);
			_readyState.SetActive(true);
			if (OnSurvivalSelectBtnClickEvent != null)
			{
				OnSurvivalSelectBtnClickEvent(_levelItem.Scene);
			}
			DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge = null;
			PlayerPrefs.SetString(_levelItem.Scene.Name, "play");
		}

		private void BundleDownloaded(string bundleName)
		{
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is MissionSetting)
			{
				MissionSetting missionSetting = observable as MissionSetting;
				if (missionSetting.Mission != _levelItem.Scene)
				{
					_readyState.SetActive(false);
					_notReadyState.SetActive(true);
				}
			}
		}
	}
}
