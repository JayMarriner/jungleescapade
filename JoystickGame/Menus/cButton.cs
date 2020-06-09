using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickGame.Menus
{
    public class cButton
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        public GameState Destination { get; }

        int yOffset;

        bool down;
        public bool isClicked, canTransition;

        Color colour = new Color(255, 255, 255, 255);

        public cButton(Texture2D newTexture, int w, int h, int o, GameState d)
        {
            texture = newTexture;
            rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Destination = d;

            SetPosition(w, h, o);
        }

        public void Update(MouseState mouse)
        {
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {
                if (colour.A == 255) down = false;
                if (colour.A == 0) down = true;
                if (down) colour.A += 3; else colour.A -= 3;
                if (mouse.LeftButton == ButtonState.Pressed) isClicked = true;



            }

            else if (colour.A < 255)
            {
                colour.A += 3;
                isClicked = false;
            }

            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
        }

        public void SetPosition(int sWidth, int sHeight, int offset)
        {
            position = new Vector2(sWidth / 2 - (rectangle.Width / 2), sHeight / 2 - (rectangle.Height / 2) - offset);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, colour);
        }
    }
}
