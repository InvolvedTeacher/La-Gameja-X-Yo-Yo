using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaGamejaXYoYo.scripts.actions.attack {
    internal partial class Attack : Action {

        internal Manager mManager;
        public Attack(Manager manager) {
            mManager = manager;
        }
    }
}
