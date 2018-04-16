using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine.Renderer;
using OpenTK;
using BubbleGhostGame2D;
using BehaviourEngine.Interfaces;
using Aiv.Fast2D;

namespace BehaviourEngine.Test
{
    public class Candle : GameObject
    {
        private AnimationRenderer renderer;
        private SwitchCandle animation;
        public Candle(string fileName, Vector2 drawPosition, GameObject target ) : base( ( int )RenderLayer.Pawn, "Candles" )
        {
            renderer = AddBehaviour<AnimationRenderer>(new AnimationRenderer(this, fileName, drawPosition, false,
                true, 3, 32, 32, new[] { 1, 0, 1, 2, 0 }, .5f));

            renderer.Owner.Transform.Position = drawPosition;
            renderer.Pivot = new Vector2(renderer.Width / 2, renderer.Height / 2);
        }
    }

    public class SwitchCandle : Component, IUpdatable
    {
        private float range;
        private GameObject target;
        public SwitchCandle(GameObject owner,GameObject target, float range) : base(owner)
        {
            this.range = range;
            this.target = target;
        }

        public void Update()
        {
            if (Engine.GetKey(KeyCode.Space))
            {
                Owner.GetComponent<AnimationRenderer>().currentFrame = 3;
                Owner.GetComponent<AnimationRenderer>().Stop = true;
            }

            Vector2 dist = (target.Transform.Position - Owner.Transform.Position);

            if (dist.Length < range) return;

        }
    }
}
