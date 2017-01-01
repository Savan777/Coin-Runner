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
    class PlayerSprite : Sprite //playersprite inheriting from door sprite
    {
        //class variables for player
        private int speed = 5;
        private Texture2D up, down, left, right;
        GamePadState pad1,oldPad1;

        //constructor to get all player pics and rec
        public PlayerSprite(Texture2D tx, Rectangle rc, Texture2D rcUp, Texture2D rcDown, Texture2D rcLeft, Texture2D rcRight):base(tx,rc)
        {
            //setting player pics and rec
            SetPic(tx);
            SetRec(rc);
            SetPicUp(rcUp);
            SetPicDwn(rcDown);
            SetPicLf(rcLeft);
            SetPicRht(rcRight);
        }

        #region getters and setter for pics, and rec for player
        //public Rectangle GetRec()
        //{
        //    return rec;
        //}

        //public Texture2D GetPic()
        //{
        //    return pic;
        //}

        //public void SetPic(Texture2D tx)
        //{
        //    pic = tx;
        //}

        //public void SetRec(Rectangle rc)
        //{
        //    rec = rc;
        //}

        public Texture2D GetPicUp()
        {
            return up;
        }
        public void SetPicUp(Texture2D rcUp)
        {
            up = rcUp;
        }
        public Texture2D GetPicDwn()
        {
            return down;
        }
        public void SetPicDwn(Texture2D rcDown)
        {
            down = rcDown;
        }
        public Texture2D GetPicLf()
        {
            return left;
        }
        public void SetPicLf(Texture2D rcLeft)
        {
            left = rcLeft;
        }
        public Texture2D GetPicRht()
        {
            return right;
        }
        public void SetPicRht(Texture2D rcRight)
        {
            right = rcRight;
        }
        #endregion

        public void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            pad1 = GamePad.GetState(PlayerIndex.One);
            

            //checking what the postion of the left thumbstick is and setting playerPic faceing accordingly
            if (pad1.ThumbSticks.Left.X < 0)
            {
                pic = left;
            }
            else if (pad1.ThumbSticks.Left.X > 0)
            {
                pic = right;
            }
            else if (pad1.ThumbSticks.Left.Y < 0)
            {
                pic = down;
            }
            else if (pad1.ThumbSticks.Left.Y > 0)
            {
                pic = up;
            }

            //give the player an extra boost speed if the A button on the controller is pressed once 
            if (oldPad1.Buttons.A == ButtonState.Released & pad1.Buttons.A == ButtonState.Pressed)
            {
                speed = 10;
            }
            else
            {
                speed = 5;
            }
            oldPad1 = pad1;

            //players X and Y movement is that of the speed and thumbstick direction
            rec.X += (int)(pad1.ThumbSticks.Left.X * speed);
            rec.Y += (int)(pad1.ThumbSticks.Left.Y * (-speed));

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

        //drawing player on screen
        public new void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pic, rec, Color.White);
        }
    }
}
