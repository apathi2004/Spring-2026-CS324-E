using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace group_8_assignment4
{
    public class RocketFins
    {
        private float finWidth;
        private float finHeight;
        private Color color;
        private int numFins;
        private Texture2D pixel;

        public RocketFins(float finWidth, float finHeight, Color color, int numFins = 2)
        {
            this.finWidth = finWidth;
            this.finHeight = finHeight;
            this.color = color;
            this.numFins = numFins;
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public void Display(SpriteBatch spriteBatch, float centerX, float bodyTopY, float bodyWidth, float bodyHeight)
        {
            float finYStart = bodyTopY + bodyHeight * 0.6f;

            if (numFins == 2)
            {
                Vector2 leftTop = new Vector2(centerX - bodyWidth / 2, finYStart);
                Vector2 leftOuter = new Vector2(centerX - bodyWidth / 2 - finWidth, finYStart + finHeight / 2);
                Vector2 leftBottom = new Vector2(centerX - bodyWidth / 2, finYStart + finHeight);

                DrawFilledTriangle(spriteBatch, leftTop, leftOuter, leftBottom, color);
                DrawTriangleOutline(spriteBatch, leftTop, leftOuter, leftBottom, Color.Black, 2);

                Vector2 rightTop = new Vector2(centerX + bodyWidth / 2, finYStart);
                Vector2 rightOuter = new Vector2(centerX + bodyWidth / 2 + finWidth, finYStart + finHeight / 2);
                Vector2 rightBottom = new Vector2(centerX + bodyWidth / 2, finYStart + finHeight);

                DrawFilledTriangle(spriteBatch, rightTop, rightOuter, rightBottom, color);
                DrawTriangleOutline(spriteBatch, rightTop, rightOuter, rightBottom, Color.Black, 2);
            }
            else if (numFins == 3)
            {
                Vector2 leftTop = new Vector2(centerX - bodyWidth / 2, finYStart);
                Vector2 leftOuter = new Vector2(centerX - bodyWidth / 2 - finWidth, finYStart + finHeight / 2);
                Vector2 leftBottom = new Vector2(centerX - bodyWidth / 2, finYStart + finHeight);

                DrawFilledTriangle(spriteBatch, leftTop, leftOuter, leftBottom, color);
                DrawTriangleOutline(spriteBatch, leftTop, leftOuter, leftBottom, Color.Black, 2);

                Vector2 rightTop = new Vector2(centerX + bodyWidth / 2, finYStart);
                Vector2 rightOuter = new Vector2(centerX + bodyWidth / 2 + finWidth, finYStart + finHeight / 2);
                Vector2 rightBottom = new Vector2(centerX + bodyWidth / 2, finYStart + finHeight);

                DrawFilledTriangle(spriteBatch, rightTop, rightOuter, rightBottom, color);
                DrawTriangleOutline(spriteBatch, rightTop, rightOuter, rightBottom, Color.Black, 2);

                Vector2 centerTop = new Vector2(centerX, finYStart + finHeight * 0.3f);
                Vector2 centerLeft = new Vector2(centerX - finWidth / 3, finYStart + finHeight);
                Vector2 centerRight = new Vector2(centerX + finWidth / 3, finYStart + finHeight);

                DrawFilledTriangle(spriteBatch, centerTop, centerLeft, centerRight, color);
                DrawTriangleOutline(spriteBatch, centerTop, centerLeft, centerRight, Color.Black, 2);
            }
        }

        private void DrawFilledTriangle(SpriteBatch spriteBatch, Vector2 p1, Vector2 p2, Vector2 p3, Color color)
        {
            Vector2[] points = new Vector2[] { p1, p2, p3 };
            Array.Sort(points, (a, b) => a.Y.CompareTo(b.Y));

            Vector2 top = points[0];
            Vector2 mid = points[1];
            Vector2 bot = points[2];

            for (float y = top.Y; y <= bot.Y; y++)
            {
                float leftX, rightX;

                if (y < mid.Y)
                {
                    leftX = Interpolate(top.Y, top.X, mid.Y, mid.X, y);
                    rightX = Interpolate(top.Y, top.X, bot.Y, bot.X, y);
                }
                else
                {
                    leftX = Interpolate(mid.Y, mid.X, bot.Y, bot.X, y);
                    rightX = Interpolate(top.Y, top.X, bot.Y, bot.X, y);
                }

                if (leftX > rightX)
                {
                    float temp = leftX;
                    leftX = rightX;
                    rightX = temp;
                }

                spriteBatch.Draw(pixel, new Rectangle((int)leftX, (int)y, (int)(rightX - leftX), 1), color);
            }
        }

        private void DrawTriangleOutline(SpriteBatch spriteBatch, Vector2 p1, Vector2 p2, Vector2 p3, Color color, float thickness)
        {
            DrawLine(spriteBatch, p1, p2, color, thickness);
            DrawLine(spriteBatch, p2, p3, color, thickness);
            DrawLine(spriteBatch, p3, p1, color, thickness);
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness)
        {
            float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            float length = Vector2.Distance(start, end);

            spriteBatch.Draw(pixel, start, null, color, angle, Vector2.Zero,
                new Vector2(length, thickness), SpriteEffects.None, 0);
        }

        private float Interpolate(float y1, float x1, float y2, float x2, float y)
        {
            if (Math.Abs(y2 - y1) < 0.001f) return x1;
            return x1 + (x2 - x1) * (y - y1) / (y2 - y1);
        }
    }
}