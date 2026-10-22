using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Scene;
using DG.Tweening;
using Slugterra.Game.Environment;
using Slugterra.Game.Mission;
using Slugterra.Game.Units.Enemy;
using UnityEngine;

namespace Slugterra.Scene.Game
{
	[SceneName("6_1_Train", "", false, false)]
	public class Train_6_1 : SlugterraSceneController
	{
		public GameObject train;

		public float trainStartPosition = -45f;

		public GameObject stopper0;

		[SerializeField]
		private ObjectiveTarget enemyGroup0_Defeted;

		public GameObject[] mobsGroup0;

		[SerializeField]
		private SphereCollider[] viewAreaGroup0;

		public GameObject crane;

		public GameObject barrel;

		public GameObject explosionSpot;

		public GameObject group1;

		public GameObject[] mobsGroup1;

		[SerializeField]
		private SphereCollider[] viewAreaGroup1;

		public float pandusDownSpeed;

		public Transform pandusTrain1_0;

		public Transform pandusTrain1_1;

		public Transform pandusTrain2_0;

		public Transform pandusTrain2_1;

		public GameObject stopper1;

		private void Start()
		{
			train.transform.localPosition = new Vector3(train.transform.localPosition.x, train.transform.localPosition.y, trainStartPosition);
			GameObject[] array = mobsGroup0;
			foreach (GameObject gameObject in array)
			{
				gameObject.GetComponent<SentinelController>().MyBehavior.CanMove = false;
			}
			GameObject[] array2 = mobsGroup1;
			foreach (GameObject gameObject2 in array2)
			{
				gameObject2.GetComponent<SentinelController>().MyBehavior.CanMove = false;
			}
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQGQ");
			crane.GetComponent<Animation>().Play("Idle_not_lowered");
		}

		public void MoveTrain(float point, float duration, string action)
		{
			train.transform.DOLocalMoveZ(point, duration);
			if (!(action == "none"))
			{
				if (action == "action0")
				{
					StartCoroutine(CraneLowerign());
				}
				if (action == "action1")
				{
					StartCoroutine(Group1_Enable());
				}
			}
		}

		private IEnumerator CraneLowerign()
		{
			for (int i = 0; i < viewAreaGroup0.Length; i++)
			{
				if (viewAreaGroup0[i] != null)
				{
					viewAreaGroup0[i].radius = 30f;
				}
			}
			yield return new WaitForSeconds(4.5f);
			crane.GetComponent<Animation>().Play("lowering");
			StartCoroutine(EnableGroup0());
		}

		private IEnumerator EnableGroup0()
		{
			yield return new WaitForSeconds(1f);
			for (int i = 1; i < mobsGroup0.Length; i++)
			{
				if (mobsGroup0[i] != null)
				{
					mobsGroup0[i].GetComponent<SentinelController>().MyBehavior.CanMove = true;
				}
			}
		}

		private void OnEnable()
		{
			enemyGroup0_Defeted.OnObjectiveCompleteEvent += EnemyGroup0_Defeted;
		}

		private void OnDisable()
		{
			enemyGroup0_Defeted.OnObjectiveCompleteEvent -= EnemyGroup0_Defeted;
		}

		private void EnemyGroup0_Defeted()
		{
			StartCoroutine(MoveTrainBack0());
		}

		private IEnumerator MoveTrainBack0()
		{
			yield return new WaitForSeconds(0.1f);
			MoveTrain(87f, 0.8f, "none");
			StartCoroutine(BarrelExplose());
			crane.GetComponent<Animation>().Play("turn");
			barrel.GetComponent<Animation>().Play("BarrelAnimation");
			stopper0.SetActive(false);
		}

		private IEnumerator BarrelExplose()
		{
			yield return new WaitForSeconds(0.8f);
			barrel.GetComponent<ExplosionEnvironmentController>().OnDamage(10f);
			explosionSpot.SetActive(true);
			StartCoroutine(MoveTrainBack1());
		}

		private IEnumerator MoveTrainBack1()
		{
			yield return new WaitForSeconds(0.5f);
			MoveTrain(50f, 10f, "none");
		}

		private IEnumerator Group1_Enable()
		{
			group1.SetActive(true);
			yield return new WaitForSeconds(0f);
			StartCoroutine(MoveTrainBack2());
			StartCoroutine(viewAreaGroup1_Eneble());
		}

		private IEnumerator viewAreaGroup1_Eneble()
		{
			yield return new WaitForSeconds(1f);
			for (int i = 0; i < viewAreaGroup1.Length; i++)
			{
				if (viewAreaGroup1[i] != null)
				{
					viewAreaGroup1[i].radius = 30f;
				}
			}
		}

		private IEnumerator MoveTrainBack2()
		{
			yield return new WaitForSeconds(10f);
			MoveTrain(156.4f, 3f, "none");
			StartCoroutine(PandusDrop());
		}

		private IEnumerator PandusDrop()
		{
			yield return new WaitForSeconds(2f);
			pandusTrain1_0.DOLocalRotate(new Vector3(90f, 180f, 0f), pandusDownSpeed);
			pandusTrain1_1.DOLocalRotate(new Vector3(90f, 180f, 0f), pandusDownSpeed);
			pandusTrain2_0.DOLocalRotate(new Vector3(90f, 0f, 0f), pandusDownSpeed);
			pandusTrain2_1.DOLocalRotate(new Vector3(90f, 0f, 0f), pandusDownSpeed);
			StartCoroutine(EnablePathfinding_Group1());
		}

		private IEnumerator EnablePathfinding_Group1()
		{
			yield return new WaitForSeconds(2f);
			for (int i = 0; i < mobsGroup1.Length; i++)
			{
				if (mobsGroup1[i] != null)
				{
					mobsGroup1[i].GetComponent<SentinelController>().MyBehavior.CanMove = true;
				}
			}
			stopper1.SetActive(false);
		}
	}
}
