using Godot;
using System;

public partial class SawMain : Node2D, INeedsTrack
{
	[Export] public Texture2D StartTexture;
	[Export] public Texture2D MiddleTexture;
	[Export] public Texture2D EndTexture;
	
	public float _trackLength { get; set; }
	private float _distance;
	private float _direction = 1.0f;
	private const int PieceSize = 32;
	private const int EndSize = 16;
	private Node2D _track;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void Initialize(int trackLength)
	{
		_track = GetNode<Node2D>("Track");
		int middleLength = trackLength - 2 * EndSize;
		int numMiddlePieces = Mathf.RoundToInt(middleLength / PieceSize);
		_trackLength = numMiddlePieces * PieceSize + 2 * EndSize;
		_distance = 0;

		Sprite2D start = new Sprite2D();
		start.Texture = StartTexture;
		start.Position = Vector2.Zero;
		_track.AddChild(start);

		for (int i = 0; i < numMiddlePieces; i++)
		{
			Sprite2D middle = new Sprite2D();
			middle.Texture = MiddleTexture;
			middle.Position = new Vector2((i + 1) * PieceSize, 0);
			_track.AddChild(middle);
		}
		
		Sprite2D end = new Sprite2D();
		end.Texture = EndTexture;
		end.Position = new Vector2((numMiddlePieces + 1) * PieceSize, 0);
		_track.AddChild(end);

		Saw saw = GetNode<Saw>("Saw");
		saw.Initialize(_trackLength, _distance, _direction);
	}

	
}
