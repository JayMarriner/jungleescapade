using JoystickGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickGame.PickUps
{
    public class PickUp
    {
        
        private Texture2D armourTex, weaponTex;
        private Vector2 position;
        private ContentManager content;
        public Rectangle rectangle;

        private float ActiveTime;

        private int width, height, rows, columns, currentFrame, totalFrames;
        private int itemID; //0 = armour, 1 = weapon

        public int ItemID { get { return itemID; } }

        public PickUp(int r, int c, ContentManager cm, Vector2 p, int id)
        {
            rows = r;
            columns = c;

            currentFrame = 0;
            totalFrames = rows * columns;
            content = cm;
            position = p;

            armourTex = content.Load<Texture2D>("PickUps/ArmourPickUp");
            weaponTex = content.Load<Texture2D>("PickUps/WeaponPickUp");

            itemID = id;

            //width = 336; all collectables must be the same size
            //height = 42;

            switch (itemID)
            {
                case 0:
                    width = armourTex.Width / columns;
                    height = armourTex.Height / rows;
                    break;
                case 1:
                    width = weaponTex.Width / columns;
                    height = weaponTex.Height / rows;
                    break;
            }      
        }

        public void Update(GameTime gameTime)
        {

            rectangle = new Rectangle((int)position.X, (int)position.Y, width, height);

            ActiveTime += (float)gameTime.ElapsedGameTime.Milliseconds;

            if (ActiveTime > 300f)
            {
                currentFrame++;
                ActiveTime = 0;
            }

            if (currentFrame == totalFrames)
                currentFrame = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int currRow = currentFrame / columns;
            int currCol = currentFrame % columns;

            Rectangle srcRect = new Rectangle(width * currCol, height * currRow, width, height);

            switch(itemID)
            {
                case 0: //The pickup is armour
                    spriteBatch.Draw(armourTex, rectangle, srcRect, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    break;
                case 1: //The pickup is a weapon
                    spriteBatch.Draw(weaponTex, rectangle, srcRect, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    break;
            }            
        }
    }
}
