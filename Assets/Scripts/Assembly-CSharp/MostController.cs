using System.Collections;
using AppsMinistry.Core.Sound;
using Slugterra.Game.Duel;
using UnityEngine;

public class MostController : MonoBehaviour
{
	public GameObject Motor_1;

	public GameObject Motor_2;

	[SerializeField]
	private SoundContainer _explosionSound;

	[SerializeField]
	private SoundContainer _explosionSound2;

	public float distans;

	public Transform center;

	public bool ely;

	public bool boss;

	private int countElements;

	public float Timer = 25f;

	private float _timer;

	public bool invert;

	private bool open;

	[SerializeField]
	private GameObject collison;

	private DuelManager temp;

	private void Start()
	{
		countElements = 0;
		_timer = Timer;
		temp = Object.FindObjectOfType<DuelManager>();
		if (temp != null)
		{
			if (Vector3.Distance(temp.BossTransform.position, center.position) < 5f)
			{
				boss = true;
			}
			if (Vector3.Distance(temp.PlayerController.transform.position, center.position) < 5f)
			{
				ely = true;
			}
		}
		if (_explosionSound != null)
		{
			_explosionSound.RegisterInSoundManager();
		}
		if (_explosionSound2 != null)
		{
			_explosionSound2.RegisterInSoundManager();
		}
	}

	public void Temp()
	{
		countElements++;
		if (countElements == 2)
		{
			open = true;
		}
	}

	public void GoMost()
	{
		countElements = 2;
		if (countElements == 2)
		{
			open = true;
		}
	}

	private IEnumerator GoRespawn()
	{
		yield return new WaitForSeconds(2f);
		if ((bool)Motor_1)
		{
			Motor_1.SendMessage("Respawn");
		}
		if ((bool)Motor_2)
		{
			Motor_2.SendMessage("Respawn");
		}
	}

	public SoundItem PlayExplosionSound()
	{
		return (!(_explosionSound != null)) ? null : _explosionSound.Play();
	}

	public SoundItem PlayExplosionSound2()
	{
		return (!(_explosionSound2 != null)) ? null : _explosionSound2.Play();
	}

	private void Update()
	{
		if (countElements != 2)
		{
			return;
		}
		if (open)
		{
			if ((boss && Vector3.Distance(center.position, temp.BossTransform.position) < distans) || (ely && Vector3.Distance(center.position, temp.PlayerController.transform.position) < distans))
			{
				return;
			}
			if (invert)
			{
				if ((bool)Motor_1)
				{
					Motor_1.GetComponent<Animation>().Play("down");
				}
				transform.GetComponent<Animation>().Play("down");
				if ((bool)Motor_2)
				{
					Motor_2.GetComponent<Animation>().Play("down");
				}
				PlayExplosionSound2();
				if (collison != null)
				{
					collison.SetActive(false);
				}
			}
			else
			{
				if ((bool)Motor_1)
				{
					Motor_1.GetComponent<Animation>().Play("up");
				}
				transform.GetComponent<Animation>().Play("up");
				if ((bool)Motor_2)
				{
					Motor_2.GetComponent<Animation>().Play("up");
				}
				PlayExplosionSound();
				if (collison != null)
				{
					collison.SetActive(true);
				}
			}
			open = false;
		}
		_timer -= Time.deltaTime;
		if (!(_timer <= 0f))
		{
			return;
		}
		countElements = 0;
		_timer = Timer;
		if (invert)
		{
			if ((bool)Motor_1)
			{
				Motor_1.GetComponent<Animation>().Play("up");
			}
			transform.GetComponent<Animation>().Play("up");
			if ((bool)Motor_2)
			{
				Motor_2.GetComponent<Animation>().Play("up");
			}
			PlayExplosionSound();
			if (collison != null)
			{
				collison.SetActive(true);
			}
		}
		else
		{
			if ((bool)Motor_1)
			{
				Motor_1.GetComponent<Animation>().Play("down");
			}
			transform.GetComponent<Animation>().Play("down");
			if ((bool)Motor_2)
			{
				Motor_2.GetComponent<Animation>().Play("down");
			}
			PlayExplosionSound2();
			if (collison != null)
			{
				collison.SetActive(false);
			}
		}
		StartCoroutine("GoRespawn");
	}
}
