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
    class Sprite
    {
        //class variables for door
        protected Rectangle rec;
        protected Texture2D pic;

        public Sprite(Texture2D tx,Rectangle rc)//constructor
        {
            SetPic(tx);
            SetRec(rc);
        }

        #region getters and setters for pic and recs of door sprite
        public Texture2D GetPic()
        {
            return pic;
        }
        public Rectangle GetRec()
        {
            return rec;
        }

        public void SetPic(Texture2D tx)
        {
            pic = tx;
        }
        public void SetRec(Rectangle rc)
        {
            rec = rc;
        }
        #endregion 

        //draw door on screen
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pic, rec, Color.White);
        }
    }
}
