using Slugterra.UI.Shops.PaymentStates;

namespace AppsMinistry.Core.Input.Window.Conditions
{
	public class IWECEUpgradeButton : InputWindowElementCondition
	{
		public UpgradePaymentState paymentState;

		public override bool IsActive()
		{
			return !paymentState.IsMax;
		}
	}
}
