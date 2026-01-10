using Godot;
using LaGamejaXYoYo.scripts;
using System;

public partial class TargetTileHighlight : Sprite2D {
	
	public override void _Ready() {
		// Start hidden
		UpdateVisibility(false);
	}

	public override void _Process(double delta) {
	}

	public void UpdatePosition(Vector2 position) {
		Position = position; 
	}
	public void UpdateVisibility(bool visible) {
		Visible = visible;
	}
}
