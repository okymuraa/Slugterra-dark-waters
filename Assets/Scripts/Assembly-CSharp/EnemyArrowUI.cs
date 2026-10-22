using System.Collections.Generic;
using Slugterra.Game.Mission;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Ally;
using Slugterra.UI.Game;
using UnityEngine;

public class EnemyArrowUI : MonoBehaviour
{
	private Transform eliTransform;

	[SerializeField]
	private ObjectiveTarget objectiveEnable;

	[SerializeField]
	private OffScreenTarget offScreenTarget;

	[SerializeField]
	private List<UnitController> units;

	[SerializeField]
	private Transform finalPoint;

	private UnitController currentTarget;

	private bool trigger;

	private bool disable;

	private void Start()
	{
		eliTransform = Object.FindObjectOfType<EliController>().transform;
	}

	private void OnEnable()
	{
		objectiveEnable.OnObjectiveCompleteEvent += SetEnable;
	}

	private void OnDisable()
	{
		objectiveEnable.OnObjectiveCompleteEvent -= SetEnable;
	}

	private void FixedUpdate()
	{
		if (currentTarget == null && trigger)
		{
			trigger = false;
			SetEnable();
		}
	}

	private void SetDisable()
	{
		disable = true;
		offScreenTarget.SwitchTarget(null);
	}

	private void SetEnable()
	{
		if (disable)
		{
			return;
		}
		float num = 1000f;
		for (int i = 0; i < units.Count; i++)
		{
			if (units[i] != null && Vector3.Distance(eliTransform.position, units[i].transform.position) < num)
			{
				num = Vector3.Distance(eliTransform.position, units[i].transform.position);
				currentTarget = units[i];
				offScreenTarget.SwitchTarget(units[i].transform);
				trigger = true;
			}
		}
		if (currentTarget == null && !disable && finalPoint != null)
		{
			offScreenTarget.SwitchTarget(finalPoint);
		}
	}
}
