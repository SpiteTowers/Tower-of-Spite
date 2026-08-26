using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LevelGenerator : Node2D
{
	[Export] public PackedScene[] RoomOptions;
	
	private List<Node> _spawnedObjects = new();
	private HashSet<PackedScene> _activeBgWallEnemies = new();
	private HashSet<PackedScene> _activeCeilingEnemies = new();
	private HashSet<PackedScene> _activeWallEnemies = new();
	private HashSet<PackedScene> _activeFloorEnemies = new();
	private HashSet<PackedScene> _activeConstantEnemies = new();
	private Random _rand =  new Random();
	public Room[] Rooms;
	
	private static Random _random = new Random();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SpawnRooms(int numRooms, Node2D target)
	{
		Rooms = new Room[numRooms];
		Rooms[0] = RoomOptions[_random.Next(RoomOptions.Length)].Instantiate<Room>();
		GetParent().AddChild(Rooms[0]);
		_spawnedObjects.Add(Rooms[0]);
		Rooms[0].Position = Vector2.Zero;
		for (int i = 1; i < numRooms; i++)
		{
			bool invalidRoom = false;
			do
			{
				int roomIndex = _random.Next(RoomOptions.Length);
				Room newRoom = RoomOptions[roomIndex].Instantiate<Room>();
				if ((newRoom.BottomConnections & Rooms[i - 1].TopConnections) != 0)
				{
					Rooms[i] = newRoom;
					GetParent().AddChild(newRoom);
					_spawnedObjects.Add(newRoom);
					newRoom.Position = new Vector2(0, -i * 656);
				}
				else
				{
					invalidRoom = true;
					newRoom.QueueFree();
				}
			} while (invalidRoom);
		}
		SpawnEnemies(target);
	}

	public void ActivateEnemy(PackedScene enemy, EnemyType type)
	{
		switch (type)
		{
			case EnemyType.BackgroundWall:
				_activeBgWallEnemies.Add(enemy);
				break;
			case EnemyType.Ceiling:
				_activeCeilingEnemies.Add(enemy);
				break;
			case EnemyType.Wall:
				_activeWallEnemies.Add(enemy);
				break;
			case EnemyType.Floor:
				_activeFloorEnemies.Add(enemy);
				break;
			case EnemyType.Constant:
				_activeConstantEnemies.Add(enemy);
				break;
		}
	}

	public void DeactivateEnemy(PackedScene enemy, EnemyType type)
	{
		switch (type)
		{
			case EnemyType.BackgroundWall:
				_activeBgWallEnemies.Remove(enemy);
				break;
			case EnemyType.Ceiling:
				_activeCeilingEnemies.Remove(enemy);
				break;
			case EnemyType.Wall:
				_activeWallEnemies.Remove(enemy);
				break;
			case EnemyType.Floor:
				_activeFloorEnemies.Remove(enemy);
				break;
			case EnemyType.Constant:
				_activeConstantEnemies.Remove(enemy);
				break;
		}
	}

	private void SpawnEnemies(Node2D target)
	{
		int roomCount = 0;
		foreach (var room in Rooms)
		{
			EnemySpawnPoint[] spawnPoints =
				room.GetNode("Enemy Spawn Points").GetChildren().OfType<EnemySpawnPoint>().ToArray();

			foreach (var spawnPoint in spawnPoints)
			{
				if (spawnPoint.Type == EnemyType.Constant && roomCount == 0)
				{
					SpawnConstantEnemies(spawnPoint.GlobalPosition, target);
				}
				
				PackedScene enemyScene = GetRandomEnemy(spawnPoint.Type);

				if (enemyScene == null)
				{
					continue;
				}
				
				Node2D enemy = enemyScene.Instantiate<Node2D>();
				GetParent().AddChild(enemy);
				_spawnedObjects.Add(enemy);
				enemy.GlobalPosition = spawnPoint.GlobalPosition;
				if (enemy is IPlayerTargeter targeter)
				{
					targeter.SetTarget(target);
				}

				if (enemy is INeedsTrack track)
				{
					track.Initialize(spawnPoint.Length);
				}
			}

			roomCount++;
		}
	}

	private void SpawnConstantEnemies(Vector2 spawnPoint, Node2D target)
	{
		foreach (PackedScene constantEnemy in _activeConstantEnemies)
		{
			Node2D enemy = constantEnemy.Instantiate<Node2D>();
			GetParent().AddChild(enemy);
			_spawnedObjects.Add(enemy);
			enemy.GlobalPosition = spawnPoint;
			if (enemy is IPlayerTargeter targeter)
			{
				targeter.SetTarget(target);
			}
		}
	}

	private PackedScene GetRandomEnemy(EnemyType type)
	{
		HashSet<PackedScene> pool;
		
		switch (type)
		{
			case EnemyType.BackgroundWall:
				pool = _activeBgWallEnemies;
				break;
			case EnemyType.Ceiling:
				pool = _activeCeilingEnemies;
				break;
			case EnemyType.Wall:
				pool = _activeWallEnemies;
				break;
			case EnemyType.Floor:
				pool = _activeFloorEnemies;
				break;
			default:
				return null;
		}

		return pool.Count == 0 ? null : pool.ElementAt(_random.Next(pool.Count));
	}

	public void Clear()
	{
		foreach (Node node in _spawnedObjects)
		{
			node.QueueFree();
		}

		_spawnedObjects.Clear();
		Rooms = null;
	}
}
