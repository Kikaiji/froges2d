using Godot;
using System;

public partial class AmmoDisplay : Control
{
	private Label _currentAmmoDisplay;
	private Label _maxAmmoDisplay;
	
	public override void _Ready(){
		_currentAmmoDisplay = GetChild(0) as Label;
		_maxAmmoDisplay = GetChild(2) as Label;
	}
	
	protected void SetCurrentAmmoDisplay(int count){
		_currentAmmoDisplay.Text = count.ToString();
	}
	
	private void SetMaximumAmmoDisplay(int count){
		
	}
}
