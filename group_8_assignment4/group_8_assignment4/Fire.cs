using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;

namespace group_8_assignment4
{
    public class Fire
    {
        public float CurrentHeight;
        
        private float width;
        private float maxHeight;
        private float theta;
        private Color fireColor;
        private Texture2D pixel;
        private float animationSpeed;

        public Fire(float width, float maxHeight, string colorScheme)
        {
            this.width = width;
            this.maxHeight = maxHeight;
            this.CurrentHeight = maxHeight;
            this.theta = 0f;
            this.animationSpeed = 0.1f;
            
            if (colorScheme == "orange")
                fireColor = new Color(255, 140, 0);
            if (colorScheme == "red")
                fireColor = new Color(255, 42, 4);
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public void Update()
        {
            theta += animationSpeed;
        }

        public void Animate()
        {
            CurrentHeight = maxHeight - 10 * (float)Math.Sin(theta);
        }

        public void Display(SpriteBatch spriteBatch, Vector2 position)
        {

            int numLayers = 2;

            for (int i = 0; i < numLayers; i++)
            {
                float layerScale = 1.0f - (i * 0.15f);
                float layerHeight = CurrentHeight * layerScale;
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