using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace assignment7.Content
{
    public class Next
    {
        private GraphicsDevice _gd;
        public BlockType NextType { get; private set; }

        // Textures indexed by BlockType – set by Game1 after loading
        public Texture2D[] BlockTextures { get; set; } = new Texture2D[7];

        // Position of the "NEXT" panel on screen (relative to border)
        private const int PanelX = 400; // tweak to match your border layout
        private const int PanelY = 80;
        private const int CellSize = Block.CellSize;

        public Next(GraphicsDevice gd)
        {
            _gd = gd;
            NextType = PickRandom();
        }

        public BlockType Pop()
        {
            var current = NextType;
            NextType = PickRandom();
            return current;
        }

        private BlockType PickRandom()
        {
            var rng = new System.Random();
            return (BlockType)rng.Next(0, 7);
        }

        public void Draw(SpriteBatch sb, SpriteFont font, Texture2D border)
        {
            // Label
            sb.DrawString(font, "NEXT", new Vector2(PanelX, PanelY - 28), Color.White);

            // Preview cells – use the same shape definition as Block
            var tex = BlockTextures[(int)NextType];
            if (tex == null) return;

            // Draw a mini 4x4 grid centred in the panel
            var tempBlock = new Block(NextType, new[] { tex });
            var cells = tempBlock.GetCells(1, 1); // local coords
            foreach (var cell in cells)
            {
                int px = PanelX + cell[0] * CellSize;
                int py = PanelY + cell[1] * CellSize;
                sb.Draw(tex, new Vector2(px, py),
                    new Rectangle(0, 0, CellSize, CellSize), Color.White);
            }
        }
    }
}