using Godot;
using System;

public partial class SubWeaponPanel : WeaponPanel
{
	public override void _EnterTree(){
		PlayerData.sideFrogChanged += LoadNewFrog;
	}
	
	public override void _ExitTree(){
		PlayerData.sideFrogChanged -= LoadNewFrog;
	}
}
