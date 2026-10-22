namespace Slugterra.Game.Duel
{
	public class CircleIterator : DuelIterator
	{
		public CircleIterator(int length)
			: base(length)
		{
		}

		public override int Prev()
		{
			Current--;
			if (Current < 0)
			{
				Current = Length - 1;
			}
			return Current;
		}

		public override int Next()
		{
			Current++;
			if (Current >= Length)
			{
				Current = 0;
			}
			return Current;
		}

		public override bool IsPrev()
		{
			return true;
		}

		public override bool IsNext()
		{
			return true;
		}
	}
}
