using System;
using System.Linq;
using UnityEngine;

namespace Facebook.Unity.Example
{
	internal abstract class MenuBase : ConsoleBase
	{
		private static ShareDialogMode shareDialogMode;

		protected abstract void GetGui();

		protected virtual bool ShowDialogModeSelector()
		{
			return false;
		}

		protected virtual bool ShowBackButton()
		{
			return true;
		}

		protected void HandleResult(IResult result)
		{
			if (result == null)
			{
				LastResponse = "Null Response\n";
				LogView.AddLog(LastResponse);
				return;
			}
			LastResponseTexture = null;
			if (!string.IsNullOrEmpty(result.Error))
			{
				Status = "Error - Check log for details";
				LastResponse = "Error Response:\n" + result.Error;
			}
			else if (result.Cancelled)
			{
				Status = "Cancelled - Check log for details";
				LastResponse = "Cancelled Response:\n" + result.RawResult;
			}
			else if (!string.IsNullOrEmpty(result.RawResult))
			{
				Status = "Success - Check log for details";
				LastResponse = "Success Response:\n" + result.RawResult;
			}
			else
			{
				LastResponse = "Empty Response\n";
			}
			LogView.AddLog(result.ToString());
		}

		protected void OnGUI()
		{
			if (IsHorizontalLayout())
			{
				GUILayout.BeginHorizontal();
				GUILayout.BeginVertical();
			}
			GUILayout.Label(GetType().Name, LabelStyle);
			AddStatus();
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				Vector2 vector = ScrollPosition;
				vector.y += Input.GetTouch(0).deltaPosition.y;
				ScrollPosition = vector;
			}
			ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, GUILayout.MinWidth(ConsoleBase.MainWindowFullWidth));
			GUILayout.BeginHorizontal();
			if (ShowBackButton())
			{
				AddBackButton();
			}
			AddLogButton();
			if (ShowBackButton())
			{
				GUILayout.Label(GUIContent.none, GUILayout.MinWidth(ConsoleBase.MarginFix));
			}
			GUILayout.EndHorizontal();
			if (ShowDialogModeSelector())
			{
				AddDialogModeButtons();
			}
			GUILayout.BeginVertical();
			GetGui();
			GUILayout.Space(10f);
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
		}

		private void AddStatus()
		{
			GUILayout.Space(5f);
			GUILayout.Box("Status: " + Status, TextStyle, GUILayout.MinWidth(ConsoleBase.MainWindowWidth));
		}

		private void AddBackButton()
		{
			GUI.enabled = ConsoleBase.MenuStack.Any();
			if (Button("Back"))
			{
				GoBack();
			}
			GUI.enabled = true;
		}

		private void AddLogButton()
		{
			if (Button("Log"))
			{
				SwitchMenu(typeof(LogView));
			}
		}

		private void AddDialogModeButtons()
		{
			GUILayout.BeginHorizontal();
			foreach (object value in Enum.GetValues(typeof(ShareDialogMode)))
			{
				AddDialogModeButton((ShareDialogMode)value);
			}
			GUILayout.EndHorizontal();
		}

		private void AddDialogModeButton(ShareDialogMode mode)
		{
			bool flag = GUI.enabled;
			GUI.enabled = flag && mode != shareDialogMode;
			if (Button(mode.ToString()))
			{
				shareDialogMode = mode;
				FB.Mobile.ShareDialogMode = mode;
			}
			GUI.enabled = flag;
		}
	}
}
