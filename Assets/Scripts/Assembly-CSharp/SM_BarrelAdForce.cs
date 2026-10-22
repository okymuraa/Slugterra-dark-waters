using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SM_BarrelAdForce : MonoBehaviour
{
	[SerializeField]
	private float timeToOffEffects = 4f;

	[SerializeField]
	private float timePauseToAlpha = 2f;

	[SerializeField]
	private float timePauseToDestroy = 2f;

	[SerializeField]
	private bool randomRotate = true;

	[SerializeField]
	private bool isMultiplayer;

	[SerializeField]
	private int[] randomAngle;

	[SerializeField]
	private float forceMin;

	[SerializeField]
	private float forceMax;

	[SerializeField]
	private float torqueMin;

	[SerializeField]
	private float torqueMax;

	[SerializeField]
	private ParticleSystem[] stopPparticles;

	private Renderer rend;

	private Shader shaderAlpha;

	private Rigidbody rBody;

	private void Start()
	{
		rBody = GetComponent<Rigidbody>();
		rend = GetComponent<Renderer>();
		shaderAlpha = Shader.Find("Legacy Shaders/Transparent/Diffuse");
		if (randomRotate)
		{
			transform.rotation = Quaternion.Euler(270 + randomAngle[Random.Range(0, randomAngle.Length)], Random.Range(0, 360), 0f);
		}
		if (isMultiplayer)
		{
			transform.rotation = Quaternion.Euler(270f, 180f, 0f);
			rBody.AddTorque(torqueMax, 0f, torqueMax);
			rBody.AddForce(transform.forward * forceMax);
		}
		else
		{
			rBody.AddTorque(Random.Range(torqueMin, torqueMax), 0f, Random.Range(torqueMin, torqueMax));
			rBody.AddForce(transform.forward * Random.Range(forceMin, forceMax));
		}
		StartCoroutine(Action());
	}

	private IEnumerator Action()
	{
		yield return new WaitForSeconds(timeToOffEffects);
		for (int i = 0; i < stopPparticles.Length; i++)
		{
			if (stopPparticles[i] != null)
			{
				stopPparticles[i].Stop();
			}
		}
		yield return new WaitForSeconds(timePauseToAlpha);
		rend.material.shader = shaderAlpha;
		rend.material.DOFade(0f, 1.5f);
		yield return new WaitForSeconds(timePauseToDestroy);
		Object.Destroy(transform.root.gameObject);
	}
}
