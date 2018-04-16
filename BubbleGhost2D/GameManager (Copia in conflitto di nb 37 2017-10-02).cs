using System;
using BehaviourEngine.Interfaces;
using vec2 = OpenTK.Vector2;

namespace BehaviourEngine.Test
{
    public sealed class GameManager : Behaviour, IUpdatable
    {
        #region FSM
        private StateGameSetup stateSetup;
        private StateGameLoop stateLoop;
        private StateGameWin stateWin;
        private StateGameLose stateLose;
        private StateGameStart stateStart;
        private IState currentState;
        #endregion

        #region Singleton
        private static GameManager instance;
        public static GameManager Instance => instance ?? ( instance = new GameManager( null ) );
        #endregion

        #region GameObjects
        private Bubble bubble;
        private Ghost ghost;
        #endregion

        private GameManager(GameObject owner) : base(owner)
        {
            #region FSM

            stateSetup = new StateGameSetup( this );
            stateLoop  = new StateGameLoop( this );
            stateWin   = new StateGameWin( this );
            stateLose  = new StateGameLose( this );
            stateStart = new StateGameStart(this);

            stateSetup.GameStart = stateStart;
            stateLoop.WinLoop   = stateWin;
            stateLoop.LoseLoop  = stateLose;
            stateWin.GameLoop   = stateLoop;
            stateLose.GameLoop  = stateLoop;
            stateSetup.OnStateEnter();
            currentState        = stateSetup;
            #endregion
        }

        public void Update()
        {
            bubble.UpdateStates();
            currentState = currentState.OnStateUpdate();
        }

        private class StateGameSetup : IState
        {
            public StateGameStart GameStart { get; set; }
            private readonly GameManager owner;

            public StateGameSetup(GameManager owner)
            {
                this.owner = owner;
            }

            public void OnStateEnter()
            {
                #region LEVELS
                new Level(Engine.LEVEL_PATH + "/Level00" + ".csv", "Base00");
                new Level(Engine.LEVEL_PATH + "/Level01" + ".csv", "Base01");
                new Level(Engine.LEVEL_PATH + "/Level02" + ".csv", "Base02");
                //Load
                Level.Load( "Base01" );
                #endregion

                #region GAMEOBJECTS
                owner.bubble = new Bubble("Bubble", new vec2(99, 99));
                owner.ghost = new Ghost( "Ghost", new vec2(55, 55), owner.bubble);
                Engine.Spawn(owner.bubble);
                Engine.Spawn(owner.ghost);
                Engine.Spawn(new Camera());
                #endregion
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                this.OnStateExit();
                GameStart.OnStateEnter();
                return GameStart;
            }
        }

        private class StateGameStart : IState
        {
            public StateGameLoop GameLoop { private get; set; }
            private readonly GameManager owner;
            public StateGameStart(GameManager owner)
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
        private class StateGameLoop : IState
        {
            public StateGameWin WinLoop { get; set; }
            public StateGameLose LoseLoop { get; set; }
            private GameManager owner;

            public StateGameLoop( GameManager owner )
            {
                this.owner = owner;
            }
            public void OnStateEnter()
            {
                //  Engine.Spawn(owner.bubble);
                //  Engine.Spawn(owner.ghost);
                //  Engine.Spawn(new Camera());
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
