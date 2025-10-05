using Godot;
using System;

public partial class FrogWeaponMelee : FrogWeapon
{
	[Export] public float frogSwingStartLag;
	[Export] public float frogSwingUptime;
	[Export] public float frogSwingEndLag;
	
	[Export] public float frogSwingBaseDamage;
	[Export] public float frogSwingSize;
	[Export] public float frogSwingAngle;
	[Export] public float frogSwingSprite;
}
