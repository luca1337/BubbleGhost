using BubbleGhostGame2D;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using BehaviourEngine.Interfaces;
using BehaviourEngine.Renderer;

namespace BehaviourEngine.Test
{
    public class Ghost : GameObject, IDrawable
    {
        private readonly AnimationRenderer renderer;
        private InputMove input;
        private RollBack rollBack;
        private Lifes lifes;
        private Box2D box;

        public Ghost( string fileName, int life, Vector2 drawPosition, GameObject target, float range = 120 ) : base( ( int )RenderLayer.Pawn, "Ghost" )
        {
            //Animation render
            renderer = AddBehaviour( new AnimationRenderer( this, fileName, drawPosition, false,
                true, 4, 32, 32, new[ ] { 0, 1, 2, 3 }, 0.1f ) );

            AddBehaviour( new SwitchAnimation( this, target, range,true ) );
            renderer.Owner.Transform.Position = drawPosition;
            renderer.Owner.Transform.Scale    = new Vector2( 1.2f, 1.2f );
            renderer.Pivot                    = new Vector2(renderer.Width / 2, renderer.Height / 2);

            //Collider
            box                               = new Box2D( this.Transform.Position, 32, 32, new Vector4(255, 0, 255, 255), this );
            AddBehaviour( box );

            //Behaviours
            AddBehaviour( new Rotator( this, target, range ) );
            AddBehaviour( new Blower( this, target.GetComponent < Blowable >( ), range, 3 ) );

            //Input
            input                             = AddBehaviour( new InputMove( this ) );
            input.Speed                       = 90.0f;

            //Rollback
            rollBack                          = AddBehaviour( new RollBack( this ) );
            rollBack.box                      = this.box;

            //Lifes
            lifes = AddBehaviour( new Lifes( life, target, this ) );
        }
    }

    public class RollBack : Component, IUpdatable
    {
        private GameObject owner;
        public Box2D box;

        public RollBack( GameObject owner ) : base( owner )
        {
            this.owner = owner;
        }

        public void Update( )
        {
            if ( box == null || owner == null ) return;
            Vector2 oldPos = box.Position;

            box.Position   = owner.Transform.Position;

            bool prev      = Engine.IsOutOfScreen( this.owner.Transform.Position );

            if ( !prev ) return;
            owner.Transform.Position = oldPos;
            box.Position             = oldPos;
        }
    }

    public class Lifes : Component, IUpdatable
    {        
        private GameObject target;
        public int LifesCount { get; private set; }

        public Lifes( int lifes, GameObject target, GameObject owner ) : base( owner )
        {
            this.LifesCount = lifes;
            this.target = target;
        }

        public void Update( )
        {
            if ( Engine.ComputeIntersect(Engine.Boxes, target.GetComponent<Box2D>( ) ) )
            {
                LifesCount--;
            }
        }
    }
}
