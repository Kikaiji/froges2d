using Godot;
using System;

public partial class FrogWeaponTurret : FrogWeapon
{
	[Export] public FrogTurret frogWeaponTurretSpawn;
	[Export] public int frogMaxTurrets;
	[Export] public float frogTurretRechargeSpeed;
	[Export] public float frogTurretPlacementRange;
}
