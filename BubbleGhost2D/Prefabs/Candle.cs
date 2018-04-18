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

        public Candle()
        {
            renderer              = AddComponent(new AnimationRenderer(FlyWeight.Get("Candle"), 32, 32, 3, new[] { 1, 0, 1, 2, 0 }, 0.5f, false,  true));
            renderer.Sprite.pivot = new Vector2(renderer.Sprite.Width / 2, renderer.Sprite.Height / 2);
        }
    }

    public class SwitchCandle : Component, IUpdatable
    {
        private float range;
        private GameObject target;
        public SwitchCandle(GameObject target, float range) 
        {
            this.range = range;
            this.target = target;
        }

        public void Update()
        {
            if (Input.IsKeyPressed(KeyCode.Space))
            {
                Owner.GetComponent<AnimationRenderer>().SetCurrentIndex(3);
                Owner.GetComponent<AnimationRenderer>().Stop = true;
            }

            Vector2 dist = (target.Transform.Position - Owner.Transform.Position);

            if (dist.Length < range) return;

        }
    }
}
