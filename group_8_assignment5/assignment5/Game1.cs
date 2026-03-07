using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace assignment5;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private BasicEffect _basicEffect;
    private VertexPositionColor[] _land;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _basicEffect = new BasicEffect(GraphicsDevice);
        _basicEffect.VertexColorEnabled = true;
        
        float fieldOfView = MathHelper.PiOver4;
        float aspectRatio = GraphicsDevice.Viewport.AspectRatio;
        _basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(
            fieldOfView,
            aspectRatio,
            1f,
            500f);

        Vector3 eye = new Vector3(0f, 4f, 10f);
        Vector3 target = new Vector3(0f, 0f, 0f);
        Vector3 up = new Vector3(0f, 1f, 0f);
        _basicEffect.View = Matrix.CreateLookAt(eye, target, up);
        
        _basicEffect.World = Matrix.Identity;
        
        _basicEffect.VertexColorEnabled = true;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _land = new VertexPositionColor[4];
        _land[0] = new VertexPositionColor(new Vector3(-5, 0, 5), Color.SandyBrown);
        _land[1] = new VertexPositionColor(new Vector3(5, 0, 5), Color.SandyBrown);
        _land[2] = new VertexPositionColor(new Vector3(-5, 0, -5), Color.SandyBrown);
        _land[3] = new VertexPositionColor(new Vector3(5, 0, -5), Color.SandyBrown);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        GraphicsDevice.RasterizerState = RasterizerState.CullNone;
        foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                PrimitiveType.TriangleStrip,
                _land,
                0,
                2);
        }

        base.Draw(gameTime);
    }
}