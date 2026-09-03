using Godot;
using TowerofSpite.Objects.Player;

public partial class Game : Node2D
{
	private LevelGenerator _levelGenerator;
	private Player _player;
	private Camera2D _camera;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = GetNode<Player>("Player");
		_player.PlayerDied += OnPlayerDied;
		_player.ZIndex = 100;
		_camera = GetNode<Camera2D>("Camera2D");
		_levelGenerator = GetNode<LevelGenerator>("Level Generator");
		ActivateEnemies();
		BuildTower();
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

	private void BuildTower()
	{
		_levelGenerator.SpawnRooms(GameData.RoomNumber, _player);
	}

	private void ActivateEnemies()
	{
		foreach (PackedScene enemy in GameData.BgWallEnemies)
		{
			_levelGenerator.ActivateEnemy(enemy, EnemyType.BackgroundWall);
		}
		foreach (var enemy in GameData.WallEnemies)
		{
			_levelGenerator.ActivateEnemy(enemy, EnemyType.Wall);
		}
		foreach (var enemy in GameData.CeilingEnemies)
		{
			_levelGenerator.ActivateEnemy(enemy, EnemyType.Ceiling);
		}
		foreach (var enemy in GameData.FloorEnemies)
		{
			_levelGenerator.ActivateEnemy(enemy, EnemyType.Floor);
		}
		foreach (var enemy in GameData.ConstantEnemies)
		{
			_levelGenerator.ActivateEnemy(enemy, EnemyType.Constant);
		}
	}
	
	public void OnPlayerDied()
	{
		_player.DisablePlayer();
		_levelGenerator.Clear();
		
		CallDeferred(nameof(RestartLevel));
	}

	private void RestartLevel()
	{
		Vector2 spawnPosition = new Vector2(576, 512);

		_player.EnablePlayer(spawnPosition);

		BuildTower();
	}
}
