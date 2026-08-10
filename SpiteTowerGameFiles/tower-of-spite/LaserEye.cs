using Godot;
using System;

public partial class LaserEye : Node2D
{
	[Export] public Node2D Target;
	[Export] public float RayMaxLength = 50f;
	[Export] public float RayLength = 500f;
	
	[Export] public float DefaultShotTimer = 3f; // in seconds; The default time the Laser Eye waits before shooting at the player
	private float ShotTimer = 3f; // in seconds;
	[Export] public float DefaultShotDelay = 0.5f; // in seconds; The time it takes to wait before shooting at the player
	private float ShotDelay = 0.1f; // in seconds;
	[Export] public float DefaultRecharge = 0.3f;
	private float RechargeTimer = 0.3f;
	
	private RayCast2D RayCast;
	private Timer Timer;
	private float CastAngle = 0;
	
	private bool IsTracking = true;
	private bool IsCharging = false;
	private bool IsShooting = false;

	private Line2D DebugLine;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RayCast = GetNode<RayCast2D>("RayCast2D");
		RayCast.SetTargetPosition(Vector2.Down * RayMaxLength);
		
		DebugLine = GetNode<Line2D>("DebugLine");
		DebugLine.Show();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		// Null Target Protection
		if (Target == null)
		{
			RayCast.TargetPosition = Vector2.Down * RayLength;
			IsTracking = false;
		}
		else
		{
			IsTracking = true;
		}
		
		// TODO: Make main raycast go till it hits ground/wall

		if (IsTracking && !IsCharging)
		{
			_aim();
			_debugUpdate();

			ShotTimer -= (float)delta;
			if (ShotTimer <= 0)
			{
				IsCharging = true;
			}
		}
		else if (IsCharging)
		{
			if (!IsShooting)
			{
				IsShooting = true;
				ShotDelay = DefaultShotDelay;
				RechargeTimer = DefaultRecharge;
			}
			Shoot(delta);
		}
		else
		{
			ShotTimer = DefaultShotTimer;
		}
		
		// Old nonfunctional Shooting
		/*
		if (IsTracking && ShotTimer >= ShotDelay)
		{
			ShotTimer = DefaultShotTimer;
			ShotTimer -= (float)delta;
			IsTracking = ShotTimer < ShotDelay;
		}
		else if (ShotTimer <= ShotDelay)
		{
			ShotTimer -= (float)delta;
			if (ShotTimer <= 0)
			{
				Shoot();
			}
		}
		else
		{
			IsTracking = true;
			ShotTimer = DefaultShotTimer;
		*/
	}

	public void Shoot(double delta)
	{
		DebugLine.DefaultColor = Colors.Orange;
		
		if (ShotDelay >= 0)
		{
			ShotDelay -= (float)delta;
		}
		else
		{
			// TODO: Add hitbox for the shot
			DebugLine.DefaultColor = Colors.LimeGreen;
		
			if (RechargeTimer >= 0)
			{
				RechargeTimer -= (float)delta;
			}
			else
			{
				
				IsCharging = false;
				ShotTimer = DefaultShotTimer;
				IsShooting = false;
			}
		}
		
		GD.Print(ShotDelay);
		GD.Print(RechargeTimer);
	}

	private void _aim()
	{
		RayCast.TargetPosition = RayCast.TargetPosition.DirectionTo((Target.Position - RayCast.GlobalPosition) * RayLength);
		RayCast.TargetPosition *= RayLength;
	}

	private void _debugUpdate()
	{
		DebugLine.Points = new Vector2[2] {RayCast.Position, RayCast.GetTargetPosition()};
		
		if (RayCast.IsColliding())
		{
			DebugLine.SetDefaultColor(Colors.White);
		}
		else
		{
			DebugLine.SetDefaultColor(Colors.Gray);
		}
	}
}
