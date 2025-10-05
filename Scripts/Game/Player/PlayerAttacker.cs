using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerAttacker : Node2D
{
	[Export] public PlayerController PlayerBody;
	[Export] public Sprite2D WeaponSprite;
	
	[Export] public RayCast2D HitScanRay;
	[Export] public PackedScene HitScanLine;
	
	[Export] public PackedScene ProjectilePrefab;
	[Export] public PackedScene TurretPrefab;
	[Export] public ProgressBar DelayDisplay;
	[Export] public MeleeSwingTrail MeleeTrail;
	
	private Dictionary<FrogWeapon, FrogTurretObject[]> _placedTurrets;
	
	private List<EnemyBase> _enemiesInRange;
	
	private bool _attackHeld = false;
	
	private float _firingDelay = 0f;
	private float _initFiringDelay = 0f;
	private bool _ammoRegen = false;
	private float _regenTimer = 0f;
	private float _initRegenTimer = 0f;
	
	private float _sideFiringDelay = 0f;
	private float _sideInitFiringDelay = 0f;
	private bool _sideAmmoRegen = false;
	private float _sideRegenTimer = 0f;
	private float _sideInitRegenTimer = 0f;
	
	

	
	public override void _Ready(){
		_placedTurrets = new Dictionary<FrogWeapon, FrogTurretObject[]>();
		_enemiesInRange = new List<EnemyBase>();
	}
	
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
		
		if(_ammoRegen && PlayerData.Instance.MainFrog != null && (PlayerData.Instance.MainCurrentAmmo < PlayerData.Instance.MainFrog.frogMaxAmmo)){
			_regenTimer -= (float)delta;
			if(_regenTimer <= 0){
				PlayerData.Instance.TryAddAmmo(1);
				_regenTimer = _initRegenTimer;
			}
		}
		
		if(_sideAmmoRegen && PlayerData.Instance.SideFrog != null && (PlayerData.Instance.SideCurrentAmmo < PlayerData.Instance.SideFrog.frogMaxAmmo)){
			_sideRegenTimer -= (float)delta;
			if(_sideRegenTimer <= 0){
				PlayerData.Instance.TryAddSideAmmo(1);
				_sideRegenTimer = _sideInitRegenTimer;
			}
		}
		
		_sideFiringDelay -= (float)delta;
	}
	
	private void SetFiringDelay(float newDelay){
		_firingDelay = newDelay;
		_initFiringDelay = newDelay;
		DelayDisplay.Visible = true;
	}
	
	private void StartAmmoRegen(float newTimer){
		_regenTimer = newTimer;
		_initRegenTimer = newTimer;
		_ammoRegen = true;
	}
	
	private void StopAmmoRegen(){
		_ammoRegen = false;
		_regenTimer = 0f;
		_initRegenTimer = 0f;
	}
	
	public override void _EnterTree(){
		PlayerData.mainFrogChanged += UpdateWeaponSprite;
		PlayerData.playerJustAttacked += StartAttack;
		PlayerData.playerJustStopAttack += StopAttack;
		PlayerData.swapFrogs += SwapWeaponDelays;
		PlayerData.mainFrogChanged += MainFrogChanged;
		PlayerData.sideFrogChanged += SideFrogChanged;
	}
	
	public override void _ExitTree(){
		PlayerData.mainFrogChanged -= UpdateWeaponSprite;
		PlayerData.playerJustAttacked -= StartAttack;
		PlayerData.playerJustStopAttack -= StopAttack;
		PlayerData.swapFrogs -= SwapWeaponDelays;
		PlayerData.mainFrogChanged -= MainFrogChanged;
		PlayerData.sideFrogChanged -= SideFrogChanged;
	}
	
	private void SwapWeaponDelays(FrogWeapon weapon){
		(_firingDelay, _sideFiringDelay) = (_sideFiringDelay, _firingDelay);
		(_initFiringDelay, _sideInitFiringDelay) = (_sideInitFiringDelay, _initFiringDelay);
		(_ammoRegen, _sideAmmoRegen) = (_sideAmmoRegen, _ammoRegen);
		(_regenTimer, _sideRegenTimer) = (_sideRegenTimer, _regenTimer);
		(_initRegenTimer, _sideInitRegenTimer) = (_sideInitRegenTimer, _initRegenTimer);
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
		
		switch(currentWeapon.frogType){
		case WeaponType.HitScan:
			if((PlayerData.Instance.TryUseAmmo(1))) HitScanWeaponAttack(currentWeapon as FrogWeaponHitScan, mouseGlobalPosition);
			break;
		case WeaponType.Projectile:
			if((PlayerData.Instance.TryUseAmmo(1))) ProjectileWeaponAttack(currentWeapon as FrogWeaponProjectile, mouseGlobalPosition);
			break;
		case WeaponType.Turret:
			if((PlayerData.Instance.TryUseAmmo(1))) TurretWeaponAttack(currentWeapon as FrogWeaponTurret, mouseGlobalPosition);
			break;
		case WeaponType.Melee:
			MeleeWeaponAttack(currentWeapon as FrogWeaponMelee, mouseGlobalPosition);
			break;
		}
		PlayerData.Instance.PlayerJustFiredWeapon();
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
		
		if(PlayerData.Instance.MainCurrentAmmo <= 0){
			SetFiringDelay(currentHitScanWeapon.frogReloadSpeed);
			PlayerData.Instance.ReloadMainWeapon();
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
		
		StartAmmoRegen(currentTurretWeapon.frogTurretRechargeSpeed);
		
		FrogTurretObject[] currentWeaponTurrets = null;
		
		if(_placedTurrets.ContainsKey(currentTurretWeapon)){
			_placedTurrets.TryGetValue(currentTurretWeapon, out currentWeaponTurrets);
		} else {
			currentWeaponTurrets = new FrogTurretObject[currentTurretWeapon.frogMaxTurrets];
			_placedTurrets.Add(currentTurretWeapon, currentWeaponTurrets);
		}
		
		var firstEmpty = System.Array.IndexOf(currentWeaponTurrets, null);
		
		FrogTurretObject newTurret = TurretPrefab.Instantiate() as FrogTurretObject;
		
		if(firstEmpty >= 0){
			currentWeaponTurrets[firstEmpty] = newTurret;
		} else{
			currentWeaponTurrets[0].QueueFree();
			PlayerData.Instance.PlayerJustRemovedTurret();
			Array.Copy(currentWeaponTurrets, 1, currentWeaponTurrets, 0, currentWeaponTurrets.Length - 1);
			currentWeaponTurrets[currentWeaponTurrets.Length - 1] = newTurret;
		}
		
		Vector2 placementPos = Vector2.Zero;
		
		if(PlayerBody.MousePosRelativeToPlayer.Length() >= currentTurretWeapon.frogTurretPlacementRange){
			placementPos = PlayerBody.GlobalPosition + (PlayerBody.MousePosRelativeToPlayer.Normalized() * currentTurretWeapon.frogTurretPlacementRange);
		} else {
			placementPos = mouseGlobalPosition;
		}
		
		newTurret.Setup(currentTurretWeapon.frogWeaponTurretSpawn);
		GetTree().GetCurrentScene().AddChild(newTurret);
		newTurret.GlobalPosition = placementPos;
		
		PlayerData.Instance.PlayerJustPlacedTurret();
	}
	
	private void MeleeWeaponAttack(FrogWeaponMelee currentMeleeWeapon, Vector2 mouseGlobalPosition){
		Vector2 MouseDirection = PlayerBody.MousePosRelativeToPlayer.Normalized();
		
		Vector2 startPos = Vector2.FromAngle(MouseDirection.Angle() - Mathf.DegToRad(currentMeleeWeapon.frogSwingAngle/2));
		Vector2 endPos = Vector2.FromAngle(MouseDirection.Angle() + Mathf.DegToRad(currentMeleeWeapon.frogSwingAngle/2));
		
		GD.Print("Start " + startPos + ". End " + endPos);
		
		MeleeTrail.DisplayTrail(startPos, endPos, currentMeleeWeapon.frogSwingSize);
		
		for(int i = 0; i < _enemiesInRange.Count; i++){
			EnemyBase enemy = _enemiesInRange[i];
			float angleToEnemy = Mathf.RadToDeg(Mathf.Abs(MouseDirection.AngleTo(PlayerBody.GetPositionRelativeToPlayer(enemy.GlobalPosition))));
			GD.Print("Angle to enemy " + enemy + " is " + angleToEnemy);
			if(currentMeleeWeapon.frogSwingAngle * 0.5f > angleToEnemy){
				enemy.HitEnemy(PlayerData.Instance.CalculateDamage(currentMeleeWeapon.frogSwingBaseDamage));
				PlayerData.Instance.PlayerJustHitEnemy();
				GD.Print("Hit " + enemy);
			}
		}
		
		SetFiringDelay(currentMeleeWeapon.frogSwingEndLag);
	}
	
	public void BodyEnteredMeleeArea(Node2D otherBody){
		EnemyBase enemyBody = otherBody as EnemyBase;
		if(enemyBody != null){
			GD.Print("Enemy " + enemyBody + " Entered");
			_enemiesInRange.Add(enemyBody);
		}
	}
	
	public void BodyExitedMeleeArea(Node2D otherBody){
		EnemyBase enemyBody = otherBody as EnemyBase;
		
		if(enemyBody != null && _enemiesInRange.Contains(enemyBody)){
			GD.Print("Enemy " + enemyBody + " Left");
			_enemiesInRange.Remove(enemyBody);
		}
	}
}
