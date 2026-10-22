using Slugterra.Game.Environment;
using UnityEngine;

public class InteractiveObjectInfo : TriggerEnter
{
	public GameObject help;

	public ArenaInteractiveEnvironmant IO;

	public float _timer = 10f;

	private void Start()
	{
		if (IO == null)
		{
			IO = transform.parent.GetComponent<ArenaInteractiveEnvironmant>();
		}
		if (IO == null)
		{
			IO = transform.GetComponent<ArenaInteractiveEnvironmant>();
		}
		if (IO == null)
		{
			Object.Destroy(gameObject);
		}
	}

	private void FixedUpdate()
	{
		if (_timer > 0f)
		{
			_timer -= Time.deltaTime;
		}
	}

	protected override void Enter()
	{
		if (_timer <= 0f && IO.IsReady)
		{
			help.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider _ColliderOther)
	{
		if (!(_ColliderOther.tag == TagEnter))
		{
		}
	}
}
