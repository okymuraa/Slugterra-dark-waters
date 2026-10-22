using System.Collections;
using Slugterra.Game.Environment;
using UnityEngine;

public class TrainArenaController : MonoBehaviour
{
	public Transform _Start;

	public Transform _Finish;

	public Transform _Stoped;

	public Transform Train;

	public BoxCollider TouchReplase;

	public GameObject barrels;

	public Transform spawnPointBarels;

	public float Speed = 10f;

	private float _timer;

	private bool _lock;

	private bool _weit;

	private int count;

	private bool use;

	public Transform FloorPoint;

	private GameObject Clone;

	public BoxCollider triger;

	public GameObject Wood;

	public AudioSource trainWhoosh;

	public AudioSource trainStop;

	public AudioSource trainStart;

	private bool switchStart;

	private void Start()
	{
		Train.position = _Start.position;
		Train.gameObject.SetActive(false);
		_timer = Random.Range(8, 16);
		count = Random.Range(0, 3);
		Wood.SetActive(false);
		switchStart = false;
	}

	public void Temp()
	{
		StopAllCoroutines();
		use = true;
		StartCoroutine(GoRespawn(20f));
	}

	private IEnumerator GoRespawn(float t)
	{
		yield return new WaitForSeconds(t);
		_weit = false;
		_timer = 3f;
		count = -1;
		if (use)
		{
			Wood.GetComponent<Animation>().Play("a_destroy");
			use = false;
		}
		TouchReplase.gameObject.layer = 12;
	}

	private void LateUpdate()
	{
		if (_weit && Clone != null && !Clone.activeSelf)
		{
			_lock = false;
			_weit = false;
			_timer = 3f;
			count = -1;
			Wood.GetComponent<Animation>().Play("a_destroy");
			use = false;
			StopCoroutine("GoRespawn");
		}
		if (_weit)
		{
			return;
		}
		_timer -= Time.deltaTime;
		if (_timer <= 0f && !_lock)
		{
			_lock = true;
			count++;
			Train.gameObject.SetActive(true);
			if (count < 3)
			{
				if (switchStart)
				{
					trainStart.Play();
					switchStart = false;
				}
				else
				{
					trainWhoosh.Play();
				}
			}
			else
			{
				trainStop.Play();
				Wood.SetActive(true);
				TouchReplase.gameObject.layer = 27;
				if (Clone == null || (Clone != null && !Clone.activeSelf))
				{
					Object.DestroyImmediate(Clone);
					Clone = Object.Instantiate(barrels, spawnPointBarels.position, spawnPointBarels.rotation);
					Clone.transform.parent = spawnPointBarels.transform;
				}
				Wood.gameObject.GetComponent<InteractiveObjType_2>().Respawn();
				Wood.GetComponent<Animation>().Play("a_stay");
				triger.enabled = true;
				switchStart = true;
			}
		}
		if (count < 3)
		{
			if (_lock)
			{
				Train.position = Vector3.MoveTowards(Train.position, _Finish.position, Time.smoothDeltaTime * Speed);
				if (Vector3.SqrMagnitude(Train.position - _Finish.position) < 0.2f)
				{
					_lock = false;
					Wood.SetActive(false);
					_timer = Random.Range(8, 16);
					Train.gameObject.SetActive(false);
					Train.position = _Start.position;
				}
			}
		}
		else if (_lock)
		{
			Train.position = Vector3.MoveTowards(Train.position, _Stoped.position, Time.smoothDeltaTime * Speed * 0.5f);
			if (Vector3.SqrMagnitude(Train.position - _Stoped.position) < 0.2f)
			{
				_lock = false;
				_weit = true;
				StartCoroutine(GoRespawn(30f));
			}
		}
	}
}
