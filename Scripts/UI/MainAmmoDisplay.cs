using Godot;
using System;

public partial class MainAmmoDisplay : AmmoDisplay
{
	public override void _EnterTree(){
		PlayerData.mainAmmoChanged += SetCurrentAmmoDisplay;
	}
	
	public override void _ExitTree(){
		PlayerData.mainAmmoChanged -= SetCurrentAmmoDisplay;
	}
}
