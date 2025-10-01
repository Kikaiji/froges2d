using Godot;
using System;

public partial class PlayerAttacker : Node2D
{
	[Export] public PlayerController PlayerBody;
	[Export] public Sprite2D WeaponSprite;
	
	[Export] public RayCast2D HitScanRay;
	
	public override void _Process(double delta){
		AimWeaponSprite();
	}
	
	public override void _PhysicsProcess(double delta){
		var rayCastDirection = PlayerBody.GlobalPosition + (PlayerBody.MousePosRelativeToPlayer.Normalized() * 500f);
		HitScanRay.TargetPosition = rayCastDirection;
	}
	
	public override void _EnterTree(){
		PlayerData.mainFrogChanged += UpdateWeaponSprite;
		PlayerData.playerJustAttacked += WeaponAttack;
	}
	
	public override void _ExitTree(){
		PlayerData.mainFrogChanged -= UpdateWeaponSprite;
		PlayerData.playerJustAttacked -= WeaponAttack;
	}
	
	private void AimWeaponSprite(){
		this.Rotation = PlayerBody.MousePosRelativeToPlayer.Angle();
		
		if(this.Rotation >= Mathf.Pi / 2 || this.Rotation <= -(Mathf.Pi / 2)){
			WeaponSprite.FlipV = true;
		} else {
			WeaponSprite.FlipV = false;
		}
	}
	
	private void UpdateWeaponSprite(FrogWeapon newFrog){
		if(newFrog == null){
			WeaponSprite.Texture = null;
			return;
		}
		
		WeaponSprite.Texture = newFrog.frogHoldSprite;
	}
	
	private void WeaponAttack(FrogWeapon currentWeapon, Vector2 mouseGlobalPosition){
		if(currentWeapon == null) return;
		switch(currentWeapon.frogType){
			case WeaponType.HitScan:
				HitScanWeaponAttack(currentWeapon as FrogWeaponHitScan, mouseGlobalPosition);
				break;
			case WeaponType.Projectile:
				ProjectileWeaponAttack(currentWeapon as FrogWeaponProjectile, mouseGlobalPosition);
				break;
			case WeaponType.Turret:
				TurretWeaponAttack(currentWeapon as FrogWeaponTurret, mouseGlobalPosition);
				break;
			case WeaponType.Melee:
				MeleeWeaponAttack(currentWeapon as FrogWeaponMelee, mouseGlobalPosition);
				break;
		}
	}
	
	private void HitScanWeaponAttack(FrogWeaponHitScan currentHitScanWeapon, Vector2 mouseGlobalPosition){
		
		var hitCollider = HitScanRay.GetCollider();
		if(hitCollider == null){
			GD.Print("Hit nothing");
			return;
		}
		CollisionObject2D hitCollisionObject = hitCollider as CollisionObject2D;
		
		if(PlayerBody.GlobalPosition.DistanceTo(hitCollisionObject.GlobalPosition) > currentHitScanWeapon.frogHitScanRange){
			GD.Print("Target too far");
			return;
		}
		
		GD.Print("HitCollider " + hitCollider);
		GD.Print("HitCollisionObject " + hitCollisionObject);
		
		if(hitCollisionObject.GetCollisionLayerValue(3)){
			GD.Print("hit the environment " + hitCollisionObject.Name);
			return;
		}
		
		if(hitCollisionObject.GetCollisionLayerValue(2)){
			//hit that enemy baby
			GD.Print("hit an enemy " + hitCollisionObject.Name);
		}
	}
	
	private void ProjectileWeaponAttack(FrogWeaponProjectile currentProjectileWeapon, Vector2 mouseGlobalPosition){
		
	}
	
	private void TurretWeaponAttack(FrogWeaponTurret currentTurretWeapon, Vector2 mouseGlobalPosition){
		
	}
	
	private void MeleeWeaponAttack(FrogWeaponMelee currentMeleeWeapon, Vector2 mouseGlobalPosition){
		
	}
}
