using Godot;
using System;

public partial class WeaponPanel : Panel
{
	protected TextureRect _weaponIcon;
	protected TextureRect _typeIcon;
	protected Label _weaponName;
	
	public override void _Ready(){
		_weaponIcon = GetChild(0) as TextureRect;
		_typeIcon = GetChild(1) as TextureRect;
		_weaponName = GetChild(2) as Label;
	}
	
	protected void LoadNewFrog(FrogWeapon newFrog){
		if(newFrog == null){
			_weaponIcon.Texture = new CompressedTexture2D();
			_weaponName.Text = "";
			this.Visible = false;
			return;
		}
		this.Visible = true;
		_weaponIcon.Texture = newFrog.frogIcon;
		_weaponName.Text = newFrog.frogName;
	}
}
