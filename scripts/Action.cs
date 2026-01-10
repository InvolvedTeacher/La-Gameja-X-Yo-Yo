using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaGamejaXYoYo.scripts {
    internal partial class Action {
        public Action() { }

        public virtual void Execute() {

        }

        public virtual bool IsCompleted() { return true; }
    }
}
