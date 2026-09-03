using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node2D
{
	
	private MainMenu _mainMenu;
	private PauseMenu _pauseMenu;
	private Game _game;
	private Shop _shop;
	private List<PackedScene> ActiveEnemies = new List<PackedScene>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mainMenu = GetNode<MainMenu>("MainMenu");
		_mainMenu.Visible = true;
		_mainMenu.StartGameEvent += StartGame;
		_pauseMenu = GetNode<PauseMenu>("PauseMenu");
		_pauseMenu.Visible = false;
		_pauseMenu.Exit += ExitGame;
		_pauseMenu.Unpause += UnpauseGame;
		_game = GetNode<Game>("Game");
		_game.Visible = false;
		_game.SetProcess(false);
		_game.SetPhysicsProcess(false);
		_game.GoShop += GoShop;
		_game.GameOver += ExitGame;
		_shop = GetNode<Shop>("Shop");
		_shop.Visible = false;
		_shop.ShopClosed += ExitShop;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}











	public void StartGame()
	{
		_game.SetProcess(true);
		_game.SetPhysicsProcess(true);
		_mainMenu.Visible = false;
		_game.Visible = true;
		_game.StartGame();
	}

	public void UnpauseGame()
	{
		_game.SetProcess(true);
		_game.SetPhysicsProcess(true);
		_game.Visible = true;
	}

	public void ExitGame()
	{
		_pauseMenu.Visible = false;
		_game.SetProcess(false);
		_game.SetPhysicsProcess(false);
		_game.Visible = false;
		_mainMenu.Visible = true;
	}
	
	public void GoShop()
	{
		_game.SetProcess(false);
		_game.SetPhysicsProcess(false);
		_game.Visible = false;
		_shop.Visible = true;
		GameData.IsOpen = GameData.FloorNumber % 3 == 0;
		_shop.StartShop();
	}
	
	public void ExitShop()
	{
		ActiveEnemies.Add(GameData.ChosenHazard);
		_shop.Visible = false;
		_game.SetProcess(true);
		_game.SetPhysicsProcess(true);
		_game.Visible = true;
		_game.BuildTower(GameData.FloorNumber * 2);
	}
}
