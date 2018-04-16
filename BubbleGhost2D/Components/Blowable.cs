﻿namespace BubbleGhostGame2D
{
    class Blowable : Component, IStartable
    {
        public RigidBody rigidBody;

        bool IStartable.IsStarted { get; set; }

        public Blowable( GameObject owner ) : base( owner )
        {
        }

        void IStartable.Start()
        {
            rigidBody = Owner?.GetComponent<RigidBody>();
        }

        public virtual void Blow(Vector2 pushForce)
        {
            rigidBody?.AddForce( pushForce );
        }
    }
}