using Aiv.Fast2D;
using BehaviourEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleGhostGame2D
{
    public sealed class Game
    {
        private static GameManager manager;
        public static void Init()
        {
            #region Context
            BehaviourEngine.Graphics.Instance.Window = new Window(1200, 700, "BomberMan");
            BehaviourEngine.Graphics.Instance.Window.SetDefaultOrthographicSize(15);
            BehaviourEngine.Graphics.Instance.Window.SetClearColor(0.0f, 0.61f, 0.0f);
            Engine.Init(BehaviourEngine.Graphics.Instance.Window);
            #endregion
            manager = GameManager.Instance;

            #region GameManager
            GameObject.Spawn(manager);
            #endregion
        }

        public static void Run()
        {
            Engine.Run();
        }
    }
}
