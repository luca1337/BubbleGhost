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
using BehaviourEngine;

namespace BubbleGhostGame2D
{
    public class Ghost : GameObject, IDrawable
    {
        private readonly AnimationRenderer renderer;
        private InputMove input;
        private RollBack rollBack;
        private Lifes lifes;
        private BoxCollider2D box;

        public Ghost( string fileName, int life, Vector2 drawPosition, GameObject target, float range = 120 ) : base( ( int )RenderLayer.Pawn, "Ghost" )
        {
            //Animation render
            renderer = AddComponent( new AnimationRenderer( this, fileName, drawPosition, false,
                true, 4, 32, 32, new[ ] { 0, 1, 2, 3 }, 0.1f ) );

            AddComponent( new SwitchAnimation( this, target, range,true ) );
            renderer.Owner.Transform.Position = drawPosition;
            renderer.Owner.Transform.Scale    = new Vector2( 1.2f, 1.2f );
            renderer.Pivot                    = new Vector2(renderer.Width / 2, renderer.Height / 2);

            //Collider
            box                               = new BoxCollider2D( this.Transform.Position, 32, 32, new Vector4(255, 0, 255, 255), this );
            AddComponent( box );

            //Behaviours
            AddComponent( new Rotator( this, target, range ) );
            AddComponent( new Blower( this, target.GetComponent < Blowable >( ), range, 3 ) );

            //Input
            input                             = AddComponent( new InputMove( ) );
            input.Speed                       = 90.0f;

            //Rollback
            rollBack                          = AddComponent( new RollBack( this ) );
            rollBack.box                      = this.box;

            //Lifes
            lifes = AddComponent( new Lifes( life, target, this ) );
        }

        public int RenderOffset { get; set; }
        public bool Enabled { get; set; }

        public void Draw()
        {
        }
    }

    public class RollBack : Component, IUpdatable
    {
        private GameObject owner;
        public BoxCollider2D box;

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

}
