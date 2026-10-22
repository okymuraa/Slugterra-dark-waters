using System.Collections;
using Slugterra.Game.Environment;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Enemy;
using UnityEngine;

public class PumpController : MonoBehaviour
{
	public Animation Pump;

	public AnimationClip Load;

	private int s;

	private int f;

	public Transform Arrow;

	public Transform RootRot;

	private Vector3 rot;

	private Vector3 rotStart;

	private Vector3 shake;

	private Vector3 shakeStart;

	private bool _switch;

	public float restart = 3f;

	public float speedUp = 25f;

	public float speedDownMax = 7f;

	private float _speedDown;

	public float speedDownRestart = 15f;

	public float m = 1f;

	[Range(1f, 10f)]
	public int distance;

	public bool Fire;

	public ParticleSystem[] FxFire;

	public AudioSource audioFire;

	private bool audioDown;

	public ArenaInteractiveEnvironmant temp;

	private RaycastHit hit;

	public LayerMask layer;

	private BossController boss;

	private float boosHit;

	private EliController Eli;

	private float EliHit;

	private void Start()
	{
		s = 0;
		shakeStart = RootRot.localEulerAngles;
		for (f = 0; f < FxFire.Length; f++)
		{
			FxFire[f].gameObject.SetActive(false);
		}
		audioFire.gameObject.SetActive(false);
	}

	private void UseFire()
	{
		for (f = 0; f < FxFire.Length; f++)
		{
			if (Physics.SphereCast(FxFire[f].transform.position, 0.85f, FxFire[f].transform.forward, out hit, distance, layer))
			{
				if (boss == null && Eli == null)
				{
					if (hit.collider.GetComponent<HumanoidController>() is BossController)
					{
						boss = hit.collider.GetComponent<BossController>();
						boosHit = boss.Model.MaxHealth / 90f;
					}
					if (hit.collider.GetComponent<HumanoidController>() is EliController)
					{
						Eli = hit.collider.GetComponent<EliController>();
						EliHit = Eli.Model.MaxHealth / 90f;
					}
				}
				else
				{
					if (boss != null)
					{
						boss.OnDamage(boosHit, null);
					}
					if (Eli != null)
					{
						Eli.OnDamage(EliHit, null);
					}
				}
			}
		}
	}

	private void FixedUpdate()
	{
		if (audioDown)
		{
			audioFire.volume -= 0.03334f;
			if (audioFire.volume <= 0f)
			{
				audioFire.volume = 0f;
				audioFire.gameObject.SetActive(false);
				audioDown = false;
			}
		}
		if (Arrow.localEulerAngles.z != 0f)
		{
			shake.z = Random.Range(shakeStart.z - Arrow.localEulerAngles.z * 0.01f, shakeStart.z + Arrow.localEulerAngles.z * 0.01f);
		}
		else if (shake != shakeStart)
		{
			shake = shakeStart;
		}
		RootRot.localEulerAngles = shake;
		if (Fire)
		{
			Arrow.localEulerAngles = Vector3.MoveTowards(Arrow.localEulerAngles, rotStart, Time.deltaTime * speedDownRestart);
			rot.z = Arrow.localEulerAngles.z;
			if (Arrow.localEulerAngles.z < 0.5f)
			{
				Pump.SendMessage("Respawn");
				_switch = true;
				Fire = false;
				for (f = 0; f < FxFire.Length; f++)
				{
					FxFire[f].loop = true;
					FxFire[f].gameObject.SetActive(false);
				}
				s = 0;
			}
			if (rot.z < 240f)
			{
				if (FxFire[0].loop)
				{
					for (f = 0; f < FxFire.Length; f++)
					{
						FxFire[f].loop = false;
						audioDown = true;
					}
				}
			}
			else
			{
				UseFire();
			}
		}
		else if (!_switch)
		{
			rot.z += (float)s * m;
			if (rot.z >= 360f)
			{
				rot.z = 359f;
			}
			Arrow.localEulerAngles = Vector3.MoveTowards(Arrow.localEulerAngles, rot, Time.deltaTime * speedUp);
			if (Arrow.localEulerAngles.z >= 359f)
			{
				StopAllCoroutines();
				Fire = true;
				for (f = 0; f < FxFire.Length; f++)
				{
					FxFire[f].loop = true;
					FxFire[f].gameObject.SetActive(true);
				}
				audioFire.gameObject.SetActive(true);
				audioFire.volume = 1f;
				Pump[Load.name].speed = 1f;
			}
		}
		else if (s == 0)
		{
			Arrow.localEulerAngles = Vector3.MoveTowards(Arrow.localEulerAngles, rotStart, Time.deltaTime * speedUp);
			_speedDown = 0f;
		}
		else
		{
			if (_speedDown < speedDownMax)
			{
				_speedDown += 0.2f;
			}
			Arrow.localEulerAngles = Vector3.MoveTowards(Arrow.localEulerAngles, rotStart, Time.deltaTime * _speedDown);
		}
	}

	private void Add()
	{
		if (!Fire)
		{
			s++;
			Pump[Load.name].speed = s;
			StartCoroutine(RespawnPump(restart));
			_switch = false;
			if (temp != null)
			{
				temp.IsReady = false;
			}
		}
	}

	private IEnumerator RespawnPump(float t)
	{
		yield return new WaitForSeconds(t);
		Pump.SendMessage("Respawn_NotAnim");
		_switch = true;
		Fire = false;
		if (temp != null)
		{
			temp.IsReady = true;
		}
	}
}
