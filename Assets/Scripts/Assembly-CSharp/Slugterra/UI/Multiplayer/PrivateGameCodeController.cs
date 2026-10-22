using AppsMinistry.UI;

namespace Slugterra.UI.Multiplayer
{
	public class PrivateGameCodeController : UIController<PrivateGameCodeView>
	{
		protected override void _OnEnable()
		{
			View.OnCloseEvent += OnClose;
			View.OnInstructionEvent += OnInstruction;
		}

		protected override void _OnDisable()
		{
			View.OnCloseEvent -= OnClose;
			View.OnInstructionEvent -= OnInstruction;
		}

		private void OnInstruction()
		{
			if (View.EnterCode.activeSelf)
			{
				View.EnterCode.SetActive(false);
				View.InstructionCode.SetActive(true);
			}
			else
			{
				View.InstructionCode.SetActive(false);
				View.EnterCode.SetActive(true);
			}
		}

		private void OnClose()
		{
			gameObject.SetActive(false);
		}
	}
}
