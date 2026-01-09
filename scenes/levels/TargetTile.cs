using Godot;
using LaGamejaXYoYo.scripts;
using System;

public partial class TargetTile : Sprite2D {

	[Export]
	public NodePath PlayerPath;
	
	private Player mPlayer;
	public override void _Ready() {
		mPlayer = GetNode<Player>(PlayerPath);
	}

	public override void _Process(double delta) {
		Vector2 mousePosition = Utils.GetTilePosition(GetGlobalMousePosition());

		if (Utils.IsInRange(mousePosition, Utils.GetTilePosition(mPlayer.Position))) {
			Visible = true;
			Position = mousePosition;
		} else {
			Visible = false;
		}
	}
}
