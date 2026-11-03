using AppsMinistry.Core;
using Pathfinding;
using Slugterra.Game.Environment;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Game.Units.Player;
using Slugterra.Scene;
using UnityEngine;

namespace Slugterra.Game.Input
{
	public class GameCollisionMap : MonoSingleton<GameCollisionMap>
	{
		private Camera _baseCamera;

		private int _terrainLayer;

		private int _enemyLayer;

		private int _neutralLayer;

		private int _allyConstraintLayer;

		private int _maskForTapRay;

		private int _propLayer;

		public bool isTutorial;

		private Camera BaseCamera
		{
			get
			{
				if (_baseCamera == null)
				{
					OnSceneChanged();
				}
				if (_baseCamera == null)
				{
					_baseCamera = Camera.main;
				}
				if (_baseCamera == null)
				{
					_baseCamera = Object.FindObjectOfType<Camera>();
				}
				return _baseCamera;
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			SceneLoader.OnSceneChangedEvent += OnSceneChanged;
			int num = LayerMask.NameToLayer("UnitsConstraints");
			_maskForTapRay = 1 << num;
			_maskForTapRay = ~_maskForTapRay;
		}

		private void OnSceneChanged()
		{
			_terrainLayer = LayerMask.NameToLayer("Terrain");
			_enemyLayer = LayerMask.NameToLayer("EnemyLayer");
			_neutralLayer = LayerMask.NameToLayer("NeutralLayer");
			_propLayer = LayerMask.NameToLayer("PropLayer");
			GameObject gameObject = GameObject.FindWithTag("MainCamera");
			if (gameObject != null)
			{
				_baseCamera = gameObject.GetComponent<Camera>();
			}
		}

		public bool IsTapOnTerrain(Vector3 mousePosition, out Vector3 terrainPosition)
		{
			terrainPosition = Vector3.zero;
			RaycastHit hitInfo = default(RaycastHit);
			if (BaseCamera != null)
			{
				Ray ray = MousePointToRay(mousePosition);
				if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, _maskForTapRay) && hitInfo.transform.gameObject.layer == _terrainLayer)
				{
					terrainPosition = hitInfo.point;
					return true;
				}
			}
			if (hitInfo.transform != null && hitInfo.transform.gameObject.layer == _propLayer && !isTutorial)
			{
				NNConstraint nNConstraint = new NNConstraint();
				nNConstraint.walkable = true;
				nNConstraint.distanceXZ = true;
				NNInfo nearest = AstarPath.active.GetNearest(hitInfo.point, nNConstraint);
				if (nearest.node.Walkable)
				{
					terrainPosition = nearest.clampedPosition;
				}
				return true;
			}
			return false;
		}

		public bool IsTapOnEnemy(Vector3 mousePosition, out Transform enemy)
		{
			if (BaseCamera != null)
			{
				Ray ray = MousePointToRay(mousePosition);
				int num = LayerMask.NameToLayer("TouchLayer");
				int layerMask = (1 << num) | (1 << _enemyLayer) | (1 << _neutralLayer);
				RaycastHit[] hits = Physics.RaycastAll(ray, float.PositiveInfinity, layerMask);
				if (hits != null && hits.Length > 0)
				{
					// Pass 1: Prioritize living enemies
					foreach (RaycastHit hit in hits)
					{
						Transform t = hit.transform;
						UnitController unit = t.GetComponent<UnitController>() ?? t.GetComponentInParent<UnitController>();
						if (unit != null && !(unit is PlayerController) && !(unit is FollowerController))
						{
							if (unit.Model == null || !unit.Model.IsDead())
							{
								enemy = unit.transform;
								return true;
							}
						}
						Transform parent = (t.gameObject.layer == num) ? t.parent : t;
						if (parent != null && parent.gameObject.layer == _enemyLayer)
						{
							enemy = parent;
							return true;
						}
					}

					// Pass 2: Interactive / destructible environment props (cajas, barrels)
					foreach (RaycastHit hit2 in hits)
					{
						Transform t2 = hit2.transform;
						InteractiveEnvironmentController env = t2.GetComponent<InteractiveEnvironmentController>() ?? t2.GetComponentInParent<InteractiveEnvironmentController>();
						if (env != null)
						{
							enemy = env.transform;
							return true;
						}
						Transform parent2 = (t2.gameObject.layer == num) ? t2.parent : t2;
						if (parent2 != null && parent2.gameObject.layer == _neutralLayer)
						{
							enemy = parent2;
							return true;
						}
					}
				}
			}
			enemy = null;
			return false;
		}

		private Ray MousePointToRay(Vector3 mousePoint)
		{
			return BaseCamera.ScreenPointToRay(new Vector3(mousePoint.x, mousePoint.y, 0f));
		}

		public float GetScreenWidth()
		{
			if (BaseCamera != null)
			{
				return BaseCamera.pixelWidth;
			}
			return 0f;
		}

		protected override void OnReleaseResource()
		{
			SceneLoader.OnSceneChangedEvent -= OnSceneChanged;
		}
	}
}
