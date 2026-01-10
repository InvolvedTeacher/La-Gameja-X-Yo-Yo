using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.HttpRequest;
using static System.Net.Mime.MediaTypeNames;

namespace LaGamejaXYoYo.scripts.actions.attack.player {
	internal partial class Hurricane : Attack {

		[Export(PropertyHint.None, "suffix:dmg")]
		public int Damage {
			get => mDamage;
			set => mDamage = value;
		}
		private int mDamage = 1;
		public Hurricane(Manager manager) : base(manager) {
		}

		public List<Enemy> GetEnemiesInRadius(Vector2 center, int radius) {
			List<Enemy> result = new();

			foreach (Enemy enemy in mManager.GetEnemies()) {
				Vector2 enemyTile = enemy.Position;

				float distance = center.DistanceTo(enemyTile);
				if (distance < radius * 2) {
					result.Add(enemy);
				}
			}
			return result;
		}
		public override void _Draw() {
			DrawCircle(
				mManager.GetPlayer().GlobalPosition,
				Utils.GetTileSize(),
				new Color(1, 0, 0, 0.3f)
			);
		}


		public override void Execute() {
			Vector2 playerTile = Utils.GetTilePosition(mManager.GetPlayer().GlobalPosition);

			var targets = GetEnemiesInRadius(playerTile, Utils.GetTileSize());

			foreach (Enemy enemy in targets) {
				enemy.TakeDamage(mDamage);
			}
		}

		public override bool IsCompleted() { return true; }
	}
}
