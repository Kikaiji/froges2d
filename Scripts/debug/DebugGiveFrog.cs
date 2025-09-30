using Godot;
using System;

public partial class DebugGiveFrog : Panel
{
	public FrogWeapon frogWeapon;
	
	public void Setup(){
		var frogButton = GetChild(0) as Button;
		frogButton.Text = frogWeapon.frogName;
		frogButton.Icon = frogWeapon.frogIcon;
	}
	
	public void GiveFrogWeapon(){
		PlayerData.Instance.AddFrogWeaponToPlayer(frogWeapon);
	}
}
