using Godot;
using System;

public partial class LaserEye : Node2D
{
	[Export] public Node2D Target;
	[Export] public float RayLength = 500f;
	
	[Export] public float DefaultShotTimer = 3f; // in seconds; The default time the Laser Eye waits before shooting at the player
	private float ShotTimer = 3f; // in seconds;
	[Export] public float ShotDelay = 0.1f; // in seconds; When ShotTimer is below this number, it will stop tracking the player to prepare to shoot.
	
	private RayCast2D RayCast;
	private Timer Timer;
	private float CastAngle = 0;
	
	private bool IsTracking = true;

	private Line2D DebugLine;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RayCast = GetNode<RayCast2D>("RayCast2D");
		RayCast.SetTargetPosition(Vector2.Down * RayLength);
		
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

		// Shooting
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
		
		// Tracking Direction Calculation
		if (IsTracking)
		{
			// _rayCast.TargetPosition = Vector2.FromAngle(_castAngle) * RayLength;
			// _castAngle = _rayCast.TargetPosition.AngleTo(Target.Position - _rayCast.GlobalPosition);
			RayCast.TargetPosition = RayCast.TargetPosition.DirectionTo((Target.Position - RayCast.GlobalPosition) * RayLength);
			RayCast.TargetPosition *= RayLength;
			
			// _rayCast.TargetPosition = Vector2.FromAngle((Target.Position - _rayCast.TargetPosition ).Angle()) * RayLength;
		}
		
		// Debug Line
		DebugLine.Points = new Vector2[2] {RayCast.Position, RayCast.GetTargetPosition()};
		
		if (RayCast.IsColliding())
		{
			DebugLine.SetDefaultColor(Colors.Crimson);
		}
		else
		{
			DebugLine.SetDefaultColor(Colors.White);
		}
	}

	public void Shoot()
	{
		// TODO: Actually shoot the damn thing
		IsTracking = true;
		ShotTimer = DefaultShotTimer;
	}
}
