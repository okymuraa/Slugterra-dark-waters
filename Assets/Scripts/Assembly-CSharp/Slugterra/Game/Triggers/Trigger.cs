using System.Collections.Generic;
using UnityEngine;

namespace Slugterra.Game.Triggers
{
	[RequireComponent(typeof(Collider))]
	public class Trigger : MonoBehaviour
	{
		[SerializeField]
		protected TriggerMode Mode;

		[SerializeField]
		protected float Delay;

		[SerializeField]
		private TriggerDoAfter _doAfter;

		[SerializeField]
		private List<string> _tagsForTrigger = new List<string>();

		[SerializeField]
		private List<string> _layersForTrigger = new List<string>();

		private float _triggerTime;

		private bool _triggerActivated;

		protected virtual void OnCreate()
		{
		}

		protected virtual void OnStart()
		{
		}

		protected virtual void OnUpdate()
		{
		}

		protected virtual void OnReleaseResource()
		{
		}

		protected virtual void OnTrigger()
		{
		}

		private void Awake()
		{
			OnCreate();
		}

		private void Start()
		{
			OnStart();
		}

		private void Update()
		{
			OnUpdate();
			if (_triggerActivated && Time.time >= _triggerTime)
			{
				OnTrigger();
				DoAfter();
				_triggerActivated = false;
			}
		}

		private void OnDestroy()
		{
			OnReleaseResource();
		}

		private void DoAfter()
		{
			switch (_doAfter)
			{
			case TriggerDoAfter.SelfDisable:
				enabled = false;
				break;
			case TriggerDoAfter.DisableGameObject:
				gameObject.SetActive(false);
				break;
			}
		}

		private void ActivateTrigger()
		{
			_triggerTime = Time.time + Delay;
			_triggerActivated = true;
		}

		protected virtual void OnTriggerEnter(Collider collider)
		{
			if (Mode == TriggerMode.OnEnter && !ActivateByTags(collider) && !ActivateByTriggers(collider))
			{
			}
		}

		protected virtual void OnTriggerExit(Collider collider)
		{
			if (Mode == TriggerMode.OnExit && !ActivateByTags(collider) && !ActivateByTriggers(collider))
			{
			}
		}

		protected virtual void OnTriggerStay(Collider collider)
		{
			if (Mode == TriggerMode.OnStay && !ActivateByTags(collider) && !ActivateByTriggers(collider))
			{
			}
		}

		protected bool ActivateByTags(Collider collider)
		{
			foreach (string item in _tagsForTrigger)
			{
				if (collider.gameObject.tag == item)
				{
					ActivateTrigger();
					return true;
				}
			}
			return false;
		}

		protected bool ActivateByTriggers(Collider collider)
		{
			foreach (string item in _layersForTrigger)
			{
				if (collider.gameObject.layer == LayerMask.NameToLayer(item))
				{
					ActivateTrigger();
					return true;
				}
			}
			return false;
		}

		public void AddLayer(string layer)
		{
			if (_layersForTrigger == null)
			{
				_layersForTrigger = new List<string>();
			}
			_layersForTrigger.Add(layer);
		}
	}
}
