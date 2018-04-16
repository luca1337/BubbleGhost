using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using BehaviourEngine.Interfaces;
using Aiv.Fast2D;
using Aiv.Fast2D.Utils.Input;
using vec2 = OpenTK.Vector2;

namespace BehaviourEngine.Test
{
    public sealed class GameManager : Component, IUpdatable
    {
        #region FSM
        private StateGameSetup stateSetup;
        private StateGameStart stateStart;
        private StateGameLoop stateLoop;
        private StateGameWin stateWin;
        private StateGameLose stateLose;
        private IState currentState;
        #endregion

        #region Singleton
        private static GameManager instance;
        public static GameManager Instance => instance ?? ( instance = new GameManager( null ) );
        #endregion

        #region GameObjects
        private Bubble bubble;
        private Ghost ghost;
        private List < UI > userInterface;
        private Level currentLevel;

        public Ghost Ghost
        {
            get
            {
                return ghost;
            }
        }
        #endregion

        private GameManager(GameObject owner) : base(owner)
        {
            userInterface        = new List < UI >( );
                                 
            #region FSM          
            stateSetup           = new StateGameSetup( this );
            stateStart           = new StateGameStart(this);
            stateLoop            = new StateGameLoop( this );
            stateWin             = new StateGameWin( this );
            stateLose            = new StateGameLose( this );

            stateSetup.GameStart = stateStart;
            stateStart.GameLoop  = stateLoop;
            stateLoop.WinLoop    = stateWin;
            stateLoop.LoseLoop   = stateLose;
            stateWin.GameStart   = stateStart;
            stateWin.GameLoop    = stateLoop;
            stateLose.GameStart  = stateStart;

            currentState         = stateSetup;
            stateSetup.OnStateEnter();
            #endregion
        }

        public void Update()
        {
            currentState = currentState.OnStateUpdate();
        }

        private class StateGameSetup : IState
        {
            public StateGameStart GameStart { private get; set; }
            private readonly GameManager owner;

            public StateGameSetup( GameManager owner )
            {
                this.owner = owner;
            }

            public void OnStateEnter( )
            {
                //Textures
                InitTextures();

                //Sounds
                InitSounds();

                //AudioManager.PlayStream( AudioType.SOUND_BACKGROUND, Engine.SOUND_PATH + "/Menu.ogg" );

              

                //Level Load
                owner.currentLevel = new Level( Engine.LEVEL_PATH + "/Level00" + ".csv", "Base0", 0 );
                new Level( Engine.LEVEL_PATH + "/Level01" + ".csv", "Base1", 1 );
                new Level( Engine.LEVEL_PATH + "/Level02" + ".csv", "Base2", 2 );
                new Level( Engine.LEVEL_PATH + "/Level03" + ".csv", "Base3", 3 );
                new Level( Engine.LEVEL_PATH + "/Level04" + ".csv", "Base4", 4 );
                new Level( Engine.LEVEL_PATH + "/Level05" + ".csv", "Base5", 5 );
                new Level( Engine.LEVEL_PATH + "/Level06" + ".csv", "Base6", 6 );
                new Level( Engine.LEVEL_PATH + "/Level07" + ".csv", "Base7", 7 );
                new Level( Engine.LEVEL_PATH + "/Level08" + ".csv", "Base8", 8 );
                new Level( Engine.LEVEL_PATH + "/Level09" + ".csv", "Base9", 9 );
               
                //Gui
                owner.userInterface.Add( new UI( ( int ) RenderLayer.Background, new vec2( 0, Engine.Window.Height - 252 ), Engine.Window.Width, 252, "GUI" ) );
                owner.userInterface.Add( new UI( ( int ) RenderLayer.Gui, vec2.Zero, Engine.Window.Width, Engine.Window.Height, "Start" ) );
                //owner.userInterface.Add(new UI((int)RenderLayer.Background, new vec2(150, 200), (int)Engine.OrthoWidth, Engine.Window.Height, "Background"));
                //Level spawn
                Level.Load( "Base0" );

                owner.bubble = new Bubble( "Bubble", "Explosion", Map.spawnPointBubble );
                owner.ghost  = new Ghost( "Ghost",4, Map.spawnPointGhost, owner.bubble );

                //Gui spawn
                Engine.Spawn( owner.userInterface[ 0 ] );
                Engine.Spawn( owner.userInterface[ 1 ] );

                owner.userInterface.Add( new UI( owner.ghost.GetComponent< Lifes >( ).LifesCount.ToString(), FlyWeight.Get( "Font" ), Engine.Half ) );
              
            }

