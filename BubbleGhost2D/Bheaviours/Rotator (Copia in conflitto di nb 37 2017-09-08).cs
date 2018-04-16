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
        public  GameObject m_hTarget;
        private LightRadius lightRadiusComponent;
        private SpriteRenderer renderer;
        private float t;
        private float fTheta;
        public GameObject target;

        private static float Lerp(float firstFloat, float secondFloat, float t)
        {
            return secondFloat * t + firstFloat * (1 - t);
        }

        public void Start()
        {
            renderer = target.GetComponent<SpriteRenderer>();
            lightRadiusComponent = Owner.GetComponent<LightRadius>();
            lightRadiusComponent.IsEnabled = true;
            lightRadiusComponent.IsStarted = true;
            lightRadiusComponent.LightRad = 4f;
        }

        public void Update()
        {
            //TODO: fix sprite rotation when it's out of radius
            if (!lightRadiusComponent.InRadius(m_hTarget))
            {
                m_hTarget.Rotation = Lerp(m_hTarget.Rotation, 0.0f, 2f * Engine.DeltaTime);
                t = 0f;
                return;
            }

            var vDist = (-m_hTarget.Position + Owner.Position).Normalized();
            if(renderer.GetFlip())
                fTheta = (float)Math.Atan2(-vDist.Y, -vDist.X);
            else
                fTheta = (float)Math.Atan2(vDist.Y, vDist.X);

            t += Engine.DeltaTime;
            Console.WriteLine(t);
            m_hTarget.Rotation = Lerp(m_hTarget.Rotation, fTheta + MathHelper.Pi, t);
            if (t > 1f)
                t = 0f;
        }
    }
}
