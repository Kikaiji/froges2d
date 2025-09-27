using Godot;
using System;

public partial class FrogWeapon : Resource
{
	[Export] public string frogName;
	[Export] public WeaponType frogType;
	[Export] public string frogId;
	[Export] public int frogMaxAmmo;
	
}


public enum WeaponType
{
	Projectile,
	Turret,
	Melee	
}
