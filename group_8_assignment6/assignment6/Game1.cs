using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace assignment6;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch           _spriteBatch;
    private Texture2D             _pixel;
    private Pendulum              _pendulum;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth  = 900;
        _graphics.PreferredBackBufferHeight = 600;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        _pendulum = new Pendulum(
            _graphics.PreferredBackBufferWidth,
            _graphics.PreferredBackBufferHeight,
            _pixel);
    }

    protected override void Update(GameTime gameTime)
    {
        var kb = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || kb.IsKeyDown(Keys.Escape))
            Exit();

        if (kb.IsKeyDown(Keys.R))
            _pendulum.Reset();

        _pendulum.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(18, 18, 24));

        _spriteBatch.Begin();
        _pendulum.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}