using System;
using System.Reflection.Metadata.Ecma335;

namespace assignment5.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using assignment5.Content;

public class Head
{
    public float Scale;
    public float ScaledRadius;
    public Model HeadModel;
    public Vector3 HeadPosition;
    public Model Eye1Model;
    public Vector3 Eye1Position;
    public Model Eye2Model;
    public Vector3 Eye2Position;

    public Color Color;
    public Tongue Tongue;

    public Head(float scale, Model model, Color color)
    {
        Color = color;
        
        Scale = scale;
        ScaledRadius = scale * 24.24f;
        HeadModel = model;
        HeadPosition = new Vector3(0, ScaledRadius, 0);

        float eyeRadius = ScaledRadius;  
        float eyeScale = Scale * 0.1f;
        float eyeScaledRadius = eyeScale * 24.24f;

        float angleUp = MathHelper.ToRadians(30f); 
        float angleSide = MathHelper.ToRadians(30f);

        Eye1Model = model;
        Eye1Position = HeadPosition + new Vector3(
            eyeRadius * (float)Math.Sin(angleSide), 
            eyeRadius * (float)Math.Sin(angleUp),
            -eyeRadius * (float)Math.Cos(angleSide) 
        );

        Eye2Model = model;
        Eye2Position = HeadPosition + new Vector3(
            -eyeRadius * (float)Math.Sin(angleSide), 
            eyeRadius * (float)Math.Sin(angleUp), 
            -eyeRadius * (float)Math.Cos(angleSide) 
        );

        Tongue = new Tongue(HeadPosition, ScaledRadius);
    }

    public void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
    {
        DrawPart(HeadModel, HeadPosition, basicEffect, Scale, Color);
        DrawPart(Eye1Model, Eye1Position, basicEffect, Scale * 0.6f, Color.Black);
        DrawPart(Eye2Model, Eye2Position, basicEffect, Scale * 0.6f, Color.Black);
        Tongue.Draw(graphicsDevice, basicEffect);
    }

    private void DrawPart(Model model, Vector3 position, BasicEffect basicEffect, float scale,  Color color)
    {
        Matrix[] transforms = new Matrix[model.Bones.Count]; 
        model.CopyAbsoluteBoneTransformsTo(transforms);
        foreach (ModelMesh mesh in model.Meshes) 
        {
            foreach (BasicEffect effect in mesh.Effects) 
            {
                effect.View = basicEffect.View;
                effect.Projection = basicEffect.Projection;
                effect.World = transforms[mesh.ParentBone.Index] *
                               Matrix.CreateScale(scale) *
                               Matrix.CreateTranslation(position) *
                               basicEffect.World;
                
                effect.DiffuseColor = color.ToVector3();
                effect.EnableDefaultLighting();
            }
            mesh.Draw();
        }
    }
}