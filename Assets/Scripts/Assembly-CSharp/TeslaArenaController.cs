using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Sound;
using Slugterra.Game.Duel;
using Slugterra.Game.Units.Enemy;
using Slugterra.Game.Units.Player;
using UnityEngine;

public class TeslaArenaController : MonoBehaviour
{
	public AnimationClip idle;

	private float tempSpeed;

	public float Radius;

	public ParticleSystem lightningStrikes;

	public Animation Tesla_1;

	public Animation Tesla_2;

	private LightningBolt_new bolt_1;

	private LightningBolt_new bolt_2;

	private LineRenderer bolt_1_render;

	private LineRenderer bolt_2_render;

	public Transform Bolt_root_1;

	private Vector3 StartPosbolt_1;

	public Transform Bolt_root_2;

	private Vector3 StartPosbolt_2;

	private BossController boss;

	private float boosHit;

	private PlayerController Eli;

	private float EliHit;

	public bool Bolt_1_Use;

	public bool Bolt_2_Use;

	public Transform Bolt_1_point;

	public Transform Bolt_2_point;

	private bool Active_boss;

	private bool Active_eli;

	private bool ActiveWeit_boss;

	private bool ActiveWeit_eli;

	private bool ready;

	public bool soundPlay;

	[SerializeField]
	private SoundContainer _explosionSound;

	public Color StartBolt;

	private Color tempStart;

	public Color EndBolt;

	private Color tempEnd;

	private SoundItem _sound;

	private void Start()
	{
		bolt_1 = Bolt_root_1.GetComponent<LightningBolt_new>();
		bolt_2 = Bolt_root_2.GetComponent<LightningBolt_new>();
		bolt_1_render = Bolt_root_1.GetComponent<LineRenderer>();
		bolt_2_render = Bolt_root_2.GetComponent<LineRenderer>();
		bolt_1.gameObject.SetActive(false);
		bolt_2.gameObject.SetActive(false);
		StartPosbolt_1 = Bolt_root_1.position;
		StartPosbolt_2 = Bolt_root_2.position;
		tempEnd = EndBolt;
		tempStart = StartBolt;
		bolt_1_render.SetColors(tempStart, tempEnd);
		bolt_2_render.SetColors(tempStart, tempEnd);
		lightningStrikes.enableEmission = false;
		ready = true;
		if (_explosionSound != null)
		{
			_explosionSound.RegisterInSoundManager();
		}
	}

	private void OnDisable()
	{
		if (_sound != null)
		{
			_sound.Stop();
		}
	}

	private void Tesla1()
	{
		Bolt_1_Use = true;
		if (!soundPlay)
		{
			soundPlay = true;
			_sound = PlayExplosionSound();
		}
		bolt_1.target = Tesla_2.transform;
		bolt_1.gameObject.SetActive(true);
		tempEnd = EndBolt;
		tempStart = StartBolt;
		bolt_1_render.SetColors(tempStart, tempEnd);
		if (Bolt_2_Use)
		{
			ready = true;
			Tesla_2[idle.name].speed = 5f;
			bolt_2_render.SetColors(tempStart, tempEnd);
		}
		Tesla_1[idle.name].speed = 5f;
		StopAllCoroutines();
		StartCoroutine(GoRespawn(20f));
	}

	private void Tesla2()
	{
		Bolt_2_Use = true;
		if (!soundPlay)
		{
			soundPlay = true;
			_sound = PlayExplosionSound();
		}
		bolt_2.target = Tesla_1.transform;
		bolt_2.gameObject.SetActive(true);
		tempEnd = EndBolt;
		tempStart = StartBolt;
		bolt_2_render.SetColors(tempStart, tempEnd);
		if (Bolt_1_Use)
		{
			ready = true;
			Tesla_1[idle.name].speed = 5f;
			bolt_1_render.SetColors(tempStart, tempEnd);
		}
		Tesla_2[idle.name].speed = 5f;
		StopAllCoroutines();
		StartCoroutine(GoRespawn(20f));
	}

	private IEnumerator GoRespawn(float t)
	{
		yield return new WaitForSeconds(t);
		ready = false;
		tempSpeed = 5f;
		tempEnd = EndBolt;
		tempStart = StartBolt;
		bolt_1.target = Tesla_2.transform;
		bolt_2.target = Tesla_1.transform;
		lightningStrikes.enableEmission = false;
	}

