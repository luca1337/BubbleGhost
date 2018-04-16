﻿using Aiv.Fast2D;
using BehaviourEngine.Interfaces;
using OpenTK;

namespace BehaviourEngine.Test.Bhaviours
{
    internal class Blower : Component, IUpdatable
    {
        public float Force;
        private readonly float range;
        public Blowable Target;

        public Blower(GameObject owner, Blowable target, float range, float force = 1f) : base(owner)
        {
            Target = target;
            Force = force;
            this.range = range;
        }

        private static float Lerp(float firstFloat, float secondFloat, float t)
        {
            return secondFloat * t + firstFloat * (1 - t);
        }

        void IUpdatable.Update()
        {
            if(Engine.Window.GetKey(KeyCode.Space))
            {
                Vector2 dist = Target.Owner.Transform.Position - Owner.Transform.Position;
                if(dist.Length < range)
                {
                    Vector2 dir = dist.Normalized();
                    Target.Blow(Force * dir);
                }
            }
        }
    }
}
