using System.Collections.Generic;
using assignment7.Content;
using assignment7.Content.labels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace assignment7;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;

    private Texture2D _border;
    private Texture2D _background;

    // Each array holds the sprite frames for that block type
    private Texture2D[] _blockO;
    private Texture2D[] _blockI;
    private Texture2D[] _blockJ;
    private Texture2D[] _blockL;
    private Texture2D[] _blockS;
    private Texture2D[] _blockT;
    private Texture2D[] _blockZ;

    // Indexed by BlockType enum order: O I J L S T Z
    private Texture2D[][] _allBlocks;

    private Clean _clean;
    private Score _score;
    private Timer _timer;
    private assignment7.Content.Next _next;

    private int[][] _board;
    private Block _active;
    private bool _gameOver = false;
    private bool _paused = false;

    private KeyboardState _prevKeys;

    private float _dasTimer = 0f;
    private const float DasDelay = 0.15f;
    private const float DasRepeat = 0.05f;
    private bool _dasActive = false;
    private Keys _dasKey = Keys.None;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _timer = new Timer();
        _score = new Score();
        _next  = new assignment7.Content.Next(GraphicsDevice);
        _clean = new Clean(9);

        _board = new int[Block.GridRows][];
        for (int r = 0; r < Block.GridRows; r++)
            _board[r] = new int[Block.GridCols];

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _font       = Content.Load<SpriteFont>("font/Exo-Black");
        _background = Content.Load<Texture2D>("Assets/Background");
        _border     = Content.Load<Texture2D>("Assets/Border");

        _graphics.PreferredBackBufferWidth  = _border.Width;
        _graphics.PreferredBackBufferHeight = _border.Height;
        _graphics.ApplyChanges();

        // Load clean animation frames
        for (int i = 1; i <= _clean.CleanFrames.Length; i++)
            _clean.CleanFrames[i - 1] = Content.Load<Texture2D>("Assets/Fx_clean0" + i);

        // Load block sprites using your actual asset names
        // Each block type has 2 variants (1 and 2), each with up to 4 rotation frames
        _blockO = new[]
        {
            Content.Load<Texture2D>("Assets/Tetromino_block1_1"),
            Content.Load<Texture2D>("Assets/Tetromino_O1_2"),
        };
        _blockI = new[]
        {
            Content.Load<Texture2D>("Assets/Tetromino_I1_1"),
            Content.Load<Texture2D>("Assets/Tetromino_I1_2"),
            Content.Load<Texture2D>("Assets/Tetromino_I2_1"),
            Content.Load<Texture2D>("Assets/Tetromino_I2_2"),
        };
        _blockJ = new[]
        {
            Content.Load<Texture2D>("Assets/Tetromino_J1_1"),
            Content.Load<Texture2D>("Assets/Tetromino_J1_2"),
            Content.Load<Texture2D>("Assets/Tetromino_J1_3"),
            Content.Load<Texture2D>("Assets/Tetromino_J1_4"),
        };
        _blockL = new[]
        {
            Content.Load<Texture2D>("Assets/Tetromino_L1_1"),
            Content.Load<Texture2D>("Assets/Tetromino_L1_2"),
            Content.Load<Texture2D>("Assets/Tetromino_L1_3"),
            Content.Load<Texture2D>("Assets/Tetromino_L1_4"),
        };
        _blockS = new[]
        {
            Content.Load<Texture2D>("Assets/Tetromino_S1_1"),
            Content.Load<Texture2D>("Assets/Tetromino_S1_2"),
            Content.Load<Texture2D>("Assets/Tetromino_S2_1"),
            Content.Load<Texture2D>("Assets/Tetromino_S2_2"),
        };
        _blockT = new[]
        {
            Content.Load<Texture2D>("Assets/Tetromino_T1_1"),
            Content.Load<Texture2D>("Assets/Tetromino_T1_2"),
            Content.Load<Texture2D>("Assets/Tetromino_T1_3"),
            Content.Load<Texture2D>("Assets/Tetromino_T1_4"),
        };
        _blockZ = new[]
        {
            Content.Load<Texture2D>("Assets/Tetromino_Z1_1"),
            Content.Load<Texture2D>("Assets/Tetromino_Z1_2"),
            Content.Load<Texture2D>("Assets/Tetromino_Z2_1"),
            Content.Load<Texture2D>("Assets/Tetromino_Z2_2"),
        };

        // Order must match BlockType enum: O, I, J, L, S, T, Z
        _allBlocks = new[] { _blockO, _blockI, _blockJ, _blockL, _blockS, _blockT, _blockZ };

        _next.BlockTextures = new[]
        {
            _blockO[0], _blockI[0], _blockJ[0], _blockL[0],
            _blockS[0], _blockT[0], _blockZ[0]
        };

        SpawnBlock();
    }

    protected override void Update(GameTime gameTime)
    {
        var keys = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            keys.IsKeyDown(Keys.Escape))
        {
            Exit();
            return;
        }

        if (WasJustPressed(keys, Keys.P))
        {
            _paused = !_paused;
            _timer.Paused = _paused;
        }

        if (_paused || _gameOver)
        {
            if (_gameOver && WasJustPressed(keys, Keys.R))
                Restart();
            _prevKeys = keys;
            base.Update(gameTime);
            return;
        }

        _timer.Update(gameTime);

        if (!_clean.IsAnimating)
        {
            HandleInput(keys, gameTime);

            if (_timer.ShouldDrop)
            {
                bool landed = !_active.MoveDown(_board);
                if (landed) LockAndSpawn();
            }
        }
        else
        {
            var clearedRows = _clean.Update(gameTime);
            if (clearedRows != null)
            {
                RemoveRows(clearedRows);
                _score.AddLines(clearedRows.Count);
                _timer.SetLevel(_score.Value / 1000);
                SpawnBlock();
            }
        }

        _prevKeys = keys;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();

        _spriteBatch.Draw(_background, Vector2.Zero,
            new Rectangle(0, 0,
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight),
            Color.White);

        _spriteBatch.Draw(_border, Vector2.Zero, Color.White);

        DrawBoard();

        if (!_clean.IsAnimating)
        {
            _active.DrawGhost(_spriteBatch, _board);
            _active.Draw(_spriteBatch);
        }
        else
        {
            _clean.Draw(_spriteBatch);
        }

        _timer.Draw(_spriteBatch, _font, GraphicsDevice);
        _score.Draw(_spriteBatch, _font, _timer, GraphicsDevice);
        _next.Draw(_spriteBatch, _font, _border);

        if (_paused)
        {
            string msg = "PAUSED";
            var size = _font.MeasureString(msg);
            var pos = new Vector2(
                (_graphics.PreferredBackBufferWidth  - size.X) / 2f,
                (_graphics.PreferredBackBufferHeight - size.Y) / 2f);
            _spriteBatch.DrawString(_font, msg, pos, Color.Yellow);
        }

        if (_gameOver)
        {
            string msg = "GAME OVER\nPress R to restart";
            var size = _font.MeasureString(msg);
            var pos = new Vector2(
                (_graphics.PreferredBackBufferWidth  - size.X) / 2f,
                (_graphics.PreferredBackBufferHeight - size.Y) / 2f);
            _spriteBatch.DrawString(_font, msg, pos, Color.Red);
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private void DrawBoard()
    {
        for (int row = 0; row < Block.GridRows; row++)
        {
            for (int col = 0; col < Block.GridCols; col++)
            {
                int val = _board[row][col];
                if (val == 0) continue;
                var tex = _allBlocks[val - 1][0];
                if (tex == null) continue;
                int px = Block.GridOffsetX + col * Block.CellSize;
                int py = Block.GridOffsetY + row * Block.CellSize;
                _spriteBatch.Draw(tex, new Vector2(px, py),
                    new Rectangle(0, 0, Block.CellSize, Block.CellSize),
                    Color.White);
            }
        }
    }

    private void SpawnBlock()
    {
        var type    = _next.Pop();
        var textures = _allBlocks[(int)type];
        _active     = new Block(type, textures);

        if (!_active.CanPlace(_active.GridX, _active.GridY, _active.Rotation, _board))
            _gameOver = true;
    }

    private void LockAndSpawn()
    {
        _active.LockToBoard(_board);

        var fullRows = new List<int>();
        for (int row = 0; row < Block.GridRows; row++)
        {
            bool full = true;
            for (int col = 0; col < Block.GridCols; col++)
                if (_board[row][col] == 0) { full = false; break; }
            if (full) fullRows.Add(row);
        }

        if (fullRows.Count > 0)
            _clean.StartClear(fullRows);
        else
            SpawnBlock();
    }

    private void RemoveRows(List<int> rows)
    {
        rows.Sort((a, b) => b.CompareTo(a));
        foreach (int row in rows)
        {
            for (int r = row; r > 0; r--)
                _board[r] = (int[])_board[r - 1].Clone();
            _board[0] = new int[Block.GridCols];
        }
    }

    private void HandleInput(KeyboardState keys, GameTime gameTime)
    {
        if (WasJustPressed(keys, Keys.Up) || WasJustPressed(keys, Keys.X))
            _active.Rotate(_board);

        if (keys.IsKeyDown(Keys.Down))
            _active.MoveDown(_board);

        if (WasJustPressed(keys, Keys.Space))
        {
            _active.HardDrop(_board);
            LockAndSpawn();
            return;
        }

        HandleDas(keys, gameTime);
    }

    private void HandleDas(KeyboardState keys, GameTime gameTime)
    {
        bool leftDown  = keys.IsKeyDown(Keys.Left);
        bool rightDown = keys.IsKeyDown(Keys.Right);
        Keys pressed   = leftDown ? Keys.Left : (rightDown ? Keys.Right : Keys.None);

        if (pressed == Keys.None)
        {
            _dasTimer  = 0f;
            _dasActive = false;
            _dasKey    = Keys.None;
            return;
        }

        if (pressed != _dasKey)
        {
            _dasKey    = pressed;
            _dasTimer  = 0f;
            _dasActive = false;
            if (pressed == Keys.Left) _active.MoveLeft(_board);
            else                       _active.MoveRight(_board);
            return;
        }

        _dasTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (!_dasActive)
        {
            if (_dasTimer >= DasDelay) { _dasActive = true; _dasTimer = 0f; }
        }
        else
        {
            if (_dasTimer >= DasRepeat)
            {
                _dasTimer = 0f;
                if (pressed == Keys.Left) _active.MoveLeft(_board);
                else                       _active.MoveRight(_board);
            }
        }
    }

    private bool WasJustPressed(KeyboardState cur, Keys key)
        => cur.IsKeyDown(key) && !_prevKeys.IsKeyDown(key);

    private void Restart()
    {
        _score.Reset();
        _timer.Paused = false;
        _paused   = false;
        _gameOver = false;
        for (int r = 0; r < Block.GridRows; r++)
            _board[r] = new int[Block.GridCols];
        SpawnBlock();
    }
}