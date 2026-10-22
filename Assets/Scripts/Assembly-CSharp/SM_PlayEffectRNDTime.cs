using System.Collections;
using AppsMinistry.Core.Sound;
using Slugterra.UI.Game.Camera;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SM_PlayEffectRNDTime : MonoBehaviour
{
	[SerializeField]
	private SoundContainer _effectSound;

	private ParticleSystem particle;

	public float intervalMinTime = 30f;

	public float intervalMaxTime = 60f;

	public float effectWorkTime = 3f;

	public float shakeCameraTime = 3f;

	public float shakeRadius = 0.1f;

	private float timer;

	private float timeOffset;

	private float rndTimer;

	private CameraSmoothFollow camera;

	private void Start()
	{
		timeOffset = Time.time;
		particle = GetComponent<ParticleSystem>();
		camera = Object.FindObjectOfType<CameraSmoothFollow>();
		camera.RadiusShake = shakeRadius;
		if (_effectSound != null)
		{
			_effectSound.RegisterInSoundManager();
		}
		SetTime();
	}

	private void Update()
	{
		timer = Time.time - timeOffset;
		if (timer >= rndTimer)
		{
			particle.Play();
			if (_effectSound != null)
			{
				_effectSound.Play();
			}
			if (camera != null)
			{
				camera.DoShake(shakeCameraTime);
			}
			timeOffset = Time.time;
			SetTime();
			StartCoroutine(StopParticle());
		}
	}

	private IEnumerator StopParticle()
	{
		yield return new WaitForSeconds(effectWorkTime);
		particle.Stop();
	}

	private void SetTime()
	{
		rndTimer = Random.Range(intervalMinTime, intervalMaxTime);
	}
}
