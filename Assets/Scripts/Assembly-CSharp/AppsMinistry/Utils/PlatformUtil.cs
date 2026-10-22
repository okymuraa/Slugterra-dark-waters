namespace AppsMinistry.Utils
{
	public static class PlatformUtil
	{
		public static bool IsAmazon()
		{
			return false;
		}

		public static void ETShowMailComposer(string adress, string subject, string body)
		{
			AndroidSocialGate.SendMail(string.Empty, body, subject, adress);
		}

		public static void ShowAlert(string title, string msg)
		{
			AndroidMessage.Create(title, msg);
		}
	}
}
