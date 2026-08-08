using Godot;
using System;

public partial class LaserEye : Node2D
{
	[Export] public Node2D Target;
	
	[Export] public float RayLength = 500f;
	
	private RayCast2D _rayCast;
	private float _castAngle = 0;

	private Line2D _debugLine;
	private Sprite2D _debugPointer;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_rayCast = GetNode<RayCast2D>("RayCast2D");
		_rayCast.SetTargetPosition(Vector2.Down * RayLength);
		
		_debugLine = GetNode<Line2D>("Line2D");
		_debugLine.Show();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Target != null)
		{
			// _rayCast.TargetPosition = Vector2.FromAngle(_castAngle) * RayLength;
			// _castAngle = _rayCast.TargetPosition.AngleTo(Target.Position - _rayCast.GlobalPosition);
			_rayCast.TargetPosition = _rayCast.TargetPosition.DirectionTo((Target.Position - _rayCast.GlobalPosition) * RayLength);
			_rayCast.TargetPosition *= RayLength;


			// _rayCast.TargetPosition = Vector2.FromAngle((Target.Position - _rayCast.TargetPosition ).Angle()) * RayLength;
		}
		_debugLine.Points = new Vector2[2] {_rayCast.Position, _rayCast.GetTargetPosition()};
		
		if (_rayCast.IsColliding())
		{
			_debugLine.Show();
		}
	}
}
