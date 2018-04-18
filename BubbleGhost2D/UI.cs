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
     
        public UI(string fileName) 
        {
            renderer                          = AddComponent(new SpriteRenderer(FlyWeight.Get(fileName)));
            renderer.RenderOffset             = (int)RenderLayer.Gui_00;
        }

        public UI( string message, Texture spriteSheetTexture, Vector2 textPos)  
        {
            text = new Text(message, spriteSheetTexture, textPos);
            Spawn(text);
        }
    

    }
    public class Text : GameObject
    {
        private Write text;
        public Text( string message, Texture spriteSheetTexture, Vector2 position )
        {
            text = AddComponent(new Write(this, spriteSheetTexture,message, position));
        }
    }
}
