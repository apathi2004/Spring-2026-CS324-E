using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RocketAnimation
{
    public class Rocket
    {
        private Vector2 position;
        private Vector2 velocity;

        private float bodyWidth;
        private float bodyHeight;

        private RocketBody body;
        private RocketFins fins;
        private Fire fire;

        private float angle;

        public Vector2 Position => position;
        public Vector2 Velocity => velocity;

        public Rocket(Vector2 startPosition, Color bodyColor, Color coneColor, Color finColor, string fireScheme = "orange")
        {
            position = startPosition;
            velocity = new Vector2(2, 0);

            bodyWidth = 40;
            bodyHeight = 100;

            body = new RocketBody(bodyWidth, bodyHeight, bodyColor, coneColor);
            fins = new RocketFins(25, 40, finColor, 2);
            fire = new Fire(bodyWidth * 0.8f, 60, fireScheme);

            angle = 0;
        }

        public void Initialize(GraphicsDevice graphicsDevice, Matrix view, Matrix projection)
        {
            body.Initialize(graphicsDevice, view, projection);
            fins.Initialize(graphicsDevice);
            fire.Initialize(graphicsDevice);
        }

        public void Update(GameTime gameTime, int screenWidth, int screenHeight)
        {
            position += velocity;

            if (position.X > screenWidth + 50)
                position.X = -50;
            if (position.X < -50)
                position.X = screenWidth + 50;

            fire.Update(gameTime);
        }

        public void Animate()
        {
            fire.Animate();
        }

        public void Display(SpriteBatch spriteBatch)
        {
            body.Display(spriteBatch, position);

            float bodyTopY = position.Y + body.ConeHeight;
            fins.Display(spriteBatch, position.X, bodyTopY, body.Width, body.Height);

            Vector2 firePosition = new Vector2(position.X, bodyTopY + body.Height);
            fire.Display(spriteBatch, firePosition);
        }

        public void SetVelocity(Vector2 newVelocity)
        {
            velocity = newVelocity;
        }

        public void Rotate(float angleChange)
        {
            angle += angleChange;
        }
    }
}