using Aiv.Fast2D;
using OpenTK;
using BehaviourEngine;
using Aiv.Fast2D.Utils.Input;

namespace BubbleGhostGame2D
{
    public class SwitchAnimation : Component, IUpdatable
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
            if (Input.IsKeyPressed( KeyCode.Space ) )
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
                 if (Input.IsKeyPressed(KeyCode.D))
                 {
                     Owner.GetComponent<AnimationRenderer>().SetFlip(false, true);
                 }
                 
                 if (Input.IsKeyPressed(KeyCode.A))
                 {
                     Owner.GetComponent<AnimationRenderer>().SetFlip(false, false);
                 }
            }
        }
    }
}