	private void FixedUpdate()
	{
		if (Eli == null)
		{
			if (MonoSingleton<DuelManager>.Instance.PlayerController != null)
			{
				Eli = MonoSingleton<DuelManager>.Instance.PlayerController;
				EliHit = Eli.Model.MaxHealth / 500f;
			}
			return;
		}
		if (boss == null)
		{
			if (MonoSingleton<DuelManager>.Instance.BossTransform != null)
			{
				boss = MonoSingleton<DuelManager>.Instance.BossTransform.GetComponent<BossController>();
				boosHit = boss.Model.MaxHealth / 500f;
			}
			return;
		}
		if (!ready)
		{
			if (tempSpeed >= 1f)
			{
				tempSpeed -= 0.025f;
				tempEnd.a -= 0.008f;
				tempStart.a -= 0.008f;
				if (tempEnd.a < 0f)
				{
					tempEnd.a = 0f;
				}
				if (tempStart.a < 0f)
				{
					tempStart.a = 0f;
				}
				if (Bolt_1_Use)
				{
					bolt_1_render.SetColors(tempStart, tempEnd);
				}
				if (Bolt_2_Use)
				{
					bolt_2_render.SetColors(tempStart, tempEnd);
				}
				if (tempSpeed < 1f)
				{
					tempSpeed = 1f;
					if (Bolt_1_Use)
					{
						Tesla_1.SendMessage("Respawn_NotAnim");
						bolt_1.gameObject.SetActive(false);
						Tesla_1[idle.name].speed = 1f;
						Bolt_1_Use = false;
						bolt_1.target = null;
					}
					soundPlay = false;
					_sound.Stop();
					if (Bolt_2_Use)
					{
						Tesla_2.SendMessage("Respawn_NotAnim");
						bolt_2.gameObject.SetActive(false);
						Tesla_2[idle.name].speed = 1f;
						Bolt_2_Use = false;
						bolt_2.target = null;
					}
					Active_eli = false;
					Active_boss = false;
					ready = true;
				}
			}
			if (Bolt_1_Use)
			{
				Tesla_1[idle.name].speed = tempSpeed;
			}
			if (Bolt_2_Use)
			{
				Tesla_2[idle.name].speed = tempSpeed;
			}
			return;
		}
		if (Bolt_1_Use && Bolt_2_Use)
		{
			if (!Active_boss && !Active_eli)
			{
				if (Vector3.Distance(boss.transform.position, Bolt_1_point.position) < Radius)
				{
					Active_boss = true;
					ActiveWeit_boss = true;
					return;
				}
				if (Vector3.Distance(Eli.transform.position, Bolt_2_point.position) < Radius)
				{
					Active_eli = true;
					ActiveWeit_eli = true;
					return;
				}
			}
			else
			{
				if (Active_boss)
				{
					if (boss.gameObject.activeSelf)
					{
						boss.OnDamage(boosHit, null);
					}
					if (boss.HitMarker.Markers != null && boss.HitMarker.Markers[0] != null)
					{
						lightningStrikes.transform.position = boss.HitMarker.Markers[0].transform.position;
					}
					else
					{
						lightningStrikes.transform.position = boss.transform.position;
					}
					if (Active_boss && Vector3.Distance(boss.transform.position, Bolt_1_point.position) > Radius + Radius / 3f)
					{
						Active_boss = false;
						bolt_1.target = Tesla_2.transform;
						bolt_2.target = Tesla_1.transform;
						lightningStrikes.enableEmission = false;
					}
				}
				if (Active_eli)
				{
					if (Eli.gameObject.activeSelf)
					{
						Eli.OnDamage(EliHit, null);
					}
					if (Eli.HitMarker.Markers != null && boss.HitMarker.Markers[0] != null)
					{
						lightningStrikes.transform.position = Eli.HitMarker.Markers[0].transform.position;
					}
					else
					{
						lightningStrikes.transform.position = Eli.transform.position;
					}
					if (Active_eli && Vector3.Distance(Eli.transform.position, Bolt_2_point.position) > Radius + Radius / 3f)
					{
						Active_eli = false;
						bolt_1.target = Tesla_2.transform;
						bolt_2.target = Tesla_1.transform;
						lightningStrikes.enableEmission = false;
					}
				}
			}
		}
		if (ActiveWeit_boss)
		{
			ActiveWeit_boss = false;
			bolt_1.gameObject.SetActive(true);
			bolt_2.gameObject.SetActive(true);
			bolt_1.target = boss.transform;
			bolt_2.target = boss.transform;
			if (boss.HitMarker.Markers != null && boss.HitMarker.Markers[0] != null)
			{
				lightningStrikes.transform.position = boss.HitMarker.Markers[0].transform.position;
			}
			else
			{
				lightningStrikes.transform.position = boss.transform.position;
			}
			lightningStrikes.enableEmission = true;
		}
		if (ActiveWeit_eli)
		{
			ActiveWeit_eli = false;
			bolt_1.gameObject.SetActive(true);
			bolt_2.gameObject.SetActive(true);
			bolt_1.target = Eli.transform;
			bolt_2.target = Eli.transform;
			if (Eli.HitMarker.Markers != null && boss.HitMarker.Markers[0] != null)
			{
				lightningStrikes.transform.position = Eli.HitMarker.Markers[0].transform.position;
			}
			else
			{
				lightningStrikes.transform.position = Eli.transform.position;
			}
			lightningStrikes.enableEmission = true;
		}
	}

	public SoundItem PlayExplosionSound()
	{
		return (!(_explosionSound != null)) ? null : _explosionSound.Play();
	}
}
