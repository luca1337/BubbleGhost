using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourEngine.Test
{
    internal enum AnimType : byte
    {
        Animated,
        Won
    }
    public class Ghost : GameObject
    {
        private Dictionary<AnimType, AnimationRenderer> ghostRenderer;
        private readonly AnimationRenderer aRenderer;
        private InputMove input;

        //FSM
        private StateAnimate stateAnimate;
        private StateWait stateWait;
        private IState currentState;

        public Ghost(string fileName, Vector2 drawPosition, int[] keyFrames, int width, int height, float frameLenght)
        {
            //Animation render ghost
            ghostRenderer = new Dictionary<AnimType, AnimationRenderer>
            {
                {
                    AnimType.Animated,
                    AddBehaviour<AnimationRenderer>(new AnimationRenderer(fileName, drawPosition, false, true, 10,
                        width,
                        height, keyFrames, frameLenght))
                },
                {
                    AnimType.Won,
                    AddBehaviour<AnimationRenderer>(new AnimationRenderer(fileName, drawPosition, true, false, 10,
                        width,
                        height, new[] {0, 1}, frameLenght))
                }
            };
            ghostRenderer.ToList().ForEach(x => { x.Value.Owner.Position = drawPosition; x.Value.Pivot = new Vector2(x.Value.Width / 2, x.Value.Height / 2); });

            //Input
            input                       = AddBehaviour<InputMove>(new InputMove());
            input.Speed                 = 2.0f;

            //FSM
            stateAnimate = new StateAnimate();
            stateWait = new StateWait();

            stateAnimate.Next = stateWait;
            stateWait.Next = stateAnimate;
            stateWait.OnStateEnter();
            currentState = stateWait;
            //TODO:Finish state machine

        }

        public void UpdateStates()
        {
            
        }

        private class StateAnimate : IState
        {
            //TODO:Implementare gli algoritmi per gli stati della macchina di questo oggetto
            public StateWait Next { get; set; }
            public void OnStateEnter()
            {
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                return null;
            }
        }

        private class StateWait : IState
        {
            //TODO:Implementare gli algoritmi per gli stati della macchina di questo oggetto
            public StateAnimate Next {get; set; }
            public void OnStateEnter()
            {
                this.OnStateUpdate();
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                return null;
            }
        }
    }
}
