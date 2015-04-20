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
    public class Snake
    {
        bool extending;
        float moveTimer;
        public const float moveSpeed = 0.2f;
        List<Point> bodyPoints = new List<Point>();
        public Point headPoint, nextBody;
        public Texture2D angle, body, head, tail;
        Direction currentDirection = Direction.right, nextDirection = Direction.right;
        public enum Direction
        {
            up,
            down,
            left,
            right
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();

            for (int i = 1; i < bodyPoints.Count - 1; i++)
            {
                drawBody(
                    spriteBatch,
                    bodyPoints[i],
                    bodyPoints[i - 1],
                    bodyPoints[i + 1]);
            }
            drawHead(spriteBatch);
            //spriteBatch.End();
        }

        void drawBody(SpriteBatch spriteBatch, Point current, Point last, Point next) 
        {
            if (current.X != last.X)
            {
                Grid.drawSprite(
                    spriteBatch,
                    body,
                    current,
                    0f);
            }
            else if (current.Y != last.Y)
            {
                Grid.drawSprite(
                    spriteBatch,
                    body,
                    current,
                    MathHelper.PiOver2);
            }
        }

        void drawHead(SpriteBatch spriteBatch)
        {
            headPoint = bodyPoints[0];
            nextBody = bodyPoints[1];

            float rotation;
            if (headPoint.Y == nextBody.Y - 1)
                rotation = -MathHelper.PiOver2;
            else if (headPoint.Y == nextBody.Y + 1)
                rotation = MathHelper.PiOver2;
            else if (headPoint.X == nextBody.X - 1)
                rotation = MathHelper.Pi;
            else
                rotation = 0f;

            Grid.drawSprite(spriteBatch, head, headPoint, rotation);
        }

        public void Extend()
        {
            extending = true;
        }

        float getAngleRotation(Point current, Point last, Point next) 
        {
            Point negPiOver2A = new Point(next.X + 1, last.Y - 1);
            Point negPiOver2B = new Point(last.X + 1, next.Y - 1);

            Point piA = new Point(next.X - 1, last.Y - 1);
            Point piB = new Point(last.X - 1, next.Y - 1);

            Point piOver2A = new Point(next.X - 1, last.Y + 1);
            Point piOver2B = new Point(last.X - 1, next.Y + 1);

            if (current == negPiOver2A || current == negPiOver2B)
                return -MathHelper.PiOver2;
            else if (current == piA || current == piB)
                return MathHelper.Pi;
            else if (current == piOver2A || current == piOver2B)
                return MathHelper.PiOver2;
            else
                return 0f;
        }

        private void HandleInput()
        {
            //GamePadState gps = GamePad.GetState(PlayerIndex.One);
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && currentDirection != Direction.up)
                nextDirection = Direction.down;
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && currentDirection != Direction.down)
                nextDirection = Direction.up;
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && currentDirection != Direction.right)
                nextDirection = Direction.left;
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && currentDirection != Direction.left)
                nextDirection = Direction.right;
        }

        public bool isHeadAtPosition(Point position)
        {
            return (bodyPoints[0] == position);
        }

        public bool isHeadOffScreen()
        {
            Point h = bodyPoints[0];
            return (h.X < 0 || h.Y < 0 || h.X >= Grid.maxColumn || h.Y >= Grid.maxRow);
        }

        public bool isLooped()
        {
            for (int i = 1; i < bodyPoints.Count; i++)
                if (isHeadAtPosition(bodyPoints[i]))
                    return true;
            return false;
        }

        public void Load(ContentManager content)
        {
            head = content.Load<Texture2D>("Content/head");
            body = content.Load<Texture2D>("Content/body");
        }

        private void MoveSnake()
        {
            Point p1 = bodyPoints[0];
            switch (currentDirection)
            {
                case Direction.up:
                    bodyPoints[0] = new Point(p1.X, p1.Y - 1);
                    break;
                case Direction.down:
                    bodyPoints[0] = new Point(p1.X, p1.Y + 1);
                    break;
                case Direction.left:
                    bodyPoints[0] = new Point(p1.X - 1, p1.Y);
                    break;
                case Direction.right:
                    bodyPoints[0] = new Point(p1.X + 1, p1.Y);
                    break;
            }

            if (extending)
            {
                bodyPoints.Insert(1, p1);
                extending = false;
                return;
            }

            for (int i = 1; i< bodyPoints.Count; i++)
            {
                Point p2 = bodyPoints[i];
                bodyPoints[i] = p1;
                p1 = p2;
            }
        }

        public void Reset()
        {
            bodyPoints.Clear();

            bodyPoints.Add(new Point(2, 0));
            bodyPoints.Add(new Point(1, 0));
            bodyPoints.Add(new Point(0, 0));

            currentDirection = Direction.right;
            nextDirection = Direction.right;
        }

        public Snake()
        {
            Reset();
        }

        public void Update(GameTime gametime)
        {
            HandleInput();
            moveTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
            if (moveTimer < moveSpeed)
            {
                return;
            }
            moveTimer = 0f;
            currentDirection = nextDirection;
            MoveSnake();
        }

    }
}
