using System.Collections;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Enemy;
using Slugterra.Game.Wave;
using Slugterra.Scene;
using Slugterra.UI.Game;
using UnityEngine;

public class DefenceOfTheObjects : SlugterraCutScene
{
	public enum VictoryCondition
	{
		PlayerDestroy = 0,
		EnemyDestroy = 1
	}

	public float DelayTime = 5f;

	private float _timerPlayer;

	public VictoryCondition victoryCondition = VictoryCondition.EnemyDestroy;

	public Transform EnemyObject;

	public Transform PlayerObject;

	public float EnemyRespawnTime = 5f;

	public float PlayerRespawnTime = 5f;

	public Transform EnemyRespawnPoint;

	public GameObject EnemyHero;

	private GameObject cloneEnemyHero;

	private EnemyController _enemyHero;

	public Transform PlayerRespawnPoint;

	public GameObject PlayerWin;

	public GameObject EnemyWin;

	public GameObject PlayerFinish;

	public GameObject EnemyFinish;

	public Animator PlayerObj;

	public Animator EnemyObj;

	private EliController _eliController;

	public WaveController[] wave;

	private GameController gameController;

	public Animation Gate;

	private bool gateOpen;

	private void Awake()
	{
		if (victoryCondition != VictoryCondition.PlayerDestroy)
		{
			Object.Destroy(EnemyWin);
		}
		else
		{
			Object.Destroy(PlayerWin);
		}
		_timerPlayer = DelayTime;
		gateOpen = true;
	}

	private void Start()
	{
		gameController = Object.FindObjectOfType<GameController>();
		_eliController = Object.FindObjectOfType<EliController>();
		wave = Object.FindObjectsOfType<WaveController>();
	}

	private void FixedUpdate()
	{
		_timerPlayer -= Time.deltaTime;
		if (_timerPlayer <= 0f && gateOpen)
		{
			gateOpen = false;
			Gate.Play("Indastry_MOBBA_Gate_Open");
			_timerPlayer = PlayerRespawnTime;
		}
		if (_enemyHero == null)
		{
			DelayTime -= Time.deltaTime;
			if (DelayTime <= 0f)
			{
				if (cloneEnemyHero == null)
				{
					cloneEnemyHero = Object.Instantiate(EnemyHero, EnemyRespawnPoint.position, Quaternion.identity);
				}
				if (cloneEnemyHero != null)
				{
					if (_enemyHero == null)
					{
						_enemyHero = cloneEnemyHero.transform.GetComponentInChildren<EnemyController>();
					}
					if (_enemyHero != null)
					{
						StartCoroutine(enemyHero(0.1f));
					}
				}
			}
		}
		else if (_enemyHero.AttackTarget == null && Vector3.Distance(_enemyHero.transform.position, _enemyHero.MoveTarget.position) < 1f && PlayerObject.gameObject.activeSelf)
		{
			_enemyHero.MoveTarget.position = PlayerObject.position;
			_enemyHero.OnMove(4f);
		}
		for (int i = 0; i < wave.Length; i++)
		{
			wave[i].ResetCurrentWaveIndex();
		}
		if (_eliController.Model.Health <= 50f)
		{
			_eliController.transform.position = PlayerRespawnPoint.position;
			_eliController.MoveTarget.position = PlayerRespawnPoint.position;
			_eliController.Model.Health = _eliController.Model.MaxHealth;
			if (!gateOpen)
			{
				gateOpen = true;
				Gate.Play("Indastry_MOBBA_Gate_Close");
				Gate["Indastry_MOBBA_Gate_Close"].speed = 1.5f;
				_timerPlayer = PlayerRespawnTime;
			}
		}
		if (!PlayerObject.gameObject.activeSelf)
		{
			_eliController.OnDamage(10000000f, null);
		}
		if (PlayerFinish != null && PlayerFinish.activeSelf)
		{
			PlayerFinish.SetActive(false);
			EnemyObj.speed = 0f;
			StartCoroutine(_part_End(5f));
		}
		if (EnemyFinish != null && EnemyFinish.activeSelf)
		{
			EnemyFinish.SetActive(false);
			PlayerObj.speed = 0f;
			StartCoroutine(_part_End(5f));
		}
	}

	private IEnumerator enemyHero(float t)
	{
		yield return new WaitForSeconds(t);
		_enemyHero.MoveTarget.position = PlayerObject.position;
		_enemyHero.OnMove(4f);
		DelayTime = EnemyRespawnTime;
		cloneEnemyHero = null;
	}

	private IEnumerator _part_End(float t)
	{
		yield return new WaitForSeconds(t);
		gameController.OnFinishGame();
		gameController.PlayCutScene(false);
		gameObject.SetActive(false);
	}
}
