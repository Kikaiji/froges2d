using Godot;
using System;

public partial class ItemBase
{
	public ItemResource itemRes = GD.Load<ItemResource>("res://Resources/Items/testItem.tres");
	
	public void Connect(){
		PlayerData.playerHitEnemy += PlayerHitEnemy;
		PlayerData.playerKillEnemy += PlayerKilledEnemy;
		PlayerData.playerHitEnvironment += PlayerHitEnvironment;
		
		PlayerData.playerHurt += PlayerHurt;
		PlayerData.playerFireWeapon += PlayerFire;
		PlayerData.playerReloadWeapon += PlayerReload;
		
		PlayerData.playerTurretPlaced += PlayerTurretPlaced;
		PlayerData.playerTurretRemoved += PlayerTurretExpired;
		PlayerData.playerTurretKilled += PlayerTurretKilled;
	}
	
	public void Disconnect(){
		PlayerData.playerHitEnemy -= PlayerHitEnemy;
		PlayerData.playerKillEnemy -= PlayerKilledEnemy;
		PlayerData.playerHitEnvironment -= PlayerHitEnvironment;
		
		PlayerData.playerHurt -= PlayerHurt;
		PlayerData.playerFireWeapon -= PlayerFire;
		PlayerData.playerReloadWeapon -= PlayerReload;
		
		PlayerData.playerTurretPlaced -= PlayerTurretPlaced;
		PlayerData.playerTurretRemoved -= PlayerTurretExpired;
		PlayerData.playerTurretKilled -= PlayerTurretKilled;
	}
	
	public virtual void PlayerHitEnemy(){
		
	}
	
	public virtual void PlayerKilledEnemy(){
		
	}
	
	public virtual void PlayerHitEnvironment(){
		
	}
	
	public virtual void PlayerHurt(){
		
	}
	
	public virtual void PlayerFire(){
		
	}
	
	public virtual void PlayerReload(){
		
	}
	
	public virtual void PlayerTurretPlaced(){
		
	}
	
	public virtual void PlayerTurretExpired(){
		
	}
	
	public virtual void PlayerTurretKilled(){
		
	}
}
