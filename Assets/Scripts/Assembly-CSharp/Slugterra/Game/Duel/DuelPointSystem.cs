using System.Collections.Generic;
using AppsMinistry.Core;
using UnityEngine;

namespace Slugterra.Game.Duel
{
	public class DuelPointSystem : MonoSingleton<DuelPointSystem>
	{
		[SerializeField]
		private DuelMoveType _playerMoveType;

		[SerializeField]
		private DuelMoveType _enemyMoveType;

		private PointManager<PlayerDuelPoint> _playerPointManager;

		private PointManager<EnemyDuelPoint> _enemyPointManager;

		public PointManager<PlayerDuelPoint> PlayerPointManager
		{
			get
			{
				return _playerPointManager;
			}
		}

		public PointManager<EnemyDuelPoint> EnemyPointManager
		{
			get
			{
				return _enemyPointManager;
			}
		}

		protected override void OnCreate()
		{
			PlayerDuelPoint[] collection = Object.FindObjectsOfType<PlayerDuelPoint>();
			List<PlayerDuelPoint> list = new List<PlayerDuelPoint>(collection);
			list.Sort(ComparePlayerPoint);
			if (list.Count == 0)
			{
				Debug.Log("DuelPointSystem: player points is empty");
			}
			_playerPointManager = new PointManager<PlayerDuelPoint>(list, _playerMoveType);
			EnemyDuelPoint[] collection2 = Object.FindObjectsOfType<EnemyDuelPoint>();
			List<EnemyDuelPoint> list2 = new List<EnemyDuelPoint>(collection2);
			list2.Sort(CompareEnemyPoint);
			if (list2.Count == 0)
			{
				Debug.Log("DuelPointSystem: enemy points is empty");
			}
			_enemyPointManager = new PointManager<EnemyDuelPoint>(list2, _enemyMoveType);
		}

		private int ComparePlayerPoint(PlayerDuelPoint point1, PlayerDuelPoint point2)
		{
			int result = 0;
			if (!int.TryParse(point1.name.Replace("PlayerDuelPoint", string.Empty), out result))
			{
				return 0;
			}
			int result2 = 0;
			if (!int.TryParse(point2.name.Replace("PlayerDuelPoint", string.Empty), out result2))
			{
				return 0;
			}
			return ComparePoint(result, result2);
		}

		private int CompareEnemyPoint(EnemyDuelPoint point1, EnemyDuelPoint point2)
		{
			int result = 0;
			if (!int.TryParse(point1.name.Replace("EnemyDuelPoint", string.Empty), out result))
			{
				return 0;
			}
			int result2 = 0;
			if (!int.TryParse(point2.name.Replace("EnemyDuelPoint", string.Empty), out result2))
			{
				return 0;
			}
			return ComparePoint(result, result2);
		}

		private int ComparePoint(int point1, int point2)
		{
			if (point1 == point2)
			{
				return 0;
			}
			if (point1 > point2)
			{
				return 1;
			}
			return -1;
		}
	}
}
