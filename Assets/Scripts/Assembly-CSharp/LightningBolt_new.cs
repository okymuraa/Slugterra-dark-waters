using UnityEngine;

public class LightningBolt_new : MonoBehaviour
{
	public Transform target;

	public Vector3 offsetTarget = Vector3.zero;

	public int zigs = 100;

	public float speed = 1f;

	public float scale = 1f;

	public Light startLight;

	public Light endLight;

	private Perlin noise;

	private float oneOverZigs;

	private Particle[] particles;

	private LineRenderer _particle;

	private Transform[] _point;

	private Vector3 position;

	private Vector3 offset;

	private void Start()
	{
		oneOverZigs = 1f / (float)zigs;
		_particle = GetComponent<LineRenderer>();
		_particle.SetVertexCount(zigs);
		for (int i = 0; i < zigs; i++)
		{
			_particle.SetPosition(i, transform.position);
		}
	}

	private void Update()
	{
		if (noise == null)
		{
			noise = new Perlin();
		}
		float num = Time.time * speed * 0.1365143f;
		float num2 = Time.time * speed * 1.21688f;
		float num3 = Time.time * speed * 2.5564f;
		for (int i = 0; i < zigs; i++)
		{
			position = Vector3.Lerp(transform.position, target.position + offsetTarget, oneOverZigs * (float)i);
			offset = new Vector3(noise.Noise(num + position.x, num + position.y, num + position.z), noise.Noise(num2 + position.x, num2 + position.y, num2 + position.z), noise.Noise(num3 + position.x, num3 + position.y, num3 + position.z));
			position += offset * scale * ((float)i * oneOverZigs);
			_particle.SetPosition(i, position);
		}
	}
}
