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
		
		if (GameData.ChosenAbility != null) ActivateAbility(GameData.ChosenAbility);
		GameData.ChosenAbility = null;
		
		_levelGenerator.Clear();

		_player = GetNode<Player>("Player");

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
			GameData.BgWallEnemies.Remove(enemy);
		}
		else if (GameData.CeilingEnemies.Contains(enemy))
		{
			_levelGenerator.ActivateEnemy(enemy, EnemyType.Ceiling);
			GameData.CeilingEnemies.Remove(enemy);
		}
		else if (GameData.FloorEnemies.Contains(enemy))
		{
			_levelGenerator.ActivateEnemy(enemy, EnemyType.Floor);
			GameData.FloorEnemies.Remove(enemy);
		}
		else if (GameData.WallEnemies.Contains(enemy))
		{
			_levelGenerator.ActivateEnemy(enemy, EnemyType.Wall);
			GameData.WallEnemies.Remove(enemy);
		}
		else if (GameData.ConstantEnemies.Contains(enemy))
		{
			_levelGenerator.ActivateEnemy(enemy, EnemyType.Constant);
			GameData.ConstantEnemies.Remove(enemy);
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
}
