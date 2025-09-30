using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	[Export]
	private int _moveSpeed;
	[Export]
	private double _sprintSpeedModifier;
	
	private float _speedRatio = 0f;
	
	[Export]
	private double _accelerationSpeed = 200;
	
	private Vector2 _lastDirectionFaced;
	
	private AnimatedSprite2D _sprite;
	
	public override void _Ready(){
		_sprite = GetChild(2) as AnimatedSprite2D;
	}
	
	public override void _PhysicsProcess(double delta){
		
		if(Velocity == Vector2.Zero){
			Velocity = ProcessMovementInputs(delta);
		}
		Velocity = Velocity.MoveToward(ProcessMovementInputs(delta), (float)(_accelerationSpeed * delta));
		
		
		
		MoveAndSlide();
	}
	
	public override void _Process(double delta){
		if (Input.IsActionJustPressed("control_attack")) PlayerAttack();
		if (Input.IsActionJustPressed("control_swap")) PlayerSwitchFrogs();
	}
	
	
	
	public Vector2 ProcessMovementInputs(double delta){
		
		Vector2 result = Vector2.Zero;
		
		if (Input.IsActionPressed("control_up")) result.Y -= 1f;
		if (Input.IsActionPressed("control_down")) result.Y += 1f;
		if (Input.IsActionPressed("control_left")) result.X -= 1f;
		if (Input.IsActionPressed("control_right")) result.X += 1f; 
		
		result = result.Normalized() * ((float)(_moveSpeed * delta));
		
		if(result.X >= 0){
			_sprite.FlipH = true;
		} else{
			_sprite.FlipH = false;
		}
		
		if(result.IsEqualApprox(Vector2.Zero)){
			_sprite.Pause();
			_sprite.Frame = 0;
		} else{
			_sprite.Play();
		}
		
		return result;
	}
	
	private void PlayerAttack(){
		
	}
	
	private void PlayerSwitchFrogs(){
		PlayerData.Instance.SwapFrogs();
	}
	
	
}
