using Godot;
using System;
using System.Collections.Generic;

public partial class GhostClone : Area2D,  IPlayerTargeter
{
	private struct PositionSample
	{
		public Vector2 Position;
		public float Time;
	}
	
	[Export] public float Delay;

	private readonly Queue<PositionSample> _history = new();
	private Node2D _target;
	private float _startTime;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_startTime = Time.GetTicksMsec() / 1000.0f;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_target == null)
		{
			return;
		}

		
		float currentTime = Time.GetTicksMsec() / 1000.0f;
		
		_history.Enqueue(new PositionSample { Position = _target.GlobalPosition, Time = currentTime });
		float targetTime = currentTime - Delay;

		while (_history.Count > 1)
		{
			PositionSample temp = _history.ToArray()[1];

			if (temp.Time > targetTime)
			{
				break;
			}
			
			_history.Dequeue();
		}

		if (_history.Count > 0 && currentTime - _startTime >= Delay)
		{
			SetCollisionMaskValue(3, true);
			GlobalPosition = _history.Peek().Position;
		}
	}

	public void SetTarget(Node2D target)
	{
		_target = target;
	}
}
