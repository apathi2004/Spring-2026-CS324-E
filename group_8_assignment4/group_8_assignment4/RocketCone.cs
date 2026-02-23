using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_8_assignment4
{
    public class RocketCone
    {
        private float width;
        private float height;
        private Color color;
        private BasicEffect effect;

        public float Width => width;
        public float Height => height;

        public RocketCone(float width, float height, Color color)
        {
            this.width = width;
            this.height = height;
            this.color = color;
        }

        public void Initialize(GraphicsDevice graphicsDevice, Matrix view, Matrix projection)
        {
            effect = new BasicEffect(graphicsDevice)
            {
                VertexColorEnabled = true,
                View = view,
                Projection = projection
            };
        }

        public void Display(GraphicsDevice graphicsDevice, Vector2 position)
        {
            Vector2 top = new Vector2(position.X, position.Y);
            Vector2 bottomLeft = new Vector2(position.X - width / 2, position.Y + height);
            Vector2 bottomRight = new Vector2(position.X + width / 2, position.Y + height);

            VertexPositionColor[] vertices =
            {
                new VertexPositionColor(new Vector3(top, 0), color),
                new VertexPositionColor(new Vector3(bottomLeft, 0), color),
                new VertexPositionColor(new Vector3(bottomRight, 0), color)
            };

            graphicsDevice.RasterizerState = RasterizerState.CullNone;
            graphicsDevice.DepthStencilState = DepthStencilState.None;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives(
                    PrimitiveType.TriangleList,
                    vertices,
                    0,
                    1
                );
            }
        }
        
        private void DrawEdge(GraphicsDevice graphicsDevice, Vector2 a, Vector2 b)
        {
            VertexPositionColor[] line =
            {
                new VertexPositionColor(new Vector3(a,0), Color.Black),
                new VertexPositionColor(new Vector3(b,0), Color.Black)
            };

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, line, 0, 1);
            }
        }
    }
}