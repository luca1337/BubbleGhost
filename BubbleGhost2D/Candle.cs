using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine;
using OpenTK;
using Aiv.Fast2D;
using Aiv.Fast2D.Utils.Input;

namespace BubbleGhostGame2D
{
    public class Candle : GameObject
    {
        private AnimationRenderer renderer;
        private SwitchCandle animation;

        public Candle(string fileName, Vector2 drawPosition, GameObject target ) : base( ( int )RenderLayer.Pawn, "Candles" )
        {
            renderer = AddComponent(new AnimationRenderer(this, fileName, drawPosition, false,
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
            if (Input.IsKeyPressed(KeyCode.Space))
            {
                Owner.GetComponent<AnimationRenderer>().currentFrame = 3;
                Owner.GetComponent<AnimationRenderer>().Stop = true;
            }

            Vector2 dist = (target.Transform.Position - Owner.Transform.Position);

            if (dist.Length < range) return;

        }
    }
}
