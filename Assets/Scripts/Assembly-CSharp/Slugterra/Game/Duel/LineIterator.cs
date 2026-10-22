namespace Slugterra.Game.Duel
{
	public class LineIterator : DuelIterator
	{
		public LineIterator(int length)
			: base(length)
		{
		}

		public override int Prev()
		{
			if (!IsPrev())
			{
				return -1;
			}
			Current--;
			return Current;
		}

		public override int Next()
		{
			if (!IsNext())
			{
				return -1;
			}
			Current++;
			return Current;
		}

		public override bool IsPrev()
		{
			if (Current == 0)
			{
				return false;
			}
			return true;
		}

		public override bool IsNext()
		{
			if (Current == Length - 1)
			{
				return false;
			}
			return true;
		}
	}
}
