using Godot;
using System;

namespace LaGamejaXYoYo.scripts {
    public partial class Utils {
        [Export]
        public static int MaxMovementRange = 5;

        private const int kTileSize = 32;
        private const int kHalfSize = kTileSize / 2;


        public static Vector2 GetTilePosition(Vector2 worldPosition) {
            return new Vector2(worldPosition.X - worldPosition.X % kTileSize + kHalfSize,
                worldPosition.Y - worldPosition.Y % kTileSize + kHalfSize);
        }

        public static int GetTileSize() { return kTileSize; }

        public static bool IsInRange(Vector2 position1, Vector2 position2) {
            return (position1.DistanceTo(position2) <= kTileSize * MaxMovementRange);
        }
    }
}