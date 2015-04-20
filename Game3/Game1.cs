#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace GamesProgramming
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Snake snake = new Snake();
        Texture2D ship, bullet;
        Rectangle rectShip, rectBullet, snakeHead;
        String direction = "RIGHT", bulletVisible = "NO";
        gameState state = gameState.title;
        const string gameTitle = "Snake Invaders", playInstructions = "Press Enter to Begin", quitInstructions = "Press ESC to Quit";
        SpriteFont titleFont, mediumFont, miniFont;
        KeyboardState keyboardState, lastKeyboardState;

        public enum gameState
        {
            title,
            inGame,
            gameOver
        }

        void drawTitleScreen()
        {

        }

        void drawInGame()
        {
        
        }

        void drawGameOver()
        {

        }

        void drawInGame()
        {
            snake.Draw(spriteBatch);
        }

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            titleFont = Content.Load<SpriteFont>("Fonts/titleFont");
            mediumFont = Content.Load<SpriteFont>("Fonts/mediumFont");
            miniFont = Content.Load<SpriteFont>("Fonts/miniFont");
            snake.Load(Content);
            ship = Content.Load<Texture2D>("Content/spaceinvader");
            rectShip.Width = ship.Width;
            rectShip.Height = ship.Height;
            rectShip.X = 0;
            rectShip.Y = 440;

            bullet = Content.Load<Texture2D>("Content/pellet");
            rectBullet.Width = bullet.Width;
            rectBullet.Height = bullet.Height;
            rectBullet.X = 0;
            rectBullet.Y = 0;


            snakeHead.Width = snake.head.Width;
            snakeHead.Height = snake.head.Height;
            //snakeHead.X = snake.headPoint.X;
            //snakeHead.Y =  snake.headPoint.Y;
            // TODO: use this.Content to load your game content here
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            int rightSide = graphics.GraphicsDevice.Viewport.Width;
            int leftSide = 0;

            

            snakeHead.X = snake.headPoint.X*16;
            snakeHead.Y = snake.headPoint.Y*16;

            //snakeHead.Height = 128;
            //snakeHead.Width = 128;

            //snakeHead.X = 128 + snake.headPoint.X;//snake.head.Width;
            //snakeHead.Y = 128 + snake.headPoint.Y;//snake.head.Height;


            //Debug.WriteLine(snake.headPoint.X);
            //Debug.WriteLine(snake.headPoint.Y);
   

            if (direction.Equals("RIGHT"))
                rectShip.X = rectShip.X + 1;
            else //(direction.Equals("LEFT"))
                rectShip.X = rectShip.X - 1;
            if (rectShip.X + rectShip.Width > rightSide)
                direction = "LEFT";
            else if (rectShip.X < leftSide)
                direction = "RIGHT";
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                bulletVisible = "YES";
                rectBullet.X = rectShip.X + (rectShip.Width / 2) - (rectBullet.Width / 2);
                rectBullet.Y = rectShip.Y - rectBullet.Height;
            }
            if (bulletVisible.Equals("YES"))
                rectBullet.Y = rectBullet.Y - 4;
            if (bulletVisible.Equals("YES"))
            {
                //Debug.WriteLine("TEST1");
                Debug.WriteLine(snakeHead.X);
                Debug.WriteLine(snakeHead.Y);

                //Debug.WriteLine(snake.headPoint.X);
                //Debug.WriteLine(snake.headPoint.Y);

                if (rectBullet.Intersects(snakeHead))
                {
                    bulletVisible = "NO";
                    Debug.WriteLine("TEST2");
                    snake.Extend();
                }
            }

            base.Update(gameTime);
            snake.Update(gameTime);

            //if (snake.isLooped())
                
            //if (snake.isHeadAtPosition(rectBullet.))
            //{ 
            //}
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (state == gameState.title)
                drawTitleScreen();
            else if (state == gameState.inGame)
                drawInGame();
            else if (state == gameState.gameOver)
                drawGameOver();
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(ship, rectShip, Color.White);
            spriteBatch.Draw(bullet, snakeHead, Color.White);
            if (bulletVisible.Equals("YES"))
                spriteBatch.Draw(bullet, rectBullet, Color.White);
            drawInGame();
            spriteBatch.End();
        }
    }
}
