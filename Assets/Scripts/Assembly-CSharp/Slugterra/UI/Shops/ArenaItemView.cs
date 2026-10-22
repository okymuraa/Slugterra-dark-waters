using System;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using Slugterra.UI.Utils;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class ArenaItemView : UIView
	{
		public Action OnBtnEquipItem;

		public Action<string> OnUpdateRivals;

		[SerializeField]
		private GameObject _information;

		[SerializeField]
		private GameObject _menu;

		[SerializeField]
		private ArenaItemStatus _status;

		private bool _curInfoState;

		[SerializeField]
		private TextMesh _title;

		[SerializeField]
		private TextMesh _description;

		[SerializeField]
		private TextMesh _lockedText;

		[SerializeField]
		private tk2dSprite _itemPreview;

		protected EnemyType _itemValue;

		protected ArenaItemViewType _itemViewType = ArenaItemViewType.None;

		protected FoeItem _item;

		[SerializeField]
		private tk2dButton[] _curButton;

		[SerializeField]
		private tk2dUIItem[] _curButtonUi;

		public EnemyType ItemValue
		{
			get
			{
				return _itemValue;
			}
		}

		public ArenaItemViewType ItemViewType
		{
			get
			{
				return _itemViewType;
			}
			set
			{
				_itemViewType = value;
			}
		}

		public FoeItem Item
		{
			get
			{
				return _item;
			}
			set
			{
				_item = value;
			}
		}

		public tk2dButton CurButton(int Index)
		{
			return _curButton[Index];
		}

		public tk2dUIItem CurButtonUi(int Index)
		{
			return _curButtonUi[Index];
		}

		public void ShowEnemy(EnemyType type)
		{
			_itemValue = type;
			if (_itemValue == EnemyType.BossPronto && !GetIsShown(_itemValue.ToString()))
			{
				SetIsShown(_itemValue.ToString());
			}
			FoeConfig foesConfig = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetFoesConfig(type);
			_title.text = Localization.GetValue(foesConfig.title, string.Empty);
			_description.text = Localization.GetValue(foesConfig.description, string.Empty);
			string foeSpriteName = SpriteNamesHelper.GetFoeSpriteName(type);
			_itemPreview.SetSprite(foeSpriteName);
			UpdateItem();
		}

		public void UpdateForTutor()
		{
			_status.SetStatus(_itemViewType);
		}

		public void UpdateItem()
		{
			DuelSetting duelSetting = MonoSingleton<AppController>.Instance.Model.DuelSetting;
			if (_item == duelSetting.Foe)
			{
				_itemViewType = ArenaItemViewType.Acquired;
			}
			else
			{
				string levelWhenBossWillUnlocked = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelWhenBossWillUnlocked(_itemValue.ToString());
				LevelItem levelItemByLevelName = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName(levelWhenBossWillUnlocked);
				bool flag = false;
				if (levelItemByLevelName != null)
				{
					flag = levelItemByLevelName.Status != LevelStatus.Completed;
					_lockedText.text = MonoSingleton<Localization>.Instance.GetValueByKey("menu_item_unlock_title") + levelItemByLevelName.Name;
				}
				if (!flag)
				{
					_itemViewType = (GetIsShown(_itemValue.ToString()) ? (_itemViewType = ArenaItemViewType.NeedEquip) : (_itemViewType = ArenaItemViewType.New));
				}
				else
				{
					_itemViewType = ArenaItemViewType.Locked;
				}
			}
			_status.SetStatus(_itemViewType);
		}

		public void OnEquipButtonClick()
		{
			DuelSetting duelSetting = MonoSingleton<AppController>.Instance.Model.DuelSetting;
			FoeItem foe = duelSetting.Foe;
			if (OnBtnEquipItem != null)
			{
				OnBtnEquipItem();
			}
			if (foe != null && OnUpdateRivals != null)
			{
				OnUpdateRivals(foe.Type);
			}
			if (!GetIsShown(_itemValue.ToString()))
			{
				SetIsShown(_itemValue.ToString());
			}
			UpdateItem();
		}

		public void OnInfoBtnClick()
		{
			_curInfoState = !_curInfoState;
			_information.SetActive(_curInfoState);
			_menu.SetActive(!_curInfoState);
		}

		private void SetIsShown(string arenaBoss)
		{
			PlayerPrefs.SetInt(arenaBoss + "IsShown", 1);
		}

		private bool GetIsShown(string arenaBoss)
		{
			return PlayerPrefs.GetInt(arenaBoss + "IsShown", 0) == 1;
		}
	}
}
