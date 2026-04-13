using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace assignment7
{
    public class Timer
    {
        public float Elapsed { get; private set; } = 0f;

        // How often the active block drops one row (seconds)
        public float DropInterval { get; private set; } = 0.8f;
        private float _dropTimer = 0f;

        public bool ShouldDrop { get; private set; } = false;

        public bool Paused { get; set; } = false;

        public void Update(GameTime gameTime)
        {
            ShouldDrop = false;
            if (Paused) return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Elapsed += dt;
            _dropTimer += dt;

            if (_dropTimer >= DropInterval)
            {
                _dropTimer = 0f;
                ShouldDrop = true;
            }
        }

        /// <summary>Increase gravity speed – call as score climbs.</summary>
        public void SetLevel(int level)
        {
            // Each level reduces interval by ~0.05s, floor at 0.1s
            DropInterval = MathHelper.Max(0.1f, 0.8f - level * 0.05f);
        }

        public void Draw(SpriteBatch sb, SpriteFont font, GraphicsDevice gd)
        {
            int minutes = (int)(Elapsed / 60);
            int seconds = (int)(Elapsed % 60);
            string text = $"TIME\n{minutes:D2}:{seconds:D2}";
            sb.DrawString(font, text, new Vector2(400, 140), Color.White);
        }
    }
}