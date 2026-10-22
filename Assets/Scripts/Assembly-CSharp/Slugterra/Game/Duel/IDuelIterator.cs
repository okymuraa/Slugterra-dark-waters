namespace Slugterra.Game.Duel
{
	public interface IDuelIterator
	{
		int Current { get; set; }

		int Length { get; }

		int Prev();

		int Next();

		bool IsPrev();

		bool IsNext();
	}
}
