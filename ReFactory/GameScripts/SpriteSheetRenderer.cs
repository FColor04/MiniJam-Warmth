using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MainGameFramework;

namespace ReFactory.GameScripts;

public class SpriteSheetRenderer
{
    protected virtual int CurrentFrame => (int)Math.Floor((Time.TotalTime * frameRate) % sprites.Count);
    private Rectangle _rect;
    private Color _color;
    public int frameRate = 24;
    public ReadOnlyCollection<Texture2D> sprites;
    private Layer _layer;
    
    public enum Layer
    {
        Manual,
        Sprites,
        UI
    }
    
    public SpriteSheetRenderer(Texture2D inputSpriteSheet, int rows, int columns, Layer layer, bool discardEmpty = true, int frameRate = 24, Rectangle? sourceRect = null)
    {
        sourceRect ??= new Rectangle(0, 0, inputSpriteSheet.Width, inputSpriteSheet.Height);
        var sourceRectangle = sourceRect.Value;
        _layer = layer;
        var frameSize = new Point(sourceRectangle.Width / columns, sourceRectangle.Height / rows);
        
        var sprites = new List<Texture2D>();
        Color[] spriteSheetData = new Color[frameSize.X * frameSize.Y];
        
        for (int y = sourceRectangle.Y; y < sourceRectangle.Y + sourceRectangle.Height; y += frameSize.Y)
        {
            for (int x = sourceRectangle.X; x < sourceRectangle.X + sourceRectangle.Width; x += frameSize.X)
            {
                inputSpriteSheet.GetData(0, new Rectangle(x, y, frameSize.X, frameSize.Y), spriteSheetData, 0, frameSize.X * frameSize.Y);
                
                if (discardEmpty && spriteSheetData.All(pixel => pixel == new Color(0, 0, 0, 0)))
                    continue;
                
                var texture = new Texture2D(MainGame.graphicsDevice, frameSize.X, frameSize.Y);
                texture.SetData(spriteSheetData);
                sprites.Add(texture);
            }
        }

        this.frameRate = frameRate;
        this.sprites = sprites.AsReadOnly();
        switch (layer)
        {
            case Layer.Manual:
                break;
            case Layer.Sprites:
                MainGame.OnDrawSprites += Draw;
                break;
            case Layer.UI:
                MainGame.OnDrawUI += Draw;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
        }

        if (this.sprites.Count == 0)
            throw new ArgumentException(nameof(spriteSheetData));
    }

    ~SpriteSheetRenderer()
    {
        foreach (var sprite in sprites)
        {
            sprite.Dispose();
        }

        sprites = null;
        switch (_layer)
        {
            case Layer.Manual:
                break;
            case Layer.Sprites:
                MainGame.OnDrawSprites -= Draw;
                break;
            case Layer.UI:
                MainGame.OnDrawUI -= Draw;
                break;
        }
    }

    public void SetRect(Rectangle rect)
    {
        _rect = rect;
    }

    public void SetColor(Color color) => _color = color;
    public Color GetColor() => _color;

    private void Draw(float deltaTime, SpriteBatch batch)
    {
        if (_color.A == 0) return;
        batch.Draw(GetSprite(), _rect, _color);
    }

    public Texture2D GetSprite()
    {
        return sprites[CurrentFrame];
    }
}