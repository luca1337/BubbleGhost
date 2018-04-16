﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;

namespace BehaviourEngine.Test
{
    internal class Blower : Behaviour, IUpdatable, IStartable
    {
        public GameObject m_hTarget;
        private LightRadius hLightRadiusComponent;
        private const int m_fSpeed = 2;
        private float t;
        private float fSpeed;
        private float tickness = 0.004f;
        private float zero = 0.0f;
        private float fMinLerp = 0.0f;
        private float fMaxLerp = 20.0f;

        private Vector2 force;

        public void Start()
        {
            hLightRadiusComponent = m_hTarget.GetComponent<LightRadius>();
            if (hLightRadiusComponent == null) return;
            hLightRadiusComponent.IsStarted = true;
            hLightRadiusComponent.IsEnabled = true;
            hLightRadiusComponent.LightRad = 3f;
            
            
        }

        private static float Lerp(float firstFloat, float secondFloat, float t)
        {
            return secondFloat * t + firstFloat * (1 - t);
        }

        private void GetLerp(float tickness)
        {
            t      += tickness;
            fSpeed = Lerp(fMinLerp, fMaxLerp, t);
            force  = new Vector2((float)Math.Sin(Owner.Rotation) * fSpeed, -(float)Math.Cos(Owner.Rotation) * fSpeed);
        }
        public void Update()
        {
            if (Engine.GetKey(KeyCode.Space) && hLightRadiusComponent.InRadius(Owner))
            {
                GetLerp(tickness);
                if (t > 1f)
                    t = 0f;
            }
            if( ! ( Engine.GetKey(KeyCode.Space ) )  && hLightRadiusComponent.InRadius(Owner))
            {
                //if (t > 1)
                //    t = 0;
                fSpeed = Lerp(fMinLerp, fMaxLerp, 1f);
                force  = new Vector2((float)Math.Sin(Owner.Rotation) * fSpeed, -(float)Math.Cos(Owner.Rotation) * fSpeed);

            }
            else if (!hLightRadiusComponent.InRadius(Owner) && t > 0)
            {
                GetLerp(-tickness);
            }
                m_hTarget.Position += force * Engine.DeltaTime;
        }
    }
}