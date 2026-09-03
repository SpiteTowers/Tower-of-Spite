using Godot;
using System;
using System.Collections.Generic;

public static class GameData
{
	public static List<PackedScene> BgWallEnemies;
	public static List<PackedScene> WallEnemies;
	public static List<PackedScene> CeilingEnemies;
	public static List<PackedScene> FloorEnemies;
	public static List<PackedScene> ConstantEnemies;
	public static List<PackedScene> ActiveEnemies;

	public static PackedScene DoomsdayEye;
	public static int FloorNumber = 1;
	public static bool IsOpen { get; set; } = false;
	public static List<PackedScene> Hazards { get; set; } = [];
	public static List<string[]> Abilities { get; set; } = [new string[3], new string[3], new string[3]];
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
			return ["Ghost Clone", "res://Assets/ShopAssets/ghostcloneshop.png", "Follows your every movement.", ""];
		}
		else if (node is Flood)
		{
			return ["Flood", "res://Assets/ShopAssets/FloodPipeShop.png", "Floods and drains the tower quickly.", ""];
		}
		else if (node is Zombie)
		{
			return ["Zombie", "res://Assets/ShopAssets/zombieshop.png", "Moves back and forth, doesn't one-shot.", ""];
		}
		else if (node is DoomsdayEye)
		{
			return ["Doomsday Eye", "res://Assets/ShopAssets/", "Shoots a large beam throughout the tower.", ""];
		}
		else
		{
			return null;
		}
	}

	public static void AddUpgradesToShop()
	{
		Node2D hazard = ChosenHazard.Instantiate<Node2D>();
		if (hazard is LaserEye)
		{
			Hazards.Add(DoomsdayEye);
		}
	}
}
