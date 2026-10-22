namespace AppsMinistry.Domain.Observer
{
	public interface IObserver
	{
		void OnObjectChanged(object observable);
	}
}
