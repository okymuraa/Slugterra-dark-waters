using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.Shops;

public class GameConsumableController : UIController<GameConsumableView>, IObserver
{
	private ConsumableItem _model;

	protected override void _OnEnable()
	{
		View.OnBuyEvent += OnBuy;
	}

	protected override void _OnDisable()
	{
		View.OnBuyEvent -= OnBuy;
	}

	public void ApplyModel(ConsumableItem item)
	{
		if (_model != null)
		{
			_model.RemoveObserver(this);
		}
		_model = item;
		_model.AddObserver(this);
		View.ApplyModel(_model);
	}

	private void OnBuy(int count)
	{
		ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
		shopModel.OnBuyConsumable(_model, count);
	}

	public void OnObjectChanged(object observable)
	{
		if (observable is ConsumableItem)
		{
			View.ApplyModel(observable as ConsumableItem);
		}
	}

	protected override void OnReleaseResource()
	{
		base.OnReleaseResource();
		if (_model != null)
		{
			_model.RemoveObserver(this);
		}
	}
}
