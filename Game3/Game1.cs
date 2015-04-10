#region Using Statements
using System;
using System.Collections.Generic;
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
        Texture2D ship;
        Rectangle rectShip;
        String direction = "RIGHT";

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
            snake.Load(Content);
            ship = Content.Load<Texture2D>("Content/spaceinvader");
            rectShip.Width = ship.Width;
            rectShip.Height = ship.Height;
            rectShip.X = 0;
            rectShip.Y = 440;

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


            if (direction.Equals("RIGHT"))
                rectShip.X = rectShip.X + 1;
            else //(direction.Equals("LEFT"))
                rectShip.X = rectShip.X - 1;
            if (rectShip.X + rectShip.Width > rightSide)
                direction = "LEFT";
            else if (rectShip.X < leftSide)
                direction = "RIGHT";
            base.Update(gameTime);
            snake.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(ship, rectShip, Color.White);
            drawInGame();
            spriteBatch.End();
        }
    }
}
