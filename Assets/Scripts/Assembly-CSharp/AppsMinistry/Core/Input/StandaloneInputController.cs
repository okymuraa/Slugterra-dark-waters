using UnityEngine;

namespace AppsMinistry.Core.Input
{
	public class StandaloneInputController : InputController
	{
		public override void OnUpdate()
		{
			if (UnityEngine.Input.GetMouseButtonDown(0))
			{
				Tap(UnityEngine.Input.mousePosition);
			}
			else if (UnityEngine.Input.GetMouseButton(0))
			{
				OnMove(UnityEngine.Input.mousePosition);
				Drag(UnityEngine.Input.mousePosition);
			}
			else if (UnityEngine.Input.GetMouseButtonUp(0))
			{
				EndInput(UnityEngine.Input.mousePosition);
			}
			if (UnityEngine.Input.GetAxis("Vertical") > 0f || UnityEngine.Input.GetAxis("Vertical") < 0f)
			{
				OnZoom(UnityEngine.Input.GetAxis("Vertical") * 0.002f);
			}
			OnAcceleration(UnityEngine.Input.acceleration);
		}
	}
}
