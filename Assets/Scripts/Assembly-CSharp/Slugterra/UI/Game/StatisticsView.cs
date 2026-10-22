using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.UI;
using DG.Tweening;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Mission;
using Slugterra.Domain.Game.UI;
using Slugterra.Game.Mission;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class StatisticsView : UIView
	{
		public const float BonusGoldAnimationTime = 5f;

		[SerializeField]
		private List<tk2dSprite> _stars;

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
			if (!(model is LevelItem))
			{
				return;
			}
			acumulatedDelay = 1.2f;
			LevelItem levelItem = model as LevelItem;
			int num = 0;
			for (int i = 0; i < _stars.Count; i++)
			{
				_stars[i].SetSprite("star_grey");
				string starSpriteName = GetStarSpriteName(i, levelItem);
				_stars[num].SetSprite(starSpriteName);
				if (starSpriteName == "star")
				{
					_stars[num].transform.DOScale(_starAnimationScale, _starAnimationDuration).SetLoops(2, LoopType.Yoyo).SetRelative()
						.SetDelay(acumulatedDelay)
						.SetUpdate(true);
					acumulatedDelay += _starAnimationDuration * 1.2f;
					num++;
				}
			}
			UpdateRewards(levelItem);
		}

		private void UpdateRewards(LevelItem levelItem)
		{
			float totalCoinsRewards = GetTotalCoinsRewards(levelItem);
			_totalBonusGold.text = "+" + (int)totalCoinsRewards/*cast due to constrained. prefix*/;
			float totalGemsRewards = GetTotalGemsRewards(levelItem);
			_totalBonusGems.text = "+" + (int)totalGemsRewards/*cast due to constrained. prefix*/;
			acumulatedDelay += 0.4f;
			if (totalCoinsRewards > 0f && totalGemsRewards > 0f)
			{
				Tween(_totalBonusGold.transform, _totalBonusGems.transform);
			}
		}

		public float GetTotalCoinsRewards(LevelItem levelItem)
		{
			float num2;
			float num3;
			float num = (num2 = (num3 = 0f));
			ObjectiveTarget[] array = Object.FindObjectsOfType<ObjectiveTarget>();
			Objective objective = null;
			Objective objective2 = null;
			Objective objective3 = null;
			ObjectiveTarget[] array2 = array;
			foreach (ObjectiveTarget objectiveTarget in array2)
			{
				if (objectiveTarget.Model != null)
				{
					switch (objectiveTarget.Model.Type)
					{
					case ObjectiveType.Main:
						objective = objectiveTarget.Model;
						break;
					case ObjectiveType.BonusFirst:
						objective2 = objectiveTarget.Model;
						break;
					case ObjectiveType.BonusSecond:
						objective3 = objectiveTarget.Model;
						break;
					}
				}
			}
			if (objective != null && objective.IsComplete)
			{
				LevelProperties levelProperties = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelProperties(levelItem.Scene.Name);
				if (levelItem.GetObjectiveStatus(0) == 1)
				{
					num = levelProperties.realMainObjectiveGoldReward;
				}
				if (objective2 != null && objective2.IsComplete && levelItem.GetObjectiveStatus(1) == 1)
				{
					num2 = levelProperties.realFirstObjectiveGoldReward;
				}
				if (objective3 != null && objective3.IsComplete && levelItem.GetObjectiveStatus(2) == 1)
				{
					num3 = levelProperties.realSecondaryObjectiveGoldReward;
				}
			}
			return num + (num2 + num3);
		}

		public float GetObjectivesCoinsReward(LevelItem levelItem)
		{
			float num2;
			float num = (num2 = 0f);
			ObjectiveTarget[] array = Object.FindObjectsOfType<ObjectiveTarget>();
			Objective objective = null;
			Objective objective2 = null;
			Objective objective3 = null;
			ObjectiveTarget[] array2 = array;
			foreach (ObjectiveTarget objectiveTarget in array2)
			{
				if (objectiveTarget.Model != null)
				{
					switch (objectiveTarget.Model.Type)
					{
					case ObjectiveType.Main:
						objective = objectiveTarget.Model;
						break;
					case ObjectiveType.BonusFirst:
						objective2 = objectiveTarget.Model;
						break;
					case ObjectiveType.BonusSecond:
						objective3 = objectiveTarget.Model;
						break;
					}
				}
			}
			if (objective != null && objective.IsComplete)
			{
				LevelProperties levelProperties = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelProperties(levelItem.Scene.Name);
				if (levelItem.GetObjectiveStatus(0) == 1)
				{
					float realMainObjectiveGoldReward = levelProperties.realMainObjectiveGoldReward;
				}
				if (objective2 != null && objective2.IsComplete && levelItem.GetObjectiveStatus(1) == 1)
				{
					num = levelProperties.realFirstObjectiveGoldReward;
				}
				if (objective3 != null && objective3.IsComplete && levelItem.GetObjectiveStatus(2) == 1)
				{
					num2 = levelProperties.realSecondaryObjectiveGoldReward;
				}
			}
			return num + num2;
		}

		public float GetTotalGemsRewards(LevelItem levelItem)
		{
			float num2;
			float num3;
			float num = (num2 = (num3 = 0f));
			ObjectiveTarget[] array = Object.FindObjectsOfType<ObjectiveTarget>();
			Objective objective = null;
			Objective objective2 = null;
			Objective objective3 = null;
			ObjectiveTarget[] array2 = array;
			foreach (ObjectiveTarget objectiveTarget in array2)
			{
				if (objectiveTarget.Model != null)
				{
					switch (objectiveTarget.Model.Type)
					{
					case ObjectiveType.Main:
						objective = objectiveTarget.Model;
						break;
					case ObjectiveType.BonusFirst:
						objective2 = objectiveTarget.Model;
						break;
					case ObjectiveType.BonusSecond:
						objective3 = objectiveTarget.Model;
						break;
					}
				}
			}
			if (objective != null && objective.IsComplete)
			{
				LevelProperties levelProperties = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelProperties(levelItem.Scene.Name);
				if (levelItem.GetObjectiveStatus(0) == 1)
				{
					num = levelProperties.realMainObjectiveGemsReward;
				}
				if (objective2 != null && objective2.IsComplete && levelItem.GetObjectiveStatus(1) == 1 && LevelItem.GetNewObjectiveStatus(1) == 1)
				{
					num2 = levelProperties.realFirstObjectiveGemsReward;
				}
				if (objective3 != null && objective3.IsComplete && levelItem.GetObjectiveStatus(2) == 1 && LevelItem.GetNewObjectiveStatus(2) == 1)
				{
					num3 = levelProperties.realSecondaryObjectiveGemsReward;
				}
			}
			return num + (num2 + num3);
		}

		public float GetObjectivesGemsReward(LevelItem levelItem)
		{
			float num2;
			float num = (num2 = 0f);
			ObjectiveTarget[] array = Object.FindObjectsOfType<ObjectiveTarget>();
			Objective objective = null;
			Objective objective2 = null;
			Objective objective3 = null;
			ObjectiveTarget[] array2 = array;
			foreach (ObjectiveTarget objectiveTarget in array2)
			{
				if (objectiveTarget.Model != null)
				{
					switch (objectiveTarget.Model.Type)
					{
					case ObjectiveType.Main:
						objective = objectiveTarget.Model;
						break;
					case ObjectiveType.BonusFirst:
						objective2 = objectiveTarget.Model;
						break;
					case ObjectiveType.BonusSecond:
						objective3 = objectiveTarget.Model;
						break;
					}
				}
			}
			if (objective != null && objective.IsComplete)
			{
				LevelProperties levelProperties = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelProperties(levelItem.Scene.Name);
				if (levelItem.GetObjectiveStatus(0) == 1)
				{
					float realMainObjectiveGemsReward = levelProperties.realMainObjectiveGemsReward;
				}
				if (objective2 != null && objective2.IsComplete && levelItem.GetObjectiveStatus(1) == 1 && LevelItem.GetNewObjectiveStatus(1) == 1)
				{
					num = levelProperties.realFirstObjectiveGemsReward;
				}
				if (objective3 != null && objective3.IsComplete && levelItem.GetObjectiveStatus(2) == 1 && LevelItem.GetNewObjectiveStatus(2) == 1)
				{
					num2 = levelProperties.realSecondaryObjectiveGemsReward;
				}
			}
			return num + num2;
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
