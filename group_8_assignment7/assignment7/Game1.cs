using System;
using System.Text;
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
    
    private Texture2D[] _block1;
    private Texture2D[] _I1;
    private Texture2D[] _J1;
    private Texture2D[] _L1;
    private Texture2D[] _S1;
    private Texture2D[] _T1;
    private Texture2D[] _Z1;

    public Clean Clean;
    public Score Score;
    public Timer Timer;
    public Next Next;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Timer = new Timer();
        Score = new Score();
        Next = new Next(GraphicsDevice);
        Clean = new Clean(9);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _font = Content.Load<SpriteFont>("font/Exo-Black");
        
        _background = Content.Load<Texture2D>("Assets/Background");
        _border = Content.Load<Texture2D>("Assets/Border");
        
        _graphics.PreferredBackBufferWidth = _border.Width;
        _graphics.PreferredBackBufferHeight = _border.Height;
        _graphics.ApplyChanges();

        for (int i = 1; i < Clean.CleanFrames.Length + 1; i++)
        {
            Clean.CleanFrames[i - 1] = Content.Load<Texture2D>("Assets/Fx_clean0" + i.ToString());
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        Timer.Update(gameTime);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        _spriteBatch.Draw(
            _background, 
            Vector2.Zero, 
            new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), 
            Color.White);
        
        _spriteBatch.Draw(_border, Vector2.Zero, Color.White);
        
        Timer.Draw(_spriteBatch, _font, GraphicsDevice);
        Score.Draw(_spriteBatch, _font, Timer, GraphicsDevice);
        Next.Draw(_spriteBatch, _font, _border);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}