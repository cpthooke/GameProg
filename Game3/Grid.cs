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
    public static class Grid
    {
        public const int scale = 16;
        public const int halfScale = scale / 2;
        public const int maxColumn = 240 / scale;
        public const int maxRow = 320 / scale;

        public static Vector2 pointToVector2(Point p)
        {
            return new Vector2(
                p.X * scale + halfScale,
                p.Y * scale + halfScale);
        }

        public static void drawSprite(SpriteBatch spriteBatch, Texture2D texture, Point point, float rotation)
        {
            float spriteSize = (float)Math.Max(texture.Width, texture.Height);
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            spriteBatch.Draw(texture, pointToVector2(point), null, Color.White, rotation, origin, scale / spriteSize, SpriteEffects.None, 0);
        }
    }
}
