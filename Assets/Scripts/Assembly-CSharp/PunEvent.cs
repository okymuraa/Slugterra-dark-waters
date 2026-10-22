internal class PunEvent
{
	public const byte RPC = 200;

	public const byte SendSerialize = 201;

	public const byte Instantiation = 202;

	public const byte CloseConnection = 203;

	public const byte Destroy = 204;

	public const byte RemoveCachedRPCs = 205;

	public const byte SendSerializeReliable = 206;

	public const byte DestroyPlayer = 207;

	public const byte AssignMaster = 208;

	public const byte OwnershipRequest = 209;

	public const byte OwnershipTransfer = 210;

	public const byte VacantViewIds = 211;

	public const byte Damage = 212;

	public const byte Reload = 213;

	public const byte Die = 214;

	public const byte SlugDie = 215;

	public const byte HealthChange = 216;

	public const byte StandUp = 217;
}
