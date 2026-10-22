using AppsMinistry.Scene;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Scene
{
	public class SlugTestSceneController : SceneController
	{
		[SerializeField]
		private HumanoidController _enemyController;

		[SerializeField]
		private Transform _spawnPoint;

		private HumanoidController _humanoidController;

		protected override void OnLoaded()
		{
			base.OnLoaded();
		}

		private void OnGUI()
		{
			_enemyController.Model.Health += 20f;
			float width = 120f;
			float height = 30f;
			float num = 5f;
			for (SlugType slugType = SlugType.Infurnus; slugType < SlugType.Size; slugType++)
			{
				if (GUI.Button(new Rect(5f, num, width, height), slugType.ToString()))
				{
					OnFire(slugType);
				}
				num += 35f;
			}
		}

		private void OnFire(SlugType slugType)
		{
			HitMarker component = _enemyController.GetComponent<HitMarker>();
			Vector3 vector = _enemyController.transform.position + Vector3.up * 0.5f;
			if (component != null)
			{
				vector = component.GetRandomMarker().position;
			}
			Vector3 normalized = (vector - _spawnPoint.position).normalized;
			Quaternion rotation = Quaternion.LookRotation(normalized);
			GameObject slugPrefab = GameFactory.GetSlugPrefab(slugType);
			GameObject gameObject = Object.Instantiate(slugPrefab, _spawnPoint.position, rotation);
			gameObject.layer = LayerMask.NameToLayer("AllySlugLayer");
			SlugController component2 = gameObject.GetComponent<SlugController>();
			component2.SetVelocity(normalized.normalized);
			component2.SetSlug(new Slug(slugType, 3));
			int num = LayerMask.NameToLayer("EnemyLayer");
			int num2 = LayerMask.NameToLayer("NeutralLayer");
			int num3 = LayerMask.NameToLayer("PassiveLayer");
			component2.SetStrategy(new AllySlugStrategy(component2, new int[3] { num, num2, num3 }));
			component2.Parent = _humanoidController;
		}
	}
}
