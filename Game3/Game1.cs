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
        public enum GameState
        {
            Title,
            InGame,
            GameOver,
        }
        const string gameOver = "GameOver!", gameOverInstructions = "Press Enter to Continue", gameTitle = "Snake Invaders", playInstructions = "Press Enter to Begin", quitInstructions = "Press ESC to Quit", scoreFormat = "Score: {0}";
        GameState state = GameState.Title;
        KeyboardState keyboardState, lastKeyboardState;
        SpriteFont mediumFont, miniFont, titleFont;
        GraphicsDeviceManager graphics;
        int score;
        Rectangle rectBullet, rectShip, snakeHead;
        Snake snake = new Snake();
        SpriteBatch spriteBatch;
        String bulletVisible = "NO", direction = "RIGHT";
        Texture2D bullet, ship;

        //int screenWidth = 800, screenHeight = 600; 2
        //
        //ClassButton btnPlay;

        void drawText(SpriteFont font, string text, Vector2 position)
        {
            Vector2 halfSize = font.MeasureString(text) / 2.0f;
            position = position - halfSize;

            position.X = (int)position.X;
            position.Y = (int)position.Y;

            //spriteBatch.Begin();
            spriteBatch.DrawString(
                font,
                text,
                position,
                Color.White);
            //spriteBatch.End();
        }

        void drawTitleScreen()
        {
            drawText(titleFont, gameTitle, new Vector2(120f, 25f));
            drawText(mediumFont, playInstructions, new Vector2(120f, 200f));
            drawText(mediumFont, quitInstructions, new Vector2(120f, 225f));
        }

        void updateTitleScreen()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                snake.Reset();
                score = 0;
                state = GameState.InGame;
            }
        }

        void UpdateInGame(GameTime gameTime)
        {
            if (isNewButtonPressed(Keys.Back))
                state = GameState.Title;
        }

        void updateGameOver()
        {
            if (isNewButtonPressed(Keys.Back))
                state = GameState.Title;
        }
        void drawGameOver()
        {
            snake.Draw(spriteBatch);

            drawText(titleFont, gameOver, new Vector2(120f, 25f));
            drawText(mediumFont, string.Format(scoreFormat, score), new Vector2(120f, 200f));
            drawText(mediumFont, gameOverInstructions, new Vector2(120f, 225f));
        }

        bool isNewButtonPressed(Keys key)
        {
            return (keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key));
        }

        void drawInGame()
        {
            snake.Draw(spriteBatch);
            drawText(miniFont, string.Format(scoreFormat, score), new Vector2(120f, 5f));
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
            //graphics.PreferredBackBufferWidth = screenWidth; 2
            //graphics.PreferredBackBufferHeight = screenHeight;
            ////graphics.IsFullScreen = true;
            //graphics.ApplyChanges();
            //IsMouseVisible = true;

            //btnPlay = new ClassButton(Content.Load<Texture2D>("Content/Neck"), graphics.GraphicsDevice); 2
            //btnPlay.setPosition(new Vector2(350, 300));
            spriteBatch = new SpriteBatch(GraphicsDevice);
            titleFont = Content.Load<SpriteFont>("Content/titleFont");
            mediumFont = Content.Load<SpriteFont>("Content/mediumFont");
            miniFont = Content.Load<SpriteFont>("Content/miniFont");
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
            if (state == GameState.Title)
                snake.Load(Content);

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

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //MouseState mouse = Mouse.GetState(); 2
            //keyboardState = Keyboard.GetState(playerIndex.One)
            //switch(state)
            //{
            //    case GameState.Title:
            //        if (btnPlay.isClicked == true)
            //            state = GameState.InGame;
            //        btnPlay.Update(mouse);
            //        break;
            //    case GameState.InGame:
            //        break;
            //}
            lastKeyboardState = keyboardState;

            if (state == GameState.Title)
                updateTitleScreen();
            else if (state == GameState.InGame)
            {
                UpdateInGame(gameTime);
                Snake.gameActive = true;
                snakeHead.X = snake.headPoint.X * 16;
                snakeHead.Y = snake.headPoint.Y * 16;
            }
            else if (state == GameState.GameOver)
                updateGameOver();


            int rightSide = graphics.GraphicsDevice.Viewport.Width;
            int leftSide = 0;


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
                Debug.WriteLine(snakeHead.Y);

                //Debug.WriteLine(snake.headPoint.X);
                //Debug.WriteLine(snake.headPoint.Y);

                if (rectBullet.Intersects(snakeHead))
                {
                    bulletVisible = "NO";
                    //Debug.WriteLine("TEST2");
                    snake.Extend();
                    score++;
                }
            }

            if (snake.isLooped())
                state = GameState.GameOver;
            if (snake.isHeadOffScreen())
                state = GameState.GameOver;
            base.Update(gameTime);
            snake.Update(gameTime);

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
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (state == GameState.Title)
                drawTitleScreen();
            else if (state == GameState.InGame)
            {
                drawInGame();
                spriteBatch.Draw(ship, rectShip, Color.White);
                //spriteBatch.Draw(bullet, snakeHead, Color.White);
            }
            if (bulletVisible.Equals("YES"))
                spriteBatch.Draw(bullet, rectBullet, Color.White);
            else if (state == GameState.GameOver)
                drawGameOver();

            spriteBatch.End();

            //switch(state) 2
            //{
            //    case GameState.Title:
            //        spriteBatch.Draw(Content.Load<Texture2D>("Content/Pellet"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            //        btnPlay.Draw(spriteBatch);
            //        break;
            //    case GameState.InGame:
            //        break;
            //
            //}

                Debug.WriteLine(snake.headPoint.X);

            //drawInGame();
            
            base.Draw(gameTime);
        }
    }
}
