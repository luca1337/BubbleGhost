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
        // private Lifes lifes;
        private BoxCollider2D box;

        public Ghost(int life, GameObject target, float range = 120)
        {
            //Animation render
            renderer = AddComponent(new AnimationRenderer(FlyWeight.Get("Ghost"), 32, 32, 4, new[] { 0, 1, 2, 3 }, 0.1f, false, true));
            renderer.RenderOffset = (int)RenderLayer.Player;
            AddComponent(new SwitchAnimation( target, range, true));
            renderer.Sprite.pivot = new Vector2(renderer.Sprite.Width / 2, renderer.Sprite.Height / 2);

            //Collider
            box = new BoxCollider2D(Vector2.One);
            
            AddComponent(box);

            //Behaviours
            AddComponent(new Rotator( target, range));
            AddComponent(new Blower( target.GetComponent<Blowable>(), range, 3));

            //Input
            input = AddComponent(new InputMove());
            input.Speed = 90.0f;

            //Rollback
            rollBack = AddComponent(new RollBack());
            rollBack.box = this.box;

            //Lifes
           // lifes = AddComponent(new Lifes(life, target, this));
        }

        public int RenderOffset { get; set; }
        public bool Enabled { get; set; }

        public void Draw()
        {
        }
    }

    public class RollBack : Component, IUpdatable
    {
        public BoxCollider2D box;


        public void Update()
        {
            if (box == null || Owner == null) return;
            Vector2 oldPos = box.internalTransform.Position;

            box.internalTransform.Position = Owner.Transform.Position;

            bool prev = true;
           // bool prev = Engine.IsOutOfScreen(this.owner.Transform.Position);

            if (!prev) return;
            Owner.Transform.Position = oldPos;
            box.internalTransform.Position = oldPos;
        }
    }

}
