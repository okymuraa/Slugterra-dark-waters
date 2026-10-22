using UnityEngine;

public class PhysicsFacade
{
	public static Collider[] OverlapSphere(Vector3 position, float radius)
	{
		return Physics.OverlapSphere(position, radius);
	}

	public static Collider[] OverlapSphere(Vector3 position, float radius, int layerMask)
	{
		return Physics.OverlapSphere(position, radius, layerMask);
	}

	public static bool Raycast(Ray ray)
	{
		bool flag = false;
		return Physics.Raycast(ray);
	}

	public static bool Raycast(Ray ray, float maxDistance)
	{
		bool flag = false;
		return Physics.Raycast(ray, maxDistance);
	}

	public static bool Raycast(Ray ray, out RaycastHit hitInfo)
	{
		bool flag = false;
		return Physics.Raycast(ray, out hitInfo);
	}

	public static bool Raycast(Vector3 origin, Vector3 direction)
	{
		bool flag = false;
		return Physics.Raycast(origin, direction);
	}

	public static bool Raycast(Ray ray, float maxDistance, int layerMask)
	{
		bool flag = false;
		return Physics.Raycast(ray, maxDistance, layerMask);
	}

	public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
	{
		bool flag = false;
		return Physics.Raycast(ray, out hitInfo, maxDistance);
	}

	public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance)
	{
		bool flag = false;
		return Physics.Raycast(origin, direction, maxDistance);
	}

	public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo)
	{
		bool flag = false;
		return Physics.Raycast(origin, direction, out hitInfo);
	}

	public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance, int layerMask)
	{
		bool flag = false;
		return Physics.Raycast(ray, out hitInfo, maxDistance, layerMask);
	}

	public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance, int layerMask)
	{
		bool flag = false;
		return Physics.Raycast(origin, direction, maxDistance, layerMask);
	}

	public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance)
	{
		bool flag = false;
		return Physics.Raycast(origin, direction, out hitInfo, maxDistance);
	}

	public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask)
	{
		bool flag = false;
		return Physics.Raycast(origin, direction, out hitInfo, maxDistance, layerMask);
	}
}
