using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Game.Units;
using Slugterra.Game.Duel;
using Slugterra.Game.Mission;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Scene;
using UnityEngine;

namespace Slugterra.Game.Units.Player
{
	public static class PlayerManager
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<PlayerController> OnAddPlayerEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<PlayerController> OnAddLocalPlayerEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<PlayerController, PlayerController> OnDuelEvent__BackingField;

		private static List<PlayerController> _players = new List<PlayerController>();

		public static PlayerController LocalPlayer { get; private set; }

		public static event Action<PlayerController> OnAddPlayerEvent
		{
			add
			{
				Action<PlayerController> action = OnAddPlayerEvent__BackingField;
				Action<PlayerController> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAddPlayerEvent__BackingField, (Action<PlayerController>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<PlayerController> action = OnAddPlayerEvent__BackingField;
				Action<PlayerController> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAddPlayerEvent__BackingField, (Action<PlayerController>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private static event Action<PlayerController> OnAddLocalPlayerEvent
		{
			add
			{
				Action<PlayerController> action = OnAddLocalPlayerEvent__BackingField;
				Action<PlayerController> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAddLocalPlayerEvent__BackingField, (Action<PlayerController>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<PlayerController> action = OnAddLocalPlayerEvent__BackingField;
				Action<PlayerController> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAddLocalPlayerEvent__BackingField, (Action<PlayerController>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private static event Action<PlayerController, PlayerController> OnDuelEvent
		{
			add
			{
				Action<PlayerController, PlayerController> action = OnDuelEvent__BackingField;
				Action<PlayerController, PlayerController> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDuelEvent__BackingField, (Action<PlayerController, PlayerController>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<PlayerController, PlayerController> action = OnDuelEvent__BackingField;
				Action<PlayerController, PlayerController> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDuelEvent__BackingField, (Action<PlayerController, PlayerController>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public static void Add(PlayerController player)
		{
			if (!(player != null))
			{
				return;
			}
			if (player.IsLocal)
			{
				if (LocalPlayer != null)
				{
					UnityEngine.Debug.LogError("Several local players");
				}
				LocalPlayer = player;
			}
			_players.Add(player);
			RaiseAddPlayerEvents(player);
		}

		public static PlayerController RespawnLocalPlayer(GameModel gameModel)
		{
			PlayerController localPlayer = LocalPlayer;
			if (localPlayer != null)
			{
				Humanoid humanoid = localPlayer.Model as Humanoid;
				MonoCommand.ExecuteOn<ShieldCommand>(localPlayer.GameObject, new object[2] { humanoid, 3f });
				humanoid.Health = humanoid.MaxHealth;
				AnimatorStateInfo currentAnimatorStateInfo = localPlayer.Animator.GetCurrentAnimatorStateInfo(0);
				localPlayer.OnStandUp();
				localPlayer.StateMachine.NextState<IdleState>(new object[0]);
				SlugterraSceneController slugterraSceneController = UnityEngine.Object.FindObjectOfType<SlugterraSceneController>();
				if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(slugterraSceneController.GetType()).Type == LevelType.Arena)
				{
					MonoSingleton<DuelManager>.Instance.SetBossTarget();
				}
				RespawnFollowers(localPlayer);
				ComeBackObjectiveTarget[] array = UnityEngine.Object.FindObjectsOfType<ComeBackObjectiveTarget>();
				ComeBackObjectiveTarget[] array2 = array;
				foreach (ComeBackObjectiveTarget comeBackObjectiveTarget in array2)
				{
					comeBackObjectiveTarget.Reset();
				}
				slugterraSceneController.OnRespawn();
				gameModel.RespawnCount++;
				gameModel.GameFailed = false;
			}
			return localPlayer;
		}

		private static void RespawnFollowers(PlayerController player)
		{
			FollowerController[] array = UnityEngine.Object.FindObjectsOfType<FollowerController>();
			FollowerController[] array2 = array;
			foreach (FollowerController followerController in array2)
			{
				if (followerController.enabled && followerController.Type == AllyType.Follower)
				{
					followerController.Model.Health = followerController.Model.MaxHealth;
					followerController.OnRespawn();
				}
				else
				{
					AllyType type = followerController.Type;
					if (type == AllyType.Follower)
					{
						UnityEngine.Object.Destroy(followerController.GameObject);
						GameObject assistant = GameFactory.GetAssistant(type);
						GameObject gameObject = UnityEngine.Object.Instantiate(assistant);
						WarNode node = MonoSingleton<WarSystem>.Instance.GetNode(player.Transform, player.Transform, 2f);
						if (node != null)
						{
							Transform optimalPoint = node.GetOptimalPoint(player.Transform.position, player.Transform.position, 2f);
							gameObject.transform.position = optimalPoint.position;
						}
						else
						{
							gameObject.transform.position = player.transform.position;
						}
					}
				}
				MonoCommand.ExecuteOn<ShieldCommand>(followerController.GameObject, new object[2] { followerController.Model, 3f });
			}
		}

		public static void OnAddLocalPlayer(Action<PlayerController> onAdd)
		{
			if (LocalPlayer != null)
			{
				onAdd(LocalPlayer);
			}
			else
			{
				OnAddLocalPlayerEvent += onAdd;
			}
		}

		public static void OnDuel(Action<PlayerController, PlayerController> onStart)
		{
			OnDuelEvent += onStart;
			DuelCheck();
		}

		public static void Remove(PlayerController player)
		{
			if (player != null)
			{
				_players.Remove(player);
				if (LocalPlayer == player)
				{
					LocalPlayer = null;
				}
			}
		}

		private static void DuelCheck()
		{
			if (OnDuelEvent__BackingField != null && _players != null && _players.Count >= 2 && LocalPlayer != null)
			{
				PlayerController localPlayer = LocalPlayer;
				PlayerController playerController = null;
				if (playerController != null)
				{
					OnDuelEvent__BackingField(localPlayer, playerController);
					OnDuelEvent__BackingField = null;
				}
			}
		}

		private static void RaiseAddPlayerEvents(PlayerController player)
		{
			if (OnAddPlayerEvent__BackingField != null)
			{
				OnAddPlayerEvent__BackingField(player);
			}
			if (player.IsLocal && OnAddLocalPlayerEvent__BackingField != null)
			{
				OnAddLocalPlayerEvent__BackingField(player);
				OnAddLocalPlayerEvent__BackingField = null;
			}
			DuelCheck();
		}
	}
}
