using Godot;
using System;
using System.Collections.Generic;

public class GameData
{
	private static GameData _instance;

	public static GameData Instance
	{
		get { return _instance ??= new GameData(); }
		private set => _instance = value;
	}
	
	public Dictionary<string, FrogWeapon> FrogWeaponDict { get; private set; }

	public void Setup()
	{
		if(FrogWeaponDict == null){
			
		}
		FrogWeaponDict = ResourceFileLoader.Instance.LoadFolderAsDict<FrogWeapon>("res://Resources/FrogWeapons/");
	}

	public FrogWeapon GetFrogWeapon(string frogId)
	{
		FrogWeapon result;
		if (FrogWeaponDict.TryGetValue(frogId, out result)) return result;
		return null;
	}
}
