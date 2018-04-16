using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace BehaviourEngine.Test
{
    public class Rotator : Behaviour, IUpdatable, IStartable
    {
        public GameObject m_hTarget;
        private LightRadius lightRadiusComponent;
        private float t;

        private static float Lerp(float fFrom, float fTo, float t)
        {
            return fTo * t + fFrom * (1 - t);
        }

        public void Start()
        {
            lightRadiusComponent           = Owner.GetComponent<LightRadius>();
            lightRadiusComponent.IsEnabled = true;
            lightRadiusComponent.IsStarted = true;
            lightRadiusComponent.LightRad  = 3f;
        }

        public void Update()
        {
            if (!lightRadiusComponent.InRadius(m_hTarget)) { t = 0f; m_hTarget.Rotation = Lerp(m_hTarget.Rotation, 0 , 1); return; }

            var vDist = (-m_hTarget.Position + Owner.Position).Normalized();
            var fTheta = (float)Math.Atan2(vDist.Y, vDist.X);
            t += Engine.DeltaTime;
            m_hTarget.Rotation = Lerp(m_hTarget.Rotation, fTheta + MathHelper.PiOver2, t);
            if (!(t > 1f)) return;
            t = 0f;
        }
    }
}
