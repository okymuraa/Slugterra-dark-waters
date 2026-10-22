using System.Collections.Generic;
using AppsMinistry.Domain;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Domain.Shops;
using Slugterra.Game.Duel;

namespace Slugterra.Domain.UI.Game
{
	public class InGameModel : DomainObject
	{
		private LevelType _levelType;

		private Humanoid _eliModel;

		private Humanoid _assistantModel;

		private Humanoid _bossModel;

		private Slug _attackSlug;

		private Slug _defenceSlug;

		private Slug _specialSlug;

		private bool _isUseAssistant;

		private bool _availableLeftArrow;

		private bool _availableRightArrow;

		private List<ConsumableItem> _consumableItems;

		private DuelState _duelState;

		public LevelType LevelType
		{
			get
			{
				return _levelType;
			}
		}

		public Humanoid Eli
		{
			get
			{
				return _eliModel;
			}
			set
			{
				_eliModel = value;
				CommitAndNotify();
			}
		}

		public Humanoid Assistant
		{
			get
			{
				return _assistantModel;
			}
			set
			{
				_assistantModel = value;
				CommitAndNotify();
			}
		}

		public Humanoid Boss
		{
			get
			{
				return _bossModel;
			}
			set
			{
				_bossModel = value;
				CommitAndNotify();
			}
		}

		public Slug AttackSlug
		{
			get
			{
				return _attackSlug;
			}
			set
			{
				_attackSlug = value;
				CommitAndNotify();
			}
		}

		public Slug DefenceSlug
		{
			get
			{
				return _defenceSlug;
			}
			set
			{
				_defenceSlug = value;
				CommitAndNotify();
			}
		}

		public Slug SpecialSlug
		{
			get
			{
				return _specialSlug;
			}
			set
			{
				_specialSlug = value;
				CommitAndNotify();
			}
		}

		public bool IsUseAssistant
		{
			get
			{
				return _isUseAssistant;
			}
			set
			{
				_isUseAssistant = value;
				CommitAndNotify();
			}
		}

		public bool IsLeftArrow
		{
			get
			{
				return _availableLeftArrow;
			}
			set
			{
				_availableLeftArrow = value;
				CommitAndNotify();
			}
		}

		public bool IsRightArrow
		{
			get
			{
				return _availableRightArrow;
			}
			set
			{
				_availableRightArrow = value;
				CommitAndNotify();
			}
		}

		public List<ConsumableItem> ConsumableItems
		{
			get
			{
				return _consumableItems;
			}
			set
			{
				_consumableItems = value;
				CommitAndNotify();
			}
		}

		public DuelState CurrentDuelState
		{
			get
			{
				return _duelState;
			}
			set
			{
				_duelState = value;
				CommitAndNotify();
			}
		}

		public InGameModel(LevelType levelType)
		{
			_levelType = levelType;
			_consumableItems = new List<ConsumableItem>();
			if (levelType == LevelType.Arena)
			{
				_availableLeftArrow = true;
				_availableRightArrow = true;
			}
		}
	}
}
