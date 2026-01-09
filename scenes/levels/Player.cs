using Godot;
using LaGamejaXYoYo.scripts;
using System;

public partial class Player : CharacterBody2D {
	public const float mSpeed = 100.0f;

	private NavigationAgent2D mNavigationAgent2D = null;
	private bool mNewTileTarget = false;
	private bool mMovingToNewTile = false;
	private Vector2 mTargetPosition = new Vector2(0.0f, 0.0f);

	public override void _Ready() {
		mNavigationAgent2D = GetNode<NavigationAgent2D>("NavigationAgent2D");
	}

	public override void _Input(InputEvent @event) {
		base._Input(@event);

		if (@event is InputEventMouseButton eventMouseButton) {
			mNewTileTarget = true;
		}
	}

	public override void _PhysicsProcess(double delta) {
		if (mNewTileTarget) {
			Vector2 newTargetPos = Utils.GetTilePosition(GetGlobalMousePosition());
			if (Utils.IsInRange(newTargetPos, Utils.GetTilePosition(Position))) {
				mNavigationAgent2D.TargetPosition = mTargetPosition = Utils.GetTilePosition(GetGlobalMousePosition());

				mMovingToNewTile = true;
			}
			mNewTileTarget = false;
		}

		if (mNavigationAgent2D.IsTargetReached()) {
			if (mMovingToNewTile) {
				Position = mTargetPosition;
				mMovingToNewTile = false;
			}
			return;
		}

		Vector2 velocity = GlobalPosition.DirectionTo(mNavigationAgent2D.GetNextPathPosition()) * mSpeed;
		if (mNavigationAgent2D.AvoidanceEnabled) {
			mNavigationAgent2D.Velocity = velocity;
		}

		Velocity = velocity;

		MoveAndSlide();
	}
}
