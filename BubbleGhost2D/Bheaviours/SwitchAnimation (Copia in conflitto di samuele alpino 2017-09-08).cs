using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourEngine.Test.Bhaviours
{
    public class SwitchAnimation : Behaviour, IUpdatable, IStartable
    {
        private SpriteRenderer spriteRenderer;
        private GameObject target;
        private LightRadius radiusComponent;
        private GameObject Enemy { get; set; }

        public SwitchAnimation(GameObject target, GameObject enemy)
        {
            this.target     = target;
            this.Enemy      = enemy;
            spriteRenderer  = target.GetComponent<SpriteRenderer>();
            radiusComponent = Enemy.GetComponent<LightRadius>();

        }

        public void Start()
        {
        }

        public void Update()
        {
            if (!radiusComponent.InRadius(target)) return;

            if (Engine.GetKey(KeyCode.Space))
                spriteRenderer.Texture = FlyWeight.Get("GhostBlow");
            else
                spriteRenderer.Texture = FlyWeight.Get("Ghost");

            if (!radiusComponent.InRadius(target))
            {
                //FLIP RELATED
                if (Engine.GetKey(KeyCode.D))
                {
                    spriteRenderer.SetFlip(false, true);
                }

                if (Engine.GetKey(KeyCode.A))
                {
                    spriteRenderer.SetFlip(false, false);
                }
            }
        }
    }
}
