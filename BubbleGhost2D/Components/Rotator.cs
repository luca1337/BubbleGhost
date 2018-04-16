﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine.Interfaces;
using BehaviourEngine.Renderer;
using OpenTK;

namespace BubbleGhostGame2D
{
    public class Rotator : Component, IUpdatable, IStartable
    {
        private AnimationRenderer renderer;
        private GameObject target;
        private float range;
        private float t;

        public Rotator(GameObject owner, GameObject target, float range) : base(owner)
        {
            this.target = target;
            this.range = range;
        }

        public bool IsStarted { get; set; }

        private static float Lerp(float firstFloat, float secondFloat, float t)
        {
            return secondFloat * t + firstFloat * (1 - t);
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

        void IUpdatable.Update()
        {
            Vector2 dist = (target.Transform.Position - Owner.Transform.Position);

            if (dist.Length < range)
            {
                if(renderer.GetFlip())
                    renderer.SetFlip(false, false);
                GetLerped(dist, Engine.DeltaTime);
            }
            else
                GetLerped(dist, Engine.DeltaTime, true);
        }
    }
}