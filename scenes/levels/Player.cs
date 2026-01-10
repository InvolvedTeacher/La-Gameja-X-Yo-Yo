using Godot;
using LaGamejaXYoYo.scripts;
using LaGamejaXYoYo.scripts.player;
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

	//** Actions
	private PlayerMoveAction mPlayerMoveAction = null;
	//
	//**

	public override void _Ready() {
		mNavigationAgent2D = GetNode<NavigationAgent2D>("NavigationAgent2D");
		mAuxNavAgent = GetNode<NavigationAgent2D>("AuxNavigationAgent");
	}

	public override void _Input(InputEvent @event) {
		base._Input(@event);

		if (Manager.sGameState == Manager.GameState.PlayerActions) {
			if (@event is InputEventMouseButton ) {
				NewMoveAction();
			}
		}
	}

	private bool IsInRangeForMovement(Vector2 target) {
		mAuxNavAgent.TargetPosition = target;

		//** Force navigation calculation
		//** Known godot issue due to multithreading **
		mAuxNavAgent.IsNavigationFinished();
		//**

		if (!mAuxNavAgent.IsTargetReachable()) {
			return false;
		}

		Vector2[] navigationPath = mAuxNavAgent.GetCurrentNavigationPath();

		float distance = 0.0f;
		Vector2 previous = Position;

		foreach (Vector2 path in navigationPath) {
			distance += previous.DistanceTo(path);
			previous = path;
		}

		return distance < MaxMovementRangeInTiles * Utils.GetTileSize();
	}

	private bool IsMoveActionDone() { 
		return mPlayerMoveAction != null; 
	}
	private void NewMoveAction() {
		// Only 1 move action
		if (IsMoveActionDone()) {
			return;
		}

		Vector2 newTargetPos = Utils.GetTilePosition(GetGlobalMousePosition());
		if (IsInRangeForMovement(newTargetPos)) {
			mPlayerMoveAction = new(newTargetPos, this);
		}
	}

	// To be used by Move Action
	public void MoveToNewTile(Vector2 target) {
		mNavigationAgent2D.TargetPosition = mTargetPosition = target;
		mMovingToNewTile = true;
	}

	public bool MovementFinished() {
		return !mMovingToNewTile;
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
		if (Manager.sGameState == Manager.GameState.PlayerActions && !IsMoveActionDone()) {
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

	public void ExecuteActions() {
		if (IsMoveActionDone()) {
			mPlayerMoveAction.Execute();
		}
	}

	public bool AllActionsFinished() {
		bool completed = true;
		if (mPlayerMoveAction != null) {
			if (mPlayerMoveAction.IsCompleted()) {
				mPlayerMoveAction = null;
			} else {
				completed = false;
			}
		}
		return completed;
	}
}
