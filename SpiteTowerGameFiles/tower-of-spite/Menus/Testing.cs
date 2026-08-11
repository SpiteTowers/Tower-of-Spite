using Godot;
using System;

public partial class Testing : Node
{
	[Export]
	public PackedScene PlayerScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPlayerMovementPressed()
	{
		Node2D player = PlayerScene.Instantiate<Node2D>();
		AddChild(player);
		player.Position = new Vector2(500, 300);
	}
}
