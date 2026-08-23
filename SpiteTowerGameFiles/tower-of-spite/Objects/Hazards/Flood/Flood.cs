using Godot;
using System;

public partial class Flood : Area2D
{
	[Export] public float RiseSpeed = 20.0f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += Vector2.Up * RiseSpeed * (float)delta;
	}
}
