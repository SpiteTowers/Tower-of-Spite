using Godot;
using System;

public partial class DoomsdayEye : Node2D, IPlayerTargeter
{
	[Export] public float TriggerInterval { get; set; } = 10.0f; 
	[Export] public float FadeInDuration { get; set; } = 3.0f;
	[Export] public float StayOnDuration { get; set; } = 0.25f;

	private ColorRect _leftWarning;
	private CollisionObject2D _leftArea;
	private ColorRect _middleWarning;
	private CollisionObject2D _middleArea; 
	private ColorRect _rightWarning;
	private CollisionObject2D _rightArea;
	private Node2D _target;
	
	private readonly Random _rand = new Random();
	private float _timer = 0.0f;

	public override void _Ready()
	{
		_leftWarning = GetNode<ColorRect>("Left/LeftWarning");
		_leftArea = GetNode<CollisionObject2D>("Left");
		_middleWarning = GetNode<ColorRect>("Middle/MiddleWarning");
		_middleArea = GetNode<CollisionObject2D>("Middle");
		_rightWarning = GetNode<ColorRect>("Right/RightWarning");
		_rightArea = GetNode<CollisionObject2D>("Right");

		ResetWarningLane(_leftWarning, _leftArea);
		ResetWarningLane(_middleWarning, _middleArea);
		ResetWarningLane(_rightWarning, _rightArea);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_target != null)
		{
			GlobalPosition = new Vector2(0, _target.GlobalPosition.Y-328);
		}
		
		_timer += (float)delta;

		if (_timer >= TriggerInterval)
		{
			_timer = 0.0f; 
			int chosenLane = _rand.Next(0, 3);
			
			switch (chosenLane)
			{
				case 0:
					StartWarningSequence(_leftWarning, _leftArea);
					break;
				case 1:
					StartWarningSequence(_middleWarning, _middleArea);
					break;
				case 2:
					StartWarningSequence(_rightWarning, _rightArea);
					break;
			}
		}
	}

	private void StartWarningSequence(ColorRect warningRect, CollisionObject2D areaNode)
	{
		warningRect.Visible = true; 

		Tween tween = CreateTween();

		tween.TweenProperty(warningRect, "modulate:a", 1.0f, FadeInDuration);

		tween.TweenCallback(Callable.From(() => 
		{
			areaNode.SetCollisionLayerValue(3, true);
		}));

		tween.TweenInterval(StayOnDuration);

		tween.TweenCallback(Callable.From(() => 
		{
			ResetWarningLane(warningRect, areaNode);
		}));
	}

	private void ResetWarningLane(ColorRect warningRect, CollisionObject2D areaNode)
	{
		warningRect.Visible = false;
		warningRect.Modulate = new Color(warningRect.Modulate.R, warningRect.Modulate.G, warningRect.Modulate.B, 0.0f);
		areaNode.SetCollisionLayerValue(3, false);
	}

	public void SetTarget(Node2D target)
	{
		_target = target;
	}
}