using Godot;
using LaGamejaXYoYo.scripts;
using System;

public partial class Player : CharacterBody2D {
	[Export]
	public int MaxMovementRangeInTiles = 5;
	[Export]
	public float mSpeed = 100.0f;

	[Export]
	public TargetTileHighlight mTargetTileHighlight = null;

	private NavigationAgent2D mNavigationAgent2D = null;
	private NavigationAgent2D mAuxNavAgent = null;

	private bool mNewTileTarget = false;
	private bool mMovingToNewTile = false;
	private Vector2 mTargetPosition = new Vector2(0.0f, 0.0f);

	public override void _Ready() {
		mNavigationAgent2D = GetNode<NavigationAgent2D>("NavigationAgent2D");
		mAuxNavAgent = GetNode<NavigationAgent2D>("AuxNavigationAgent");
	}

	public override void _Input(InputEvent @event) {
		base._Input(@event);

		if (@event is InputEventMouseButton) {
			MoveToNewTile();
		}
	}

	private bool IsInRangeForMovement(Vector2 target) {
		mAuxNavAgent.TargetPosition = target;

        //** Force navigation calculation
		//** Known godot issue due to multithreading **
        mAuxNavAgent.IsNavigationFinished();
		//**

		Vector2[] navigationPath = mAuxNavAgent.GetCurrentNavigationPath();
		float distance = 0.0f;
		Vector2 previous = Position;

		foreach (Vector2 path in navigationPath) {
			distance += previous.DistanceTo(path);
			GD.Print(distance);
			previous = path;
		}

		return distance < MaxMovementRangeInTiles * Utils.GetTileSize();
	}

	private void MoveToNewTile() {
		Vector2 newTargetPos = Utils.GetTilePosition(GetGlobalMousePosition());
		if (IsInRangeForMovement(newTargetPos)) {
			mNavigationAgent2D.TargetPosition = mTargetPosition = Utils.GetTilePosition(GetGlobalMousePosition());
			mMovingToNewTile = true;
		}
	}

	public override void _PhysicsProcess(double delta) {
		if (!mMovingToNewTile) {
			return;
		}

		if (mNavigationAgent2D.IsTargetReached()) {
			Position = mTargetPosition;
			mMovingToNewTile = false;
			return;
		}

		Vector2 velocity = GlobalPosition.DirectionTo(mNavigationAgent2D.GetNextPathPosition()) * mSpeed;
		if (mNavigationAgent2D.AvoidanceEnabled) {
			mNavigationAgent2D.Velocity = velocity;
		}

		Velocity = velocity;

		MoveAndSlide();
	}


	public override void _Process(double delta) {
		// Handle target tile map highlight
		Vector2 mousePosition = Utils.GetTilePosition(GetGlobalMousePosition());
		if (IsInRangeForMovement(mousePosition)) {
			mTargetTileHighlight.UpdatePosition(mousePosition);
			mTargetTileHighlight.UpdateVisibility(true);
		} else {
			mTargetTileHighlight.UpdateVisibility(false);
		}
	}
}
