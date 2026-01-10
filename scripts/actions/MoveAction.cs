using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaGamejaXYoYo.scripts.actions {
    internal class MoveAction : Action {

        private Vector2 mTargetPosition;
        private BaseCharacter mEntity;
    
        public MoveAction(Vector2 target, BaseCharacter entity) {
            mTargetPosition = target;
            mEntity = entity;
        }

        public override void Execute() {
            mEntity.MoveToNewTile(mTargetPosition);
        }

        public override bool IsCompleted() {
            return mEntity.MovementFinished();
        }


        public Vector2 GetTargetPostion() { return mTargetPosition; }
    }
}
