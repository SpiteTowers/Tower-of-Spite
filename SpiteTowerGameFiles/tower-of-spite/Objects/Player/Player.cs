using Godot;

namespace TowerofSpite.Objects.Player;

public partial class Player : CharacterBody2D
{
	[Export] public float GravityScale = 1.0f;
	[Export] public int DashSpeed = 800;
	[Export] public float DashTime = 0.15f;
	[Export] public float DashDeceleration = 2500.0f;
	
	private float DashTimer = 0.0f;
	private bool CanDash = true;
	private bool Dashing = false;
	private const float Speed = 300.0f;
	private const float JumpVelocity = -400.0f;

	public override void _PhysicsProcess(double delta)
	{
		if (IsOnFloor())
		{
			CanDash = true;
		}
		
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * GravityScale * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}
		
		// Handle Dash
		if (Input.IsActionJustPressed("ability") && CanDash && !Dashing)
		{
			Vector2 dashDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
			if (dashDirection == Vector2.Zero)
			{
				dashDirection = Vector2.Right;
			}
			
			dashDirection = dashDirection.Normalized();
			
			velocity = dashDirection * DashSpeed;
			
			Dashing = true;
			CanDash = false;
			DashTimer = DashTime;
		}

		if (Dashing)
		{
			DashTimer -= (float)delta;

			if (DashTimer <= 0)
			{
				Dashing = false;
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
}