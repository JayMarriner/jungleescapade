using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickGame.Map
{
    public class MapTiles
    {
        protected Texture2D texture;

        protected int tileID;
        public int TileID { get { return tileID; } }
        private Rectangle rectangle;
        public Rectangle Rectangle
        {
            get { return rectangle; }
            protected set { rectangle = value; }
        }

        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(texture, rectangle, Color.White);
         
        }
    }

    public class CollisionTiles : MapTiles
    {
        public CollisionTiles(int i, Rectangle newRectangle)
        {
            texture = Content.Load<Texture2D>("MapAssets/Tile" + i);
            this.Rectangle = newRectangle;
            tileID = i;
        }
    }
}

