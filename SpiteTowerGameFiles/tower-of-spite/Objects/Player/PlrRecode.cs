using Godot;
using System;
public partial class PlrRecode : CharacterBody2D
{
	[Export] public float RunSpeed = 450.0f;
	[Export(PropertyHint.Range, "0,1")] public float Deceleration = 0.3f;
	[Export(PropertyHint.Range, "0,1")] public float Acceleration = 0.3f;
	[Export] public float JumpHeight = -400.0f;
	[Export(PropertyHint.Range, "0,1")] private float _jumpCutoff = 0.25f;
	
	[Export] public int MaxAirJumps = 0;
	private int _jumpCount;

	private bool _wasOnFloor;
	private bool _justLeftFloor;

	[Export] public float MaxStandardFallSpeed = 500f;
	[Export(PropertyHint.Range, "1,5")] public float FastFallMultiplier = 1.5f;
	private float _maxFallSpeed = 500f;
	private bool _isFastFalling = false;
	
	// Ripped from original code
	private float _dropThroughTimer = 0.0f;
	private float _dropThroughTime = 0.15f;
	private bool _droppingThrough = false;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
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
			
			velocity.Y = JumpHeight;
		}
		
		// Variable Jump Height Cutoff
		if (Input.IsActionJustReleased("jump") && velocity.Y < 0)
		{
			velocity.Y *= _jumpCutoff;
		}

		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		
		if (direction != Vector2.Zero)
		{
			velocity.X = Mathf.MoveToward(velocity.X, direction.X * RunSpeed, RunSpeed * Acceleration);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, RunSpeed * Deceleration);
		}

		// Fast Fall
		if (velocity.Y > 0f && !IsOnFloor() && Input.IsActionJustPressed("move_down") && !_isFastFalling)
		{
			_isFastFalling = true;
			_maxFallSpeed *= FastFallMultiplier;
			velocity.Y = _maxFallSpeed;
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
		velocity.Y = Math.Min(velocity.Y, _maxFallSpeed);

		// _wasOnFloor = IsOnFloor();
		Velocity = velocity;
		MoveAndSlide();
		// _justLeftFloor = _wasOnFloor && !IsOnFloor() && velocity.Y >= 0f;
		//
		// if (_justLeftFloor)
		// {
		// 	
		// }
	}
}


