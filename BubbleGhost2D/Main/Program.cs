using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;
using BehaviourEngine;

namespace BubbleGhostGame2D
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Init window
            Engine.Init(Engine.FixedWidth, Engine.FixedHeight, "BubbleGhost");

            //Run Game Engine
            Engine.Run();
        }
    }
}
