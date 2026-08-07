using Godot;
using System;

public partial class LaserEye : Node2D
{
	[Export] public Node2D Target;
	[Export] public float RayMaxLength = 50f;
	
	private RayCast2D _rayCast;

	private Line2D _debugLine;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_rayCast = GetNode<RayCast2D>("RayCast2D");
		_rayCast.SetTargetPosition(Vector2.Down * RayMaxLength);
		
		_debugLine = GetNode<Line2D>("Line2D");
		_debugLine.Show();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		// Making sure the target even exists
		if (Target != null)
		{
			_rayCast.TargetPosition = Target.Position;
		}
		
		// Makes the debug line have the same points as the raycast for visibility's sake
		_debugLine.Points = new Vector2[2] {_rayCast.Position, _rayCast.GetTargetPosition() - Position};
		
		// TODO: Make main raycast go till it hits ground/wall
		// TODO: Make a Shoot function that stops tracking the player and.. yknow, *shoots*
	}
}
