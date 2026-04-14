using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace assignment7
{
    public class Clean
    {
        public Texture2D[] CleanFrames;
        private int _frameIndex = 0;
        private float _frameTimer = 0f;
        private const float FrameDuration = 0.05f; // seconds per animation frame

        public bool IsAnimating { get; private set; } = false;

      
        private List<int> _clearingRows = new();

        public Clean(int frameCount)
        {
            CleanFrames = new Texture2D[frameCount];
        }

       
        public void StartClear(List<int> rows)
        {
            _clearingRows = rows;
            _frameIndex = 0;
            _frameTimer = 0f;
            IsAnimating = rows.Count > 0;
        }

       
        public List<int> Update(GameTime gameTime)
        {
            if (!IsAnimating) return null;

            _frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_frameTimer >= FrameDuration)
            {
                _frameTimer = 0f;
                _frameIndex++;
                if (_frameIndex >= CleanFrames.Length)
                {
                    IsAnimating = false;
                    _frameIndex = 0;
                    return _clearingRows; // signal Game1 to actually remove rows
                }
            }
            return null;
        }

        public void Draw(SpriteBatch sb)
        {
            if (!IsAnimating) return;
            var frame = CleanFrames[_frameIndex];
            if (frame == null) return;

            foreach (int row in _clearingRows)
            {
                int py = Block.GridOffsetY + row * Block.CellSize;
                // Stretch the animation frame across the whole row
                var dest = new Rectangle(Block.GridOffsetX, py,
                    Block.GridCols * Block.CellSize, Block.CellSize);
                sb.Draw(frame, dest, Color.White);
            }
        }
    }
}