            private void InitTextures( )
            {
                FlyWeight.Add( "GUI"       , "Assets/gui.dat" );
                FlyWeight.Add( "Start"     , "Assets/start.dat");
                FlyWeight.Add( "Wall"      , "Assets/brick.dat");
                FlyWeight.Add( "Flames0"   , "Assets/Flame00.dat");
                FlyWeight.Add( "Flames1"   , "Assets/Flame01.dat");
                FlyWeight.Add( "Flames2"   , "Assets/Flame02.dat");
                FlyWeight.Add( "Ghost"     , "Assets/Ghost.dat");
                FlyWeight.Add( "GhostBlow" , "Assets/GhostBlow.dat");
                FlyWeight.Add( "GhostLose" , "Assets/GhostLose.dat");
                FlyWeight.Add( "GhostWin"  , "Assets/GhostWin.dat");
                FlyWeight.Add( "Bubble"    , "Assets/bubble2.dat");
                FlyWeight.Add( "Font"      , "Assets/Font.dat");
                FlyWeight.Add( "Explosion" , "Assets/explosion.dat");
                FlyWeight.Add( "Candle"    , "Assets/candle_only_light.dat" );
                FlyWeight.Add( "Background"    , "Assets/bg.dat" );
            }


            private void InitSounds( )
            {
                AudioManager.AddSource( AudioType.SOUND_BACKGROUND );
                AudioManager.AddSource( AudioType.SOUND_MENU );
            }

            public void OnStateExit() { }

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
                Engine.Destroy( owner.userInterface[1] );
            }

            public IState OnStateUpdate()
            {
                if (!Input.IsKeyPressed( KeyCode.Space ))
                {
                    AudioManager.PlayStream( AudioType.SOUND_MENU, "Sounds/Background.ogg");
                    return this;
                }
                AudioManager.Stop(AudioType.SOUND_BACKGROUND);
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
            public StateGameLoop( GameManager owner )
            {
                this.owner = owner;
            }
            public void OnStateEnter()
            {
                //GameObjects
                owner.ghost.Transform.Position  = Map.spawnPointGhost;
                owner.bubble.Transform.Position = Map.spawnPointBubble;
                Engine.Spawn( owner.bubble ) ;
                Engine.Spawn( owner.ghost );
                Engine.Spawn( owner.userInterface[ 0 ] );
                Engine.Spawn( new Camera( ) );
                Engine.Spawn(owner.userInterface[2]);

            }

            public void OnStateExit()
            {
                Engine.Destroy(owner.userInterface[ 0 ] );
                Engine.Destroy(owner.bubble);
                Engine.Destroy(owner.ghost);
            }

            public IState OnStateUpdate()
            {
                owner.bubble.UpdateStates();

                owner.userInterface[2].text.GetComponent<Write>().message = owner.ghost.GetComponent<Lifes>().LifesCount.ToString();

                if ( Engine.ComputeIntersect(Engine.levelColliders, owner.bubble.GetComponent < Box2D >( ) ) )
                {
                    Engine.Boxes.Clear();
                    Console.WriteLine(Engine.levelColliders.Count);
                    owner.currentLevel.NextLevel( true );
                    OnStateExit();
                    WinLoop.OnStateEnter();
                    return WinLoop;
                }

                return this;
            }
        }

        private class StateGameWin : IState
        {
            public StateGameStart GameStart { get; set; }
            public StateGameLoop GameLoop { get; set; }
            private GameManager owner;

            public StateGameWin( GameManager owner )
            {
                this.owner = owner;
            }

            public void OnStateEnter( )
            {
            }

            public void OnStateExit( )
            {
            }

            public IState OnStateUpdate( )
            {
                OnStateExit();
                GameLoop.OnStateEnter();
                return GameLoop;

                //When this class is completed, fix this
                //return this;
            }
        }

        private class StateGameLose : IState
        {
            public StateGameStart GameStart { get; set; }
            private GameManager owner;

            public StateGameLose(GameManager owner)
            {
                this.owner = owner;
            }

            public void OnStateEnter( )
            {
            }

            public void OnStateExit( )
            {
            }

            public IState OnStateUpdate( )
            {
                return this;
            }
        }
    }
}
