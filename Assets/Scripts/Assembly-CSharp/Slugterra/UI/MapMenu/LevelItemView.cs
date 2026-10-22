using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using DG.Tweening;
using Slugterra.Domain.Game.UI;
using Slugterra.UI.Game.Camera;
using UnityEngine;

namespace Slugterra.UI.MapMenu
{
	public class LevelItemView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPlayEvent__BackingField;

		[SerializeField]
		private tk2dButton _levelButton;

		public GameObject pathNext;

		[SerializeField]
		private tk2dSprite _levelButtonRound;

		[SerializeField]
		private TextMesh _number;

		[SerializeField]
		private tk2dSprite _levelIconLocked;

		[SerializeField]
		private tk2dSprite _levelIconDuel;

		[SerializeField]
		private tk2dSprite _levelIconDaily;

		[SerializeField]
		private tk2dSprite _levelScreen;

		[SerializeField]
		private GameObject _score;

		[SerializeField]
		private List<tk2dSprite> _stars;

		[SerializeField]
		private Color _numberColorCompleted = new Color32(byte.MaxValue, 179, 0, byte.MaxValue);

		[SerializeField]
		private Color _numberColorDefault = new Color32(0, 207, 237, byte.MaxValue);

		[SerializeField]
		private float _starHighlightAnimationTime = 0.25f;

		[SerializeField]
		private GameObject _readyToDownloadState;

		[SerializeField]
		private float _starHighlightAnimationScale = 0.2f;

		private static MapScenePathView mapScenePathView;

		[SerializeField]
		private MapCamera mapCamera;

		public tk2dButton LevelButton
		{
			get
			{
				return _levelButton;
			}
		}

		public event Action OnPlayEvent
		{
			add
			{
				Action action = OnPlayEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnPlayEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public void OnBtnLevelClick(tk2dButton source)
		{
			if (OnPlayEvent__BackingField != null)
			{
				OnPlayEvent__BackingField();
			}
			Type scene = LevelItemController.SelectedLevel.Model.Scene;
		}

		public void MoveCamera()
		{
			mapCamera.MoveTo(LevelItemController.SelectedLevel.Model);
		}

		private void OnEnable()
		{
			_levelButton.ButtonPressedEvent += OnBtnLevelClick;
			mapCamera = UnityEngine.Object.FindObjectOfType<MapCamera>();
		}

		private void OnDisable()
		{
			_levelButton.ButtonPressedEvent -= OnBtnLevelClick;
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (!(model is LevelItem))
			{
				return;
			}
			LevelItem levelItem = model as LevelItem;
			_number.text = levelItem.Name;
			_levelButtonRound.SetSprite(GetButtonRoundSpriteName(levelItem.Status));
			if (levelItem.Status == LevelStatus.Locked || levelItem.Status == LevelStatus.None)
			{
				_levelButton.enabled = false;
				_levelScreen.gameObject.SetActive(false);
			}
			else
			{
				_levelButton.enabled = true;
				_levelScreen.gameObject.SetActive(true);
				_levelScreen.SetSprite(GetScreenSpriteName(levelItem));
				if (mapScenePathView == null)
				{
					mapScenePathView = UnityEngine.Object.FindObjectOfType<MapScenePathView>();
				}
				mapScenePathView.UpdateView(levelItem);
				UpdateDownloadState(levelItem);
			}
			SetLevelIcon(levelItem);
			if (levelItem.Status == LevelStatus.Locked || levelItem.Status == LevelStatus.None || levelItem.Type == LevelType.Arena)
			{
				_score.SetActive(false);
				return;
			}
			_score.SetActive(true);
			LevelItem item = levelItem;
			for (int i = 0; i < _stars.Count; i++)
			{
				_stars[i].SetSprite(GetStarSpriteName(i, item));
			}
		}

		public void UpdateDownloadState(LevelItem levelItem)
		{
			if (levelItem.Status != LevelStatus.Locked && levelItem.Status != LevelStatus.None)
			{
			}
		}

		public void HighlightStars(LevelItem item)
		{
			TweenCallback tweenCallback = () =>
			{
				LevelItem.Last = -1;
			};
			bool flag = false;
			if (_score.activeInHierarchy)
			{
				for (int num = 0; num < _stars.Count; num++)
				{
					if (GetStarSpriteName(num, item) == "star")
					{
						_stars[num].transform.DOScale(_starHighlightAnimationScale, _starHighlightAnimationTime).SetLoops(2, LoopType.Yoyo).SetDelay(0.4f + (float)num * _starHighlightAnimationTime * 1.2f)
							.SetRelative(true)
							.OnComplete(tweenCallback);
						flag = true;
					}
				}
			}
			if (!flag)
			{
				tweenCallback();
			}
		}

		private void SetLevelIcon(LevelItem levelItem)
		{
			_number.gameObject.SetActive(false);
			_levelIconLocked.gameObject.SetActive(false);
			_levelIconDuel.gameObject.SetActive(false);
			_levelIconDaily.gameObject.SetActive(false);
			if (levelItem.Status == LevelStatus.Locked || levelItem.Status == LevelStatus.None)
			{
				_levelIconLocked.gameObject.SetActive(true);
			}
			else if (levelItem.Type == LevelType.Action)
			{
				if (levelItem.IsDaily())
				{
					_levelIconDaily.gameObject.SetActive(true);
					if (levelItem.Status == LevelStatus.Completed)
					{
						_levelIconDaily.SetSprite("icon_daily_orange");
					}
				}
				else
				{
					_number.gameObject.SetActive(true);
					_number.color = ((levelItem.Status != LevelStatus.Completed) ? _numberColorDefault : _numberColorCompleted);
				}
			}
			else if (levelItem.Type == LevelType.Arena)
			{
				_levelIconDuel.gameObject.SetActive(true);
			}
		}

		private string GetButtonRoundSpriteName(LevelStatus status)
		{
			if (status == LevelStatus.Locked || status == LevelStatus.None)
			{
				return "btn_round_disable";
			}
			return "btn_round_enable";
		}

		private string GetScreenSpriteName(LevelItem levelItem)
		{
			if (levelItem.Type == LevelType.Action)
			{
				if (levelItem.Status == LevelStatus.Completed)
				{
					return "screenround_orange";
				}
				if (levelItem.IsDaily())
				{
					return "screenround_green";
				}
			}
			else if (levelItem.Type == LevelType.Arena)
			{
				return "screenround_cyan";
			}
			return "screenround_blue";
		}

		private string GetStarSpriteName(int index, LevelItem item)
		{
			if (index > item._scores.Count)
			{
				return "star_grey";
			}
			return (item.GetObjectiveStatus(index) != 1) ? "star_grey" : "star";
		}

		private void SelectLevelOnTVOS()
		{
		}
	}
}
