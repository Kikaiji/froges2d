using Godot;
using System;

public partial class EnemyBase : RigidBody2D
{
	public int EnemyBaseHealth = 10;
	
	public void HitEnemy(float damageTaken){
		EnemyOnHitEffects(damageTaken);
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
