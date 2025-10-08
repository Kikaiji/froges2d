using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyData
{
	private static EnemyData _instance;

	public static EnemyData Instance
	{
		get { return _instance ??= new EnemyData(); }
		set => _instance = value;
	}
	
	private List<EnemyBase> _currentEnemies = new List<EnemyBase>();
	
	public int EnemyCount() => _currentEnemies.Count;
	
	public delegate void EnemyEvents(EnemyBase enemy);
	public static event EnemyEvents enemySpawned;
	public static event EnemyEvents enemyKilled;
	public static event EnemyEvents enemyHit;
	
	public void EnemyJustSpawned(EnemyBase enemy){
		_currentEnemies.Add(enemy);
		
		if(enemySpawned != null){
			enemySpawned.Invoke(enemy);
		}
	}
	
	public void EnemyJustKilled(EnemyBase enemy){
		_currentEnemies.RemoveAt(_currentEnemies.IndexOf(enemy));
		
		if(enemyKilled != null){
			enemyKilled.Invoke(enemy);
		}
	}
	
	public void EnemyJustHit(EnemyBase enemy){
		if(enemyHit != null) {
			enemyHit.Invoke(enemy);
		}	
	}
	
	public EnemyBase FindClosestEnemy(Vector2 startingGlobalPosition){
		
		if(_currentEnemies.Count == 0){
			return null;
		}
		
		EnemyBase currentResult;
		
		currentResult = _currentEnemies[0];
		
		for(int i = 1; i < _currentEnemies.Count; i++){
			if(startingGlobalPosition.DistanceTo(currentResult.GlobalPosition) > startingGlobalPosition.DistanceTo(_currentEnemies[i].GlobalPosition)){
				currentResult = _currentEnemies[i];
			}
		}
		
		return currentResult;
	} 
}
