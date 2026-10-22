using System;
using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units
{
	public class AreaDamageCommand : MonoCommand<AreaDamageCommand>
	{
		private Vector3 epicenter = Vector3.zero;

		private float hittingArea;

		private float damage;

		private int[] layers;

		private Attacker parent;

		private Transform directDamaged;

		private bool isAttenuation;

		protected override void OnStart(object[] args)
		{
			if (args == null || args.Length == 0 || args.Length < 4)
			{
				Finish();
				return;
			}
			epicenter = (Vector3)args[0];
			hittingArea = (float)args[1];
			damage = (float)args[2];
			layers = (int[])args[3];
			parent = null;
			if (args.Length > 4)
			{
				parent = (Attacker)args[4];
			}
			if (args.Length > 5)
			{
				directDamaged = (Transform)args[5];
			}
			if (args.Length > 6)
			{
				isAttenuation = (bool)args[6];
			}
			if (hittingArea == 0f)
			{
				Finish();
				return;
			}
			int num = 0;
			for (int i = 0; i < layers.Length; i++)
			{
				num |= 1 << layers[i];
			}
			int num2 = 0;
			int num3 = LayerMask.NameToLayer("CoverLayer");
			num2 = 1 << num3;
			try
			{
				Collider[] array = PhysicsFacade.OverlapSphere(epicenter, hittingArea);
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j] != null && array[j].gameObject != null && array[j].gameObject.activeSelf)
					{
						bool flag = true;
						for (int k = 0; k < layers.Length; k++)
						{
							if (array[j].gameObject.layer == layers[k])
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							continue;
						}
					}
					IDestructible component = array[j].GetComponent<IDestructible>();
					Transform transform = array[j].transform;
					if (parent != null)
					{
						parent.damageType = ((!(transform == directDamaged)) ? DamageType.Splash : DamageType.Direct);
						parent.hitTransform = array[j].transform;
					}
					if (component == null)
					{
						return;
					}
					Vector3 vector = epicenter;
					Vector3 zero = Vector3.zero;
					HitMarker component2 = transform.GetComponent<HitMarker>();
					if (!(component2 != null))
					{
						continue;
					}
					foreach (Transform marker in component2.Markers)
					{
						zero = marker.position - vector;
						float magnitude = (marker.position - vector).magnitude;
						Ray ray = new Ray(vector, zero);
						RaycastHit hitInfo;
						if (!Physics.Raycast(ray, out hitInfo, magnitude, num2, QueryTriggerInteraction.Collide))
						{
							if (isAttenuation)
							{
								float num4 = (1f - magnitude / hittingArea) * damage;
								component.OnDamage(num4, parent);
							}
							else
							{
								component.OnDamage(damage, parent);
							}
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarningFormat("AreaDamageCommand::OnStart Error: {0} ", ex.ToString());
			}
			Finish();
		}
	}
}
