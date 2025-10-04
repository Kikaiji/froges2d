using Godot;
using System;

public class PlayerData
{
	private static PlayerData _instance;

	public static PlayerData Instance
	{
		get { return _instance ??= new PlayerData(); }
		set => _instance = value;
	}
	
	public delegate void PlayerAttackEvents(FrogWeapon attackingWeapon, Vector2 mousePosition);
	public static event PlayerAttackEvents playerJustAttacked;
	public static event PlayerAttackEvents playerJustStopAttack;
	
	public void PlayerStartAttack(Vector2 mousePosition){
		if(playerJustAttacked != null){
			playerJustAttacked.Invoke(_mainFrog, mousePosition);
		}
	}
	
	public void PlayerStopAttack(Vector2 mousePosition){
		if(playerJustStopAttack != null){
			playerJustStopAttack.Invoke(_mainFrog, mousePosition);
		}
	}
	
	public delegate void PlayerMovementEvents();
	public static event PlayerMovementEvents playerStartMoving;
	public static event PlayerMovementEvents playerStopMoving;
	
	public void PlayerStartedMoving(){
		playerStartMoving?.Invoke();
	}
	
	public void PlayerStoppedMoving(){
		playerStopMoving?.Invoke();
	}
	
	public delegate void FrogModified(FrogWeapon newWeapon);
	public static event FrogModified sideFrogChanged;
	public static event FrogModified mainFrogChanged;
	public static event FrogModified swapFrogs;
	
	private FrogWeapon _mainFrog;
	public FrogWeapon MainFrog => _mainFrog;
	private FrogWeapon _sideFrog;
	public FrogWeapon SideFrog => _mainFrog;
	
	public delegate void AmmoCountModified(int newCount);
	public static event AmmoCountModified sideAmmoChanged;
	public static event AmmoCountModified mainAmmoChanged;
	
	public void ReloadMainWeapon(){
		
		_mainCurrentAmmo = _mainFrog.frogMaxAmmo;
		PlayerJustReloadedWeapon();
		
		if(mainAmmoChanged != null){
			mainAmmoChanged.Invoke(_mainCurrentAmmo);
		}
	}
	
	public bool TryUseAmmo(int ammoToUse){
		if(_mainCurrentAmmo >= ammoToUse){
			_mainCurrentAmmo -= ammoToUse;
			
			if(mainAmmoChanged != null){
				mainAmmoChanged.Invoke(_mainCurrentAmmo);
			}
			
			return true;
		} else {
			return false;
		}
		
	}
	
	
	
	private int _mainCurrentAmmo;
	public int MainCurrentAmmo => _mainCurrentAmmo;
	private int _sideCurrentAmmo;
	public int SideCurrentAmmo => _sideCurrentAmmo;
	
	public delegate void StatModified();
	
	private int _currentHealth;
	public int CurrentHealth => _currentHealth;
	private int _maxHealth;
	public int MaxHealth => _maxHealth;
	
	public static event StatModified currentHealthChanged;
	public static event StatModified maxHealthChanged;
	
	public void AddFrogWeaponToPlayer(FrogWeapon newFrog){
		if(_mainFrog == null || (_mainFrog != null && _sideFrog != null)){
			ChangeMainFrog(newFrog);
			return;
		}
		
		ChangeSideFrog(newFrog);
		SwapFrogs();
	}
	
	public void ChangeMainFrog(FrogWeapon newFrog){
		_mainFrog = newFrog;
		_mainCurrentAmmo = _mainFrog.frogMaxAmmo;
		
		if(mainFrogChanged != null && mainAmmoChanged != null){
			mainFrogChanged.Invoke(_mainFrog);
			mainAmmoChanged.Invoke(_mainFrog.frogMaxAmmo);
		}
	}
	
	public void RemoveMainFrog(){
		_mainFrog = null;
		_mainCurrentAmmo = 0;
		
		SwapFrogs();
	}
	
	public void ChangeSideFrog(FrogWeapon newFrog){
		_sideFrog = newFrog;
		_sideCurrentAmmo = _sideFrog.frogMaxAmmo;
		
		if(sideFrogChanged != null && sideAmmoChanged != null){
			sideFrogChanged.Invoke(_sideFrog);
			sideAmmoChanged.Invoke(_sideFrog.frogMaxAmmo);
		}
	}
	
	public void RemoveSideFrog(){
		_sideFrog = null;
		_sideCurrentAmmo = 0;
		
		if(sideFrogChanged != null && sideAmmoChanged != null){
			sideFrogChanged.Invoke(_sideFrog);
			sideAmmoChanged.Invoke(_sideCurrentAmmo);
		}
	}
	
	public void SwapFrogs(){
		(_mainFrog, _sideFrog) = (_sideFrog, _mainFrog);
		(_mainCurrentAmmo, _sideCurrentAmmo) = (_sideCurrentAmmo, _mainCurrentAmmo);
		
		if(mainFrogChanged != null && mainAmmoChanged != null){
			mainFrogChanged.Invoke(_mainFrog);
			mainAmmoChanged.Invoke(_mainCurrentAmmo);
		}
		
		if(sideFrogChanged != null && sideAmmoChanged != null){
			sideFrogChanged.Invoke(_sideFrog);
			sideAmmoChanged.Invoke(_sideCurrentAmmo);
		}
		
		if(swapFrogs != null){
			swapFrogs.Invoke(null);
		}
	}
	
	public void ModifyCurrentHealth(int changeAmount){
		_currentHealth += changeAmount;
		
		if(_currentHealth > _maxHealth) _currentHealth = _maxHealth;
		if(_currentHealth < 0) _currentHealth = 0;
		
		if(currentHealthChanged != null){
			currentHealthChanged.Invoke();
		}
	}
	
	public float CalculateDamage(float baseDamage){
		//this will apply modifiers to the damage passed in
		GD.Print("Damage Calcs not implemented yet");
		return baseDamage;
	}
	
	public delegate void PlayerTriggerEvents();
	public static event PlayerTriggerEvents playerHitEnemy;
	public static event PlayerTriggerEvents playerKillEnemy;
	public static event PlayerTriggerEvents playerHitEnvironment;
	
	public void PlayerJustHitEnemy() => playerHitEnemy?.Invoke();
	public void PlayerJustKilledEnemy() => playerKillEnemy?.Invoke();
	public void PlayerJustHitEnvironment() => playerHitEnvironment?.Invoke();
	
	public static event PlayerTriggerEvents playerHurt;
	
	public static event PlayerTriggerEvents playerFireWeapon;
	public static event PlayerTriggerEvents playerReloadWeapon;
	
	public void PlayerJustFiredWeapon() => playerFireWeapon?.Invoke();
	public void PlayerJustReloadedWeapon() => playerReloadWeapon?.Invoke();
	
	
	
}
