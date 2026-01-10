using Godot;

public partial class FollowCamera2D : Camera2D {
    [Export] public NodePath PlayerPath;

    // Dead zone size (in pixels)
    [Export] public Vector2 DeadZoneSize = new Vector2(200, 120);

    // World bounds (top-left and bottom-right)
    [Export] public Vector2 WorldMin;
    [Export] public Vector2 WorldMax;

    private Node2D mPlayer;

    public override void _Ready() {
        mPlayer = GetNode<Node2D>(PlayerPath);
    }

    public override void _Process(double delta) {
        if (mPlayer == null)
            return;

        Vector2 cameraPos = GlobalPosition;
        Vector2 playerPos = mPlayer.GlobalPosition;

        // Dead zone rectangle centered on the camera
        Rect2 deadZone = new Rect2(
            cameraPos - DeadZoneSize * 0.5f,
            DeadZoneSize
        );

        // Move camera only if player leaves dead zone
        if (playerPos.X < deadZone.Position.X)
            cameraPos.X = playerPos.X + DeadZoneSize.X * 0.5f;
        else if (playerPos.X > deadZone.End.X)
            cameraPos.X = playerPos.X - DeadZoneSize.X * 0.5f;

        if (playerPos.Y < deadZone.Position.Y)
            cameraPos.Y = playerPos.Y + DeadZoneSize.Y * 0.5f;
        else if (playerPos.Y > deadZone.End.Y)
            cameraPos.Y = playerPos.Y - DeadZoneSize.Y * 0.5f;

        // Clamp camera to world bounds
        Vector2 halfViewport = GetViewportRect().Size * 0.5f * Zoom;

        cameraPos.X = Mathf.Clamp(
            cameraPos.X,
            WorldMin.X + halfViewport.X,
            WorldMax.X - halfViewport.X
        );

        cameraPos.Y = Mathf.Clamp(
            cameraPos.Y,
            WorldMin.Y + halfViewport.Y,
            WorldMax.Y - halfViewport.Y
        );

        GlobalPosition = cameraPos;
    }
}