using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine;
using OpenTK;
using Graphics = BehaviourEngine.Graphics;

namespace BubbleGhostGame2D
{
    public class Rotator : Component, IUpdatable, IStartable
    {
        public bool IsStarted { get; set; }

        private AnimationRenderer renderer;
        private GameObject target;
        private float range;
        private float t;

        public Rotator(GameObject target, float range)
        {
            this.target = target;
            this.range = range;
        }

        public void Update()
        {
            Vector2 dist = (target.Transform.Position - Owner.Transform.Position);

            if (dist.Length < range)
            {
                bool flip = renderer.Sprite.FlipX && renderer.Sprite.FlipY;
                if (flip)
                    renderer.SetFlip(false, false);
                GetLerped(dist, Graphics.Instance.Window.deltaTime);
            }
            else
                GetLerped(dist, Graphics.Instance.Window.deltaTime, true);
        }

        public void Start()
        {
            renderer = Owner.GetComponent<AnimationRenderer>();
        }

        private void GetLerped(Vector2 distance, float tickness, bool inverse = false)
        {
            float theta = (float)Math.Atan2(distance.Y, distance.X) + MathHelper.Pi;
            t += tickness;
            Owner.Transform.Rotation = Lerp( Owner.Transform.Rotation, !inverse ? theta : 0.0f, t );
            if (t > 1f)
                t = 0f;
        }

        private static float Lerp(float firstFloat, float secondFloat, float t)
        {
            return secondFloat * t + firstFloat * (1 - t);
        }

       
    }
}
