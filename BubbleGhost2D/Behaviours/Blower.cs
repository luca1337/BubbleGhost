using System;
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
        private LightRadius hLightRadiusComponent;
        public GameObject m_hTarget;
        private const int m_fSpeed = 2;
        public void Start()
        {
            hLightRadiusComponent = m_hTarget.GetComponent<LightRadius>();
            if (hLightRadiusComponent == null) return;
            hLightRadiusComponent.IsStarted = true;
            hLightRadiusComponent.IsEnabled = true;
            hLightRadiusComponent.LightRad = 3f;
        }

        public void Update()
        {
            if (!Engine.GetKey(KeyCode.Space) || !hLightRadiusComponent.InRadius(Owner)) return;
            Vector2 force = new Vector2((float)Math.Sin(Owner.Rotation) * m_fSpeed, -(float)Math.Cos(Owner.Rotation) * m_fSpeed);
            m_hTarget.Position += force * Engine.DeltaTime;
        }
    }
}
