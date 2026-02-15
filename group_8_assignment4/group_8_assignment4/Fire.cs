using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace group_8_assignment4
{
    public class Fire
    {
        private float width;
        private float maxHeight;
        private string colorScheme;
        private float animationTime;
        private float animationSpeed;
        private Color[] colors;
        private Texture2D pixel;

        public Fire(float width, float maxHeight, string colorScheme = "orange")
        {
            this.width = width;
            this.maxHeight = maxHeight;
            this.colorScheme = colorScheme;
            this.animationTime = 0;
            this.animationSpeed = 0.05f;

            if (colorScheme == "orange")
            {
                colors = new Color[]
                {
                    new Color(255, 140, 0),
                    new Color(255, 165, 0),
                    new Color(255, 200, 0),
                    new Color(255, 255, 0)
                };
            }
            else if (colorScheme == "red")
            {
                colors = new Color[]
                {
                    new Color(200, 0, 0),
                    new Color(255, 0, 0),
                    new Color(255, 100, 0),
                    new Color(255, 140, 0)
                };
            }
            else
            {
                colors = new Color[] { new Color(255, 165, 0) };
            }
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public void Update(GameTime gameTime)
        {
            animationTime += animationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 16.67f;
        }

        public float Animate()
        {
            float scale = 0.7f + 0.3f * Math.Abs((float)Math.Sin(animationTime));
            return scale;
        }

        public void Display(SpriteBatch spriteBatch, Vector2 position)
        {
            float scale = Animate();
            float currentHeight = maxHeight * scale;

            int numLayers = 4;

            for (int i = 0; i < numLayers; i++)
            {
                float layerScale = 1.0f - (i * 0.15f);
                float layerHeight = currentHeight * layerScale;
                float layerWidth = width * layerScale;

                float waveOffset = (float)Math.Sin(animationTime + i) * 3;

                Vector2[] points = new Vector2[11];

                for (int j = 0; j < 5; j++)
                {
                    float t = j / 4.0f;
                    float px = position.X - layerWidth / 2 + waveOffset;
                    float py = position.Y + layerHeight * t;
                    px += (float)Math.Sin(t * Math.PI) * (layerWidth * 0.1f);
                    points[j] = new Vector2(px, py);
                }

                points[5] = new Vector2(position.X + waveOffset, position.Y + layerHeight);

                for (int j = 4; j >= 0; j--)
                {
                    float t = j / 4.0f;
                    float px = position.X + layerWidth / 2 + waveOffset;
                    float py = position.Y + layerHeight * t;
                    px -= (float)Math.Sin(t * Math.PI) * (layerWidth * 0.1f);
                    points[10 - j] = new Vector2(px, py);
                }

                Color layerColor = colors[Math.Min(i, colors.Length - 1)];
                DrawFilledPolygon(spriteBatch, points, layerColor);
            }
        }

        private void DrawFilledPolygon(SpriteBatch spriteBatch, Vector2[] points, Color color)
        {
            if (points.Length < 3) return;

            float minY = float.MaxValue;
            float maxY = float.MinValue;

            foreach (var point in points)
            {
                if (point.Y < minY) minY = point.Y;
                if (point.Y > maxY) maxY = point.Y;
            }

            for (float y = minY; y <= maxY; y++)
            {
                float leftX = float.MaxValue;
                float rightX = float.MinValue;

                for (int i = 0; i < points.Length; i++)
                {
                    Vector2 p1 = points[i];
                    Vector2 p2 = points[(i + 1) % points.Length];

                    if ((p1.Y <= y && p2.Y > y) || (p2.Y <= y && p1.Y > y))
                    {
                        float x = p1.X + (y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y);
                        if (x < leftX) leftX = x;
                        if (x > rightX) rightX = x;
                    }
                }

                if (leftX != float.MaxValue && rightX != float.MinValue)
                {
                    spriteBatch.Draw(pixel, new Rectangle((int)leftX, (int)y, (int)(rightX - leftX), 1), color);
                }
            }
        }
    }
}