using Aiv.Fast2D;
using Aiv.Fast2D.Utils.Input;
using BehaviourEngine;
using OpenTK;
using Graphics = BehaviourEngine.Graphics;

namespace BubbleGhostGame2D
{
    class InputMove : Component, IUpdatable
    {

        public float Speed;

        public InputMove( ) 
        {
        }

        public void Update()
        {
            if (Input.IsKeyPressed(KeyCode.W))
                Owner.Transform.Position -= new Vector2(0f, Speed) * Graphics.Instance.Window.deltaTime;

            if (Input.IsKeyPressed(KeyCode.S))
                Owner.Transform.Position += new Vector2(0f, Speed) * Graphics.Instance.Window.deltaTime;

            if (Input.IsKeyPressed(KeyCode.D))
                Owner.Transform.Position += new Vector2(Speed, 0f) * Graphics.Instance.Window.deltaTime;

            if (Input.IsKeyPressed(KeyCode.A))
                Owner.Transform.Position -= new Vector2(Speed, 0f) * Graphics.Instance.Window.deltaTime;
        }
    }
}
