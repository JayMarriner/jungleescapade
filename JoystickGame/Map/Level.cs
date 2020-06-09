using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickGame.Map
{
   public class Level
    {
        ContentManager content;
        Rectangle rect;
        public Texture2D arrowTex;
        public Rectangle arrowRect { get { return rect; } }

        public Level(Vector2 p, int size, ContentManager content)
        {
            rect = new Rectangle((int)p.X, (int)p.Y, size, size);

            arrowTex = content.Load<Texture2D>("MapAssets/Arrow");
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(arrowTex, arrowRect, Color.White);
        }
    }
}
