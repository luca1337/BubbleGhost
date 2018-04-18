using BehaviourEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace BubbleGhostGame2D
{
    public class Wall : GameObject
    {
        private SpriteRenderer renderer;
        private BoxCollider2D collider;

        public Wall()
        {
            renderer = new SpriteRenderer(FlyWeight.Get("Wall"));
            collider = new BoxCollider2D(Vector2.One);
        }
    }
}
