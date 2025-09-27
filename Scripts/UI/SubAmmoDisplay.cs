using Godot;
using System;

public partial class SubAmmoDisplay : AmmoDisplay
{
	public override void _EnterTree(){
		PlayerData.sideAmmoChanged += SetCurrentAmmoDisplay;
	}
	
	public override void _ExitTree(){
		PlayerData.sideAmmoChanged -= SetCurrentAmmoDisplay;
	}
}
