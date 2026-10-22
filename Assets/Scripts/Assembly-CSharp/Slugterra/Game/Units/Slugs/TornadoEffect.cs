using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using Slugterra.App;
using Slugterra.Domain.Game.UI;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	[SingletonCommand]
	public class TornadoEffect : UnitEffect
	{
		private HumanoidController _controller;

		private GameObject _effect;

		private float _finishTime;

		private float _dotRadius;

		private float _damagePerSecond;

		private GameObject _dotArea;

		private GameObject _shield;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_controller = args[0] as HumanoidController;
			if (_controller == null)
			{
				Finish();
				return;
			}
			_finishTime = (float)args[1] + Time.time;
			_dotRadius = (float)args[2];
			_damagePerSecond = (float)args[3];
			_controller.Model.AntiNegativeAbilities = true;
			if (!(_controller.Transform.Find("Shield") == null))
			{
				return;
			}
			_shield = new GameObject("Shield");
			_shield.transform.parent = _controller.Transform;
			_shield.transform.localPosition = new Vector3(0f, 1.1f, 0f);
			CapsuleCollider capsuleCollider = _shield.AddComponent<CapsuleCollider>();
			capsuleCollider.height = 2.2f;
			capsuleCollider.radius = _dotRadius;
			capsuleCollider.isTrigger = true;
			_dotArea = new GameObject("DotArea");
			_dotArea.transform.parent = _controller.Transform;
			_dotArea.transform.localPosition = new Vector3(0f, 1.1f, 0f);
			Rigidbody rigidbody = _dotArea.AddComponent<Rigidbody>();
			rigidbody.isKinematic = true;
			CapsuleCollider capsuleCollider2 = _dotArea.AddComponent<CapsuleCollider>();
			DamageOverTimeTrigger damageOverTimeTrigger = _dotArea.AddComponent<DamageOverTimeTrigger>();
			damageOverTimeTrigger.DamagePerSecond = _damagePerSecond;
			if (_controller.GameObject.layer == LayerMask.NameToLayer("AllyLayer"))
			{
				_shield.layer = LayerMask.NameToLayer("AllyLayer");
				_dotArea.layer = LayerMask.NameToLayer("AllySlugLayer");
				damageOverTimeTrigger.AddLayer("EnemyLayer");
			}
			else if (_controller.GameObject.layer == LayerMask.NameToLayer("EnemyLayer"))
			{
				_shield.layer = LayerMask.NameToLayer("EnemyLayer");
				_dotArea.layer = LayerMask.NameToLayer("EnemySlugLayer");
				damageOverTimeTrigger.AddLayer("AllyLayer");
			}
			capsuleCollider2.height = 2.2f;
			capsuleCollider2.radius = _dotRadius;
			capsuleCollider2.isTrigger = true;
			if (args.Length <= 4)
			{
				return;
			}
			GameObject gameObject = args[4] as GameObject;
			if (gameObject != null)
			{
				_effect = Object.Instantiate(gameObject, transform.position, Quaternion.Euler(270f, 0f, 0f));
				_effect.transform.parent = transform;
				if (MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Type == LevelType.Action)
				{
					_effect.transform.localScale = new Vector3(_dotRadius / 1.3f, _dotRadius / 1.3f, 1f);
				}
				if (MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Type == LevelType.Arena)
				{
					ParticleSystem component = _effect.GetComponent<ParticleSystem>();
					ParticleSystem.ColorOverLifetimeModule colorOverLifetime = component.colorOverLifetime;
					Gradient gradient = new Gradient();
					gradient.SetKeys(new GradientColorKey[2]
					{
						new GradientColorKey(new Color(1f, 0.9f, 0.4f), 0f),
						new GradientColorKey(new Color(0.08f, 1f, 0f), 1f)
					}, new GradientAlphaKey[2]
					{
						new GradientAlphaKey(0.7f, 0f),
						new GradientAlphaKey(0f, 0.8f)
					});
					colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
				}
			}
		}

		private void Update()
		{
			if (_controller.Model == null || _controller.Model.IsDead())
			{
				Finish();
			}
			else if (_finishTime <= Time.time)
			{
				Finish();
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			Object.Destroy(_dotArea);
			Object.Destroy(_shield);
			if (_effect != null)
			{
				Object.Destroy(_effect);
			}
			if (_controller != null)
			{
				_controller.Model.AntiNegativeAbilities = false;
			}
		}
	}
}
