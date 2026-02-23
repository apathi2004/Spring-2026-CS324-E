using System;
using System.Net.Mime;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace group_8_assignment4
{
    public class RocketBody
    {
        private float width;
        private float height;
        private Color bodyColor;
        private RocketCone cone;
        private Texture2D pixel;
        private Texture2D window;
        private Texture2D alien;
        private Color aliencolor;

        public float Width => width;
        public float Height => height;
        public float ConeHeight => cone.Height;

        public RocketBody(float width, float height, Color bodyColor, Color coneColor, Texture2D window, Texture2D alien, Color aliencolor)
        {
            this.width = width;
            this.height = height;
            this.bodyColor = bodyColor;
            this.window = window;
            this.alien = alien;
            this.aliencolor = aliencolor;
            
            this.cone = new RocketCone(width, height / 3, coneColor);
        }

        public void Initialize(GraphicsDevice graphicsDevice, Matrix view, Matrix projection)
        {
            cone.Initialize(graphicsDevice, view, projection);
            
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public void Display(SpriteBatch spriteBatch, Vector2 rocketTopPosition)
        {
            cone.Display(spriteBatch.GraphicsDevice, rocketTopPosition);

            Vector2 bodyTopLeft = new Vector2(
                rocketTopPosition.X - width / 2f,
                rocketTopPosition.Y + cone.Height
            );

            Rectangle bodyRect = new Rectangle(
                (int)bodyTopLeft.X,
                (int)bodyTopLeft.Y,
                (int)width,
                (int)height
            );

            spriteBatch.Draw(pixel, bodyRect, bodyColor);

            DrawRectangleOutline(spriteBatch, bodyRect, Color.Black, 2);

            DrawWindow(spriteBatch, bodyTopLeft);
        }

        private void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rect, Color color, int thickness)
        {
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color);
        }

        private void DrawWindow(SpriteBatch spriteBatch, Vector2 bodyTopLeft)
        {
            float windowScale = width / window.Width * 0.8f;
            float alienScale = windowScale * 0.4f;
            Vector2 windowPosition = new Vector2(
                bodyTopLeft.X + width / 2f, 
                bodyTopLeft.Y + height * 0.25f 
            );
            
            Vector2 windowOrigin = new Vector2(window.Width / 2f, window.Height / 2f);
            Vector2 alienOrigin = new Vector2(alien.Width / 2f, alien.Height / 2f);
            
            spriteBatch.Draw(alien, windowPosition, null, aliencolor, 0f, alienOrigin, alienScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(window, windowPosition, null, Color.White, 0f, windowOrigin, windowScale,
                SpriteEffects.None, 0f);
        }
    }
}