using Godot;
using System;
public partial class PlrRecode : CharacterBody2D
{
	// General Player Movement stats, hopefully can make upgrades change this
	[Export] public float RunSpeed = 450.0f;
	[Export(PropertyHint.Range, "0,1")] public float Deceleration = 0.3f;
	[Export(PropertyHint.Range, "0,1")] public float Acceleration = 0.3f;
	[Export] public float JumpHeight = -400.0f;
	[Export(PropertyHint.Range, "0,1")] private float _jumpCutoff = 0.25f;
	
	// If this isnt obvious just by the name then you really need to get better at reading
	// (Its the double jump stuff)
	[Export] public int MaxAirJumps = 0;
	private int _jumpCount;
	
	// Coyote Time Shenanigans
	private bool _wasOnFloor; 
	private bool _justLeftFloor;

	// Fall Speed Stuff
	[Export] public float MaxStandardFallSpeed = 500f;
	[Export(PropertyHint.Range, "1,5")] public float FastFallMultiplier = 1.5f;
	private float _maxFallSpeed = 500f;
	private bool _isFastFalling = false;
	
	// Ripped from original code
	// Dropping through platform stats
	private float _dropThroughTimer = 0.0f;
	private float _dropThroughTime = 0.15f;
	private bool _droppingThrough = false;

	public Vector2 _velocity;

	public override void _PhysicsProcess(double delta)
	{
		_velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			_velocity += GetGravity() * (float)delta;
		}

		// Reset midair actions
		if (IsOnFloor())
		{
			_isFastFalling = false;
			_maxFallSpeed = MaxStandardFallSpeed;
			_jumpCount = MaxAirJumps;
		}
		
		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && (_jumpCount > 0 || IsOnFloor()))
		{
			if (!IsOnFloor())
			{
				_jumpCount--;
			}
			
			_velocity.Y = JumpHeight;
		}
		
		// Variable Jump Height Cutoff
		if (Input.IsActionJustReleased("jump") && _velocity.Y < 0)
		{
			_velocity.Y *= _jumpCutoff;
		}

		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		
		if (direction != Vector2.Zero)
		{
			_velocity.X = Mathf.MoveToward(_velocity.X, direction.X * RunSpeed, RunSpeed * Acceleration);
		}
		else
		{
			_velocity.X = Mathf.MoveToward(Velocity.X, 0, RunSpeed * Deceleration);
		}

		// Fast Fall
		if (_velocity.Y > 0f && !IsOnFloor() && Input.IsActionJustPressed("move_down") && !_isFastFalling)
		{
			_isFastFalling = true;
			_maxFallSpeed *= FastFallMultiplier;
			_velocity.Y = _maxFallSpeed;
		}
		
		
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
		
		// Limiting Fall Speed
		_velocity.Y = Math.Min(_velocity.Y, _maxFallSpeed);

		// _wasOnFloor = IsOnFloor();
		Velocity = _velocity;
		MoveAndSlide();
		// _justLeftFloor = _wasOnFloor && !IsOnFloor() && velocity.Y >= 0f;
		//
		// if (_justLeftFloor)
		// {
		// 	
		// }
	}
}


