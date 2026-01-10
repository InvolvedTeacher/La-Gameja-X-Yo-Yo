using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaGamejaXYoYo.scripts.actions {
    internal class MoveAction : Action {

        private Vector2 mTargetPosition;
        private BaseCharacter mPlayer;
    
        public MoveAction(Vector2 target, Player player) {
            mTargetPosition = target;
            mPlayer = player;
        }

        public override void Execute() {
            mPlayer.MoveToNewTile(mTargetPosition);
        }

        public override bool IsCompleted() {
            return mPlayer.MovementFinished();
        }


        public Vector2 GetTargetPostion() { return mTargetPosition; }
    }
}
