using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace group_8_assignment4
{
    public class Fire
    {
        private float width;
        private float maxHeight;
        private float animationTime;
        private float animationSpeed;
        private Color fireColor;
        private Texture2D pixel;

        public Fire(float width, float maxHeight, string colorScheme = "orange")
        {
            this.width = width;
            this.maxHeight = maxHeight;
            this.animationTime = 0;
            this.animationSpeed = 0.05f;

            fireColor = new Color(255, 140, 0);
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public void Update(GameTime gameTime)
        {
        }

        public float Animate()
        {
            return 1.0f;
        }

        public void Display(SpriteBatch spriteBatch, Vector2 position)
        {
            float scale = Animate();
            float currentHeight = maxHeight * scale;

            int numLayers = 2;

            for (int i = 0; i < numLayers; i++)
            {
                float layerScale = 1.0f - (i * 0.15f);
                float layerHeight = currentHeight * layerScale;
                float layerWidth = width * layerScale;
                
                Rectangle fireRect = new Rectangle(
                    (int)(position.X - layerWidth / 2),
                    (int)position.Y,
                    (int)layerWidth,
                    (int)layerHeight
                );

                spriteBatch.Draw(pixel, fireRect, fireColor);
            }
        }

    }
}