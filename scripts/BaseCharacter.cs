using Godot;
using System;

public abstract partial class BaseCharacter : CharacterBody2D {

    [Export]
    public int MaxMovementRangeInTiles {
        get => mMaxMovementRangeInTiles;
        set => mMaxMovementRangeInTiles = value;
    }
    internal int mMaxMovementRangeInTiles = 5;

    [Export]
    public float Speed {
        get => mSpeed;
        set => mSpeed = value;
    }
    internal float mSpeed = 100.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}

	public abstract void MoveToNewTile(Vector2 target);

	public abstract bool MovementFinished();
}
