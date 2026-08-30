using Godot;
using System;

public partial class Zombie : CharacterBody2D
{
	[Export] public float Speed;
	
	private RayCast2D _wallCheck;
	private RayCast2D _floorCheck;
	private float _direction = 1.0f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_wallCheck = GetNode<RayCast2D>("WallCheck");
		_floorCheck = GetNode<RayCast2D>("FloorCheck");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_wallCheck.IsColliding() || !_floorCheck.IsColliding())
		{
			_direction *= - 1;
			_wallCheck.TargetPosition = new Vector2(_direction * 12, 0);
			_floorCheck.TargetPosition = new Vector2(_direction * 10, 30);
			
		}

		if (!IsOnFloor())
		{
			Velocity += GetGravity() * (float)delta;
		}
		
		Velocity = new Vector2(_direction * Speed, Velocity.Y);

		MoveAndSlide();
	}
}
