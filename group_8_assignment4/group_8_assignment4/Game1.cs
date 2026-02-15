using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace group_8_assignment4
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private int screenWidth = 800;
        private int screenHeight = 600;

        private List<Rocket> rockets;

        private SpriteFont font;

        private Color backgroundColor = new Color(20, 20, 40);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
        }

        protected override void Initialize()
        {
            base.Initialize();

            rockets = new List<Rocket>();
            CreateRockets();
        }

        private void CreateRockets()
        {
            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 1), Vector3.Zero, Vector3.Up);
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, screenWidth, screenHeight, 0, 0, 1);

            Rocket rocket1 = new Rocket(
                new Vector2(150, 200),
                new Color(200, 0, 0),
                Color.White,
                new Color(128, 128, 128),
                "orange"
            );
            rocket1.SetVelocity(new Vector2(2, 0));
            rocket1.Initialize(GraphicsDevice, view, projection);
            rockets.Add(rocket1);

            Rocket rocket2 = new Rocket(
                new Vector2(400, 400),
                new Color(0, 100, 200),
                new Color(30, 30, 30),
                new Color(80, 80, 80),
                "red"
            );
            rocket2.SetVelocity(new Vector2(1.5f, 0));
            rocket2.Initialize(GraphicsDevice, view, projection);
            rockets.Add(rocket2);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            try
            {
                font = Content.Load<SpriteFont>("Font");
            }
            catch
            {
                font = null;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var rocket in rockets)
            {
                rocket.Update(gameTime, screenWidth, screenHeight);
                rocket.Animate();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            spriteBatch.Begin();

            DrawStars();

            foreach (var rocket in rockets)
            {
                rocket.Display(spriteBatch);
            }

            DrawUI();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawStars()
        {
            Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            System.Random random = new System.Random(42);

            for (int i = 0; i < 50; i++)
            {
                int x = random.Next(0, screenWidth);
                int y = random.Next(0, screenHeight);
                int size = random.Next(1, 3);

                spriteBatch.Draw(pixel, new Rectangle(x, y, size, size), Color.White);
            }
        }

        private void DrawUI()
        {
            if (font == null) return;

            string title = "Hierarchical Rocket Animation";
            Vector2 titleSize = font.MeasureString(title);
            Vector2 titlePosition = new Vector2(screenWidth / 2 - titleSize.X / 2, 20);
            spriteBatch.DrawString(font, title, titlePosition, Color.White);

            string[] instructions = new string[]
            {
                "Press ESC to exit",
                "Rocket 1: Red body, white cone, gray fins, orange fire",
                "Rocket 2: Blue body, black cone, dark gray fins, red fire",
                "Demonstrates hierarchical modeling - all parts move as one"
            };

            float yOffset = screenHeight - 100;
            foreach (string instruction in instructions)
            {
                spriteBatch.DrawString(font, instruction, new Vector2(10, yOffset), Color.LightGray, 
                    0, Vector2.Zero, 0.7f, SpriteEffects.None, 0);
                yOffset += 20;
            }
        }
    }
}