﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleGhostGame2D
{
    internal class Game : GameObject
    {
        public Game()
        {
            AddBehaviour<GameManager>(GameManager.Instance);
        }
    }
}