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
	
	protected void LoadNewFrog(FrogWeapon newFrog){
		if(newFrog == null){
			_maxAmmoDisplay.Text = "0";
			SetCurrentAmmoDisplay(0);
			return;
		}
		
		if(newFrog.frogType == WeaponType.Melee){
			this.Visible = false;
		} else {
			this.Visible = true;
		}
		
		_maxAmmoDisplay.Text = newFrog.frogMaxAmmo.ToString();
	}
	
	protected void SetCurrentAmmoDisplay(int count){
		_currentAmmoDisplay.Text = count.ToString();
	}
	
	private void SetMaximumAmmoDisplay(int count){
		
	}
}
