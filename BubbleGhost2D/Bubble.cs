using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using BehaviourEngine.Renderer;
using OpenTK;
using BubbleGhostGame2D;

namespace BubbleGhostGame2D
{
    public class Bubble : GameObject
    {
        public enum AnimType : byte
        {
            Normal,
            Exploding
        }

        public bool Exploded { get; set; }

        private Dictionary < AnimType, AnimationRenderer >      bubbleRenderer;
        private readonly Box2D                                  collider;
        private static readonly int[]                           frames = { 0, 1, 2, 1 };
        private readonly float                                  time;
        private float                                           updateTime;
        private float                                           min;
        private const float                                     max    = 1.0f;

        //FSM                                                                 
        private IState                                          currentState;
        private StateExplode                                    stateExplode;
        private StateWait                                       stateWait;

        public Bubble( string fileNameBubble, string fileNameExplosion, Vector2 position ) : base( ( int )RenderLayer.Pawn, "Bubble" )
        {
            #region Init
            Exploded         = false;     
            bubbleRenderer   = new Dictionary < AnimType, AnimationRenderer >( );
                             
            RigidBody r      = this.AddBehaviour < RigidBody >( new RigidBody( this ) );
            r.LinearFriction = 3f;
            this.AddBehaviour < Blowable >( new Blowable( this ) );
            #endregion

            #region Renderer

            bubbleRenderer.Add( AnimType.Normal, AddBehaviour( new AnimationRenderer( this, fileNameBubble, position, false, true, 3, 32, 32, new[ ] { 0, 1, 2, 1 }, 0.2f ) ) );

            bubbleRenderer.Add( AnimType.Exploding, AddBehaviour( new AnimationRenderer( this, fileNameExplosion, position, true, false, 3, 32, 32, frames, 0.2f ) ) );

            foreach (var anims in bubbleRenderer)
            {
                anims.Value.Owner.Transform.Position = position;
                anims.Value.Pivot                    = new Vector2( anims.Value.Width / 2, anims.Value.Height / 2 );
                anims.Value.Owner.Transform.Scale    = new Vector2( 0.8f, 0.8f );
            }

            time                                     = bubbleRenderer[ AnimType.Exploding ].LenghtFrames;

            #endregion

            #region Collider

            collider = new Box2D( position, 20f, 20f, new Vector4(255, 0, 255, 255), this );
            AddBehaviour< Box2D >( collider );

            #endregion                         
                                               
            #region FSM                        

            stateExplode      = new StateExplode( this );
            stateWait         = new StateWait( this );
            stateExplode.Next = stateWait;
            stateWait.Next    = stateExplode;
            stateWait.OnStateEnter( );
            currentState      = stateWait;

            #endregion

        }

        public void UpdateStates()
        {
            if ( bubbleRenderer[ AnimType.Normal ] == null || bubbleRenderer[ AnimType.Exploding ] == null ) return;
            currentState = currentState.OnStateUpdate( );
            collider.Position = bubbleRenderer[ AnimType.Normal ].Owner.Transform.Position;
            if ( !bubbleRenderer[ AnimType.Normal ].Owner.Active )
                collider.Position = bubbleRenderer[ AnimType.Exploding ].Owner.Transform.Position;
        }

        private void SetAnimation(AnimType eType, bool update, bool render)
        {
            bubbleRenderer[ eType ].Stop = update;
            bubbleRenderer[ eType ].Show = render;
        }

        private bool GetState()
        {
            return bubbleRenderer[ AnimType.Exploding ].Stop == false &&
                   bubbleRenderer[ AnimType.Exploding ].Show;
        }

        private class StateExplode : IState
        {
            public StateWait Next;
            private readonly Bubble target;
            public StateExplode(Bubble target)
            {
                this.target = target;
            }

            public void OnStateEnter( )
            {
                //OnStateUpdate( );
            }

            public void OnStateExit( )
            {
            }

            public IState OnStateUpdate( )
            {
                if (!Next.IsLevelCollider)
                {
                    target.updateTime += Engine.DeltaTime;
                    target.SetAnimation(AnimType.Normal, true, false);
                    CameraManager.Instance.Shake(1.9f, 0.2f);
                    if (target.updateTime <= target.time) return this;
                    target.min += Engine.DeltaTime;
                    target.SetAnimation(AnimType.Exploding, false, true);
                    Engine.Destroy(target);
                    if (!(target.min >= max)) return this;
                }

                Next.OnStateEnter( );
                return Next;
            }
        }

        private class StateWait : IState
        {
            public StateExplode Next;
            public bool IsLevelCollider { get; set; }
            private readonly Bubble target;

            public StateWait( Bubble target )
            {
                this.target = target;
                IsLevelCollider = false;
            }

            public void OnStateEnter()
            {
                OnStateUpdate();
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                if ( target.GetState( ) )  return this;

                if (Engine.ComputeIntersect(Engine.levelColliders, target.GetComponent<Box2D>()))
                {
                    IsLevelCollider = false;
                    return this;
                }

                else if ( Engine.ComputeIntersect(Engine.Boxes, target.GetComponent<Box2D>( ) ) )
                {
                    OnStateExit();
                    Next.OnStateEnter();
                    return Next;
                }
                return this;
            }
        }
    }
}
