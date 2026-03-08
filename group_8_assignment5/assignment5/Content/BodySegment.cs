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
        Amplitude = scale * 5f;
        _phaseOffset = phaseOffset;
    }

    public void Update(GameTime gameTime, Vector3 targetPosition)
    {
        float elapsed = (float)gameTime.TotalGameTime.TotalSeconds;

        // Sine-based vertical undulation with phase offset per segment
        float undulateY = Amplitude * (float)Math.Sin(elapsed * UndulateSpeed + _phaseOffset);

        Vector3 desiredPosition = new Vector3(
            targetPosition.X,
            targetPosition.Y + undulateY,
            targetPosition.Z
        );

        // Lerp toward the target so each segment smoothly follows the one ahead
        SegmentPosition = Vector3.Lerp(SegmentPosition, desiredPosition, 0.15f);
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
