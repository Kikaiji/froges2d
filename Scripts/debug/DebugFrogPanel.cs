using Godot;
using System;

public partial class DebugFrogPanel : Control
{
	[Export] public PackedScene FrogWeaponButton;
	
	public override void _Ready(){
		GameData.Instance.Setup();
		OpenDebugPanel();
	}
	
	private void OpenDebugPanel(){
		foreach(string key in GameData.Instance.FrogWeaponDict.Keys){
			var nextFrog = GameData.Instance.GetFrogWeapon(key);
			if(nextFrog != null){
				var newFrogButton = FrogWeaponButton.Instantiate() as DebugGiveFrog;
				newFrogButton.frogWeapon = nextFrog;
				AddChild(newFrogButton);
				newFrogButton.Setup();
			}
		}
	}
}
