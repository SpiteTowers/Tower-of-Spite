using Godot;
using System;
public partial class PlrRecode : CharacterBody2D
{
	// General Player Movement stats, hopefully can make upgrades change this
	// Run Stats
	[Export] public float RunSpeed = 450.0f;
	[Export(PropertyHint.Range, "0,1")] public float Deceleration = 0.3f;
	[Export(PropertyHint.Range, "0,1")] public float Acceleration = 0.3f;
	
	// Jump Stats
	[Export] public float JumpHeight = -400.0f;
	[Export] private float AlterJumpGravity = 0.5f;
	[Export(PropertyHint.Range, "0,1")] private float _jumpCutoff = 0.25f;
	
	// Air Jump Stats
	[Export] public int MaxAirJumps = 0;
	private int _jumpCount;
	
	// TODO: Coyote Time Shenanigans
	// private bool _wasOnFloor; 
	// private bool _justLeftFloor;

	// Falling/Fast Falling Stats
	[Export] public Vector2 PlayerGravity; // Default is Godot Default Gravity
	[Export] public float MaxStandardFallSpeed = 500f;
	[Export(PropertyHint.Range, "1,5")] public float FastFallMultiplier = 1.5f;
	private float _maxFallSpeed = 500f;
	private bool _isFastFalling = false;
	
	// Ripped from original code
	// Drop-through Stats
	private float _dropThroughTimer = 0.0f;
	private float _dropThroughTime = 0.15f;
	private bool _droppingThrough = false;
	
	private Vector2 _direction;
	public Vector2 _velocity;

	// public override void _Ready()
	// {
	// }

	public override void _PhysicsProcess(double delta)
	{
		_velocity = Velocity;
		_direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		PlayerGravity = GetGravity();
		
		if (IsOnFloor())
		{
			OnFloorReset();
		}
		
		if (CanJump())
		{
			Jump();
		}
		
		// Variable Jump Height Cutoff
		if (Input.IsActionJustReleased("jump") && _velocity.Y < 0)
		{
			_velocity.Y *= _jumpCutoff;
		}
		
		// Fix Jump Gravity
		if (Input.IsActionJustReleased("jump") || _velocity.Y > 0)
		{
			PlayerGravity /= AlterJumpGravity;
		}
		
		HandleMovement();

		// Fast Fall
		if (_velocity.Y > 0f && !IsOnFloor() && Input.IsActionJustPressed("move_down") && !_isFastFalling)
		{
			_isFastFalling = true;
			_maxFallSpeed *= FastFallMultiplier;
			_velocity.Y = _maxFallSpeed;
		}
		
		DropThroughPlatform(delta);
		
		HandleGravity(delta);
		
		// Limiting Fall Speed
		_velocity.Y = Math.Min(_velocity.Y, _maxFallSpeed);
		Velocity = _velocity;
		MoveAndSlide();
	}

	private void HandleGravity(double delta)
	{
		if (!IsOnFloor())
		{
			_velocity += PlayerGravity * (float)delta;
		}
	}

	private void OnFloorReset()
	{
		_isFastFalling = false;
		_maxFallSpeed = MaxStandardFallSpeed;
		_jumpCount = MaxAirJumps;
	}

	private bool CanJump()
	{
		return Input.IsActionJustPressed("jump") && (_jumpCount > 0 || IsOnFloor());
	}
	
	private void Jump()
	{
		if (!IsOnFloor())
		{
			_jumpCount--;
		}

		PlayerGravity *= AlterJumpGravity;
		_velocity.Y = JumpHeight;
	}

	private void HandleMovement()
	{
		if (_direction != Vector2.Zero)
		{
			_velocity.X = Mathf.MoveToward(_velocity.X, _direction.X * RunSpeed, RunSpeed * Acceleration);
		}
		else
		{
			_velocity.X = Mathf.MoveToward(Velocity.X, 0, RunSpeed * Deceleration);
		}
	}

	private void DropThroughPlatform(double delta)
	{
		// Ripped from original code
		// Drop through platforms
		if (Input.IsActionPressed("move_down"))
		{
			_droppingThrough = true;
			_dropThroughTimer = _dropThroughTime;
			
			SetCollisionMaskValue(2, false);
		}
		
		// Ripped from original code
		// Handles falling through platforms
		if (_droppingThrough)
		{
			_dropThroughTimer -= (float)delta;
			
			if (_dropThroughTimer <= 0)
			{
				_droppingThrough = false;
				SetCollisionMaskValue(2, true);
			}
		}
	}
}


