using System;

namespace BubbleGhostGame2D
{
    public class Write : Component, IUpdatable, IDrawable
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
