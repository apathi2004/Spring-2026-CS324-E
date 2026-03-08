using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace assignment5.Content;

public class Tongue
{
    private VertexPositionColor[] _vertices;
    private Vector3 _headPosition;
    private float _headRadius;

    public Tongue(Vector3 headPosition, float headRadius)
    {
        _headPosition = headPosition;
        _headRadius = headRadius;
        BuildTongue();
    }

    private void BuildTongue()
    {
        Vector3 base1 = _headPosition + new Vector3(-0.05f, 0, -_headRadius);
        Vector3 base2 = _headPosition + new Vector3(0.05f, 0, -_headRadius);

        Vector3 mid1 = _headPosition + new Vector3(-0.04f, 0, -_headRadius - 0.3f);
        Vector3 mid2 = _headPosition + new Vector3(0.04f, 0, -_headRadius - 0.3f);

        Vector3 fork1 = _headPosition + new Vector3(-0.04f, 0f, -_headRadius - 0.5f);
        Vector3 fork2 = _headPosition + new Vector3(0.04f, 0f, -_headRadius - 0.5f);


        Vector3 leftTip1 = _headPosition + new Vector3(-0.15f, 0f, -_headRadius - 0.8f);
        Vector3 leftTip2 = _headPosition + new Vector3(-0.07f, 0f, -_headRadius - 0.8f);


        Vector3 rightTip1 = _headPosition + new Vector3(0.07f, 0f, -_headRadius - 0.8f);
        Vector3 rightTip2 = _headPosition + new Vector3(0.15f, 0f, -_headRadius - 0.8f);

        Color tongueColor = Color.Red;

        _vertices = new VertexPositionColor[]
        {
            new VertexPositionColor(base1, tongueColor),
            new VertexPositionColor(base2, tongueColor),
            new VertexPositionColor(mid1, tongueColor),
            new VertexPositionColor(mid2, tongueColor),
            new VertexPositionColor(fork1, tongueColor),
            new VertexPositionColor(fork2, tongueColor),

            new VertexPositionColor(fork1, tongueColor),
            new VertexPositionColor(leftTip1, tongueColor),

            new VertexPositionColor(leftTip1, tongueColor),
            new VertexPositionColor(leftTip2, tongueColor),

            new VertexPositionColor(leftTip2, tongueColor),
            new VertexPositionColor(fork2, tongueColor),
            new VertexPositionColor(fork2, tongueColor),
            new VertexPositionColor(rightTip1, tongueColor),

            new VertexPositionColor(rightTip1, tongueColor),
            new VertexPositionColor(rightTip2, tongueColor),
        };
    }

    public void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
    {
        basicEffect.VertexColorEnabled = true;
        basicEffect.LightingEnabled = false;

        foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            graphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                PrimitiveType.TriangleStrip,
                _vertices,
                0,
                _vertices.Length - 2
            );
        }
    }
}