using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Mission;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Enemy;
using Slugterra.Game.Units.Slugs;
using Slugterra.Scene;
using UnityEngine;

public class PreviewSlugs : SlugterraCutScene
{
	[SerializeField]
	private Renderer blackPlane;

	[SerializeField]
	private AnimationCurve blackCurve;

	private float _time;

	private float timeOffset;

	private Animation _cameraAnimation;

	public EliController _eliController;

	[SerializeField]
	private SentinelController[] sentController;

	private Vector3 mobStartTmp0;

	private Vector3 mobStartTmp1;

	private Humanoid _eliModel;

	private SlugType _type = SlugType.Infurnus;

	private bool followSlug = true;

	[SerializeField]
	private GameObject previewCamera;

	[SerializeField]
	private GameObject detector;

	private Vector3 detectorStartPos;

	private Vector3 detectorStartRot;

	private GameObject firedSlug;

	private CrystalidSlugController crystalidSlugController;

	private TazerlingSlugController tazerlingSlugController;

	public float smoothTime = 0.3f;

	private float xVelocity;

	private float yVelocity;

	private float zVelocity;

	private bool timerEnable;

	public float fireRate = 9f;

	private float nextFire;

	[SerializeField]
	private float crystalidThresherDistance = 22f;

	public SlugType Type
	{
		set
		{
			_type = value;
			if (!_eliModel.Slugs.Keys.Contains(_type))
			{
				_eliModel.AddSlug(new Slug(_type, 1));
			}
		}
	}

	protected override void _OnEnable()
	{
		base._OnEnable();
		MonoSingleton<MissionStatisticsController>.Instance.OnEliFireEvent += OnEliFire;
	}

	protected override void _OnDisable()
	{
		base._OnDisable();
		if (MonoSingleton<MissionStatisticsController>.Instance != null)
		{
			MonoSingleton<MissionStatisticsController>.Instance.OnEliFireEvent -= OnEliFire;
		}
	}

	private void SlugChanged(SlugType type)
	{
		Type = type;
	}

	private void Start()
	{
		_eliController.DisableControl(GetType());
		_eliModel = _eliController.Model as Humanoid;
		_cameraAnimation = previewCamera.GetComponent<Animation>();
		detectorStartPos = detector.transform.position;
		detectorStartRot = previewCamera.transform.eulerAngles;
		_eliModel.Slugs.Clear();
		mobStartTmp0 = sentController[0].transform.position;
		mobStartTmp1 = sentController[1].transform.position;
	}

	public void Attack()
	{
		timerEnable = true;
		timeOffset = Time.time;
	}

	private void StartAttackSlug()
	{
		MonoBehaviour.print("RUN ATTACK");
		int num = Mathf.RoundToInt(Random.Range(0, 2));
		if (sentController[num] != null)
		{
			_eliController.SetCanAttack(true);
			_eliController.AttackTarget = sentController[num].transform;
			_eliController.Animator.speed = 2f;
			_eliModel.Clear(Time.time);
			_eliController.OnUseSlug(_type);
		}
	}

	private void OnEliFire(SlugType sltype)
	{
		firedSlug = _eliController.RealisedSlugs[0].gameObject;
		if (_type == SlugType.Crystalid || _type == SlugType.CrystalidGhoul || _type == SlugType.Thresher || _type == SlugType.ThresherGhoul)
		{
			firedSlug.GetComponent<CrystalidSlugController>().Distance[1] = crystalidThresherDistance;
		}
		followSlug = true;
		_cameraAnimation.Play("Center_animation");
		_eliController.SetCanAttack(false);
		_eliController.SetAttackTarget(null);
		_eliController.Animator.speed = 1f;
		EffectStop();
		SentinelController[] array = sentController;
		foreach (SentinelController sentinelController in array)
		{
			sentinelController.Model.Health = sentinelController.Model.MaxHealth;
			sentinelController.gameObject.GetComponentInChildren<ProgressBarController>().Progress = sentinelController.Model.Health / sentinelController.Model.MaxHealth;
		}
	}

	public void StopElliFire()
	{
		MonoBehaviour.print("STOP ATTACK");
		timerEnable = false;
		nextFire = 0f;
		if (firedSlug != null)
		{
			Object.Destroy(firedSlug);
		}
		detector.transform.position = detectorStartPos;
		_cameraAnimation.Stop();
		previewCamera.transform.eulerAngles = detectorStartRot;
		blackPlane.material.color = new Vector4(0f, 0f, 0f, 1f);
		EffectStop();
	}

	private void EffectStop()
	{
		SentinelController[] array = sentController;
		foreach (SentinelController sentinelController in array)
		{
			UnitEffect[] components = sentinelController.GetComponents<UnitEffect>();
			UnitEffect[] array2 = components;
			foreach (UnitEffect unitEffect in array2)
			{
				unitEffect.Finish();
			}
		}
		UnitEffect[] components2 = MonoSingleton<AppController>.Instance.gameObject.GetComponents<UnitEffect>();
		UnitEffect[] array3 = components2;
		foreach (UnitEffect unitEffect2 in array3)
		{
			unitEffect2.Finish();
		}
		UnitEffect[] components3 = _eliController.GetComponents<UnitEffect>();
		UnitEffect[] array4 = components3;
		foreach (UnitEffect unitEffect3 in array4)
		{
			unitEffect3.Finish();
		}
		sentController[0].MyBehavior.ConvertToEnemy();
		sentController[0].MoveTo(mobStartTmp0);
		sentController[0].transform.position = mobStartTmp0 + new Vector3(0f, 0f, 1f);
		sentController[1].MyBehavior.ConvertToEnemy();
		sentController[1].MoveTo(mobStartTmp1);
		sentController[1].transform.position = mobStartTmp1 + new Vector3(0f, 0f, 1f);
	}

	private void Update()
	{
		if (timerEnable)
		{
			_time = Time.time - timeOffset;
			blackPlane.material.color = new Vector4(0f, 0f, 0f, blackCurve.Evaluate(_time));
			if (Time.time > nextFire)
			{
				StartAttackSlug();
				nextFire = Time.time + fireRate;
			}
		}
		if (firedSlug != null && followSlug)
		{
			float x = Mathf.SmoothDamp(detector.transform.position.x, firedSlug.transform.position.x, ref xVelocity, smoothTime);
			float y = Mathf.SmoothDamp(detector.transform.position.y, firedSlug.transform.position.y, ref yVelocity, smoothTime);
			float z = Mathf.SmoothDamp(detector.transform.position.z, firedSlug.transform.position.z + 1f, ref zVelocity, smoothTime);
			detector.transform.position = new Vector3(x, y, z);
		}
	}
}
