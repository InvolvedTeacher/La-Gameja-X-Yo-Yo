using Godot;
using System;

public partial class Enemy1 : BaseCharacter {



	public override void MoveToNewTile(Vector2 target) {
		// Todo
	}

	public override bool MovementFinished() {
		return true; // Todo
	}


	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}
}
