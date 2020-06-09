using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace JoystickGame.Sprites
    {
    public class Level
        {
        Rectangle rect;
        public Rectangle Rect { get { return rect; } }

        public Level(Vector2 p, int size)
            {
            rect = new Rectangle((int)p.X, (int)p.Y, size, size);
            }
        }
    }