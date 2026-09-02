using Godot;
using System;
using System.Collections.Generic;

public static class GameData
{
	[Export] public static PackedScene[] BgWallEnemies;
	[Export] public static PackedScene[] WallEnemies;
	[Export] public static PackedScene[] CeilingEnemies;
	[Export] public static PackedScene[] FloorEnemies;
	[Export] public static PackedScene[] ConstantEnemies;
	[Export] public static int RoomNumber;
	
	public static bool IsOpen { get; set; } = true;
	public static List<PackedScene> Hazards { get; set; } = [];
	public static List<string[]> Abilities { get; set; } = [];
	public static PackedScene ChosenHazard { get; set; }
	public static string ChosenAbility { get; set; }
	public static int PlayerMoney { get; set; } = 1000;
	
	
	public static string[] GetHazardInfo(Node2D node)
	{
		if (node is LaserEye)
		{
			return ["Laser Eye", "res://Assets/ShopAssets/LaserEyeShop.png", "Tracks you on sight. Don't let it shoot you.", ""];
		}
		else if (node is SawMain)
		{
			return ["Sawblade", "res://Assets/ShopAssets/SawbladeShop.png", "Follows its track.", ""];
		}
		else if (node is GhostClone)
		{
			return ["Ghost Clone", "", "Follows your every movement.", ""];
		}
		else if (node is Flood)
		{
			return ["Flood", "res://Assets/ShopAssets/FloodPipeShop.png", "Floods and drains the tower quickly.", ""];
		}
		else if (node is Zombie)
		{
			return ["Zombie", "", "Moves back and forth, doesn't one-shot.", ""];
		}
		else if (node is DoomsdayEye)
		{
			return ["Doomsday Eye", "", "Shoots a large beam throughout the tower.", ""];
		}
		else
		{
			return null;
		}
	}
}
