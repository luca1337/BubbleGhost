using Aiv.Fast2D;
using OpenTK;
using BehaviourEngine.Interfaces;
using BehaviourEngine.Renderer;

namespace BehaviourEngine.Test.Bhaviours
{
    public class SwitchAnimation : Behaviour, IUpdatable
    {
        private readonly GameObject target;
        private readonly float radius;
        private bool isGhost;
        public SwitchAnimation(GameObject owner, GameObject target, float radius, bool isGhost) : base(owner)
        {
            this.isGhost = isGhost;
            this.target  = target;
            this.radius  = radius;
        }

        public void Update()
        {
            if ( Engine.GetKey( KeyCode.Space ) )
            {
                Owner.GetComponent < AnimationRenderer >( ).currentFrame = 3;
                Owner.GetComponent < AnimationRenderer >( ).Stop = true;
            }
            else
            {
                Owner.GetComponent < AnimationRenderer >( ).Stop = false;
            }


            Vector2 dist = (target.Transform.Position - Owner.Transform.Position);

            if (dist.Length < radius) return;

            if (isGhost)
            {
                 if (Engine.GetKey(KeyCode.D))
                 {
                     Owner.GetComponent<AnimationRenderer>().SetFlip(false, true);
                 }
                 
                 if (Engine.GetKey(KeyCode.A))
                 {
                     Owner.GetComponent<AnimationRenderer>().SetFlip(false, false);
                 }
            }
        }
    }
}
