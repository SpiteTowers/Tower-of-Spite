using Godot;
using System;

public partial class Saw : StaticBody2D
{

	[Export] public float Speed = 100.0f;
	[Export] public PackedScene StartPiece;
	[Export] public PackedScene MiddlePiece;
	[Export] public PackedScene EndPiece;
	
	private float _trackLength;
	private float _distance;
	private float _direction = 1.0f;
	private Line2D _track;
	private const float PieceSize = 32.0f;
	private const float EndSize = 16.0f;

	public void Initialize(float trackLength)
	{
		_trackLength = trackLength;
		_distance = 0;
		_track = GetNode<Line2D>("Track");

		_track.Points = new Vector2[]
		{
			Vector2.Zero, new Vector2(_trackLength, 0)
		};

	}
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		_distance += Speed * (float)delta * _direction;

		if (_distance >= _trackLength)
		{
			_distance = _trackLength;
			_direction *= -1;
		}
		else if (_distance <= _trackLength)
		{
			_distance = 0;
			_direction *= -1;
		}

		Position = new Vector2(_distance, 0);
	}
}
