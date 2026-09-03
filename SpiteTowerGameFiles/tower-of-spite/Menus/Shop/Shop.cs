using Godot;
using System;
using System.Collections.Generic;

public partial class Shop : Node2D
{
	private readonly Random _random =  new Random();
	private Button _hazardCard1;
	private Button _hazardCard2;
	private Button _hazardCard3;
	private Button _abilityCard1;
	private Button _abilityCard2;
	private Button _abilityCard3;
	private Sprite2D _shopClosed;
	private Label _playerGold;
	private int _ability1Price = 0;
	private int _ability2Price = 0;
	private int _ability3Price = 0;
	private int _chosenHazard = 0;
	private int _chosenAbility = 0;
	private PackedScene _option1Packed;
	private PackedScene _option2Packed;
	private PackedScene _option3Packed;
	private string _option1Attribute;
	private string _option2Attribute;
	private string _option3Attribute;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hazardCard1 = GetNode<Button>("HazardCards/HazardCard");
		_hazardCard2 = GetNode<Button>("HazardCards/HazardCard2");
		_hazardCard3 = GetNode<Button>("HazardCards/HazardCard3");
		_abilityCard1 = GetNode<Button>("AbilityCards/AbilityCard");
		_abilityCard2 = GetNode<Button>("AbilityCards/AbilityCard2");
		_abilityCard3 = GetNode<Button>("AbilityCards/AbilityCard3");
		_shopClosed = GetNode<Sprite2D>("ShopClosed");
		_playerGold = GetNode<Label>("PlayerGold");

		_playerGold.Text = $"Player Gold:{GameData.PlayerMoney}";

		if (GameData.IsOpen)
		{
			_shopClosed.Visible = false;
		}
		else
		{
			_shopClosed.Visible = true;
		}
		
