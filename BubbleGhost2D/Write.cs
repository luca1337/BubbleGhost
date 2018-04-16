using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using BehaviourEngine.Interfaces;
using BehaviourEngine.Renderer;

namespace BehaviourEngine.Test
{
    public class Write : Behaviour, IUpdatable, IDrawable
    {
        private TextMesh text;
        public string message;
       
        public Write(GameObject owner, Texture spriteSheetText, string message, Vector2 position) : base(owner)
        {
            text          = new TextMesh(spriteSheetText);
            text.SetTextColor( new Vector4(255, 0, 0, 0));
            text.position = position;
            this.message  = message;
        }
  
        public void Draw()
        {
            text.DrawText();
        }
    
        public void Update()
        {
            text.UpdateText(message);
            Console.WriteLine(message);
        }
    }
}
