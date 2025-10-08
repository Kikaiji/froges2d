using Godot;
using System;

public partial class FrogTurretObject : RigidBody2D
{
	[Export] public Sprite2D TurretSprite;
	[Export] public FrogTurret TurretResource;
	[Export] public PackedScene ProjectilePrefab;
	[Export] public ProgressBar TurretHealthDisplay;
	
	private float _turretInitLifeTime = 10f;
	private float _turretCurrentLifeTime = 10f;
	private float _turretFireRate = 0f;
	private EnemyBase _currentTarget;
	
	private float _turretTargetRefresh = 0f;
	private float _turretTargetRefreshRate = 0.25f;
	
	public void Setup(FrogTurret turretResource){
		TurretResource = turretResource;
		TurretSprite.Texture = TurretResource.turretSprite;
		
		_turretFireRate = TurretResource.turretBaseFireRate;
		_turretCurrentLifeTime = TurretResource.turretLifeTime;
		_turretInitLifeTime = TurretResource.turretLifeTime;
		TurretHealthDisplay.MaxValue = _turretInitLifeTime;
	}
	
	public override void _EnterTree(){
		EnemyData.enemyKilled += TargetMaybeKilled;
	}
	
	public override void _ExitTree(){
		EnemyData.enemyKilled -= TargetMaybeKilled;
	}
	
	public override void _PhysicsProcess(double delta){
		
		_turretCurrentLifeTime -= (float)delta;
		TurretHealthDisplay.Value = _turretCurrentLifeTime;
		if(_turretCurrentLifeTime <= 0){
			PlayerData.Instance.PlayerTurretJustKilled();
			QueueFree();
		}
		
		_turretFireRate -= (float)delta;
		if(_turretFireRate <= 0){
			TurretFire();	
		}
		
		_turretTargetRefresh -= (float)delta;
		if(_turretTargetRefresh <= 0){
			RefreshTarget();
			_turretTargetRefresh = _turretTargetRefreshRate;
		}
	}
	
	private void TurretFire(){
		if(_currentTarget == null || (TurretResource.turretVisionRange < GlobalPosition.DistanceTo(_currentTarget.GlobalPosition))) return;
		
		var newProjectile = ProjectilePrefab.Instantiate() as ProjectileObject;
		Vector2 fireDirection = (_currentTarget.GlobalPosition - GlobalPosition);
		
		newProjectile.Setup(TurretResource.turretProjectile, fireDirection.Normalized());
		GetTree().GetCurrentScene().AddChild(newProjectile);
		newProjectile.GlobalPosition = GlobalPosition + (fireDirection.Normalized() * TurretResource.turretBarrelDistance);
		
		_turretFireRate = TurretResource.turretBaseFireRate;
	}
	
	private void TurretHit(){
		
	}
	
	private void TargetMaybeKilled(EnemyBase enemy){
		if(enemy == _currentTarget) RefreshTarget();
	}
	
	private void RefreshTarget(){
		_currentTarget = EnemyData.Instance.FindClosestEnemy(GlobalPosition);
	}
}
