using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace assignment5.Content;

public class Cylinder
{

    private VertexPositionNormalTexture[] _vertices;
    private short[] _indices;
    private Vector3 _position;
    private float _radius;
    private float _height;
    private Color _color;
    private int _segments = 24;

    public Cylinder(Vector3 position, float radius, float height, Color color)
    {
        _position = position;
        _radius = radius;
        _height = height;
        _color = color;
        BuildCylinder();
    }

    private void BuildCylinder()
    {
        int vertCount = (_segments + 1) * 2;
        _vertices = new VertexPositionNormalTexture[vertCount];
        _indices = new short[_segments * 6];

        for (int i = 0; i <= _segments; i++)
        {
            float angle = (float)(i * 2 * Math.PI / _segments);
            float nx = (float)Math.Cos(angle);
            float nz = (float)Math.Sin(angle);
            float x = _position.X + _radius * nx;
            float z = _position.Z + _radius * nz;
            Vector3 normal = Vector3.Normalize(new Vector3(nx, 0, nz));

            _vertices[i] = new VertexPositionNormalTexture(
                new Vector3(x, _position.Y, z), normal, Vector2.Zero);

            _vertices[i + (_segments + 1)] = new VertexPositionNormalTexture(
                new Vector3(x, _position.Y + _height, z), normal, Vector2.Zero);
        }

        for (int i = 0; i < _segments; i++)
        {
            int bottom1 = i;
            int bottom2 = i + 1;
            int top1 = i + (_segments + 1);
            int top2 = i + (_segments + 1) + 1;

            _indices[i * 6 + 0] = (short)bottom1;
            _indices[i * 6 + 1] = (short)top1;
            _indices[i * 6 + 2] = (short)bottom2;
            _indices[i * 6 + 3] = (short)bottom2;
            _indices[i * 6 + 4] = (short)top1;
            _indices[i * 6 + 5] = (short)top2;
        }
    }

    public void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
    {
        basicEffect.LightingEnabled = true;
        basicEffect.EnableDefaultLighting();
        basicEffect.VertexColorEnabled = false;
        basicEffect.DiffuseColor = _color.ToVector3();
        basicEffect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
        basicEffect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1f, -1f, 0.5f));
        basicEffect.DirectionalLight0.DiffuseColor = new Vector3(1f, 1f, 1f);
        basicEffect.DirectionalLight0.Enabled = true;

        foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(
                PrimitiveType.TriangleList,
                _vertices,
                0,
                _vertices.Length,
                _indices,
                0,
                _segments * 2
            );
        }
    }
}