using Godot;
using System;

public partial class ProjectileObject : RigidBody2D
{
	public float ProjectileSpeed;
	public float ProjectileLifeTime;
	public int ProjectileBounceCount;
	
	[Export] public CollisionShape2D ProjectileShape;
	[Export] public Sprite2D ProjectileSprite;
	
	public void Setup(Projectile projectileResource, Vector2 flightDirection){
		ProjectileShape.Shape = projectileResource.projectileCollisionShape;
		ProjectileSprite.Texture = projectileResource.projectileSprite;
		ProjectileSprite.GlobalRotation = flightDirection.Angle();
		
		ProjectileSpeed = projectileResource.projectileBaseSpeed;
		ProjectileLifeTime = projectileResource.projectileLifeTime;
		ProjectileBounceCount = projectileResource.projectileBounceCount;
		
		this.ApplyCentralForce(flightDirection * ProjectileSpeed);
		
		this.Scale = new Vector2(projectileResource.projectileScaleX, projectileResource.projectileScaleY);
	}
	
	public override void _PhysicsProcess(double delta){
		ProjectileLifeTime -= (float)delta;
		
		if(ProjectileLifeTime <= 0){
			QueueFree();
		}
	}
	
	public void HitObject(Node other){
		if(ProjectileBounceCount <= 0) {
			QueueFree();
			return;
		}
		
		ProjectileBounceCount -= 1;
	}	
}
