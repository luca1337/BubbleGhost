using System;
using System.Collections.Generic;
using OpenTK;
using BubbleGhostGame2D;
using BehaviourEngine;

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

        private Dictionary<AnimType, AnimationRenderer> bubbleRenderer;
        private BoxCollider2D collider;
        private int[] frames            = { 0, 1, 2, 1 };
        private const float frameLenght = 0.2f;

        public Bubble(string fileNameBubble, string fileNameExplosion, Vector2 position) : base("Bubble") // base( ( int )RenderLayer.Pawn, "Bubble" )
        {
            Exploded       = false;
            
            #region Renderer
            bubbleRenderer = new Dictionary<AnimType, AnimationRenderer>();
            bubbleRenderer.Add(AnimType.Normal, AddComponent(new AnimationRenderer(FlyWeight.Get("Bubble"), 32, 32, 3, new[] { 0, 1, 2, 1 }, frameLenght, false, true)));
            bubbleRenderer.Add(AnimType.Exploding, AddComponent(new AnimationRenderer(FlyWeight.Get("Explosion"), 32, 32, 3, frames, frameLenght, true, false)));
            bubbleRenderer[AnimType.Normal].RenderOffset = (int)RenderLayer.Weapon;
            bubbleRenderer[AnimType.Exploding].RenderOffset = (int)RenderLayer.Weapon;
            #endregion

            Rigidbody2D r = this.AddComponent(new Rigidbody2D());
            r.LinearFriction = 3f;
            this.AddComponent(new Blowable());

            collider = new BoxCollider2D(new Vector2(1f, 1f));
            collider.TriggerEnter += OnTriggerEnter;
            AddComponent(collider);

            #region Explosion
            //target.updateTime += Graphics.Instance.Window.deltaTime;
            //target.SetAnimation(AnimType.Normal, true, false);
            //CameraManager.Instance.Shake(1.9f, 0.2f);

            //if (target.updateTime <= target.time)
            //return this;

            //target.min += Graphics.Instance.Window.deltaTime;
            //target.SetAnimation(AnimType.Exploding, false, true);

            //if (!(target.min >= max))
            //return this;
            #endregion

        }

        private void OnTriggerEnter(Collider2D other)
        {
            bubbleRenderer[AnimType.Normal].Show    = false;
            bubbleRenderer[AnimType.Exploding].Show = true;
        }
    }
}
