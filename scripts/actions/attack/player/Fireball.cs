using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.HttpRequest;
using static System.Net.Mime.MediaTypeNames;

namespace LaGamejaXYoYo.scripts.actions.attack.player {
	internal partial class FireBall : Attack {

		public enum Direction { Left, Right, Up, Down }

		[Export(PropertyHint.None, "suffix:dmg")]
		public int Damage {
			get => mDamage;
			set => mDamage = value;
		}
		private int mDamage = 1;

		private Sprite2D mIndicator;

		private Vector2 mLocation;
		private Direction mDirection;
		
		public FireBall(Manager manager, Vector2 location, Direction direction) : base(manager) {
			mIndicator = mManager.GetNode<Sprite2D>("FireballIndicator");
			mIndicator.Visible = true;
			mIndicator.Position = location;
            // Todo: calculate direction

            mLocation = location;
			mDirection = direction;

        }

		public List<Enemy> GetEnemiesInArea(Vector2 center, int radius) {
			List<Enemy> result = new();

			// Todo

			return result;
		}

		public override void Execute() {
			Vector2 playerTile = Utils.GetTilePosition(mManager.GetPlayer().GlobalPosition);

			var targets = GetEnemiesInArea(playerTile, Utils.GetTileSize());

			foreach (Enemy enemy in targets) {
				enemy.TakeDamage(mDamage);
			}

			mIndicator.Visible = false;
		}

		public override bool IsCompleted() { return true; }
	}
}
