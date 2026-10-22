using UnityEngine;

namespace EasyInput.Misc
{
	[AddComponentMenu("EasyInputHelper/Miscellaneous/CameraFollowBall")]
	public class CameraFollowBall : MonoBehaviour
	{
		public GameObject objectToFollow;

		private Transform objectTransform;

		private Vector3 newPosition;

		private void Start()
		{
			objectTransform = objectToFollow.transform;
		}

		private void Update()
		{
			newPosition = objectTransform.position;
			newPosition.y = 2f;
			newPosition.z -= 5f;
			transform.position = newPosition;
		}
	}
}
