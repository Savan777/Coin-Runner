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
    class CoinSprite : EnemySprite
    {
        //class variables
        private Random rnd;
        private int counter = 0;
        protected float roataion;
        private Vector2 origin;

        //constructor for coin 
        public CoinSprite(Texture2D tx, Rectangle rc):base(tx,rc,null)
        {
            SetPic(tx);
            SetRec(rc);
            origin = new Vector2(pic.Width / 2, pic.Height / 2);
        }

        //get coin rec and pic 
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

        public override void Update(GameTime gameTime,GraphicsDeviceManager graphics)
        {
            //use random generator to creater a random location for the coin to appear at different locations each time
            //around the screen every second or so 
            rnd = new Random();

            counter++;

            roataion += 0.03f; //rotate coin at speed of 0.03f

            if (counter % 60*5 == 0)
            {
                rec.Location = new Point(rnd.Next(70,1100), rnd.Next(70,630));
                counter = 0;
            }
            
        }

        //draw coin on screen
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pic, rec, null, Color.White, roataion, origin, SpriteEffects.None, 0);
        }
    }
}