		RunShop(GameData.IsOpen, GameData.Hazards, GameData.Abilities);
	}

	private void RunShop(bool isOpen, List<PackedScene> hazards, List<string[]> abilities = null)
	{
		DisplayHazards(hazards);
		if (isOpen)
		{
			DisplayAbilities(abilities);
		}
	}

	private void DisplayHazards(List<PackedScene> hazards)
	{
		_option1Packed = hazards[_random.Next(hazards.Count)];
		hazards.Remove(_option1Packed);
		Node2D option1 = _option1Packed.Instantiate<Node2D>();
		string[] option1Attributes = GameData.GetHazardInfo(option1);
		option1.QueueFree();
		GetNode<Label>("HazardCards/HazardCard/BoxContainer/VBoxContainer/Title").Text = option1Attributes[0];
		GetNode<TextureRect>("HazardCards/HazardCard/BoxContainer/VBoxContainer/Image").Texture = GD.Load<Texture2D>(option1Attributes[1]);
		GetNode<Label>("HazardCards/HazardCard/BoxContainer/VBoxContainer/Desc").Text = option1Attributes[2];
		GetNode<Label>("HazardCards/HazardCard/BoxContainer/VBoxContainer/Income").Text = option1Attributes[3];
		
		_option2Packed = hazards[_random.Next(hazards.Count)];
		hazards.Remove(_option2Packed);
		Node2D option2 = _option2Packed.Instantiate<Node2D>();
		string[] option2Attributes = GameData.GetHazardInfo(option2);
		option2.QueueFree();
		GetNode<Label>("HazardCards/HazardCard2/BoxContainer/VBoxContainer/Title").Text = option2Attributes[0];
		GetNode<TextureRect>("HazardCards/HazardCard2/BoxContainer/VBoxContainer/Image").Texture = GD.Load<Texture2D>(option2Attributes[1]);
		GetNode<Label>("HazardCards/HazardCard2/BoxContainer/VBoxContainer/Desc").Text = option2Attributes[2];
		GetNode<Label>("HazardCards/HazardCard2/BoxContainer/VBoxContainer/Income").Text = option2Attributes[3];
		
		_option3Packed = hazards[_random.Next(hazards.Count)];
		hazards.Remove(_option3Packed);
		Node2D option3 = _option3Packed.Instantiate<Node2D>();
		string[] option3Attributes = GameData.GetHazardInfo(option3);
		option3.QueueFree();
		GetNode<Label>("HazardCards/HazardCard3/BoxContainer/VBoxContainer/Title").Text = option3Attributes[0];
		GetNode<TextureRect>("HazardCards/HazardCard3/BoxContainer/VBoxContainer/Image").Texture = GD.Load<Texture2D>(option3Attributes[1]);
		GetNode<Label>("HazardCards/HazardCard3/BoxContainer/VBoxContainer/Desc").Text = option3Attributes[2];
		GetNode<Label>("HazardCards/HazardCard3/BoxContainer/VBoxContainer/Income").Text = option3Attributes[3];
	}

	private void DisplayAbilities(List<String[]> abilities)
	{
		string[] option1Array = abilities[_random.Next(abilities.Count)];
		abilities.Remove(option1Array);
		_ability1Price = int.Parse(option1Array[2]);
		_option1Attribute = option1Array[0];
		GetNode<Label>("AbilityCards/AbilityCard/MarginContainer/VBoxContainer/Title").Text = option1Array[0];
		GetNode<Label>("AbilityCards/AbilityCard/MarginContainer/VBoxContainer/Desc").Text = option1Array[1];
		GetNode<Label>("AbilityCards/AbilityCard/MarginContainer/VBoxContainer/Price").Text = option1Array[2];
		
		string[] option2Array = abilities[_random.Next(abilities.Count)];
		abilities.Remove(option2Array);
		_ability2Price = int.Parse(option2Array[2]);
		_option2Attribute = option2Array[0];
		GetNode<Label>("AbilityCards/AbilityCard2/MarginContainer/VBoxContainer/Title").Text = option2Array[0];
		GetNode<Label>("AbilityCards/AbilityCard2/MarginContainer/VBoxContainer/Desc").Text = option2Array[1];
		GetNode<Label>("AbilityCards/AbilityCard2/MarginContainer/VBoxContainer/Price").Text = option2Array[2];
		
		string[] option3Array = abilities[_random.Next(abilities.Count)];
		abilities.Remove(option3Array);
		_ability3Price = int.Parse(option3Array[2]);
		_option3Attribute = option3Array[0];
		GetNode<Label>("AbilityCards/AbilityCard3/MarginContainer/VBoxContainer/Title").Text = option3Array[0];
		GetNode<Label>("AbilityCards/AbilityCard3/MarginContainer/VBoxContainer/Desc").Text = option3Array[1];
		GetNode<Label>("AbilityCards/AbilityCard3/MarginContainer/VBoxContainer/Price").Text = option3Array[2];
	}

	public void OnHazardCardPressed()
	{
		if (GameData.ChosenHazard != null && _chosenHazard == 2)
		{
			_hazardCard2.Modulate = new Color(1, 1, 1);
		}
		else if (GameData.ChosenHazard != null && _chosenHazard == 3)
		{
			_hazardCard3.Modulate = new Color(1, 1, 1);
		}
		_hazardCard1.Modulate = new Color(1.3f, 1.3f, 1.3f);
		_chosenHazard = 1;
		GameData.ChosenHazard = _option1Packed;
	}
	
	public void OnHazardCardPressed2()
	{
		if (GameData.ChosenHazard != null && _chosenHazard == 1)
		{
			_hazardCard1.Modulate = new Color(1, 1, 1);
		}
		else if (GameData.ChosenHazard != null && _chosenHazard == 3)
		{
			_hazardCard3.Modulate = new Color(1, 1, 1);
		}
		_hazardCard2.Modulate = new Color(1.3f, 1.3f, 1.3f);
		_chosenHazard = 2;
		GameData.ChosenHazard = _option2Packed;
	}
	
	public void OnHazardCardPressed3()
	{
		if (GameData.ChosenHazard != null && _chosenHazard == 1)
		{
			_hazardCard1.Modulate = new Color(1, 1, 1);
		}
		else if (GameData.ChosenHazard != null && _chosenHazard == 2)
		{
			_hazardCard2.Modulate = new Color(1, 1, 1);
		}
		_hazardCard3.Modulate = new Color(1.3f, 1.3f, 1.3f);
		_chosenHazard = 3;
		GameData.ChosenHazard = _option3Packed;
	}
	
	public void OnAbilityCardPressed()
	{
		if (_ability1Price <= GameData.PlayerMoney)
		{
			if (GameData.ChosenAbility != null && _chosenAbility == 2)
			{
				_abilityCard2.Modulate = new Color(1, 1, 1);
			}
			else if (GameData.ChosenAbility != null && _chosenAbility == 3)
			{
				_abilityCard3.Modulate = new Color(1, 1, 1);
			}

			_abilityCard1.Modulate = new Color(1.3f, 1.3f, 1.3f);
			_chosenAbility = 1;
			GameData.ChosenAbility = _option1Attribute;
		}
	}
	
	public void OnAbilityCardPressed2()
	{
		if (_ability2Price <= GameData.PlayerMoney)
		{
			if (GameData.ChosenAbility != null && _chosenAbility == 1)
			{
				_abilityCard1.Modulate = new Color(1, 1, 1);
			}
			else if (GameData.ChosenAbility != null && _chosenAbility == 3)
			{
				_abilityCard3.Modulate = new Color(1, 1, 1);
			}

			_abilityCard2.Modulate = new Color(1.3f, 1.3f, 1.3f);
			_chosenAbility = 2;
			GameData.ChosenAbility = _option2Attribute;
		}
	}
	
	public void OnAbilityCardPressed3()
	{
		if (_ability3Price <= GameData.PlayerMoney)
		{
			if (GameData.ChosenAbility != null && _chosenAbility == 1)
			{
				_abilityCard1.Modulate = new Color(1, 1, 1);
			}
			else if (GameData.ChosenAbility != null && _chosenAbility == 2)
			{
				_abilityCard2.Modulate = new Color(1, 1, 1);
			}

			_abilityCard3.Modulate = new Color(1.3f, 1.3f, 1.3f);
			_chosenAbility = 3;
			GameData.ChosenAbility = _option3Attribute;
		}
	}

	public void OnExitShopPressed()
	{
		GetTree().ChangeSceneToFile("res://Game/Game.tscn");
	}
}
