using Godot;
using System;

public partial class DebugRemoveFrog : Control
{
	public void RemoveMainFrog(){
		PlayerData.Instance.RemoveMainFrog();
	}
	
	public void RemoveSideFrog(){
		PlayerData.Instance.RemoveSideFrog();
	}
}
