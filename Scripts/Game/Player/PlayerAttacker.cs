using Godot;
using System;

public partial class PlayerAttacker : Node2D
{
	[Export] public PlayerController PlayerBody;
	[Export] public Sprite2D WeaponSprite;
	
	[Export] public RayCast2D HitScanRay;
	[Export] public PackedScene HitScanLine;
	
	[Export] public PackedScene ProjectilePrefab;
	[Export] public ProgressBar DelayDisplay;
	
	private float _firingDelay = 0f;
	private float _initFiringDelay = 0f;
	private float _sideFiringDelay = 0f;
	private float _sideInitFiringDelay = 0f;
	private bool _attackHeld = false;
	
	public override void _Process(double delta){
		AimWeaponSprite();
		
		if(_attackHeld && _firingDelay <= 0) WeaponAttack(PlayerData.Instance.MainFrog, GetGlobalMousePosition()); 
	}
	
	public override void _PhysicsProcess(double delta){
		var rayCastDirection = PlayerBody.GlobalPosition + (PlayerBody.MousePosRelativeToPlayer.Normalized() * 500f);
		HitScanRay.TargetPosition = rayCastDirection;
		
		if(_firingDelay > 0){
			_firingDelay -= (float)delta;
			DelayDisplay.Value = (_firingDelay / _initFiringDelay) * 100;
		} else{
			DelayDisplay.Visible = false;
		}
		
		_sideFiringDelay -= (float)delta;
	}
	
	private void SetFiringDelay(float newDelay){
		_firingDelay = newDelay;
		_initFiringDelay = newDelay;
		DelayDisplay.Visible = true;
	}
	
	public override void _EnterTree(){
		PlayerData.mainFrogChanged += UpdateWeaponSprite;
		PlayerData.playerJustAttacked += StartAttack;
		PlayerData.playerJustStopAttack += StopAttack;
		PlayerData.swapFrogs += SwapFiringDelay;
		PlayerData.mainFrogChanged += MainFrogChanged;
		PlayerData.sideFrogChanged += SideFrogChanged;
	}
	
	public override void _ExitTree(){
		PlayerData.mainFrogChanged -= UpdateWeaponSprite;
		PlayerData.playerJustAttacked -= StartAttack;
		PlayerData.playerJustStopAttack -= StopAttack;
		PlayerData.swapFrogs -= SwapFiringDelay;
		PlayerData.mainFrogChanged -= MainFrogChanged;
		PlayerData.sideFrogChanged -= SideFrogChanged;
	}
	
	private void SwapFiringDelay(FrogWeapon weapon){
		(_firingDelay, _sideFiringDelay) = (_sideFiringDelay, _firingDelay);
	}
	
	private void MainFrogChanged(FrogWeapon weapon){
		if(weapon != null){
			SetFiringDelay(0.5f);
			return;
		}
		
		SetFiringDelay(0);
	}
	
