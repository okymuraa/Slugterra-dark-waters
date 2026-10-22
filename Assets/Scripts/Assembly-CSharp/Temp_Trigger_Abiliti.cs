using System.Collections.Generic;
using AppsMinistry.Core.Command;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Slugs;
using UnityEngine;

public class Temp_Trigger_Abiliti : MonoBehaviour
{
	[SerializeField]
	private List<float> _effectTimes;

	[SerializeField]
	private List<float> _effectDeceys;

	public List<float> EffectTimes
	{
		get
		{
			if (_effectTimes == null || _effectTimes.Count < 5)
			{
				InitEffectTimes();
			}
			return _effectTimes;
		}
	}

	public List<float> EffectDeceys
	{
		get
		{
			if (_effectDeceys == null || _effectDeceys.Count < 5)
			{
				InitEffectDeceys();
			}
			return _effectDeceys;
		}
	}

	private void InitEffectTimes()
	{
		_effectTimes = new List<float>();
		for (int i = 0; i < 5; i++)
		{
			_effectTimes.Add(1f);
		}
	}

	private void InitEffectDeceys()
	{
		_effectDeceys = new List<float>();
		for (int i = 0; i < 5; i++)
		{
			_effectDeceys.Add(0f);
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		if ((bool)collider.GetComponent<UnitController>())
		{
			OnUseNegativeEffect(collider.transform);
		}
	}

	private void OnUseNegativeEffect(Transform unitTransform)
	{
		UnitController component = unitTransform.GetComponent<UnitController>();
		MonoCommand.ExecuteOn<KnockBackEffect>(component.GameObject, new object[4]
		{
			component,
			_effectTimes[1],
			_effectDeceys[1],
			null
		});
	}
}
