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

	public abstract void MoveToNewTile(Vector2 target);

	public abstract bool MovementFinished();

    public abstract void ExecuteActions();

    public abstract bool AllActionsFinished();


    }
