using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using BehaviourEngine;

namespace BubbleGhostGame2D
{
    public class UI : GameObject
    {
        private readonly SpriteRenderer renderer;
        public Text text;
     
        public UI( int RenderOffset, Vector2 drawPosition, int width, int height, string fileName ) : base ( RenderOffset )
        {
            renderer                          = AddComponent(new SpriteRenderer(this, width, height));
            renderer.IsTile                   = false;
            renderer.Owner.Transform.Position = drawPosition;
            renderer.Texture                  = FlyWeight.Get(fileName);
        }

        public UI( string message, Texture spriteSheetTexture, Vector2 position )  //: base( ( int ) RenderLayer.Pawn )
        {
            text = new Text(message, spriteSheetTexture, position);
            GameObject.Spawn(text);
        }
    

    }
    public class Text : GameObject
    {
        private Write text;
        public Text( string message, Texture spriteSheetTexture, Vector2 position ) : base( ( int ) RenderLayer.Pawn )
        {
            text = AddComponent(new Write(this, spriteSheetTexture,message, position));
        }
    }
}
