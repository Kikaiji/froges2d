using Godot;
using System;

public partial class CameraFollow : Camera2D
{
	[Export] public CharacterBody2D PlayerCharacter;
	
	[Export] public double CameraOffsetStrength = 1.0;
	[Export] public float CameraAdjustSpeed = 1f;
	
	public override void _PhysicsProcess(double delta){
		var _goalPosition = PlayerCharacter.GlobalPosition + (PlayerCharacter.Velocity * (float)CameraOffsetStrength);
		var _difference = this.Offset.DistanceTo(_goalPosition);
		this.Offset = this.Offset.MoveToward(_goalPosition, (CameraAdjustSpeed * _difference) * (float)delta);
	}
	
	private float EaseOutCirc(float progress){
		return MathF.Sqrt(1 - Mathf.Pow(progress - 1, 2));
	}
	
}
