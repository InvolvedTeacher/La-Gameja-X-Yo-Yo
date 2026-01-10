using Godot;
using System;
using System.Collections.Generic;

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

	[Export]
	public PackedScene Enemy1 {
		get => mEnemy1;
		set => mEnemy1 = value;
	}
	private PackedScene mEnemy1;

	private List<Enemy> enemies = new List<Enemy>();

	private float mElapsedTime = 0.0f;

	public static GameState sGameState = GameState.Error;

	private static HashSet<Vector2> mOccupiedTiles = new HashSet<Vector2>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		sGameState = GameState.PlayerActions;
		Enemy newEnemy = Enemy1.Instantiate<Enemy>();
		AddChild(newEnemy);
		newEnemy.Spawn(new Vector2(336, 176), mPlayer);

		enemies.Add(newEnemy);
	}

	public override void _Input(InputEvent @event) {
		base._Input(@event);

		if (Input.IsActionJustPressed("ui_accept")) {
			if (sGameState == GameState.PlayerActions) {
				mElapsedTime = 0.0f;
				sGameState = GameState.ExecuteActions;
			}
		}
		if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.E) {
			Enemy newEnemy = Enemy1.Instantiate<Enemy>();
			AddChild(newEnemy);
			newEnemy.Spawn(new Vector2(336, 176), mPlayer);

			enemies.Add(newEnemy);
		}
	}

	public static bool ReserveTile(Vector2 pos) {
		if (mOccupiedTiles.Contains(pos)) {
			return false;
		}

		mOccupiedTiles.Add(pos);
		return true;
	}

	private void PrepareEnemyActions() {
		foreach (var enemy in enemies) {
			enemy.PrepareActions();
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
					foreach (var enemy in enemies) {
						enemy.ExecuteActions(); 
					}

					sGameState = GameState.ExecutingActions;
				}
				break;
			case GameState.ExecutingActions: {
					// Wait untill all actions are completed
					bool completed = true;

					completed &= mPlayer.AllActionsFinished();

					foreach (var enemy in enemies) {
						completed &= enemy.AllActionsFinished();
					}

					// Enemies prepare next turn
					if (completed) {
						sGameState = GameState.PlayerActions;
						mOccupiedTiles.Clear();
						PrepareEnemyActions();
					}

				}
				break;
		}
		if (sGameState == GameState.ExecuteActions) {

		}
	}
}
