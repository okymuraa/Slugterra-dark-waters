using System;
using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.Domain.Game.UI;

namespace Slugterra.UI.MapMenu
{
	public class LevelItemController : UIController<LevelItemView>, IObserver
	{
		public static Action<LevelItemController> OnSelectlevelItem;

		private static LevelItemController _selectedLevel;

		private static LevelItemController _lastPlayedLevel;

		private LevelItem _model;

		public static LevelItemController SelectedLevel
		{
			get
			{
				return _selectedLevel;
			}
			protected set
			{
				SelectLevelItem(value);
			}
		}

		public static LevelItemController LastPlayedLevel
		{
			get
			{
				return _lastPlayedLevel;
			}
			protected set
			{
				SetLastLevel(value);
			}
		}

		public LevelItem Model
		{
			get
			{
				return _model;
			}
		}

		public void OnObjectChanged(object observable)
		{
			if ((observable as LevelItem).Equals(_model))
			{
				View.ApplyModel(_model);
			}
		}

		protected override void _OnEnable()
		{
			View.OnPlayEvent += OnPlay;
			DownloadManager instance = DonDestroyMonoSingleton<DownloadManager>.Instance;
			instance.finishedBundleLoading = (Action<string>)Delegate.Combine(instance.finishedBundleLoading, new Action<string>(UpdateDownloadIconState));
		}

		protected override void _OnDisable()
		{
			View.OnPlayEvent -= OnPlay;
			if (DonDestroyMonoSingleton<DownloadManager>.IsAlive)
			{
				DownloadManager instance = DonDestroyMonoSingleton<DownloadManager>.Instance;
				instance.finishedBundleLoading = (Action<string>)Delegate.Remove(instance.finishedBundleLoading, new Action<string>(UpdateDownloadIconState));
			}
		}

		private void OnPlay()
		{
			SelectLevelItem(this);
		}

		public void ApplyModel(LevelItem item)
		{
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
			_model = item;
			_model.AddObserver(this);
			if (_model.Order == LevelItem.Current && _model.Type == LevelType.Action && !_model.Name.Contains("daily"))
			{
				SelectLevelItem(this);
			}
			if (_model.Order == LevelItem.Last)
			{
				SetLastLevel(this);
			}
			View.ApplyModel(_model);
		}

		protected override void OnReleaseResource()
		{
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
		}

		public static void SelectLevelItem(LevelItemController item)
		{
			if (item != null && item != _selectedLevel)
			{
				_selectedLevel = item;
				if (OnSelectlevelItem != null)
				{
					OnSelectlevelItem(item);
				}
				LevelItem.NewScores.Clear();
			}
		}

		protected static void SetLastLevel(LevelItemController item)
		{
			if (item != null && item != _lastPlayedLevel)
			{
				_lastPlayedLevel = item;
			}
		}

		public void HighlightStars()
		{
			View.HighlightStars(Model);
		}

		public void UpdateDownloadIconState(string bundleName)
		{
			View.UpdateDownloadState(Model);
		}
	}
}
