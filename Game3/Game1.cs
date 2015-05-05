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
    public class Game1 : Game
    {
        public enum GameState
        {
            Title,
            InGame,
            GameOver,
        }
        const string gameOver = "GameOver!", gameOverInstructions = "Press ESC to Exit", gameTitle = "Snake Invaders", playInstructions = "Press Enter to Begin", quitInstructions = "Press ESC to Quit", scoreFormat = "Score: {0}";
        GameState state = GameState.Title;
        KeyboardState keyboardState, lastKeyboardState;
        SpriteFont mediumFont, miniFont, titleFont;
        GraphicsDeviceManager graphics;
        int score;
        Rectangle rectBullet, rectShip, snakeBody, snakeHead;
        Snake snake = new Snake();
        SpriteBatch spriteBatch;
        String bulletVisible = "NO", direction = "RIGHT";
        Texture2D bullet, logo, ship;

        void drawText(SpriteFont font, string text, Vector2 position)
        {
            Vector2 halfSize = font.MeasureString(text) / 2.0f;
            position = position - halfSize;

            position.X = (int)position.X;
            position.Y = (int)position.Y;

            spriteBatch.DrawString(
                font,
                text,
                position,
                Color.Yellow);
        }

        void drawTitleScreen()
        {
            spriteBatch.Draw(logo, new Vector2((graphics.PreferredBackBufferWidth - logo.Width) / 2, 25f), Color.White);

            //drawText(titleFont, gameTitle, new Vector2(120f, 25f));
            drawText(mediumFont, playInstructions, new Vector2(graphics.PreferredBackBufferWidth / 2, 200f));
            drawText(mediumFont, quitInstructions, new Vector2(graphics.PreferredBackBufferWidth / 2, 225f));
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
            if (isNewButtonPressed(Keys.Enter))
                state = GameState.Title;
        }

        void updateGameOver()
        {
            if (isNewButtonPressed(Keys.Enter))
                state = GameState.Title;
        }
        void drawGameOver()
        {
            snake.Draw(spriteBatch);

            drawText(titleFont, gameOver, new Vector2(graphics.PreferredBackBufferWidth / 2, 25f));
            drawText(mediumFont, string.Format(scoreFormat, score), new Vector2(graphics.PreferredBackBufferWidth / 2, 200f));
            drawText(mediumFont, gameOverInstructions, new Vector2(graphics.PreferredBackBufferWidth / 2, 225f));
        }

        bool isNewButtonPressed(Keys key)
        {
            return (keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key));
        }

        void drawInGame()
        {
            snake.Draw(spriteBatch);
            drawText(miniFont, string.Format(scoreFormat, score), new Vector2(120f, 10f));
        }

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            logo = Content.Load<Texture2D>("Content/SI");

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

            snakeBody.Width = snake.body.Width;
            snakeBody.Height = snake.body.Height;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
          
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            lastKeyboardState = keyboardState;

            if (state == GameState.Title)
                updateTitleScreen();
            else if (state == GameState.InGame)
            {
                UpdateInGame(gameTime);
                Snake.gameActive = true;
                snakeHead.X = snake.headPoint.X * 16;
                snakeHead.Y = snake.headPoint.Y * 16;
                snakeBody.X = snake.nextBody.X * 16;
                snakeBody.Y = snake.nextBody.Y * 16;
                for (int i = 0; i < snake.bodyPoints.Count-1; i++)
                {
                    snakeBody.X = snake.bodyPoints[i].X * 16; //Attempts were made to get collision detection on each segment of the snake's tail, however as it stands at time of hand in, collisions only appear on the first and last part of the snake's tail.
                    snakeBody.Y = snake.bodyPoints[i].Y * 16;
                }
            }
            else if (state == GameState.GameOver)
            {
                updateGameOver();
                Snake.gameActive = false;
            }


            int rightSide = graphics.GraphicsDevice.Viewport.Width;
            int leftSide = 0;


            if (direction.Equals("RIGHT"))
                rectShip.X = rectShip.X + 3;
            else
                rectShip.X = rectShip.X - 3;
            if (rectShip.X + rectShip.Width > rightSide)
                direction = "LEFT";
            else if (rectShip.X < leftSide)
                direction = "RIGHT";
            if (state == GameState.InGame)
            {
                if (bulletVisible.Equals("NO"))
                {
                    bulletVisible = "YES";
                    rectBullet.X = rectShip.X + (rectShip.Width / 2) - (rectBullet.Width / 2);
                    rectBullet.Y = rectShip.Y - rectBullet.Height;
                }
                if (rectBullet.Y < 0)
                    bulletVisible = "NO";
            }
            if (bulletVisible.Equals("YES"))
                rectBullet.Y = rectBullet.Y - 4;
            if (bulletVisible.Equals("YES"))
            {
                if (rectBullet.Intersects(snakeHead))
                {
                    bulletVisible = "NO";
                    snake.Extend();
                    score++;
                }
                else if (rectBullet.Intersects(snakeBody))
                {
                    bulletVisible = "NO";
                    state = GameState.GameOver;
                }
            }

            if (snake.isLooped())
                state = GameState.GameOver;
            if (snake.isHeadOffScreen())
                state = GameState.GameOver;

            base.Update(gameTime);
            snake.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.DarkSeaGreen);
            if (state == GameState.Title)
                drawTitleScreen();
            else if (state == GameState.InGame)
            {
                drawInGame();
                spriteBatch.Draw(ship, rectShip, Color.White);
            }
            else if (state == GameState.GameOver)
                drawGameOver();
            if (bulletVisible.Equals("YES"))
                spriteBatch.Draw(bullet, rectBullet, Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
