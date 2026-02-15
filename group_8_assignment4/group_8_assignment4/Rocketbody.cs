using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_8_assignment4
{
    public class RocketBody
    {
        private float width;
        private float height;
        private Color bodyColor;
        private RocketCone cone;
        private Texture2D pixel;

        public float Width => width;
        public float Height => height;
        public float ConeHeight => cone.Height;

        public RocketBody(float width, float height, Color bodyColor, Color coneColor)
        {
            this.width = width;
            this.height = height;
            this.bodyColor = bodyColor;
            
            this.cone = new RocketCone(width, height / 3, coneColor);
        }

        public void Initialize(GraphicsDevice graphicsDevice, Matrix view, Matrix projection)
        {
            cone.Initialize(graphicsDevice, view, projection);
            
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public void Display(SpriteBatch spriteBatch, Vector2 position)
        {
            cone.Display(spriteBatch.GraphicsDevice, spriteBatch, position);

            Vector2 bodyPosition = new Vector2(
                position.X - width / 2,
                position.Y + cone.Height
            );

            Rectangle bodyRect = new Rectangle(
                (int)bodyPosition.X,
                (int)bodyPosition.Y,
                (int)width,
                (int)height
            );

            spriteBatch.Draw(pixel, bodyRect, bodyColor);

            DrawRectangleOutline(spriteBatch, bodyRect, Color.Black, 2);
        }

        private void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rect, Color color, int thickness)
        {
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color);
        }
    }
}