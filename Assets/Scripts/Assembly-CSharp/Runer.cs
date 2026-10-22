using AppsMinistry.Core;
using Slugterra.Game.Duel;
using UnityEngine;

public class Runer : MonoBehaviour
{
	public float speedRotation = 5f;

	public float carSpeed;

	private float timer;

	public GameObject campf;

	public Transform part1;

	public Transform part1rot;

	public Transform part2;

	public Transform part2rot;

	public Animator Fernando;

	private int roll;

	public GameObject DuelSlugCamera;

	private float Rotation;

	private float oldRotation;

	private bool check;

	private Vector3 temp;

	private void Start()
	{
		part2.localPosition = Vector3.zero;
		part1.localPosition = Vector3.zero;
		roll = Random.Range(0, 100);
		if (roll < 50)
		{
			part2rot.localEulerAngles = Vector3.up * 180f;
		}
		else
		{
			part1rot.localEulerAngles = Vector3.up * 180f;
		}
		part2.eulerAngles = Vector3.forward * -10f;
		oldRotation = 10f;
		campf.SetActive(false);
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer > 1f && !(MonoSingleton<DuelManager>.Instance.StateMachine.CurrentState is SlugFightState))
		{
			if (carSpeed < speedRotation)
			{
				if (carSpeed < 2f)
				{
					carSpeed += Time.deltaTime * 0.5f;
				}
				else
				{
					carSpeed += Time.deltaTime;
				}
			}
			transform.eulerAngles += Vector3.forward * Time.smoothDeltaTime * carSpeed;
		}
		if (Fernando != null)
		{
			Fernando.SetFloat("Speed", carSpeed * 10f);
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				Fernando.transform.parent.parent.parent.position += Vector3.forward * 0.1f;
			}
			if (Input.GetKey(KeyCode.RightArrow))
			{
				Fernando.transform.parent.parent.parent.position -= Vector3.forward * 0.1f;
			}
		}
		if (timer > 40f && !campf.activeSelf)
		{
			campf.SetActive(true);
		}
		if (timer > 60f && campf.activeSelf)
		{
			campf.SetActive(false);
			timer = 2f;
		}
		Rotation = transform.eulerAngles.z;
		if (Rotation >= oldRotation)
		{
			if (!check)
			{
				part1.eulerAngles = Vector3.forward * -10f;
				oldRotation = transform.eulerAngles.z + 10f;
				check = true;
			}
			else
			{
				part2.eulerAngles = Vector3.forward * -10f;
				oldRotation = transform.eulerAngles.z + 10f;
				check = false;
			}
			oldRotation = Mathf.RoundToInt(oldRotation);
		}
		if (oldRotation >= 360f && Rotation < 1f)
		{
			oldRotation = 0f;
		}
	}

	private void LateUpdate()
	{
		if (part1.localEulerAngles.z < (float)Mathf.RoundToInt(part1.localEulerAngles.z))
		{
			temp.z = Mathf.RoundToInt(part1.localEulerAngles.z);
			part1.localEulerAngles = temp;
		}
		if (part2.localEulerAngles.z < (float)Mathf.RoundToInt(part2.localEulerAngles.z))
		{
			temp.z = Mathf.RoundToInt(part2.localEulerAngles.z);
			part2.localEulerAngles = temp;
		}
	}
}
