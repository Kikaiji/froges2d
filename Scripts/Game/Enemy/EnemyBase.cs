using Godot;
using System;

public partial class EnemyBase : RigidBody2D
{
	public int EnemyBaseHealth = 10;
	public float EnemyKnockBackMultiplier = 1f;
	
	[Export] public CpuParticles2D HitParticles;
	
	public void HitEnemy(float damageTaken){
		EnemyOnHitEffects(damageTaken);
	}
	
	public virtual void EnemyApplyKnockBack(Vector2 knockBackDirection, float knockBackForce){
		ApplyCentralForce(knockBackDirection * knockBackForce * EnemyKnockBackMultiplier);
		HitParticles.Direction = knockBackDirection;
		HitParticles.Restart();
	}
	
	protected virtual void EnemyOnHitEffects(float damageTaken){
		
	}
	
	public void KillEnemy(){
		PlayerData.Instance.PlayerJustKilledEnemy();
		EnemyOnDeathEffects();
	}
	
	protected virtual void EnemyOnDeathEffects(){
		
	}
}
