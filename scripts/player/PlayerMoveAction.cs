using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaGamejaXYoYo.scripts.player {
    internal class PlayerMoveAction : Action {

        private Vector2 mTargetPosition;
        private Player mPlayer;
    
        public PlayerMoveAction(Vector2 target, Player player) {
            mTargetPosition = target;
            mPlayer = player;
        }

        public override void Execute() {
            mPlayer.MoveToNewTile(mTargetPosition);
        }

        public override bool IsCompleted() {
            return mPlayer.MovementFinished();
        }
    }
}
