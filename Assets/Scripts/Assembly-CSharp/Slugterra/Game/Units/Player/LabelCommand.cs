using System.Collections;
using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Player
{
	public class LabelCommand : MonoCommand<LabelCommand>
	{
		private PlayerController _controller;

		private GameObject _labelPrefab;

		protected override void OnStart(object[] args)
		{
			_controller = (PlayerController)args[0];
			_labelPrefab = (GameObject)args[1];
			StartCoroutine(Label(0.2f));
		}

		private IEnumerator Label(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			if (_controller.AttackTarget != null)
			{
				Object.Instantiate(_labelPrefab, _controller.GameObject.transform.position + Vector3.up * 2f, Quaternion.identity);
			}
			Finish();
		}

		protected override void OnReleaseResources()
		{
			StopAllCoroutines();
		}
	}
}
