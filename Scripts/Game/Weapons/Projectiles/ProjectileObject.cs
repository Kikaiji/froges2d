using Godot;
using System;

public partial class ProjectileObject : RigidBody2D
{
	public float ProjectileSpeed;
	public float ProjectileLifeTime;
	public int ProjectileBounceCount;
	public float ProjectileKnockBack;
	
	private Vector2 currentFlightDirection;
	
	[Export] public CollisionShape2D ProjectileShape;
	[Export] public Sprite2D ProjectileSprite;
	
	public void Setup(Projectile projectileResource, Vector2 flightDirection){
		ProjectileShape.Shape = projectileResource.projectileCollisionShape;
		ProjectileSprite.Texture = projectileResource.projectileSprite;
		ProjectileSprite.GlobalRotation = flightDirection.Angle();
		
		ProjectileSpeed = projectileResource.projectileBaseSpeed;
		ProjectileLifeTime = projectileResource.projectileLifeTime;
		ProjectileBounceCount = projectileResource.projectileBounceCount;
		ProjectileKnockBack = projectileResource.projectileBaseKnockBack;
		
		this.ApplyCentralForce(flightDirection * ProjectileSpeed);
		currentFlightDirection = flightDirection;
		this.Scale = new Vector2(projectileResource.projectileScaleX, projectileResource.projectileScaleY);
	}
	
	public override void _PhysicsProcess(double delta){
		ProjectileLifeTime -= (float)delta;
		
		if(ProjectileLifeTime <= 0){
			QueueFree();
		}
	}
	
	public void HitObject(Node other){
		
		EnemyBase enemy = other as EnemyBase;
		
		if(enemy != null){
			Vector2 direction = (enemy.GlobalPosition - this.GlobalPosition);
			direction = direction.Normalized() + currentFlightDirection;
			enemy.EnemyApplyKnockBack(direction, ProjectileKnockBack); 
		}
		
		if(ProjectileBounceCount <= 0) {
			QueueFree();
			return;
		}
		
		ProjectileBounceCount -= 1;
		currentFlightDirection = LinearVelocity.Normalized();
	}	
}
