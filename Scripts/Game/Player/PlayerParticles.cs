using Godot;
using System;

public partial class PlayerParticles : Node2D
{
	[Export] public PlayerController PlayerBody;
	
	public CpuParticles2D BigDustCloud;
	public CpuParticles2D SmallDustCloud;
	
	private Texture2D _BigDustTexture;
	private Texture2D _BigDustTextureFlipped;
	
	private Texture2D _SmallDustTexture;
	private Texture2D _SmallDustTextureFlipped;
	
	[Export] public float SmallCloudStartDelay = 0.33f;
	[Export] public float SmallCloudEmitInterval = 0.5f;
	
	private float _smallCloudEmitTimer = 0f;
	private bool _smallCloudEmit = false;
	
	private float _playerFacingX = 0f;
	
	public override void _Ready(){
		BigDustCloud = GetChild(0) as CpuParticles2D;
		SmallDustCloud = GetChild(1) as CpuParticles2D;
		
		_BigDustTexture = BigDustCloud.Texture;
		var img = _BigDustTexture.GetImage();
		img.FlipX();
		_BigDustTextureFlipped = ImageTexture.CreateFromImage(img);
		
		_SmallDustTexture = SmallDustCloud.Texture;
		img = _SmallDustTexture.GetImage();
		img.FlipX();
		_SmallDustTextureFlipped = ImageTexture.CreateFromImage(img);
		
	}
	
	public override void _EnterTree(){
		PlayerData.playerStartMoving += DustCloudParticleStart;
		PlayerData.playerStopMoving += DustCloudParticleStop;
	}
	
	public override void _ExitTree(){
		PlayerData.playerStartMoving -= DustCloudParticleStart;
		PlayerData.playerStopMoving -= DustCloudParticleStop;
	}
	
	public override void _PhysicsProcess(double delta){
		CheckPlayerDirection();
		
		if(_smallCloudEmit){
			_smallCloudEmitTimer -= (float)delta;
			
			if(_smallCloudEmitTimer <= 0){
				SmallDustCloud.Emitting = true;
				_smallCloudEmitTimer = SmallCloudEmitInterval;
			}
		}
	}
	
	private void FlipParticlesLeft(){
		BigDustCloud.Texture = _BigDustTextureFlipped;
		SmallDustCloud.Texture = _SmallDustTextureFlipped;
		this.Scale = new Vector2(-1f, this.Scale.Y);
	}
	
	private void FlipParticlesRight(){
		BigDustCloud.Texture = _BigDustTexture;
		SmallDustCloud.Texture = _SmallDustTexture;
		this.Scale = new Vector2(1f, this.Scale.Y);
	}
	
	private void CheckPlayerDirection(){
		var dir = MathF.Sign(PlayerBody.TrueMovementDir.X);
		
		if(dir == 0) return;
			if(dir == 1f){
				FlipParticlesRight();
			} else
			{
				FlipParticlesLeft();
			}
		
	}
	
	private void DustCloudParticleStart(){
		
		if(PlayerBody.TrueMovementDir.X >= 0){
			FlipParticlesRight();
		} else
		{
			FlipParticlesLeft();
		}
		
		_playerFacingX = MathF.Sign(PlayerBody.TrueMovementDir.X);
		
		BigDustCloud.Restart();
		_smallCloudEmitTimer = SmallCloudStartDelay;
		_smallCloudEmit = true;		
		
		
	}
	
	private void DustCloudParticleStop(){
		_smallCloudEmit = false;
	}
}
