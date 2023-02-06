using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ReFactory.GameScripts;

public class Sand : Entity
{
    private Texture2D _sprite;
    public override Texture2D Sprite => _sprite;
    public override Vector2 Origin => Vector2.Zero;

    public Sand(Vector2 position) : base()
    {
        this.position = position;
        _sprite = GameContent.SandTextures.Random();
    }
}