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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //sprite use
        Sprite door;
        PlayerSprite player;
        EnemySprite enemy;
        CoinSprite coin;

        //picture and texture use for exit door
        Texture2D DoorPic;
        Rectangle DoorRec;

        //picture and texture use for player
        Texture2D playerPic,playerDown,playerUp,playerLeft,playerRight;
        Rectangle playerRec;

        //picture and texture use for enemy 
        Texture2D enemyPic;
        Rectangle enemyRec;

        //picture and texture use for coin
        Texture2D coinPic;
        Rectangle coinRec;

        //list and array use for enemies
        Rectangle[] EnemiesRec = new Rectangle[4];
        List<EnemySprite> Enemies = new List<EnemySprite>();

        //ints use for variables for game
        int score = 0;
        int time = 0;

        //--------------- background effects
        byte redIntensity, greenIntensity, blueIntensity;
        Color backGround1;
        bool redCountingUp = true;
        bool greenCountingUp = true;
        bool blueCountingUp = true;

        //------Health
        int health = 100;
        Rectangle healthBarRec;
        Texture2D healthBarColor;
        bool gameOver = false;//using a bool to check if game is over or not
        bool Win = false;//use a bool to check if player won game or not

        Song music;
        SoundEffectInstance musicInstance;
        SoundEffect tune;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //setting new dimesions for window screen
            this.graphics.PreferredBackBufferHeight = 700;
            this.graphics.PreferredBackBufferWidth = 1200;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //instantiating variables accordingly
            DoorRec = new Rectangle(0, 0, 100, 100);
            playerRec = new Rectangle(Window.ClientBounds.Width - (playerRec.Width+200), Window.ClientBounds.Height - (playerRec.Height+200), 70, 70);
            enemyRec = new Rectangle(300, 300, 100, 100);
            coinRec = new Rectangle(300,200,70,70);

            redIntensity = 3;
            greenIntensity = 5;
            blueIntensity = 7;

            healthBarRec = new Rectangle(900, 0, 100, 20);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        SpriteFont font1;//creating a font variable
        Vector2 fontPos;//using fontPos to set postion of the 2d text on screen
        SpriteFont healhFont;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            music = Content.Load<Song>(@"Sound\music");
            tune = Content.Load<SoundEffect>(@"Sound\Background");

            //loading all pics necessary
            font1 = Content.Load<SpriteFont>(@"Fonts\Font1");
            healhFont = Content.Load<SpriteFont>(@"Fonts\HealthFont");
            fontPos = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            DoorPic = Content.Load<Texture2D>(@"Pics\door");
            playerDown = Content.Load<Texture2D>(@"Pics\Raul Down");
            playerUp = Content.Load<Texture2D>(@"Pics\Raul 1Up");
            playerLeft = Content.Load<Texture2D>(@"Pics\Raul Left");
            playerRight = Content.Load<Texture2D>(@"Pics\Raul Right");
            enemyPic = Content.Load<Texture2D>(@"Pics\shrek");
            coinPic = Content.Load<Texture2D>(@"Pics\coin");

            playerPic = playerDown;

            //instantiating sprites
            door = new Sprite(DoorPic, DoorRec);
            player = new PlayerSprite(playerPic, playerRec, playerUp, playerDown, playerLeft, playerRight);
            enemy = new EnemySprite(enemyPic, enemyRec,player);
            coin = new CoinSprite(coinPic, coinRec);

            //adding enemies to list 
            for (int i = 0; i < EnemiesRec.Length; i++)
            {
                EnemiesRec[i].Width = enemyRec.Width;
                EnemiesRec[i].Height = enemyRec.Height;
                EnemiesRec[i].X = i + enemyRec.X;
                EnemiesRec[i].Y = i * enemyRec.Y;
                Enemies.Add(new EnemySprite(enemyPic, EnemiesRec[i],player));
            }

            healthBarColor = new Texture2D(GraphicsDevice, 1, 1);
            healthBarColor.SetData(new[] { Color.Red });
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            this.Window.Title = "Savan Patel";

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //updating sprites
            player.Update(gameTime, graphics);
            enemy.Update(gameTime, graphics);
            coin.Update(gameTime, graphics);

            //checking for intersections of enemies with player, enemies with door, player with door 
            // if any intersection b/w player and enemies, loose certain health and decrease health bar to refeclt the damage
            #region Intersections and Timing & Scoring
            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Update(gameTime, graphics);

                //if the game is not over and win is not true then increase time and score accordingly
                if (!gameOver && !Win)
                {
                    time++;

                    if (time % (60 * 5) == 0) //increase score every 5 seconds by 1
                    {
                        score++;
                    }

                    if (player.GetRec().Intersects(Enemies[i].GetRec()))
                    {
                        healthBarRec.Width = healthBarRec.Width - 5;
                        health = health - 5;
                        GamePad.SetVibration(PlayerIndex.One, 1, 1);
                    }
                    else
                    {
                        GamePad.SetVibration(PlayerIndex.One, 0, 0);
                    }

                    if (Enemies[i].GetRec().Intersects(player.GetRec()) || Enemies[i].GetRec().Intersects(door.GetRec()))
                    {
                        Enemies.RemoveAt(i);

                        i--;

                        Enemies.Add(enemySpawn());
                    }

                    if (player.GetRec().Intersects(enemy.GetRec()))
                    {
                        healthBarRec.Width = healthBarRec.Width - 5;
                        health = health - 5;
                        GamePad.SetVibration(PlayerIndex.One, 1, 1);
                    }
                    else
                    {
                        GamePad.SetVibration(PlayerIndex.One, 0, 0);
                    }

                    if (musicInstance == null)
                    {
                        musicInstance = tune.CreateInstance();
                        musicInstance.IsLooped = true;
                        musicInstance.Play();
                    }
                    else
                    {
                        musicInstance.Play();
                    }
                }
            }
            #endregion

            //check to see if game should be over or not
            if (healthBarRec.Width <= 0)
            {
                gameOver = true; //if so then gameOver is declared true
                GamePad.SetVibration(PlayerIndex.One, 1, 1);
                if (musicInstance != null)
                {
                    musicInstance.Pause();
                }
            }

            //check to see if player has made it to door or not to declare win
            if (player.GetRec().Intersects(door.GetRec()))
            {
                Win = true;
                if (musicInstance != null)
                {
                    musicInstance.Pause();
                }
            }

            #region Background
            //-------background color effect
            if (redIntensity == 255)
            {
                redCountingUp = false;
            }
            if (redIntensity == 0)
            {
                redCountingUp = true;
            }
            if (redCountingUp)
            {
                redIntensity += 10;
            }
            else
            {
                redIntensity -= 6;
            }
            if (blueIntensity == 255)
            {
                blueCountingUp = false;
            }
            if (blueIntensity == 0)
            {
                blueCountingUp = true;
            }
            if (blueCountingUp)
            {
                blueIntensity += 6;
            }
            else
            {
                blueIntensity -= 14;
            }
            if (greenIntensity == 255)
            {
                greenCountingUp = false;
            }
            if (greenIntensity == 0)
            {
                greenCountingUp = true;
            }
            if (greenCountingUp)
            {
                greenIntensity += 14;
            }
            else
            {
                greenIntensity -= 10;
            }
            #endregion

            //intersection with player and coin
            //increase score by 100 if player hits coin and vibrate left motor at full speed only 
            if(player.GetRec().Intersects(coin.GetRec()))
            {
                score = score + 100;
                GamePad.SetVibration(PlayerIndex.One, 1, 0);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            string healthTxt = "Health: ";
            Vector2 textPos = new Vector2(800, 0);
            //if game is not over than draw all sprites, and accessories on screen
            if (!gameOver)
            {
                backGround1 = new Color(redIntensity, greenIntensity, blueIntensity);
                GraphicsDevice.Clear(backGround1);
       
                door.Draw(gameTime, spriteBatch);
                player.Draw(gameTime, spriteBatch);
                enemy.Draw(gameTime, spriteBatch);
                coin.Draw(gameTime, spriteBatch);

                foreach (EnemySprite s in Enemies)
                {
                    s.Draw(gameTime, spriteBatch);
                }

                spriteBatch.Draw(healthBarColor, healthBarRec, Color.White);
                spriteBatch.DrawString(healhFont, healthTxt, textPos, Color.White);
                spriteBatch.DrawString(healhFont, "Exit", new Vector2(10, 0), Color.White);
                spriteBatch.DrawString(healhFont, "Score: " + Convert.ToString(score), new Vector2(1025, 0), Color.White);
                spriteBatch.DrawString(healhFont, "Time: " + Convert.ToString(time/60), new Vector2(1025, 25), Color.White);
            }
            //if game is over than only draw gameover text and clear screen to pitch black
            if (gameOver)
            { 
                GraphicsDevice.Clear(Color.Black);

                string output = "GAME OVER !!!";
                Vector2 FontOrigin = font1.MeasureString(output) / 2;
                spriteBatch.DrawString(font1, output, fontPos, Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 1.0f);
            }
            spriteBatch.End();
            //if player has won than draw winner text on screen displaying score, time, and health 
            if (Win)
            {
                spriteBatch.Begin();
                GraphicsDevice.Clear(Color.Black);
         
                fontPos = new Vector2(500,300);
                string winOutPut = "WINNER !!!!";
                spriteBatch.DrawString(font1, winOutPut, fontPos, Color.Red);
                spriteBatch.DrawString(font1, "Score: " + Convert.ToString(score), new Vector2(400, 375), Color.White);
                spriteBatch.DrawString(font1, "Time: " + Convert.ToString(time/60), new Vector2(650, 375), Color.White);
                spriteBatch.DrawString(font1, "Health: " + Convert.ToString(health), new Vector2(525, 475), Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        //method to spawn enemies on screen without forver spawing ontop of player
        private EnemySprite enemySpawn()
        {
            EnemySprite enemyTemp;
            Rectangle enemyTempRec;
            enemyTempRec = enemyRec;

            while (enemyTempRec.Intersects(player.GetRec()))
            {
                enemyTempRec.X += player.GetRec().Width;
            }

            enemyTemp = new EnemySprite(enemyPic, enemyTempRec,player);

            return enemyTemp;
        }
    }
}
