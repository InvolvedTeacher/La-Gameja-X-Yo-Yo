using Godot;
using LaGamejaXYoYo.scripts.actions;
using System;

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

	public override void Attack() {
		// Todo
	}

	public override void PrepareActions() {
		// basic enemy - just move to player
		GD.Print("PrepareActions enemy 1");
		GD.Print("Target: " + mPlayer.GetTilePosition());

		mMoveAction = new(mPlayer.GetTilePosition(), this);
	}

	public override void ExecuteActions() {
		GD.Print("ExecuteActions enemy 1");

		if (mMoveAction != null) {
			mMoveAction.Execute();
		}
	}

	public override bool AllActionsFinished() {
		GD.Print("AllActionsFinished enemy 1");

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

}
