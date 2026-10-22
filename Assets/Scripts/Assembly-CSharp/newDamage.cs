using Slugterra.Game;
using Slugterra.Game.Units;
using UnityEngine;

public class newDamage : MonoBehaviour
{
	private Vector3 epicenter = Vector3.zero;

	private float hittingArea;

	private float damage;

	public LayerMask layers;

	public LayerMask damageLayerMask;

	private Collider[] hitColliders;

	private IDestructible controller;

	private Transform unitTransform;

	private int g;

	private int m;

	private Vector3 origin;

	private Vector3 direction;

	private HitMarker hitMarker;

	private Transform marker;

	private float timer;

	private void Start()
	{
		epicenter = transform.position;
		hittingArea = 7f;
		damage = 100f;
		hitColliders = PhysicsFacade.OverlapSphere(epicenter, hittingArea);
		g = hitColliders.Length;
		timer = 0.01f;
	}

	private void FixedUpdate()
	{
		if (g >= 0)
		{
			try
			{
				controller = hitColliders[g].GetComponent<IDestructible>();
				unitTransform = hitColliders[g].transform;
				origin = epicenter;
				direction = Vector3.zero;
				hitMarker = unitTransform.GetComponent<HitMarker>();
				if (hitMarker != null)
				{
					for (m = 0; m < hitMarker.Markers.Count; m++)
					{
						marker = hitMarker.Markers[m];
						direction = marker.position - origin;
						float magnitude = (marker.position - origin).magnitude;
						Ray ray = new Ray(origin, direction);
						RaycastHit hitInfo;
						if (!PhysicsFacade.Raycast(ray, out hitInfo, magnitude, damageLayerMask))
						{
							controller.OnDamage(damage);
							break;
						}
					}
				}
			}
			catch
			{
			}
			g--;
		}
		if (g <= -1)
		{
			g--;
		}
		timer = 0.01f;
	}
}
