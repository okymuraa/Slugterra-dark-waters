using System.Collections.Generic;
using UnityEngine;

namespace AppsMinistry.Core.Input.Strategy
{
	public class KeyboardInputStrategy : TouchInputStrategy
	{
		private Dictionary<KeyCode, float> timing = new Dictionary<KeyCode, float>();

		private bool tutorSkipping;

		private bool showHelp;

		private bool _wasMovingKeyboard;

		private Vector2 _lastMoveDir = Vector2.up;

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
			UpdateKeyboardMovement();
			UpdateMouseCombat();
			DownKeys();
			PressKeys();
			UpKeys();
		}

		private void UpdateMouseCombat()
		{
			if (UnityEngine.Input.GetMouseButtonDown(1))
			{
				Use_OnBegineAttack();
			}
			float scroll = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
			if (scroll > 0.05f)
			{
				OnUseNextSlug();
			}
			else if (scroll < -0.05f)
			{
				OnUsePreviousSlug();
			}
		}

		private Vector2 GetKeyboardMoveVector()
		{
			float x = 0f;
			float y = 0f;
			if (GetKeyPress(KeyCode.W) || GetKeyPress(KeyCode.UpArrow))
			{
				y += 1f;
			}
			if (GetKeyPress(KeyCode.S) || GetKeyPress(KeyCode.DownArrow))
			{
				y -= 1f;
			}
			if (GetKeyPress(KeyCode.D) || GetKeyPress(KeyCode.RightArrow))
			{
				x += 1f;
			}
			if (GetKeyPress(KeyCode.A) || GetKeyPress(KeyCode.LeftArrow))
			{
				x -= 1f;
			}
			Vector2 vec = new Vector2(x, y);
			if (vec.sqrMagnitude > 1f)
			{
				vec.Normalize();
			}
			return vec;
		}

		private void UpdateKeyboardMovement()
		{
			if (UnityEngine.Input.GetMouseButton(0) || UnityEngine.Input.GetMouseButton(1))
			{
				if (_wasMovingKeyboard)
				{
					_wasMovingKeyboard = false;
					Use_OnMoveEndEvent();
				}
				return;
			}
			Vector2 moveVec = GetKeyboardMoveVector();
			if (moveVec.sqrMagnitude > 0.01f)
			{
				_wasMovingKeyboard = true;
				_lastMoveDir = moveVec.normalized;
				Use_OnMoveEvent(moveVec);
			}
			else if (_wasMovingKeyboard)
			{
				_wasMovingKeyboard = false;
				Use_OnMoveEndEvent();
			}
		}

		private void DownKeys()
		{
			if (GetKeyDown(KeyCode.UpArrow) || GetKeyDown(KeyCode.W))
			{
				Use_OnButtonDown(InputEvent.Up);
				timing[KeyCode.UpArrow] = Time.deltaTime;
			}
			if (GetKeyDown(KeyCode.DownArrow) || GetKeyDown(KeyCode.S))
			{
				Use_OnButtonDown(InputEvent.Down);
				timing[KeyCode.DownArrow] = Time.deltaTime;
			}
			if (GetKeyDown(KeyCode.LeftArrow) || GetKeyDown(KeyCode.A))
			{
				Use_OnButtonDown(InputEvent.Left);
				timing[KeyCode.LeftArrow] = Time.deltaTime;
			}
			if (GetKeyDown(KeyCode.RightArrow) || GetKeyDown(KeyCode.D))
			{
				Use_OnButtonDown(InputEvent.Right);
				timing[KeyCode.RightArrow] = Time.deltaTime;
			}
			if (GetKeyDown(KeyCode.Space) || GetKeyDown(KeyCode.LeftShift))
			{
				Use_OnMoveEndEvent();
				Use_OnSwipe(_lastMoveDir);
				Use_OnSwipeClick(_lastMoveDir);
			}
			if (GetKeyDown(KeyCode.Return))
			{
				Use_OnButtonDown(InputEvent.Enter);
				timing[KeyCode.Return] = Time.deltaTime;
			}
			else if (GetKeyDown(KeyCode.KeypadEnter) || GetKeyDown(KeyCode.E))
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
			else if (GetKeyDown(KeyCode.PageUp) || GetKeyDown(KeyCode.Q))
			{
				OnUseNextSlug();
			}
			else if (GetKeyDown(KeyCode.PageDown))
			{
				OnUsePreviousSlug();
			}
			else if (GetKeyDown(KeyCode.Escape))
			{
				tutorSkipping = !tutorSkipping;
				showHelp = false;
				OnBackBtn();
			}
			if (GetKeyDown(KeyCode.Alpha1))
			{
				OnUseSlugByIndex(0);
			}
			else if (GetKeyDown(KeyCode.Alpha2))
			{
				OnUseSlugByIndex(1);
			}
			else if (GetKeyDown(KeyCode.Alpha3))
			{
				OnUseSlugByIndex(2);
			}
		}

