using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Cameras
{
	public class ProtectCameraFromWallClip : MonoBehaviour
	{
		public class RayHitComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
			}
		}

		public float clipMoveTime = 0.05f;

		public float returnTime = 0.4f;

		public float sphereCastRadius = 0.1f;

		public bool visualiseInEditor;

		public float closestDistance = 0.5f;

		public LayerMask LayerMask;

		public Transform m_Cam;

		public Transform m_Pivot;

		private float m_OriginalDist;

		private float m_MoveVelocity;

		private float m_CurrentDist;

		private Ray m_Ray;

		private RaycastHit[] m_Hits;

		private RayHitComparer m_RayHitComparer;

		public bool protecting { get; private set; }

		private void Awake()
		{
			if (m_Cam == null)
			{
				m_Cam = GetComponentInChildren<Camera>().transform;
			}
			if (m_Pivot == null)
			{
				m_Pivot = m_Cam.parent;
			}
			m_OriginalDist = m_Cam.localPosition.magnitude;
			m_CurrentDist = m_OriginalDist;
			m_RayHitComparer = new RayHitComparer();
		}

		private void LateUpdate()
		{
			float num = m_OriginalDist;
			m_Ray.origin = m_Pivot.position + m_Pivot.forward * sphereCastRadius;
			m_Ray.direction = -m_Pivot.forward;
			Collider[] array = Physics.OverlapSphere(m_Ray.origin, sphereCastRadius, LayerMask);
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].isTrigger && !(array[i].attachedRigidbody != null))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				m_Ray.origin += m_Pivot.forward * sphereCastRadius;
				m_Hits = Physics.RaycastAll(m_Ray, m_OriginalDist - sphereCastRadius, LayerMask);
			}
			else
			{
				m_Hits = Physics.SphereCastAll(m_Ray, sphereCastRadius, m_OriginalDist + sphereCastRadius, LayerMask);
			}
			Array.Sort(m_Hits, m_RayHitComparer);
			float num2 = float.PositiveInfinity;
			for (int j = 0; j < m_Hits.Length; j++)
			{
				if (m_Hits[j].distance < num2 && !m_Hits[j].collider.isTrigger && !(m_Hits[j].collider.attachedRigidbody != null))
				{
					num2 = m_Hits[j].distance;
					num = 0f - m_Pivot.InverseTransformPoint(m_Hits[j].point).z;
					flag2 = true;
				}
			}
			if (flag2)
			{
				Debug.DrawRay(m_Ray.origin, -m_Pivot.forward * (num + sphereCastRadius), Color.red);
			}
			protecting = flag2;
			m_CurrentDist = Mathf.SmoothDamp(m_CurrentDist, num, ref m_MoveVelocity, (!(m_CurrentDist > num)) ? returnTime : clipMoveTime);
			m_CurrentDist = Mathf.Clamp(m_CurrentDist, closestDistance, m_OriginalDist);
			m_Cam.localPosition = -Vector3.forward * m_CurrentDist;
		}
	}
}
