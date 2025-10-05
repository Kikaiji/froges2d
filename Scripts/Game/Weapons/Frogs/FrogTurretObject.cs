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
	
	public void Setup(FrogTurret turretResource){
		TurretResource = turretResource;
		TurretSprite.Texture = TurretResource.turretSprite;
		
		_turretFireRate = TurretResource.turretBaseFireRate;
		_turretCurrentLifeTime = TurretResource.turretLifeTime;
		_turretInitLifeTime = TurretResource.turretLifeTime;
		TurretHealthDisplay.MaxValue = _turretInitLifeTime;
	}
	
	public override void _PhysicsProcess(double delta){
		
		_turretCurrentLifeTime -= (float)delta;
		TurretHealthDisplay.Value = _turretCurrentLifeTime;
		if(_turretCurrentLifeTime <= 0){
			QueueFree();
		}
		
		_turretFireRate -= (float)delta;
		if(_turretFireRate <= 0){
			TurretFire();	
		}
	}
	
	private void TurretFire(){
		
	}
	
	private void TurretHit(){
		
	}
}
