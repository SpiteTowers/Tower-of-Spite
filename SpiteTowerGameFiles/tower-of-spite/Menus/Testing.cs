using Godot;
using System;
using TowerofSpite.Objects.Player;

public partial class Testing : Node
{
	[Export] public PackedScene PlayerScene;
	[Export] public PackedScene EyeScene;
	[Export] public PackedScene SawScene;
	[Export] public PackedScene GhostCloneScene;
	[Export] public PackedScene FloodScene;
	
	private Player _player;
	private LaserEye _eye;
	private SawMain _saw;
	private GhostClone _clone;
	private Flood _flood;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SpawnPlayer();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnEyeTestingPressed()
	{
		_eye = EyeScene.Instantiate<LaserEye>();
		AddChild(_eye);
		_eye.Position = new Vector2(500, 300);
		_eye.SetTarget(_player);
	}

	public void OnSawTestingPressed()
	{
		_saw = SawScene.Instantiate<SawMain>();
		AddChild(_saw);
		_saw.Position = new Vector2(50, 175);
		_saw.Initialize(150);
	}

	public void OnGhostCloneTestingPressed()
	{
		_clone = GhostCloneScene.Instantiate<GhostClone>();
		AddChild(_clone);
		_clone.Position = new Vector2(500, 300);
		_clone.SetTarget(_player);
	}

	public void OnFloodTestingPressed()
	{
		_flood = FloodScene.Instantiate<Flood>();
		AddChild(_flood);
		_flood.Position = new Vector2(0, 300);
	}
	
	private void OnLevelTestingPressed()
	{
		GetTree().ChangeSceneToFile("res://Game/Game.tscn");
	}

	public void OnPlayerDied()
	{
		if (_flood != null)
		{
			_flood.QueueFree();
		}

		CallDeferred(MethodName.SpawnPlayer);
	}
	
	private void SpawnPlayer()
	{
		if (_player != null)
		{
			_player.QueueFree();
			_player = null;
		}
		_player = PlayerScene.Instantiate<Player>();
		AddChild(_player);
		_player.Position = new Vector2(700, 300);
		if (_eye != null)
		{
			_eye.SetTarget(_player);
		}

		if (_clone != null)
		{
			_clone.SetTarget(_player);
		}
		_player.PlayerDied += OnPlayerDied;
	}
}
