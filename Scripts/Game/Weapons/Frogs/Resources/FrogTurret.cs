using Godot;
using System;

public partial class FrogTurret : Resource
{
	[Export] public Projectile turretProjectile;
	[Export] public float turretBaseFireRate;
	[Export] public float turretBaseDamage;
	[Export] public float turretLifeTime;
	[Export] public Texture2D turretSprite;
	[Export] public TurretAimType turretAimType;
}

public enum TurretAimType
{
	Pattern,
	Aimed
}
