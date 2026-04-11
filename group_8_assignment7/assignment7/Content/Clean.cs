using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace assignment7.Content;

public class Clean
{
    public Texture2D[] CleanFrames;
    public int CurrentFrame;

    public Clean(int numFrames)
    {
        CleanFrames = new Texture2D[numFrames];
        CurrentFrame = 0;
    }
}