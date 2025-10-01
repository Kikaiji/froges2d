using Godot;
using System;

public partial class FrogWeaponProjectile : FrogWeapon
{
	[Export] public Projectile frogWeaponProjectile;
	[Export] public float frogBaseFireRate;
	[Export] public float frogBaseDamage;
	[Export] public float frogReloadSpeed;
}
