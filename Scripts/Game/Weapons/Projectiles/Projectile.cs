using Godot;
using System;

public partial class Projectile : Resource
{
	[Export] public float projectileBaseSpeed;
	[Export] public float projectileScaleX;
	[Export] public float projectileScaleY;
	
	[Export] public Shape2D projectileCollisionShape;
	[Export] public Texture2D projectileSprite;
	[Export] public float projectileLifeTime;
}
