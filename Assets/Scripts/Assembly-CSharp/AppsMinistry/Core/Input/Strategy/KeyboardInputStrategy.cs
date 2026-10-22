using System.Collections.Generic;
using UnityEngine;

namespace AppsMinistry.Core.Input.Strategy
{
	public class KeyboardInputStrategy : TouchInputStrategy
	{
		private Dictionary<KeyCode, float> timing = new Dictionary<KeyCode, float>();

		private bool tutorSkipping;

		private bool showHelp;

		public override void Activate()
		{
			base.Activate();
		}

		public override void Deactivate()
		{
			base.Deactivate();
		}

		public override void Update()
		{
			base.Update();
			DownKeys();
			PressKeys();
			UpKeys();
		}

		private void DownKeys()
		{
			if (GetKeyDown(KeyCode.UpArrow))
			{
				Use_OnButtonDown(InputEvent.Up);
				Use_OnMoveEvent(new Vector2(0f, 1f));
				if (!timing.ContainsKey(KeyCode.UpArrow))
				{
					timing.Add(KeyCode.UpArrow, Time.deltaTime);
				}
			}
			else if (GetKeyDown(KeyCode.DownArrow))
			{
				Use_OnButtonDown(InputEvent.Down);
				Use_OnMoveEvent(new Vector2(0f, -1f));
				if (!timing.ContainsKey(KeyCode.DownArrow))
				{
					timing.Add(KeyCode.DownArrow, Time.deltaTime);
				}
			}
			else if (GetKeyDown(KeyCode.LeftArrow))
			{
				Use_OnButtonDown(InputEvent.Left);
				Use_OnMoveEvent(new Vector2(-1f, 0f));
				if (!timing.ContainsKey(KeyCode.LeftArrow))
				{
					timing.Add(KeyCode.LeftArrow, Time.deltaTime);
				}
			}
			else if (GetKeyDown(KeyCode.RightArrow))
			{
				Use_OnButtonDown(InputEvent.Right);
				Use_OnMoveEvent(new Vector2(1f, 0f));
				if (!timing.ContainsKey(KeyCode.RightArrow))
				{
					timing.Add(KeyCode.RightArrow, Time.deltaTime);
				}
			}
			else if (GetKeyDown(KeyCode.Return))
			{
				Use_OnButtonDown(InputEvent.Enter);
				if (!timing.ContainsKey(KeyCode.Return))
				{
					timing.Add(KeyCode.Return, Time.deltaTime);
				}
			}
			else if (GetKeyDown(KeyCode.KeypadEnter))
			{
				OnUseSlug();
			}
			else if (GetKeyDown(KeyCode.Keypad7))
			{
				OnUseNextPowerUp();
			}
			else if (GetKeyDown(KeyCode.Keypad4))
			{
				OnUsePreviousPowerUp();
			}
			else
			{
				if (GetKeyDown(KeyCode.KeypadPeriod))
				{
					return;
				}
				if (GetKeyDown(KeyCode.PageUp))
				{
					OnUseNextSlug();
				}
				else if (GetKeyDown(KeyCode.PageDown))
				{
					OnUsePreviousSlug();
				}
				else if (GetKeyDown(KeyCode.W))
				{
					Use_OnMoveEndEvent();
					Use_OnSwipeClick(new Vector2(0f, 1f));
				}
				else if (GetKeyDown(KeyCode.S))
				{
					Use_OnMoveEndEvent();
					Use_OnSwipeClick(new Vector2(0f, -1f));
				}
				else if (GetKeyDown(KeyCode.D))
				{
					Use_OnMoveEndEvent();
					Use_OnSwipeClick(new Vector2(1f, 0f));
				}
				else if (GetKeyDown(KeyCode.A))
				{
					Use_OnMoveEndEvent();
					Use_OnSwipeClick(new Vector2(-1f, 0f));
				}
				else if (GetKeyDown(KeyCode.Escape))
				{
					if (tutorSkipping || !showHelp)
					{
					}
					tutorSkipping = !tutorSkipping;
					showHelp = false;
					OnBackBtn();
				}
			}
		}

		private void PressKeys()
		{
			if (GetKeyPress(KeyCode.UpArrow))
			{
				Use_OnButtonPress(InputEvent.Up);
				Use_OnMoveEvent(new Vector2(0f, 1f));
				timing[KeyCode.UpArrow] += Time.deltaTime;
			}
			else if (GetKeyPress(KeyCode.DownArrow))
			{
				Use_OnButtonPress(InputEvent.Down);
				Use_OnMoveEvent(new Vector2(0f, -1f));
				timing[KeyCode.DownArrow] += Time.deltaTime;
			}
			else if (GetKeyPress(KeyCode.LeftArrow))
			{
				Use_OnAccelMoveLeft();
				Use_OnButtonPress(InputEvent.Left);
				Use_OnMoveEvent(new Vector2(-1f, 0f));
				timing[KeyCode.LeftArrow] += Time.deltaTime;
			}
			else if (GetKeyPress(KeyCode.RightArrow))
			{
				Use_OnAccelMoveRight();
				Use_OnButtonPress(InputEvent.Right);
				Use_OnMoveEvent(new Vector2(1f, 0f));
				timing[KeyCode.RightArrow] += Time.deltaTime;
			}
			else if (GetKeyPress(KeyCode.Return))
			{
				Use_OnButtonPress(InputEvent.Enter);
				timing[KeyCode.Return] += Time.deltaTime;
			}
		}

		private void UpKeys()
		{
			if (GetKeyUp(KeyCode.UpArrow))
			{
				Use_OnButtonUp(InputEvent.Up);
				Use_OnMoveEndEvent();
				timing.Remove(KeyCode.UpArrow);
			}
			else if (GetKeyUp(KeyCode.DownArrow))
			{
				Use_OnButtonUp(InputEvent.Down);
				Use_OnMoveEndEvent();
				timing.Remove(KeyCode.DownArrow);
			}
			else if (GetKeyUp(KeyCode.LeftArrow))
			{
				Use_OnButtonUp(InputEvent.Left);
				Use_OnMoveEndEvent();
				timing.Remove(KeyCode.LeftArrow);
			}
			else if (GetKeyUp(KeyCode.RightArrow))
			{
				Use_OnButtonUp(InputEvent.Right);
				Use_OnMoveEndEvent();
				timing.Remove(KeyCode.RightArrow);
			}
			else if (GetKeyUp(KeyCode.Return))
			{
				if (timing[KeyCode.Return] > 0.3f)
				{
					OnOpenMinimap();
				}
				else
				{
					Use_OnButtonUp(InputEvent.Enter);
					OnUsePowerUp();
				}
				timing.Remove(KeyCode.Return);
			}
			else if (GetKeyUp(KeyCode.Keypad0))
			{
				Use_OnNextTargetBtnEvent();
			}
			else if (GetKeyUp(KeyCode.Keypad1))
			{
				Use_OnPrevTargetBtnEvent();
			}
			else if (GetKeyUp(KeyCode.LeftControl))
			{
				Use_OnBegineAttack();
			}
			else if (GetKeyUp(KeyCode.H) && !tutorSkipping)
			{
				OnShowHelpForController();
				showHelp = true;
				tutorSkipping = true;
			}
		}

		private bool GetKeyUp(KeyCode key)
		{
			return UnityEngine.Input.GetKeyUp(key);
		}

		private bool GetKeyDown(KeyCode key)
		{
			return UnityEngine.Input.GetKeyDown(key);
		}

		private bool GetKeyPress(KeyCode key)
		{
			return UnityEngine.Input.GetKey(key);
		}
	}
}
