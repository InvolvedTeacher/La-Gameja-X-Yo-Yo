using Godot;
using Godot.Collections;
using LaGamejaXYoYo.scripts;
using System;

public partial class EnemyIndicatorManager : CanvasLayer {
    [Export] public PackedScene IndicatorScene;
    [Export] public float ScreenPadding = 20f;
    [Export] public Manager manager;

    private Dictionary<Enemy, Control> _indicators = new();

    private Control GetOrCreateIndicator(Enemy enemy) {
        if (_indicators.TryGetValue(enemy, out var indicator))
            return indicator;

        indicator = IndicatorScene.Instantiate<Control>();
        AddChild(indicator);
        _indicators[enemy] = indicator;

        enemy.TreeExited += () => {
            indicator.QueueFree();
            _indicators.Remove(enemy);
        };

        return indicator;
    }

    private void HideIndicator(Enemy enemy) {
        if (_indicators.TryGetValue(enemy, out var indicator))
            indicator.Visible = false;
    }

    private void UpdateIndicator(Enemy enemy, Camera2D camera, Rect2 viewportRect) {
        Vector2 screenPos = GetViewport().GetCanvasTransform() * enemy.GlobalPosition;
        bool onScreen = viewportRect.HasPoint(screenPos);

        if (onScreen) {
            HideIndicator(enemy);
            return;
        }

        Control indicator = GetOrCreateIndicator(enemy);
        indicator.Visible = true;

        // Clamp to screen edge
        Vector2 edgePos = ClampToRectEdge(screenPos, viewportRect, ScreenPadding);

        indicator.Position = edgePos;
    }

    private Vector2 ClampToRectEdge(Vector2 screenPos, Rect2 rect, float padding) {
        return new Vector2(Mathf.Clamp(screenPos.X, rect.Position.X + padding, rect.Position.X + rect.Size.X - padding - Utils.GetTileSize()),
            Mathf.Clamp(screenPos.Y, rect.Position.Y + padding, rect.Position.Y + rect.Size.Y - padding - Utils.GetTileSize()));
    }

    public override void _Process(double delta) {
        Camera2D camera = GetViewport().GetCamera2D();
        if (camera == null)
            return;

        Rect2 viewportRect = GetViewport().GetVisibleRect();

        foreach (Enemy enemy in manager.GetEnemies()) {
            UpdateIndicator(enemy, camera, viewportRect);
        }
    }
}
