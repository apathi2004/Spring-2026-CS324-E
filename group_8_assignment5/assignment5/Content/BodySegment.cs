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
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Calculate undulation offset
        float undulateY = Amplitude * (float)Math.Sin(elapsed * UndulateSpeed + _phaseOffset);

        // Calculate the desired position behind the target at a fixed distance
        Vector3 direction = SegmentPosition - targetPosition;
        if (direction.Length() > 0)
            direction.Normalize();

        float desiredDistance = Scale * 24.24f * 0.5f;
        Vector3 desiredPosition = targetPosition + direction * desiredDistance;

        // Apply vertical undulation to desired position
        desiredPosition.Y = targetPosition.Y + undulateY;

        // Lerp from current position toward desired position for smooth following
        float lerpSpeed = 10.0f;
        SegmentPosition = Vector3.Lerp(desiredPosition, SegmentPosition, lerpSpeed * deltaTime);
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

                effect.World =
                    transforms[mesh.ParentBone.Index] *
                    Matrix.CreateScale(Scale) *
                    Matrix.CreateTranslation(SegmentPosition);

                effect.DiffuseColor = Color.ToVector3();
                effect.LightingEnabled = false;
            }

            mesh.Draw();
        }
    }
}