using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace JoystickGame.Sprites
{
    public class Player
    {
        ContentManager content;
        Texture2D texNormal, texWeapon;
        
        public Vector2 pos, velocity;
        public Rectangle destRect;
        Direction currentDirection = Direction.Down;
        public int armour;

        Rectangle upRect, leftRect, downRect, rightRect;
        int rectDim = 3;

        int sizeModifier = 2;
        int rows, columns, height, width, currentFrame, totalFrames, outputFrame, offset;
        float timeDown;
        float delay = 250f;
        int speedModifier = 13;
        public bool hFlip,vFlip, wEquipped, canShoot;

        public bool HasPickedUp { get; set; }
        public bool WEquipped { get { return wEquipped; } }
        public int OutputFrame { get { return outputFrame; } }
        public Direction CurrentDirection { get { return currentDirection; } }

        public Rectangle UpRect { get => upRect; set => upRect = value; }
        public Rectangle LeftRect { get => leftRect; set => leftRect = value; }
        public Rectangle DownRect { get => downRect; set => downRect = value; }
        public Rectangle RightRect { get => rightRect; set => rightRect = value; }

        public List<Rectangle> checks = new List<Rectangle>();
        
        public bool CanMoveRight { get; set; }
        public bool CanMoveLeft { get; set; }
        public bool CanMoveUp { get; set; }
        public bool CanMoveDown { get; set; }

        public Player(ContentManager cm, Vector2 p, int r, int c, int newArmour)
        {
            content = cm;
            pos = p;
            rows = r;
            columns = c;

            texNormal = content.Load<Texture2D>("Player/CharacterSpriteSheetp");
            texWeapon = content.Load<Texture2D>("Player/CharacterSpriteSheetWeapon");

            armour = newArmour;

            currentFrame = 0;
            totalFrames = r * c;

            width = (texNormal.Width / columns);
            height = (texNormal.Height / rows);

            //destRect = new Rectangle(0, 0, width, height);       

            checks.AddRange(new List<Rectangle> { UpRect, LeftRect, DownRect, RightRect });
        }

        public void Update(GameTime gt)
        {
            // player position = Debug.WriteLine(pos.X + " Y:" + pos.Y);
            destRect = new Rectangle((int)pos.X, (int)pos.Y, width, height);

            upRect = new Rectangle(destRect.X, destRect.Y - rectDim, destRect.Width, rectDim);
            leftRect = new Rectangle(destRect.X - rectDim, destRect.Y, rectDim, destRect.Height);
            downRect = new Rectangle(destRect.X, destRect.Y + destRect.Height, destRect.Width, rectDim);
            rightRect = new Rectangle(destRect.X + destRect.Width, destRect.Y, rectDim, destRect.Height);

            Console.WriteLine("CanMoveUp: " + CanMoveUp);

            Input(gt);
            pos += velocity;
        }

        void Input(GameTime gt)
        {
            KeyboardState ks = Keyboard.GetState();

            //Moving
            //Going Left
            if (ks.IsKeyDown(Keys.A) && CanMoveLeft)
            {
                

                hFlip = false;

                currentDirection = Direction.Left;

                offset = 6;

                timeDown += gt.ElapsedGameTime.Milliseconds;

                if (timeDown >= delay)
                {
                    currentFrame++;
                    outputFrame = currentFrame + offset;
                    timeDown = 0;
                }

                if (outputFrame >= offset + 2)
                    currentFrame = 0;

                velocity.X = -(float)gt.ElapsedGameTime.TotalMilliseconds / speedModifier;
                velocity.Y = 0;
            }

            //Going Right
            else if (ks.IsKeyDown(Keys.D) && CanMoveRight)
            {
                hFlip = true;

                currentDirection = Direction.Right;

                offset = 6;

                timeDown += gt.ElapsedGameTime.Milliseconds;

                if (timeDown >= delay)
                {
                    currentFrame++;
                    outputFrame = currentFrame + offset;
                    timeDown = 0;
                }

                if (outputFrame >= offset + 2)
                    currentFrame = 0;

                velocity.X = (float)gt.ElapsedGameTime.TotalMilliseconds / speedModifier;
                velocity.Y = 0;
            }

            //Going Up
            else if (ks.IsKeyDown(Keys.W) && CanMoveUp)
            {
                vFlip = false;

                currentDirection = Direction.Up;

                offset = 0;

                timeDown += gt.ElapsedGameTime.Milliseconds;

                if (timeDown >= delay)
                {
                    currentFrame++;
                    outputFrame = currentFrame + offset;
                    timeDown = 0;
                }

                if (outputFrame >= offset + 2)
                    currentFrame = 0;

                velocity.Y = -(float)gt.ElapsedGameTime.TotalMilliseconds / speedModifier;
                velocity.X = 0;
            }

            //Going Down
            else if (ks.IsKeyDown(Keys.S) && CanMoveDown)
            {
                vFlip = false;

                currentDirection = Direction.Down;

                offset = 3;

                timeDown += gt.ElapsedGameTime.Milliseconds;

                if (timeDown >= delay)
                {
                    currentFrame++;
                    outputFrame = currentFrame + offset;
                    timeDown = 0;
                }

                if (outputFrame >= offset + 2)
                    currentFrame = 0;

                velocity.Y = (float)gt.ElapsedGameTime.TotalMilliseconds / speedModifier;
                velocity.X = 0;
            }

            else
            {
                switch (currentDirection)
                {
                    case Direction.Up:
                        outputFrame = 0;
                        hFlip = false;
                        break;
                    case Direction.Down:
                        outputFrame = 3;
                        hFlip = false;
                        break;
                    case Direction.Left:
                        outputFrame = 6;
                        hFlip = false;
                        break;
                    case Direction.Right:
                        outputFrame = 6;
                        hFlip = true;
                        break;
                }

                velocity.X = 0;
                velocity.Y = 0;

            }

            if(ks.IsKeyDown(Keys.E))
            {
                if (HasPickedUp)
                {
                    if (!wEquipped)
                    {
                        wEquipped = true;
                        canShoot = true;
                    }

                    else
                    {
                        wEquipped = false;
                        canShoot = false;
                    }
                }
                        
            }
        }

        public void Draw(SpriteBatch sb)
        {
            int curRow = outputFrame / columns;
            int curCol = outputFrame % columns;

            Rectangle srcRect = new Rectangle(width * curCol, height * curRow, width, height);

            if (!wEquipped)
            {
                if (!hFlip)
                    sb.Draw(texNormal, destRect, srcRect, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                else
                    sb.Draw(texNormal, destRect, srcRect, Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                if (!hFlip)
                    sb.Draw(texWeapon, destRect, srcRect, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                else
                    sb.Draw(texWeapon, destRect, srcRect, Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
            }

            #region Old Code
            /*
            if (!vFlip)
                sb.Draw(texWeapon, destRect, srcRect, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            else
                sb.Draw(texWeapon, destRect, srcRect, Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipVertically, 0);
            */
            #endregion
        }
    }
}