	private void SideFrogChanged(FrogWeapon weapon){
		if(weapon != null){
			_sideFiringDelay = 0.5f;
			_sideInitFiringDelay = 0.5f;
			return;
		}
		
		_sideFiringDelay = 0f;
		_sideInitFiringDelay = 0f;
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
	
	private void StartAttack(FrogWeapon currentWeapon, Vector2 mouseGlobalPosition){
		if(currentWeapon == null ) return;
		
		switch(currentWeapon.frogFiringType){
				case FiringType.Auto:
					_attackHeld = true;
					break;
				case FiringType.Semi:
					if(_firingDelay < 0) WeaponAttack(currentWeapon, mouseGlobalPosition);
					break;
		}
		
	}
	
	private void StopAttack(FrogWeapon currentWeapon, Vector2 mouseGlobalPosition){
		_attackHeld = false;
	}
	
	private void WeaponAttack(FrogWeapon currentWeapon, Vector2 mouseGlobalPosition){
		if((PlayerData.Instance.TryUseAmmo(1))){
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
			PlayerData.Instance.PlayerJustFiredWeapon();
			
			if(PlayerData.Instance.MainCurrentAmmo <= 0){
				SetFiringDelay(currentWeapon.frogReloadSpeed);
				PlayerData.Instance.ReloadMainWeapon();
			}
		} else{
			SetFiringDelay(currentWeapon.frogReloadSpeed);
			PlayerData.Instance.ReloadMainWeapon();
		}
	}
	
	private void HitScanWeaponAttack(FrogWeaponHitScan currentHitScanWeapon, Vector2 mouseGlobalPosition){
		
		SetFiringDelay(currentHitScanWeapon.frogBaseFireRate);
		
		var hitScanLine = HitScanLine.Instantiate() as HitScanLine;
		GetTree().GetCurrentScene().AddChild(hitScanLine);
		hitScanLine.GlobalPosition = PlayerBody.GlobalPosition + (PlayerBody.MousePosRelativeToPlayer.Normalized() * currentHitScanWeapon.frogBarrelDistance);
		var lineEnd = PlayerBody.MousePosRelativeToPlayer.Normalized() * currentHitScanWeapon.frogHitScanRange;
		
		var hitCollider = HitScanRay.GetCollider();
		
		if(hitCollider == null){
			GD.Print("Hit nothing");
			hitScanLine.DisplayHitScanLine(lineEnd, currentHitScanWeapon.frogHitScanLineSprite, currentHitScanWeapon.frogHitScanLineWidth, 0.2f);
			return;
		}
		
		HitScanRay.ForceRaycastUpdate();
		var hitPoint = HitScanRay.GetCollisionPoint();
		CollisionObject2D hitCollisionObject = hitCollider as CollisionObject2D;
		
		if(PlayerBody.GlobalPosition.DistanceTo(hitCollisionObject.GlobalPosition) > currentHitScanWeapon.frogHitScanRange + currentHitScanWeapon.frogBarrelDistance){
			GD.Print("Target too far");
			hitScanLine.DisplayHitScanLine(lineEnd, currentHitScanWeapon.frogHitScanLineSprite, currentHitScanWeapon.frogHitScanLineWidth, 0.04f);
			return;
		}
		
		
		hitScanLine.DisplayHitScanLine(hitPoint - hitScanLine.GlobalPosition, currentHitScanWeapon.frogHitScanLineSprite, currentHitScanWeapon.frogHitScanLineWidth, 0.2f);
		GD.Print("HitCollider " + hitCollider);
		GD.Print("HitCollisionObject " + hitCollisionObject);
		
		//Layer 3 = Environment
		if(hitCollisionObject.GetCollisionLayerValue(3)){
			PlayerData.Instance.PlayerJustHitEnvironment();
			GD.Print("hit the environment " + hitCollisionObject.Name);
			return;
		}
		
		//Layer 2 = Enemy
		if(hitCollisionObject.GetCollisionLayerValue(2)){
			//hit that enemy baby
			EnemyBase enemy = hitCollider as EnemyBase;
			if(enemy != null){
				enemy.HitEnemy(PlayerData.Instance.CalculateDamage(currentHitScanWeapon.frogBaseDamage));
			}
			PlayerData.Instance.PlayerJustHitEnemy();
			GD.Print("hit an enemy " + hitCollisionObject.Name);
		}
	}
	
	private void ProjectileWeaponAttack(FrogWeaponProjectile currentProjectileWeapon, Vector2 mouseGlobalPosition){
		var newProjectile = ProjectilePrefab.Instantiate() as ProjectileObject;
		newProjectile.Setup(currentProjectileWeapon.frogWeaponProjectile, PlayerBody.MousePosRelativeToPlayer.Normalized());
		GetTree().GetCurrentScene().AddChild(newProjectile);
		newProjectile.GlobalPosition = PlayerBody.GlobalPosition + (PlayerBody.MousePosRelativeToPlayer.Normalized() * currentProjectileWeapon.frogBarrelDistance);
		
		SetFiringDelay(currentProjectileWeapon.frogBaseFireRate);
	}
	
	private void TurretWeaponAttack(FrogWeaponTurret currentTurretWeapon, Vector2 mouseGlobalPosition){
		
	}
	
	private void MeleeWeaponAttack(FrogWeaponMelee currentMeleeWeapon, Vector2 mouseGlobalPosition){
		
	}
}
