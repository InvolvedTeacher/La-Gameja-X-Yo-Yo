using Godot;
using System;

public partial class Manager : Node2D
{
	[Export]
	public Player Player;

	[Export(PropertyHint.None, "suffix:sec")]
	public float TurnTime {
		get => mTurnTime; 
		set => mTurnTime = value;
	}

	private float mTurnTime;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
