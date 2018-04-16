using BehaviourEngine;
using OpenTK;

namespace BubbleGhostGame2D
{
    class Blowable : Component, IStartable
    {
        public Rigidbody2D rigidBody;

        bool IStartable.IsStarted { get; set; }

        void IStartable.Start()
        {
            rigidBody = Owner?.GetComponent<Rigidbody2D>();
        }

        public virtual void Blow(Vector2 pushForce)
        {
            rigidBody?.AddForce( pushForce );
        }
    }
}
