using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace assignment7.Content.labels;

public class Next
{
    public String Text;
    public Rectangle Box;
    public Texture2D BoxTexture;
    // add in functionality to store next Tetramino that will be added to screen

    public Next(GraphicsDevice graphicsDevice)
    {
        Text = "Next";
        Box = new Rectangle(
            new Vector2(10, 15).ToPoint(), 
            new Vector2(110, 60).ToPoint());
        BoxTexture = new Texture2D(graphicsDevice, 1, 1);
        BoxTexture.SetData(new[] { Color.White });
    }

    public void Update() //Piece nextPiece
    {
        //NextPiece = nextPiece;
    }

    public void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D Border)
    {
        spriteBatch.Draw(
            BoxTexture,
            Box,
            Color.White);
        spriteBatch.DrawString(
            font,
            new StringBuilder(Text),
            new Vector2(Box.X + 5, Box.Y),
            Color.Navy);
    }
}