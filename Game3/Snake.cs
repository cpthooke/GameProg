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
        public const float moveSpeed = 0.2f;
        float moveTimer;
        Texture2D head, body, angle, tail;
        List<Point> bodyPoints = new List<Point>();
        Direction currentDirection = Direction.right;
        Direction nextDirection = Direction.right;
        bool extending;

        public Snake()
        {
            Reset();
        }

        public enum Direction
        {
            up,
            down,
            left,
            right
        }

        public void Load(ContentManager content)
        {
            head = content.Load<Texture2D>("Content/head");
            body = content.Load<Texture2D>("Content/body");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            for (int i = 1; i < bodyPoints.Count - 1; i++)
            {
                drawBody(
                    spriteBatch,
                    bodyPoints[i],
                    bodyPoints[i - 1],
                    bodyPoints[i + 1]);
            }
            drawHead(spriteBatch);
            spriteBatch.End();
        }

        void drawHead(SpriteBatch spriteBatch)
        {
            Point headPoint = bodyPoints[0];
            Point nextBody = bodyPoints[1];

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

        public void Reset()
        {
            bodyPoints.Clear();

            bodyPoints.Add(new Point(2, 0));
            bodyPoints.Add(new Point(1, 0));
            bodyPoints.Add(new Point(0, 0));

            currentDirection = Direction.right;
            nextDirection = Direction.right;
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

        private void HandleInput()
        { }

        private void MoveSnake()
        { }


    }
}
