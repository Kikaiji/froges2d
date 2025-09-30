using Godot;
using System;

public partial class MainWeaponPanel : WeaponPanel
{
	public override void _EnterTree(){
		PlayerData.mainFrogChanged += LoadNewFrog;
	}
	
	public override void _ExitTree(){
		PlayerData.mainFrogChanged -= LoadNewFrog;
	}
}
