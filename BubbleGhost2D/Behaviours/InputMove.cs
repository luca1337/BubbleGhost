using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BehaviourEngine.Test
{
    class InputMove : Behaviour, IUpdatable
    {

        public float Speed;

        public void Update()
        {
            if (Engine.GetKey(KeyCode.W))
            {
                Owner.Position -= new Vector2(0, Speed) * Engine.DeltaTime;
            }

            if (Engine.GetKey(KeyCode.S))
            {
                Owner.Position += new Vector2(0, Speed) * Engine.DeltaTime;
            }

            if (Engine.GetKey(KeyCode.D))
            {
                Owner.Position += new Vector2(Speed, 0) * Engine.DeltaTime;
            }

            if (Engine.GetKey(KeyCode.A))
            {
                Owner.Position -= new Vector2(Speed, 0) * Engine.DeltaTime;
            }
        }
    }
}
