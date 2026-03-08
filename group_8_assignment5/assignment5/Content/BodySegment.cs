using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace assignment5.Content;

public class BodySegment
{
    public Vector3 SegmentPosition;
    public Model Model;
    public float UndulateSpeed;
    public float Amplitude;
    public float Scale;
    public Color Color;

    private float _phaseOffset;

    public BodySegment(Model model, float scale, Color color, Vector3 startPosition, float phaseOffset)
    {
        Model = model;
        Scale = scale;
        Color = color;
        SegmentPosition = startPosition;
        UndulateSpeed = 2.0f;
        Amplitude = scale * 2f;
        _phaseOffset = phaseOffset;
    }

    public void Update(GameTime gameTime, Vector3 targetPosition)
    {
        float elapsed = (float)gameTime.TotalGameTime.TotalSeconds;
        float undulateY = Amplitude * (float)Math.Sin(elapsed * UndulateSpeed + _phaseOffset);

        // Place segment exactly behind the target at a fixed distance
        Vector3 direction = SegmentPosition - targetPosition;
        if (direction.Length() > 0)
            direction.Normalize();

        float desiredDistance = Scale * 24.24f * 0.5f;
        SegmentPosition = targetPosition + direction * desiredDistance;
        SegmentPosition.Y = targetPosition.Y + undulateY;
    }

    public void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
    {
        Matrix[] transforms = new Matrix[Model.Bones.Count];
        Model.CopyAbsoluteBoneTransformsTo(transforms);

        foreach (ModelMesh mesh in Model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.View = basicEffect.View;
                effect.Projection = basicEffect.Projection;
                effect.World = transforms[mesh.ParentBone.Index] *
                               Matrix.CreateScale(Scale) *
                               Matrix.CreateTranslation(SegmentPosition) *
                               basicEffect.World;

                effect.DiffuseColor = Color.ToVector3();
                effect.EnableDefaultLighting();
            }
            mesh.Draw();
        }
    }
}
    // Will need to get each body segment to undulate up and down, can use sine to accomplish this but need
    // to incorporate lerp somewhere in the process
