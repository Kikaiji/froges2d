using Godot;
using System;

public partial class FrogWeapon : Resource
{
	[Export] public string frogName;
	[Export] public WeaponType frogType;
	[Export] public string frogId;
	[Export] public int frogMaxAmmo;
	[Export] public CompressedTexture2D frogIcon;
	
}


public enum WeaponType
{
	Hitscan,
	Projectile,
	Turret,
	Melee	
}
