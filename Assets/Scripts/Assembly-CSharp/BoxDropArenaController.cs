using UnityEngine;

public class BoxDropArenaController : MonoBehaviour
{
	public GameObject DropBox;

	public float Speed;

	public Animation spline;

	public AnimationClip Track;

	public Transform StartPoint;

	public Transform EndPoint;

	public Transform[] Node;

	public GameObject[] Clone;

	private int i;

	private float timer;

	private int roll;

	private void Start()
	{
		spline["Take 001"].speed = Speed;
		spline.Play();
		Clone = new GameObject[Node.Length];
		Clone[1] = Object.Instantiate(DropBox, Node[1].position, Node[1].rotation);
		Clone[1].transform.parent = Node[1];
		Clone[2] = Object.Instantiate(DropBox, Node[2].position, Node[2].rotation);
		Clone[2].transform.parent = Node[2];
	}

	private void FixedUpdate()
	{
		for (i = 0; i < Node.Length; i++)
		{
			if (Clone[i] != null && Vector3.SqrMagnitude(Node[i].position - EndPoint.position) < 1f)
			{
				Object.Destroy(Clone[i]);
			}
		}
		for (i = 0; i < Node.Length; i++)
		{
			if (Clone[i] != null && Vector3.SqrMagnitude(Node[i].position - EndPoint.position) < 0.5f)
			{
				Object.Destroy(Clone[i]);
			}
		}
		if (timer > 0f)
		{
			timer -= Time.deltaTime;
			return;
		}
		for (i = 0; i < Node.Length; i++)
		{
			if (Clone[i] == null && Vector3.SqrMagnitude(Node[i].position - StartPoint.position) < 0.5f)
			{
				roll = Random.Range(0, 100);
				if (roll > 55)
				{
					timer = 1f;
					break;
				}
				Clone[i] = Object.Instantiate(DropBox, Node[i].position, Node[i].rotation);
				Clone[i].transform.parent = Node[i];
				timer = Random.Range(0.5f, 2f);
				break;
			}
		}
	}
}
