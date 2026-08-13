using Godot;
using System;

public partial class LevelGenerator : Node2D
{
	[Export] public PackedScene[] RoomOptions;
	
	public Room[] rooms;
	
	private static Random random = new Random();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SpawnRooms(int numRooms)
	{
		rooms = new Room[numRooms];
		rooms[0] = RoomOptions[random.Next(RoomOptions.Length)].Instantiate<Room>();
		GetParent().AddChild(rooms[0]);
		for (int i = 1; i < numRooms; i++)
		{

			bool invalidRoom = false;
			do
			{
				int roomIndex = random.Next(RoomOptions.Length);
				Room newRoom = RoomOptions[roomIndex].Instantiate<Room>();
				if ((newRoom.BottomConnections & rooms[i - 1].TopConnections) != 0)
				{
					rooms[i] = newRoom;
					GetParent().AddChild(newRoom);
				}
				else
				{
					invalidRoom = true;
					newRoom.QueueFree();
				}
			} while (invalidRoom);
		}
	}
}
