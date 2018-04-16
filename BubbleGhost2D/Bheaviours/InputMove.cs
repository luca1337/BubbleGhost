using Aiv.Fast2D;
using BehaviourEngine.Interfaces;
using OpenTK;

namespace BubbleGhostGame2D
{
    class InputMove : Component, IUpdatable
    {

        public float Speed;

        public InputMove(GameObject owner) : base(owner)
        {
        }

        public void Update()
        {
            if (Engine.GetKey(KeyCode.W))
            {
                Owner.Transform.Position -= new Vector2(0f, Speed) * Engine.DeltaTime;
            }

            if (Engine.GetKey(KeyCode.S))
            {
                Owner.Transform.Position += new Vector2(0f, Speed) * Engine.DeltaTime;
            }

            if (Engine.GetKey(KeyCode.D))
            {
                Owner.Transform.Position += new Vector2(Speed, 0f) * Engine.DeltaTime;
            }

            if (Engine.GetKey(KeyCode.A))
            {
                Owner.Transform.Position -= new Vector2(Speed, 0f) * Engine.DeltaTime;
            }
        }
    }
}
