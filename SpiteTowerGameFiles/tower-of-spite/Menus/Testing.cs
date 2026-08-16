using Godot;
using System;
using TowerofSpite.Objects.Player;

public partial class Testing : Node
{
	[Export] public PackedScene PlayerScene;
	[Export] public PackedScene EyeScene;
	[Export] public PackedScene SawScene;
	[Export] public PackedScene GhostCloneScene;
	
	private Player player;
	private LaserEye eye;
	private SawMain saw;
	private GhostClone clone;
	
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
		eye = EyeScene.Instantiate<LaserEye>();
		AddChild(eye);
		eye.Position = new Vector2(500, 300);
		eye.SetTarget(player);
	}

	public void OnSawTestingPressed()
	{
		saw = SawScene.Instantiate<SawMain>();
		AddChild(saw);
		saw.Position = new Vector2(50, 175);
		saw.Initialize(150);
	}

	public void OnGhostCloneTestingPressed()
	{
		clone = GhostCloneScene.Instantiate<GhostClone>();
		AddChild(clone);
		clone.Position = new Vector2(500, 300);
		clone.SetTarget(player);
	}

	public void OnPlayerDied()
	{
		CallDeferred(MethodName.SpawnPlayer);
	}
	
	private void SpawnPlayer()
	{
		player = PlayerScene.Instantiate<Player>();
		AddChild(player);
		player.Position = new Vector2(500, 300);
		if (eye != null)
		{
			eye.SetTarget(player);
		}

		if (clone != null)
		{
			clone.SetTarget(player);
		}
		player.PlayerDied += OnPlayerDied;
	}
}
