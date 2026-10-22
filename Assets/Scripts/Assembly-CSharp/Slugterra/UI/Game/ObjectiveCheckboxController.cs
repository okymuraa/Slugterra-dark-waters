using AppsMinistry.UI;

namespace Slugterra.UI.Game
{
	public class ObjectiveCheckboxController : UIController<ObjectiveCheckboxView>
	{
		private ObjectiveCheckboxState _state;

		public ObjectiveCheckboxState State
		{
			get
			{
				return _state;
			}
			set
			{
				SetState(value);
			}
		}

		protected void SetState(ObjectiveCheckboxState state)
		{
			if (state != _state)
			{
				View.ApplyModel(state);
				_state = state;
			}
		}
	}
}
