using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BehaviourEngine.Test
{
    public class Bubble : GameObject, IUpdatable
    {
        public enum AnimType : byte
        {
            Normal,
            Exploding
        }

        private readonly Dictionary
            <AnimType, AnimationRenderer>     bubbleRenderer;
        private LightRadius                   radius;
        private readonly Box2D                collider;
        private Vector2                       colliderPosition;
        private static readonly int[]         iFrames = { 13, 15, 17, 19 };
        private readonly float                fTime;
        private float                         fUpdate;
        private float                         fMin;
        private const float                   fMax    = 0.57f;

        //FSM                                               
        private IState                        currentState;
        private StateExplode                  stateExplode;
        private StateWait                     stateWait;

        public Bubble(string fileName, Vector2 position)
        {
            #region Init
            bubbleRenderer                    = new Dictionary<AnimType, AnimationRenderer>();
            radius                            = AddBehaviour<LightRadius>(new LightRadius());
            #endregion

            #region Renderer
            bubbleRenderer.Add(AnimType.Normal, AddBehaviour<AnimationRenderer>(new AnimationRenderer(fileName, position, false, true, 5, 192, 192, new[] { 7, 10, 7 }, 0.5f)));
            bubbleRenderer.Add(AnimType.Exploding, AddBehaviour<AnimationRenderer>(new AnimationRenderer(fileName, position, true, false, 5, 192, 192, iFrames, 0.5f)));
            foreach (var hAnims in bubbleRenderer)
            {
                hAnims.Value.Owner.Position = position;
                hAnims.Value.Pivot = new Vector2(hAnims.Value.Width / 2, hAnims.Value.Height / 2);
            }
            fTime = bubbleRenderer[AnimType.Exploding].LenghtFrames;
            #endregion

            #region Collider
            collider                           = new Box2D(position, 0.7f, 0.7f, this);
            collider.Pivot                     = new Vector2(collider.width / 2, collider.height / 2);
            AddBehaviour<Box2D>(collider);     
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
            if(!bubbleRenderer[AnimType.Normal].Owner.Active)
            collider.Position        = bubbleRenderer[AnimType.Exploding].Position;
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
