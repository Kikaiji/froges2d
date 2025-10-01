using Godot;
using System;

public partial class FrogWeapon : Resource
{
	[Export] public string frogName;
	[Export] public WeaponType frogType;
	[Export] public string frogId;
	[Export] public int frogMaxAmmo;
	[Export] public Texture2D frogIcon;
	[Export] public Texture2D frogHoldSprite;
}


public enum WeaponType
{
	HitScan,
	Projectile,
	Turret,
	Melee	
}
