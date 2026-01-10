using Godot;
using System;
using System.Collections.Generic;

public partial class Manager : Node2D {
	public enum GameState {
		Error = -1,
		PlayerActions,
		ExecuteMovement,
		Move,
		PrepareAttack,
		Attack,
		TakeDamage
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

	private List<Enemy> mEnemies = new List<Enemy>();

	private float mElapsedTime = 0.0f;

	public static GameState sGameState = GameState.Error;

	private static HashSet<Vector2> mOccupiedTiles = new HashSet<Vector2>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		sGameState = GameState.PlayerActions;
		Enemy newEnemy = Enemy1.Instantiate<Enemy>();
		AddChild(newEnemy);
		newEnemy.Spawn(new Vector2(336, 176), mPlayer);

		mEnemies.Add(newEnemy);
	}

	public List<Enemy> GetEnemies() { return mEnemies; }

	public Player GetPlayer() { return mPlayer; }

	public override void _Input(InputEvent @event) {
		base._Input(@event);

		if (Input.IsActionJustPressed("ui_accept")) {
			if (sGameState == GameState.PlayerActions) {
				mElapsedTime = 0.0f;
				sGameState = GameState.ExecuteMovement;
			}
		}
		if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.E) {
			Enemy newEnemy = Enemy1.Instantiate<Enemy>();
			AddChild(newEnemy);
			newEnemy.Spawn(new Vector2(336, 176), mPlayer);

			mEnemies.Add(newEnemy);
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
		foreach (var enemy in mEnemies) {
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
						sGameState = GameState.ExecuteMovement;

					} else {
						mElapsedTime += (float)delta;
					}
				}
				break;
			case GameState.ExecuteMovement: {
					// Execute all player and enemies movement
					mPlayer.ExecuteMovement();
					foreach (var enemy in mEnemies) {
						enemy.ExecuteMovement();
					}

					sGameState = GameState.Move;
				}
				break;
			case GameState.Move: {
					// Wait untill movement is completed
					bool completed = true;

					completed &= mPlayer.MovementActionFinished();

					foreach (var enemy in mEnemies) {
						completed &= enemy.MovementActionFinished();
					}

					if (completed) {
						sGameState = GameState.PrepareAttack;
					}
				}
				break;
			case GameState.PrepareAttack: {
					mPlayer.ExecuteAttack();
					foreach (var enemy in mEnemies) {
						enemy.ExecuteAttack();
					}

					sGameState = GameState.Attack;
				}
				break;
			case GameState.Attack: {
					bool completed = true;

					completed &= mPlayer.AttackActionFinished();

					foreach (var enemy in mEnemies) {
						completed &= enemy.AttackActionFinished();
					}

					if (completed) {
						sGameState = GameState.TakeDamage;
					}
				}
				break;

			case GameState.TakeDamage: {


					// Enemies prepare next turn
					sGameState = GameState.PlayerActions;
					mOccupiedTiles.Clear();
					PrepareEnemyActions();

				}
				break;

		}
	}
}
