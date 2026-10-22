namespace Slugterra.Game.Duel
{
	public abstract class DuelIterator : IDuelIterator
	{
		private int _length;

		protected int _current;

		public int Current
		{
			get
			{
				return _current;
			}
			set
			{
				_current = value;
			}
		}

		public int Length
		{
			get
			{
				return _length;
			}
		}

		public DuelIterator(int length)
		{
			_length = length;
			_current = 0;
		}

		public abstract int Prev();

		public abstract int Next();

		public abstract bool IsPrev();

		public abstract bool IsNext();
	}
}
