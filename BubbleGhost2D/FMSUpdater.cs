using BehaviourEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleGhostGame2D
{
    public class FSMUpdater : Component, IUpdatable
    {
        private IState currentState;

        public FSMUpdater(IState currentState)
        {
            this.currentState = currentState;
        }

        public void Update()
        {
            currentState = currentState.OnStateUpdate();
        }
    }
}
