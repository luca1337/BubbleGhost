using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleGhostGame2D
{
    public interface IState
    {
        void OnStateEnter();
        void OnStateExit();
        IState OnStateUpdate();
    }
}
