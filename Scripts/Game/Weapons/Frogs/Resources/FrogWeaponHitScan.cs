using Godot;
using System;

public partial class FrogWeaponHitScan : FrogWeapon
{
	[Export] public float frogBaseFireRate;
	[Export] public float frogBaseDamage;
	[Export] public float frogHitScanRange;
	[Export] public Texture2D frogHitScanLineSprite;
	[Export] public float frogHitScanLineWidth;
	[Export] public int frogHitScanPierceCount;
}
