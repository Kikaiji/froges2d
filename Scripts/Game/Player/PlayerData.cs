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
	
	public delegate void FrogModified(FrogWeapon newWeapon);
	public static event FrogModified sideFrogChanged;
	public static event FrogModified mainFrogChanged;
	
	private FrogWeapon _mainFrog;
	public FrogWeapon MainFrog => _mainFrog;
	private FrogWeapon _sideFrog;
	public FrogWeapon SideFrog => _mainFrog;
	
	public delegate void AmmoCountModified(int newCount);
	public static event AmmoCountModified sideAmmoChanged;
	public static event AmmoCountModified mainAmmoChanged;
	
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
	
	public void ChangeMainFrog(FrogWeapon newFrog){
		_mainFrog = newFrog;
		
		if(mainFrogChanged != null){
			mainFrogChanged.Invoke(_mainFrog);
		}
	}
	
	public void ChangeSideFrog(FrogWeapon newFrog){
		_sideFrog = newFrog;
		
		if(sideFrogChanged != null){
			sideFrogChanged.Invoke(_sideFrog);
		}
	}
	
	public void SwitchFrog(){
		(_mainFrog, _sideFrog) = (_sideFrog, _mainFrog);
		
		if(mainFrogChanged != null){
			mainFrogChanged.Invoke(_mainFrog);
		}
		
		if(sideFrogChanged != null){
			sideFrogChanged.Invoke(_sideFrog);
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
}
