using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace JoystickGame.Sprites
{
    public class Enemy
    {
        ContentManager content;
        public Texture2D enemyTex;
        Vector2 pos, velocity, startPos;
        bool movingLeft, movingRight, movingUp, movingDown;
        public Rectangle enemyRect;
        Direction direction;
        float maxDist, distToPlayer;

        bool patrolHorizontal;

        int height, width;


        public Enemy(ContentManager cm, Vector2 p, bool h, float mD)
        {
            content = cm;
            pos = p;

            enemyTex = content.Load<Texture2D>("Enemy/enemy");

            width = (enemyTex.Width);
            height = (enemyTex.Height);
            direction = Direction.Left;
            patrolHorizontal = h;
            startPos = p;
            maxDist = mD;

            if(h)
                {
                movingRight = true;
                movingLeft = false;
                }
            else
                {
                movingUp = true;
                movingDown = false;
                }
        }

        public void Update(GameTime gt)
        {
            //Debug.WriteLine(distToPlayer);

            if(patrolHorizontal)
            {
                if(pos.X == startPos.X)
                {
                    movingLeft = false;
                    movingRight = true;
                }

                if(pos.X == startPos.X + maxDist)
                {
                    movingRight = false;                    
                    movingLeft = true;                    
                }

                if(movingLeft)
                    pos.X -= 1;
                else if (movingRight)
                    pos.X += 1;                
            }

            else
            {
                if(pos.Y == startPos.Y)
                {
                    movingUp = true;
                    movingDown = false;
                }
                if(pos.Y == startPos.Y - maxDist)
                {
                    movingUp = false;
                    movingDown = true;
                }

                if(movingUp)
                    pos.Y -= 1;
                if(movingDown)
                    pos.Y += 1;
            }

            

            enemyRect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(enemyTex, enemyRect, Color.White);
        }

        public void GetDistance(Player player)
        {
            distToPlayer = (pos - player.pos).Length();
        }
    }
}
