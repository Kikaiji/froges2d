using Godot;
using System;

public partial class MeleeAreaShape : CollisionShape2D
{
	
	public override void _EnterTree(){
		PlayerData.mainFrogChanged += SetAreaRadius;
	}
	
	public override void _ExitTree(){
		PlayerData.sideFrogChanged -= SetAreaRadius;
	}
	
	private void SetAreaRadius(FrogWeapon weapon){
		if(weapon != null && weapon.frogType == WeaponType.Melee){
			FrogWeaponMelee melee = weapon as FrogWeaponMelee;
			this.Shape.SetDeferred("radius", melee.frogSwingSize);
		}
	}
}
