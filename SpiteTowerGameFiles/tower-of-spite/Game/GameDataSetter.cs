using Godot;
using System;
using System.Collections.Generic;

public partial class GameDataSetter : Node2D
{
	[Export] public PackedScene[] BgWallEnemies;
	[Export] public PackedScene[] WallEnemies;
	[Export] public PackedScene[] CeilingEnemies;
	[Export] public PackedScene[] FloorEnemies;
	[Export] public PackedScene[] ConstantEnemies;
	[Export] public PackedScene DoomsdayEye;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{ 
		GameData.BgWallEnemies = new List<PackedScene>(BgWallEnemies);
		GameData.WallEnemies = new List<PackedScene>(WallEnemies);
		GameData.CeilingEnemies = new List<PackedScene>(CeilingEnemies);
		GameData.FloorEnemies = new List<PackedScene>(FloorEnemies);
		GameData.ConstantEnemies = new List<PackedScene>(ConstantEnemies);
		GameData.ActiveEnemies = new List<PackedScene>();
		GameData.Hazards = new List<PackedScene>();
		GameData.DoomsdayEye = DoomsdayEye;
	}
}
