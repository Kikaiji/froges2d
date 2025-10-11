using Godot;
using System;

public partial class ItemSlot : Control
{
	[Export] public int slotID;
	[Export] public Sprite2D slotSprite;
	
	public override void _EnterTree(){
		PlayerData.itemSlotUpdated += RefreshSlot;
	}
	
	public override void _ExitTree(){
		PlayerData.itemSlotUpdated -= RefreshSlot;
	}
	
	public void RefreshSlot(int index){
		if(index != slotID) return;
		
		ItemResource itemRes = PlayerData.Instance.PlayerItems[index].itemRes;
		
		slotSprite.Texture = itemRes.itemSprite;
	}
}