		private void OnUseSlugByIndex(int index)
		{
			Slugterra.Game.Units.Player.PlayerController localPlayer = UnityEngine.Object.FindObjectOfType<Slugterra.Game.Units.Player.PlayerController>();
			if (localPlayer != null)
			{
				localPlayer.SelectSlugByIndex(index);
			}
		}

		private void PressKeys()
		{
			if (GetKeyPress(KeyCode.UpArrow) || GetKeyPress(KeyCode.W))
			{
				Use_OnButtonPress(InputEvent.Up);
				timing[KeyCode.UpArrow] = (timing.ContainsKey(KeyCode.UpArrow) ? timing[KeyCode.UpArrow] : 0f) + Time.deltaTime;
			}
			if (GetKeyPress(KeyCode.DownArrow) || GetKeyPress(KeyCode.S))
			{
				Use_OnButtonPress(InputEvent.Down);
				timing[KeyCode.DownArrow] = (timing.ContainsKey(KeyCode.DownArrow) ? timing[KeyCode.DownArrow] : 0f) + Time.deltaTime;
			}
			if (GetKeyPress(KeyCode.LeftArrow) || GetKeyPress(KeyCode.A))
			{
				Use_OnAccelMoveLeft();
				Use_OnButtonPress(InputEvent.Left);
				timing[KeyCode.LeftArrow] = (timing.ContainsKey(KeyCode.LeftArrow) ? timing[KeyCode.LeftArrow] : 0f) + Time.deltaTime;
			}
			if (GetKeyPress(KeyCode.RightArrow) || GetKeyPress(KeyCode.D))
			{
				Use_OnAccelMoveRight();
				Use_OnButtonPress(InputEvent.Right);
				timing[KeyCode.RightArrow] = (timing.ContainsKey(KeyCode.RightArrow) ? timing[KeyCode.RightArrow] : 0f) + Time.deltaTime;
			}
			if (GetKeyPress(KeyCode.Return))
			{
				Use_OnButtonPress(InputEvent.Enter);
				timing[KeyCode.Return] = (timing.ContainsKey(KeyCode.Return) ? timing[KeyCode.Return] : 0f) + Time.deltaTime;
			}
		}

		private void UpKeys()
		{
			if (GetKeyUp(KeyCode.UpArrow) || GetKeyUp(KeyCode.W))
			{
				Use_OnButtonUp(InputEvent.Up);
				timing.Remove(KeyCode.UpArrow);
			}
			if (GetKeyUp(KeyCode.DownArrow) || GetKeyUp(KeyCode.S))
			{
				Use_OnButtonUp(InputEvent.Down);
				timing.Remove(KeyCode.DownArrow);
			}
			if (GetKeyUp(KeyCode.LeftArrow) || GetKeyUp(KeyCode.A))
			{
				Use_OnButtonUp(InputEvent.Left);
				timing.Remove(KeyCode.LeftArrow);
			}
			if (GetKeyUp(KeyCode.RightArrow) || GetKeyUp(KeyCode.D))
			{
				Use_OnButtonUp(InputEvent.Right);
				timing.Remove(KeyCode.RightArrow);
			}
			if (GetKeyUp(KeyCode.Return))
			{
				float held = (timing.ContainsKey(KeyCode.Return) ? timing[KeyCode.Return] : 0f);
				if (held > 0.3f)
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
			else if (GetKeyUp(KeyCode.Keypad0) || GetKeyUp(KeyCode.Tab))
			{
				Use_OnNextTargetBtnEvent();
			}
			else if (GetKeyUp(KeyCode.Keypad1))
			{
				Use_OnPrevTargetBtnEvent();
			}
			else if (GetKeyUp(KeyCode.LeftControl) || GetKeyUp(KeyCode.F))
			{
				Use_OnBegineAttack();
			}
			else if (GetKeyUp(KeyCode.M))
			{
				OnOpenMinimap();
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
