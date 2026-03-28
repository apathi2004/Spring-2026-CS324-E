using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace assignment6.Content;

public class ChainLink
{
    public Vector2 Position;
    public Vector2 Velocity;
    public float Mass;
    public float RestLength; // Can make chainlink act as spring with rest length
    public float CurrLength; // and also have a current length that will exert force on chain
    public float Width;
    public float Elasticity;
    
}