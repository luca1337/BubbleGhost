﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourEngine.Test
{
    internal class Game : GameObject
    {
        public Game() : base( ( int )RenderLayer.None )
        {
            AddBehaviour<GameManager>(GameManager.Instance);
        }
    }
}
