using Godot;
using System;

public partial class Manager : Node2D {
	public enum GameState {
		Error = -1,
		PlayerActions,
		ExecuteActions,
		ExecutingActions
	}

	[Export]
	public Player Player {
		get => mPlayer;
		set => mPlayer = value;
	}
	private Player mPlayer;

	[Export(PropertyHint.None, "suffix:sec")]
	public float TurnTime {
		get => mTurnTime;
		set => mTurnTime = value;
	}

	private float mTurnTime;
	private float mElapsedTime = 0.0f;

	public static GameState sGameState = GameState.Error;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		sGameState = GameState.PlayerActions;
	}

	public override void _Input(InputEvent @event) {
		base._Input(@event);

		if (Input.IsActionJustPressed("ui_accept")) {
			sGameState = GameState.ExecuteActions;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {

		
		switch (sGameState) {
			case GameState.Error: {
					GD.Print("Somethings really bad");
				}
				break;
			case GameState.PlayerActions: {
					if (mElapsedTime >= mTurnTime) {
						mElapsedTime = 0.0f;
						sGameState = GameState.ExecuteActions;

					} else {
						mElapsedTime += (float)delta;
					}
				}
				break;
			case GameState.ExecuteActions: {
					// Execute all player and enemies actions
					mPlayer.ExecuteActions();
					// tbd

					sGameState = GameState.ExecutingActions;
				}
				break;
			case GameState.ExecutingActions: {
					// Wait untill all actions are completed
					bool completed = true;

					completed &= mPlayer.AllActionsFinished();

					if (completed) {
						sGameState = GameState.PlayerActions;
					}

				}
				break;
		}
		if (sGameState == GameState.ExecuteActions) {

		}
	}
}
