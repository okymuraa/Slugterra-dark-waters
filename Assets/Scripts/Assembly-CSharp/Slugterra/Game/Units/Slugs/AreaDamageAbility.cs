using System.Collections.Generic;
using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class AreaDamageAbility : SlugAbility
	{
		[SerializeField]
		private bool _isAttenuation;

		[SerializeField]
		[HideInInspector]
		private List<float> _damage;

		public List<float> Damage
		{
			get
			{
				if (_damage == null || _damage.Count < 5)
				{
					Init(ref _damage);
				}
				return _damage;
			}
		}

		protected override void OnUseNegativeEffect(Transform unitTransform)
		{
			base.OnUseNegativeEffect(unitTransform);
			MonoCommand.Execute<AreaDamageCommand>(new object[7]
			{
				gameObject.transform.position,
				Controller.Model.HittingArea,
				_damage[Controller.Model.Level],
				new int[1] { GetNegativeLayer() },
				new Attacker(Controller.Parent.GetType(), Controller.Parent.Transform, Controller.GetType(), Controller.Model.Type),
				null,
				_isAttenuation
			});
		}
	}
}
