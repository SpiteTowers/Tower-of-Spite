using Godot;
using System;

public partial class Saw : Area2D
{

	[Export] public float Speed = 100.0f;
	
	private float _trackLength;
	private float _distance;
	private float _direction;

	public void Initialize(float trackLength, float distance, float direction)
	{
		_trackLength = trackLength;
		_distance = distance;
		_direction = direction;
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
		else if (_distance <= 0)
		{
			_distance = 0;
			_direction *= -1;
		}

		Position = new Vector2(_distance, 0);
	}
}
