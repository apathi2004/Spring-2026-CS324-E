using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace assignment7
{
    public enum BlockType { O, I, J, L, S, T, Z }

    public class Block
    {
        // Grid constants
        public const int TextureSize = 120;  
        public const int CellSize    = 30;   
        public const int GridCols    = 10;
        public const int GridRows    = 20;
        public const int GridOffsetX = 50;   
        public const int GridOffsetY = 100;

        public BlockType   Type      { get; private set; }
        public Texture2D[] Textures  { get; private set; }

        public int GridX    { get; set; }
        public int GridY    { get; set; }
        public int Rotation { get; private set; }

        public Rectangle[] CellRects { get; private set; }

        // Each block shape: {col, row} offsets from pivot, rotation 0
        private static readonly int[][][] Shapes = new int[][][]
        {
            // O
            new int[][]{ new[]{0,0}, new[]{1,0}, new[]{0,1}, new[]{1,1} },
            // I
            new int[][]{ new[]{-1,0}, new[]{0,0}, new[]{1,0}, new[]{2,0} },
            // J
            new int[][]{ new[]{-1,-1}, new[]{-1,0}, new[]{0,0}, new[]{1,0} },
            // L
            new int[][]{ new[]{1,-1}, new[]{-1,0}, new[]{0,0}, new[]{1,0} },
            // S
            new int[][]{ new[]{0,-1}, new[]{1,-1}, new[]{-1,0}, new[]{0,0} },
            // T
            new int[][]{ new[]{0,-1}, new[]{-1,0}, new[]{0,0}, new[]{1,0} },
            // Z
            new int[][]{ new[]{-1,-1}, new[]{0,-1}, new[]{0,0}, new[]{1,0} },
        };

        public Block(BlockType type, Texture2D[] textures)
        {
            Type      = type;
            Textures  = textures;
            Rotation  = 0;
            GridX     = GridCols / 2;
            GridY     = 1;
            CellRects = new Rectangle[4];
            RefreshCells();
        }

        // Returns the 4 absolute grid positions this block occupies
        public int[][] GetCells(int? gridX = null, int? gridY = null, int? rotation = null)
        {
            int gx  = gridX    ?? GridX;
            int gy  = gridY    ?? GridY;
            int rot = rotation ?? Rotation;

            var shape = Shapes[(int)Type];
            var cells = new int[4][];
            for (int i = 0; i < 4; i++)
            {
                var offset = RotateOffset(shape[i], rot);
                cells[i] = new int[] { gx + offset[0], gy + offset[1] };
            }
            return cells;
        }

        private int[] RotateOffset(int[] offset, int rot)
        {
            if (Type == BlockType.O) return offset; // O never rotates
            int x = offset[0], y = offset[1];
            for (int r = 0; r < rot; r++)
            {
                int tmp = x;
                x = -y;
                y = tmp;
            }
            return new int[] { x, y };
        }

        private void RefreshCells()
        {
            var cells = GetCells();
            for (int i = 0; i < 4; i++)
            {
                int px = GridOffsetX + cells[i][0] * CellSize;
                int py = GridOffsetY + cells[i][1] * CellSize;
                CellRects[i] = new Rectangle(px, py, CellSize, CellSize);
            }
        }

        public void MoveLeft(int[][] board)
        {
            if (CanPlace(GridX - 1, GridY, Rotation, board))
            {
                GridX--;
                RefreshCells();
            }
        }

        public void MoveRight(int[][] board)
        {
            if (CanPlace(GridX + 1, GridY, Rotation, board))
            {
                GridX++;
                RefreshCells();
            }
        }

        public bool MoveDown(int[][] board)
        {
            if (CanPlace(GridX, GridY + 1, Rotation, board))
            {
                GridY++;
                RefreshCells();
                return true;
            }
            return false;
        }

        public void HardDrop(int[][] board)
        {
            while (CanPlace(GridX, GridY + 1, Rotation, board))
                GridY++;
            RefreshCells();
        }

        public void Rotate(int[][] board)
        {
            int nextRot = (Rotation + 1) % 4;
            int[] kicks = { 0, -1, 1, -2 };
            foreach (int kick in kicks)
            {
                if (CanPlace(GridX + kick, GridY, nextRot, board))
                {
                    Rotation = nextRot;
                    GridX   += kick;
                    RefreshCells();
                    return;
                }
            }
        }

        public bool CanPlace(int gx, int gy, int rot, int[][] board)
        {
            var cells = GetCells(gx, gy, rot);
            foreach (var cell in cells)
            {
                int cx = cell[0], cy = cell[1];
                if (cx < 0 || cx >= GridCols)      return false;
                if (cy >= GridRows)                 return false;
                if (cy >= 0 && board[cy][cx] != 0) return false;
            }
            return true;
        }

        public void LockToBoard(int[][] board)
        {
            var cells = GetCells();
            foreach (var cell in cells)
            {
                int cx = cell[0], cy = cell[1];
                if (cy >= 0 && cy < GridRows && cx >= 0 && cx < GridCols)
                    board[cy][cx] = (int)Type + 1;
            }
        }

        public void Draw(SpriteBatch sb, float alpha = 1f)
        {
            var texture = Textures.Length > 0 ? Textures[0] : null;
            if (texture == null) return;

            var cells = GetCells();
            foreach (var cell in cells)
            {
                int px = GridOffsetX + cell[0] * CellSize;
                int py = GridOffsetY + cell[1] * CellSize;
                sb.Draw(texture,
                    new Rectangle(px, py, CellSize, CellSize),
                    new Rectangle(0, 0, TextureSize, TextureSize),
                    Color.White * alpha);
            }
        }

        public void DrawGhost(SpriteBatch sb, int[][] board)
        {
            int ghostY = GridY;
            while (CanPlace(GridX, ghostY + 1, Rotation, board)) ghostY++;
            if (ghostY == GridY) return;

            var texture = Textures.Length > 0 ? Textures[0] : null;
            if (texture == null) return;

            var cells = GetCells(GridX, ghostY, Rotation);
            foreach (var cell in cells)
            {
                int px = GridOffsetX + cell[0] * CellSize;
                int py = GridOffsetY + cell[1] * CellSize;
                sb.Draw(texture,
                    new Rectangle(px, py, CellSize, CellSize),
                    new Rectangle(0, 0, TextureSize, TextureSize),
                    Color.White * 0.25f);
            }
        }
    }
}