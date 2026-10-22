using AppsMinistry.Core;
using AppsMinistry.UI;
using DG.Tweening;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class MultiplayerStatisticsView : UIView
	{
		public const float BonusGoldAnimationTime = 5f;

		[SerializeField]
		private TextMesh _totalBonusGold;

		[SerializeField]
		private TextMesh _totalBonusGems;

		[SerializeField]
		private float _starAnimationDuration = 0.35f;

		[SerializeField]
		private float _starAnimationScale = 0.5f;

		[SerializeField]
		private float _scoreAnimationScale = 0.35f;

		[SerializeField]
		private float _scoreAnimationDuration = 0.5f;

		private float acumulatedDelay;

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (model is LevelItem)
			{
				acumulatedDelay = 1.2f;
				LevelItem levelItem = model as LevelItem;
			}
		}

		private void UpdateRewards(float coins, float gem)
		{
			_totalBonusGold.text = "+" + (int)coins/*cast due to constrained. prefix*/;
			_totalBonusGems.text = "+" + (int)gem/*cast due to constrained. prefix*/;
			acumulatedDelay += 0.4f;
			if (coins > 0f && gem > 0f)
			{
				Tween(_totalBonusGold.transform, _totalBonusGems.transform);
			}
		}

		public void GetTotalRewards(LevelItem levelItem, bool won = true)
		{
			float totalCoinsRewards = GetTotalCoinsRewards(levelItem, won);
			float totalGemsRewards = GetTotalGemsRewards(levelItem, won);
			UpdateRewards(totalCoinsRewards, totalGemsRewards);
		}

		private float GetTotalCoinsRewards(LevelItem levelItem, bool won = true)
		{
			float num = 0f;
			LevelProperties levelProperties = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelProperties(levelItem.Scene.Name);
			num = ((!won) ? levelProperties.realMainObjectiveFailGoldReward : (levelProperties.realMainObjectiveGoldReward * (float)MonoSingleton<AppController>.Instance.Model.ShopModel.AvailableMultiplier));
			float num2 = num;
			_totalBonusGold.text = "+" + (int)num2/*cast due to constrained. prefix*/;
			return num2;
		}

		private float GetTotalGemsRewards(LevelItem levelItem, bool won = true)
		{
			float num = 0f;
			LevelProperties levelProperties = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelProperties(levelItem.Scene.Name);
			num = ((!won) ? levelProperties.realMainObjectiveFailGemsReward : (levelProperties.realMainObjectiveGemsReward * (float)MonoSingleton<AppController>.Instance.Model.ShopModel.AvailableMultiplier));
			float num2 = num;
			_totalBonusGems.text = "+" + (int)num2/*cast due to constrained. prefix*/;
			return num2;
		}

		private void Tween(Transform t1, Transform t2)
		{
			t1.DOScale(_scoreAnimationScale, _scoreAnimationDuration).SetLoops(2, LoopType.Yoyo).SetRelative()
				.SetDelay(acumulatedDelay)
				.SetUpdate(true);
			t2.DOScale(_scoreAnimationScale, _scoreAnimationDuration).SetLoops(2, LoopType.Yoyo).SetRelative()
				.SetDelay(acumulatedDelay)
				.SetUpdate(true);
			acumulatedDelay += _scoreAnimationDuration * 1.3f;
		}

		private string GetStarSpriteName(int index, LevelItem item)
		{
			if (index > item._scores.Count)
			{
				return "star_grey";
			}
			int objectiveStatus = item.GetObjectiveStatus(index);
			int newObjectiveStatus = LevelItem.GetNewObjectiveStatus(index);
			if (item.Type == LevelType.Arena)
			{
				return (objectiveStatus != 1) ? "star_grey" : "star";
			}
			return (objectiveStatus != 1 || newObjectiveStatus != 1) ? "star_grey" : "star";
		}
	}
}
