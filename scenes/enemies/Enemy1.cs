using Godot;
using LaGamejaXYoYo.scripts;
using LaGamejaXYoYo.scripts.actions;
using System;
using System.Collections.Generic;

public partial class Enemy1 : Enemy {

	private MoveAction mMoveAction = null;
	private bool mMovingToNewTile = false;
	private Vector2 mTargetPosition = new Vector2(0.0f, 0.0f);


	public override void MoveToNewTile(Vector2 target) {
		mNavigationAgent2D.TargetPosition = mTargetPosition = target;
		mMovingToNewTile = true;
	}

	public override bool MovementFinished() {
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
		if (Manager.sGameState != Manager.GameState.PlayerActions) {

			mTargetTileHighlight.GlobalPosition = mTargetPosition;
			mTargetTileHighlight.UpdateVisibility(true);
		}
	}

	public override void Attack() {
		// Todo
	}

	public override void PrepareActions() {
		// basic enemy - just move to player
		Vector2 targetPosition = mPlayer.GetTilePosition();

		mAuxNavAgent.TargetPosition = targetPosition;
		//** Force navigation calculation
		//** Known godot issue due to multithreading **
		mAuxNavAgent.IsNavigationFinished();
		//**
		if (!mAuxNavAgent.IsTargetReachable()) {
			return;
		}

		Vector2[] navigationPath = mAuxNavAgent.GetCurrentNavigationPath();

		float distance = 0.0f;
		Vector2 previous = Position;
		// Store all valid candidates
		List<Vector2> candidateTiles = new();

		foreach (Vector2 path in navigationPath) {
			distance += previous.DistanceTo(path);

			if (distance > mMaxMovementRangeInTiles * Utils.GetTileSize()) {
				break;
			}

			previous = path;
			candidateTiles.Add(previous);
		}


		for (int i = candidateTiles.Count - 1; i >= 0; i--) {
			targetPosition = Utils.GetTilePosition(candidateTiles[i]);

			if (Manager.ReserveTile(targetPosition)) {
				mTargetTileHighlight.GlobalPosition = targetPosition;
				mTargetTileHighlight.UpdateVisibility(true);

				mMoveAction = new(targetPosition, this);
				break;
			}
		}
	}

	public override void ExecuteMovement() {
		if (mMoveAction != null) {
			mMoveAction.Execute();
		}
	}

	public override bool MovementActionFinished() {
		bool completed = true;
		if (mMoveAction != null) {
			if (mMoveAction.IsCompleted()) {
				mMoveAction = null;
			} else {
				completed = false;
			}
		}
		return completed;
	}

	public override void ExecuteAttack() {
		// Todo
	}

	public override bool AttackActionFinished() {
		// Todo
		return true;
	}

	public override void ExecuteDamage() {
		// Tood
	}
}
