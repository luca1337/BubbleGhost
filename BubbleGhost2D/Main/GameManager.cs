using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using BehaviourEngine.Test;
using OpenTK;

namespace BehaviourEngine
{
    public sealed class GameManager : Behaviour, IUpdatable
    {
        #region FSM
        private StateGameSetup    stateSetup;
        private StateGameLoop     stateLoop;
        private StateGameWin      stateWin;
        private StateGameLose     stateLose;
        private IState            currentState;
        #endregion

        #region Singleton
        private static GameManager instance;
        public static GameManager Instance => instance ?? (instance = new GameManager());

        #endregion

        #region GameObjects
        private Bubble         m_hBubble;
        private GameObject     hBubble;
        private Ghost          m_hGhost;
        private GameObject     hGhost;
        private Rotator        m_hRotator;
        private Blower         m_hBlower;
        #endregion

        private GameManager()
        {
            #region FSM
            stateSetup                  = new StateGameSetup(this);
            stateLoop                   = new StateGameLoop(this);
            stateWin                    = new StateGameWin(this);
            stateLose                   = new StateGameLose(this);
                                        
            stateSetup.GameLoop         = stateLoop;
            stateLoop.WinLoop           = stateWin;
            stateLoop.LoseLoop          = stateLose;
            stateWin.GameLoop           = stateLoop;
            stateLose.GameLoop          = stateLoop;
            stateSetup.OnStateEnter();
            currentState                = stateSetup;
            #endregion

        }

        public void Update()
        {
            m_hBubble.UpdateStates();
            currentState = currentState.OnStateUpdate();
        }

        private class StateGameSetup : IState
        {
            public StateGameLoop GameLoop { private get; set; }
            private readonly GameManager owner;

            public StateGameSetup(GameManager owner)
            {
                this.owner = owner;
            }

            public void OnStateEnter()
            {
                #region LEVELS
                //Create levels and load them
                Level level01    = new Level(Engine.LEVEL_PATH + "/Level01" + ".csv", "Base01");
                Level level02    = new Level(Engine.LEVEL_PATH + "/Level02" + ".csv", "Base02");
                Level.Load("Base01");
                #endregion

                #region GAMEOBJECTS
                //Create game objects: Using 'Test.' cuz of ambiguity
                Engine.Spawn(new Test.Camera());
                owner.m_hGhost   = new Ghost("Ghost", new Vector2(2, 6), new[] { 0, 1, 2, 1, }, 46, 48, 0.2f);
                owner.m_hBubble  = new Bubble("Bubble", new Vector2(3, 8));
                owner.hBubble    = Engine.Spawn(owner.m_hBubble);
                owner.hGhost     = Engine.Spawn(owner.m_hGhost);
                #endregion

                #region BEHAVIOURS
                //Behaviours
                owner.m_hRotator = new Rotator { m_hTarget = owner.hGhost };
                owner.hBubble.AddBehaviour<Rotator>(owner.m_hRotator);
                owner.hBubble.GetComponent<Rotator>().IsEnabled = true;
                owner.m_hBlower  = new Blower();
                owner.hGhost.AddBehaviour<Blower>(owner.m_hBlower);
                owner.m_hBlower.m_hTarget = owner.hBubble;
                #endregion
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                this.OnStateExit();
                GameLoop.OnStateEnter();
                return GameLoop;
            }
        }

        private class StateGameLoop : IState
        {
            public StateGameWin WinLoop { get; set; }
            public StateGameLose LoseLoop { get; set; }
            private GameManager owner;

            public StateGameLoop(GameManager owner)
            {
                this.owner = owner;
            }
            public void OnStateEnter()
            {
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                return this;
            }
        }

        private class StateGameWin : IState
        {
            public StateGameLoop GameLoop { get; set; }
            private GameManager owner;

            public StateGameWin(GameManager owner)
            {
                this.owner = owner;
            }

            public void OnStateEnter()
            {
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                return this;
            }
        }

        private class StateGameLose : IState
        {
            public StateGameLoop GameLoop { get; set; }
            private GameManager owner;

            public StateGameLose(GameManager owner)
            {
                this.owner = owner;
            }

            public void OnStateEnter()
            {
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                return this;
            }
        }
    }
}
