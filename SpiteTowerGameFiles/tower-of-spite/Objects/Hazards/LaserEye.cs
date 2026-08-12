using Godot;
using System;

public partial class LaserEye : Node2D
{
	[Export] public Node2D Target;
	[Export] public float RayLength = 250f;
	
	[Export] public float DefaultShotTimer = 3f; // in seconds; The default time the Laser Eye waits before shooting at the player
	private float ShotTimer = 3f; // in seconds;
	[Export] public float DefaultShotDelay = 0.5f; // in seconds; The time it takes to wait before shooting at the player
	private float ShotDelay = 0.1f; // in seconds;
	[Export] public float DefaultRecharge = 0.3f;
	private float RechargeTimer = 0.3f;

	private Area2D LaserCollision;
	private CollisionShape2D CollisionShape;
	private SegmentShape2D LaserShape;
	private RayCast2D RayCast;
	private Timer Timer;
	private float CastAngle = 0;
	
	private bool IsTracking = true;
	private bool IsCharging = false;
	private bool IsShooting = false;

	private Line2D DebugLine;


	public void SetTarget(Node2D target)
	{
		Target = target;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RayCast = GetNode<RayCast2D>("RayCast2D");
		RayCast.SetTargetPosition(Vector2.Down * RayLength);
		
		DebugLine = GetNode<Line2D>("DebugLine");
		DebugLine.Show();
		
		LaserCollision = GetNode<Area2D>("LaserCollision");
		CollisionShape = GetNode<CollisionShape2D>("LaserCollision/CollisionShape2D");
		LaserShape = CollisionShape.Shape as SegmentShape2D;
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

		LaserShape.A = Vector2.Zero;

		if (RayCast.IsColliding())
		{
			LaserShape.B = LaserCollision.ToLocal(RayCast.GetCollisionPoint());
		}
		else
		{
			LaserShape.B = RayCast.TargetPosition;
		}
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
			LaserCollision.SetCollisionLayerValue(3, true);
			DebugLine.DefaultColor = Colors.LimeGreen;
		
			if (RechargeTimer >= 0)
			{
				RechargeTimer -= (float)delta;
			}
			else
			{
				LaserCollision.SetCollisionLayerValue(3, false);
				IsCharging = false;
				ShotTimer = DefaultShotTimer;
				IsShooting = false;
			}
		}
		
		// GD.Print(ShotDelay);
		// GD.Print(RechargeTimer);
	}

	private void _aim()
	{
		// RayCast.TargetPosition = RayCast.TargetPosition.DirectionTo((Target.Position - RayCast.GlobalPosition) * RayLength);
		RayCast.TargetPosition = Target.Position - RayCast.GlobalPosition;
		RayCast.TargetPosition *= RayLength;
		
		// TODO: Make main raycast go till it hits ground/wall
		if (RayCast.IsColliding())
		{
			// GD.Print(RayCast.GetCollider(), RayCast.GetCollisionPoint());
			// DebugLine.SetDefaultColor(Colors.Purple);
		}
	}

	private void _debugUpdate()
	{
		DebugLine.Points = new Vector2[2] {RayCast.Position, RayCast.IsColliding() ? RayCast.GetCollisionPoint() - RayCast.GetGlobalPosition()  : RayCast.GetTargetPosition()};
		
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
