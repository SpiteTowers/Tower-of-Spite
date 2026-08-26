using Godot;

namespace TowerofSpite.Objects.Player;

public partial class Player : CharacterBody2D
{
	[Signal] public delegate void PlayerDiedEventHandler();
	
	[Export] public float GravityScale = 1.0f;
	[Export] public int DashSpeed = 800;
	[Export] public float DashTime = 0.15f;
	[Export] public float DashDeceleration = 2500.0f;
	
	private float _dropThroughTimer = 0.0f;
	private float _dropThroughTime = 0.15f;
	private bool _droppingThrough = false;
	private float _dashTimer = 0.0f;
	private bool _canDash = true;
	private bool _dashing = false;
	private const float Speed = 300.0f;
	private const float JumpVelocity = -400.0f;
	private bool _isOnFloor;

	public override void _PhysicsProcess(double delta)
	{
		_isOnFloor = IsOnFloor();
		
		// Resets Dash on floor
		if (_isOnFloor)
		{
			_canDash = true;
		}
		
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!_isOnFloor)
		{
			velocity += GetGravity() * GravityScale * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && _isOnFloor)
		{
			velocity.Y = JumpVelocity;
		}

		// Drop through platforms
		if (Input.IsActionPressed("move_down") && !_dashing)
		{
			_droppingThrough = true;
			_dropThroughTimer = _dropThroughTime;
			
			SetCollisionMaskValue(2, false);
		}
		
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
		
		// Handle Dash
		if (Input.IsActionJustPressed("ability") && _canDash && !_dashing)
		{
			Vector2 dashDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
			if (dashDirection == Vector2.Zero)
			{
				dashDirection = Vector2.Right;
			}
			
			dashDirection = dashDirection.Normalized();
			
			velocity = dashDirection * DashSpeed;
			
			_dashing = true;
			_canDash = false;
			_dashTimer = DashTime;
		}
		
		// Deals with dash timer and reseting dash
		if (_dashing)
		{
			_dashTimer -= (float)delta;

			if (_dashTimer <= 0)
			{
				_dashing = false;
			}
		}
		else
		{
			// Handle Directional Movement
			Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
			if (direction != Vector2.Zero)
			{
				velocity.X = Mathf.MoveToward(velocity.X, direction.X * Speed, DashDeceleration * (float)delta);
			}
			else
			{
				velocity.X = Mathf.MoveToward(velocity.X, 0, DashDeceleration * (float)delta);
			}
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	
	public void OnHurtboxAreaEntered(Area2D area)
	{
		KillPlayer();
	}

	private void KillPlayer()
	{
		EmitSignalPlayerDied();
	}
	public void DisablePlayer()
	{
		Visible = false;
		SetPhysicsProcess(false);
		SetProcess(false);

		SetCollisionLayerValue(4, false);
		SetCollisionMaskValue(1, false);
		SetCollisionMaskValue(2, false);
		SetCollisionMaskValue(3, false);
	}

	public void EnablePlayer(Vector2 spawnPosition)
	{
		GlobalPosition = spawnPosition;

		SetCollisionLayerValue(4, true);
		SetCollisionMaskValue(1, true);
		SetCollisionMaskValue(2, true);
		SetCollisionMaskValue(3, true);

		Visible = true;
		SetPhysicsProcess(true);
		SetProcess(true);
	}
}