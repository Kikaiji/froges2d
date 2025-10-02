using Godot;
using System;

public partial class HitScanLine : Line2D
{
	private float _lineLifeTime;
	
	public void DisplayHitScanLine(Vector2 lineEndPoint, Texture2D lineTexture, float lineWidth, float displayDuration){
		AddPoint(lineEndPoint);
		
		this.Texture = lineTexture;
		this.Width = lineWidth;
		_lineLifeTime = displayDuration;
	}
	
	public override void _PhysicsProcess(double delta){
		_lineLifeTime -= (float)delta;
		
		if(_lineLifeTime <= 0) QueueFree();
	}
}
