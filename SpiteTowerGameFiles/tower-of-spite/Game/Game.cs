using System;
using System.Linq;
using Godot;
using TowerofSpite.Objects.Player;

public partial class Game : Node2D
{
	public event Action GoShop;
	public event Action GameOver;
	
	private LevelGenerator _levelGenerator;
	private Player _player;
	private Camera2D _camera;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_camera = GetParent().GetNode<Camera2D>("Camera2D");
		_levelGenerator = GetNode<LevelGenerator>("Level Generator");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_player == null)
		{
			return;
		}
		
		_camera.GlobalPosition = new Vector2(
			576,
			_player.GlobalPosition.Y
		);
	}

	public void StartGame()
	{
		GameData.Hazards.AddRange(GameData.BgWallEnemies);
		GameData.Hazards.AddRange(GameData.WallEnemies);
		GameData.Hazards.AddRange(GameData.CeilingEnemies);
		GameData.Hazards.AddRange(GameData.FloorEnemies);
		GameData.Hazards.AddRange(GameData.ConstantEnemies);
		
		GoShop?.Invoke();
	}

	public void BuildTower(int roomCount)
	{
		ActivateEnemy(GameData.ChosenHazard);
		GameData.ChosenHazard = null;

		if (GameData.ChosenAbility != null)
			ActivateAbility(GameData.ChosenAbility);

		GameData.ChosenAbility = null;

		_levelGenerator.Clear();

		_player = GetNode<Player>("Player");

		_player.EnablePlayer(new Vector2(576, 324));
		_player.GlobalPosition = new Vector2(576, 324);

		_levelGenerator.SpawnRooms(roomCount, _player);

		_player.PlayerDied += OnPlayerDied;
		_player.PlayerTouchedGoal += OnPlayerTouchedGoal;

		_player.ZIndex = 100;
	}

	private void ActivateEnemy(PackedScene enemy)
	{
		if (GameData.BgWallEnemies.Contains(enemy))
		{
			_levelGenerator.ActivateEnemy(enemy, EnemyType.BackgroundWall);
			GameData.Hazards.Remove(enemy);
		}
		else if (GameData.CeilingEnemies.Contains(enemy))
		{
			_levelGenerator.ActivateEnemy(enemy, EnemyType.Ceiling);
			GameData.Hazards.Remove(enemy);
		}
		else if (GameData.FloorEnemies.Contains(enemy))
		{
			_levelGenerator.ActivateEnemy(enemy, EnemyType.Floor);
			GameData.Hazards.Remove(enemy);
		}
		else if (GameData.WallEnemies.Contains(enemy))
		{
			_levelGenerator.ActivateEnemy(enemy, EnemyType.Wall);
			GameData.Hazards.Remove(enemy);
		}
		else if (GameData.ConstantEnemies.Contains(enemy))
		{
			_levelGenerator.ActivateEnemy(enemy, EnemyType.Constant);
			GameData.Hazards.Remove(enemy);
		}
	}

	private void ActivateAbility(string ability)
	{
		
	}
	
	public void OnPlayerDied()
	{
		_player.DisablePlayer();
		_levelGenerator.Clear();
		GameOver?.Invoke();
	}

	public void OnPlayerTouchedGoal()
	{
		GameData.FloorNumber++;
		_levelGenerator.Clear();
		GoShop?.Invoke();
	}

	public void Deactivate()
	{
		foreach (PackedScene enemy in GameData.ActiveEnemies)
		{
			if (GameData.BgWallEnemies.Contains(enemy))
			{
				_levelGenerator.DeactivateEnemy(enemy, EnemyType.BackgroundWall);
				GameData.Hazards.Add(enemy);
			}
			else if (GameData.CeilingEnemies.Contains(enemy))
			{
				_levelGenerator.DeactivateEnemy(enemy, EnemyType.Ceiling);
				GameData.Hazards.Add(enemy);
			}
			else if (GameData.FloorEnemies.Contains(enemy))
			{
				_levelGenerator.DeactivateEnemy(enemy, EnemyType.Floor);
				GameData.Hazards.Add(enemy);
			}
			else if (GameData.WallEnemies.Contains(enemy))
			{
				_levelGenerator.DeactivateEnemy(enemy, EnemyType.Wall);
				GameData.Hazards.Add(enemy);
			}
			else if (GameData.ConstantEnemies.Contains(enemy))
			{
				_levelGenerator.DeactivateEnemy(enemy, EnemyType.Constant);
				GameData.Hazards.Add(enemy);
			}
		}
	}
}
