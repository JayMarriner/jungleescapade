using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickGame.Bullet
{
    class BulletM
    {
        Texture2D tex;
        Vector2 pos;
        Rectangle rect;
        Direction direction;

        private bool left;
        public int Timer { get; set; }
        public int LifeTime { get { return 250; } }
        public Rectangle Rect { get { return rect; } }

        public BulletM(Texture2D t, Vector2 p, Direction d)
        {
            tex = t;
            pos = p;
            direction = d;

            rect = new Rectangle(0, 0, tex.Width, tex.Height);
        }

        public void Update(GameTime gt)
        {
            Timer += gt.ElapsedGameTime.Milliseconds;

            switch (direction)
            {
                case Direction.Up:
                    pos.Y -= 10;
                    break;
                case Direction.Down:
                    pos.Y += 10;
                    break;
                case Direction.Left:
                    pos.X -= 10;
                    break;
                case Direction.Right:
                    pos.X += 10;
                    break;
            }


            /*
            if (!left)
                pos.X += 10;
            else
                pos.X -= 10;
                */

            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, rect, Color.White);
        }                                       
    }
}
