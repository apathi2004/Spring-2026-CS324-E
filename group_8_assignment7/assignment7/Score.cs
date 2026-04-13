using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace assignment7
{
    public class Score
    {
        public int Value { get; private set; } = 0;

        // Classic Tetris scoring
        private static readonly int[] LinePoints = { 0, 100, 300, 500, 800 };

        public void AddLines(int linesCleared)
        {
            if (linesCleared < 1 || linesCleared > 4) return;
            Value += LinePoints[linesCleared];
        }

        public void Reset() => Value = 0;

        public void Draw(SpriteBatch sb, SpriteFont font, Timer timer, GraphicsDevice gd)
        {
            string text = $"SCORE\n{Value:D6}";
            // Position to the right of the grid – tweak X/Y to match your border asset
            var pos = new Vector2(400, 200);
            sb.DrawString(font, text, pos, Color.White);
        }
    }
}