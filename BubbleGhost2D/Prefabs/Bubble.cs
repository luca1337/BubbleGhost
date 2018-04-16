using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BubbleGhostGame2D
{
    public class Bubble : GameObject
    {
        public enum AnimType : byte
        {
            Normal,
            Exploding
        }

        private readonly Dictionary
            <AnimType, AnimationRenderer>     bubbleRenderer;
        private LightRadius                   radius;
        private readonly BoxCollider          collider;
        private static readonly int[]         iFrames = { 3, 5, 7 };
        private readonly float                fTime;
        private float                         fUpdate;
        private float                         fMin;
        private const float                   fMax    = 0.57f;

        //FSM                                               
        private IState                        currentState;
        private StateExplode                  stateExplode;
        private StateWait                     stateWait;

        public BoxCollider Collider => collider;

        public Bubble(string fileName, Vector2 position)
        {
            #region Init
            radius                            = AddBehaviour<LightRadius>(new LightRadius());
            #endregion

            #region Renderer
            bubbleRenderer = new Dictionary<AnimType, AnimationRenderer>
            {
                {
                    AnimType.Normal,
                    AddBehaviour<AnimationRenderer>(
                        new AnimationRenderer(fileName, position, false, true, 3, 64, 50, new[] {0, 1, 0}, 0.5f))
                },
                {
                    AnimType.Exploding,
                    AddBehaviour<AnimationRenderer>(
                        new AnimationRenderer(fileName, position, true, false, 3, 64, 60, iFrames, 0.5f))
                }
            };
            bubbleRenderer.ToList().ForEach(x => { x.Value.Owner.Position = position; x.Value.Pivot = new Vector2(x.Value.Width / 2, x.Value.Height / 2);});
            fTime = bubbleRenderer[AnimType.Exploding].LenghtFrames;
            #endregion

            #region Collider
            collider                           = new BoxCollider(position, 1f, 1f, this);
            collider.Pivot                     = collider.Center;
            AddBehaviour<BoxCollider>(collider);
            #endregion                         
                                               
            #region FSM                        
            stateExplode                       = new StateExplode(this);
            stateWait                          = new StateWait(this);
            stateExplode.Next                  = stateWait;
            stateWait.Next                     = stateExplode;
            stateWait.OnStateEnter();          
            currentState                       = stateWait;
            #endregion
        }

        public void UpdateStates()
        {
            if (bubbleRenderer[AnimType.Normal] == null || bubbleRenderer[AnimType.Exploding] == null) return;
            //Update FSM current state.
            currentState             = currentState.OnStateUpdate();
            collider.Position        = bubbleRenderer[AnimType.Normal].Position;
            if (!bubbleRenderer[AnimType.Normal].Owner.Active)
                collider.Position = bubbleRenderer[AnimType.Exploding].Position;
        }

        private void SetAnimation(AnimType eType, bool update, bool render)
        {
            bubbleRenderer[eType].Stop = update;
            bubbleRenderer[eType].Show = render;
        }

        private bool GetState()
        {
            return bubbleRenderer[AnimType.Exploding].Stop == false &&
            bubbleRenderer[AnimType.Exploding].Show;
        }

        private class StateExplode : IState
        {
            public StateWait Next;
            private readonly Bubble m_hTarget;
            public StateExplode(Bubble hTarget)
            {
                m_hTarget = hTarget;
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
                m_hTarget.fUpdate += Engine.DeltaTime;
                m_hTarget.SetAnimation(AnimType.Normal, true, false);
                CameraManager.Instance.Shake(0.3f, 0.2f);
                if (m_hTarget.fUpdate - 0.1f >= m_hTarget.fTime) return this;
                m_hTarget.fMin += Engine.DeltaTime;
                m_hTarget.SetAnimation(AnimType.Exploding, false, true);
                if (!(m_hTarget.fMin >= fMax)) return this;
                Engine.Destroy(m_hTarget);
                Next.OnStateEnter();
                return Next;
            }
        }
        private class StateWait : IState
        {
            public StateExplode Next;
            private readonly Bubble m_hTarget;

            public StateWait(Bubble hTarget)
            {
                m_hTarget = hTarget;
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
                if (m_hTarget.GetState()) return this;
                if (!Engine.ComputeIntersect(m_hTarget.collider)) return this;
                Next.OnStateEnter();
                return Next;
            }
        }
    }
}
