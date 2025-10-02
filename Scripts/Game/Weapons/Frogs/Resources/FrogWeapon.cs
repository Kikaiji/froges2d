using Godot;
using System;

public partial class FrogWeapon : Resource
{
	[Export] public string frogName;
	[Export] public WeaponType frogType;
	[Export] public FiringType frogFiringType;
	[Export] public string frogId;
	[Export] public int frogMaxAmmo;
	[Export] public Texture2D frogIcon;
	[Export] public Texture2D frogHoldSprite;
	[Export] public float frogBarrelDistance;
}


public enum WeaponType
{
	HitScan,
	Projectile,
	Turret,
	Melee	
}

public enum FiringType
{
	Auto,
	Semi,
	BurstAuto,
	BurstSemi
}
