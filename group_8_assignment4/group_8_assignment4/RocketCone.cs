using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_8_assignment4
{
    public class RocketCone
    {
        private float width;
        private float height;
        private Color color;
        private BasicEffect effect;

        public float Width => width;
        public float Height => height;

        public RocketCone(float width, float height, Color color)
        {
            this.width = width;
            this.height = height;
            this.color = color;
        }

        public void Initialize(GraphicsDevice graphicsDevice, Matrix view, Matrix projection)
        {
            effect = new BasicEffect(graphicsDevice)
            {
                VertexColorEnabled = true,
                View = view,
                Projection = projection
            };
        }

        public void Display(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Vector2 position)
        {
            Vector2 top = new Vector2(position.X, position.Y);
            Vector2 bottomLeft = new Vector2(position.X - width / 2, position.Y + height);
            Vector2 bottomRight = new Vector2(position.X + width / 2, position.Y + height);

            DrawTriangle(spriteBatch, top, bottomLeft, bottomRight, color);
            
            DrawLine(spriteBatch, top, bottomLeft, Color.Black, 2);
            DrawLine(spriteBatch, bottomLeft, bottomRight, Color.Black, 2);
            DrawLine(spriteBatch, bottomRight, top, Color.Black, 2);
        }

        private void DrawTriangle(SpriteBatch spriteBatch, Vector2 p1, Vector2 p2, Vector2 p3, Color color)
        {
            Texture2D pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            DrawThickLine(spriteBatch, pixel, p1, p2, color, width);
            DrawThickLine(spriteBatch, pixel, p2, p3, color, width);
            DrawThickLine(spriteBatch, pixel, p3, p1, color, width);
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness)
        {
            Texture2D pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            float angle = (float)System.Math.Atan2(end.Y - start.Y, end.X - start.X);
            float length = Vector2.Distance(start, end);

            spriteBatch.Draw(pixel, start, null, color, angle, Vector2.Zero, 
                new Vector2(length, thickness), SpriteEffects.None, 0);
        }

        private void DrawThickLine(SpriteBatch spriteBatch, Texture2D pixel, Vector2 start, Vector2 end, Color color, float thickness)
        {
            float angle = (float)System.Math.Atan2(end.Y - start.Y, end.X - start.X);
            float length = Vector2.Distance(start, end);

            spriteBatch.Draw(pixel, start, null, color, angle, Vector2.Zero,
                new Vector2(length, thickness), SpriteEffects.None, 0);
        }
    }
}