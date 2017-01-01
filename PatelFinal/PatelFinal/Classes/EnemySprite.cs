using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PatelFinal
{
    class EnemySprite : Sprite
    {
        //class variables
        private static Random rnd = new Random();
        private int counter = 0;
        private int speed = 5;

        PlayerSprite player;

        //constructor, getting enemy rec and pic and playersprite 
        public EnemySprite(Texture2D tx, Rectangle rc, PlayerSprite character)
            : base(tx, rc)
        {
            SetRec(rc);
            SetPic(tx);
            player = character;
        }

        #region getters and setters for enemy
        public Rectangle GetRec()
        {
            return rec;
        }

        public Texture2D GetPic()
        {
            return pic;
        }

        public void SetPic(Texture2D tex)
        {
            pic = tex;
        }

        public void SetRec(Rectangle rc)
        {
            rec = rc;
        }
        #endregion

        public virtual void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            //increase counter till it reaches 5 seconds 
            counter++;
            //once coutner equals 5 seconds increase speed of enemies to 7 and reset counter 
            if (counter == 60 * 7)
            {
                speed = 3;
                counter = 0;
            }
            else
            {
                speed = 5;
            }

            //getting player location
            int enemyDx = player.GetRec().X - rec.X;
            int enemyDy = player.GetRec().Y - rec.Y;

            //getting player angle 
            float angle = (float)Math.Atan2(enemyDy, enemyDx);

            //chasing player according to angle and player location
            rec.X += (int)(Math.Cos(angle) * speed);
            rec.Y += (int)(Math.Sin(angle) * speed);

            //stops image from going off screen
            if (rec.Bottom >= graphics.PreferredBackBufferHeight)
            {
                rec.Y -= 10;
            }
            if (rec.Top <= 0)
            {
                rec.Y += 10;
            }
            if (rec.Left <= 0)
            {
                rec.X += 10;
            }
            if (rec.Right >= graphics.PreferredBackBufferWidth)
            {
                rec.X -= 10;
            }

        }

        //draw enemy 
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pic, rec, Color.White);
        }
    }
}
