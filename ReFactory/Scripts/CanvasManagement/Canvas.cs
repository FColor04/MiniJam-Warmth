using System;
using MainGameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReFactory;
using ReFactory.Utility;

namespace CanvasManagement;

public class Canvas
{
    public virtual int X { get; protected set; }
    public virtual int Y { get; protected set; }
    public virtual int Width { get; protected set; }
    public virtual int Height { get; protected set; }
    protected bool DrawRenderTarget => RenderTarget != null;
    protected RenderTarget2D RenderTarget;
    public virtual int RenderWidth { get; protected set; }
    public virtual int RenderHeight { get; protected set; }
    private SpriteBatch SpriteBatch { get; }
    public CanvasLayer Layer { get; }

    public Rectangle Rect
    {
        get => new(X, Y, Width, Height);
        set
        {
            X = value.X;
            Y = value.Y;
            Width = value.Width;
            Height = value.Height;
        }
    }

    public Point Size
    {
        get => new(Width, Height);
        set
        {
            Width = value.X;
            Height = value.Y;
        }
    }
    
    public Point RenderTargetSize
    {
        get => new(RenderWidth, RenderHeight);
    }

    public Point Location
    {
        get => new(X, Y);
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }
    
    public Vector2 MouseNormalizedPosition => WindowToLocalNormalizedPosition(Input.MousePosition);
    public Vector2 MousePosition => MouseNormalizedPosition * RenderTargetSize.ToVector2();

    public event Action<SpriteBatch, Canvas> OnDraw;
    
    private Vector2 WindowToLocalNormalizedPosition(Point positionWithinWindow)
    {
        return (positionWithinWindow - Location).ToVector2() / Size.ToVector2();
    }
    private Vector2 LocalNormalizedToWindowPosition(Vector2 positionWithinRect)
    {
        return (positionWithinRect * Size.ToVector2()) + Location.ToVector2();
    }
    
    public Canvas(int x, int y, int width, int height, CanvasLayer layer, int? renderWidth = null, int? renderHeight = null)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Layer = layer;
        
        renderWidth ??= width;
        renderHeight ??= height;

        if ((renderWidth != width || renderHeight != height) && renderHeight != 0 && renderWidth != 0)
            RenderTarget = new RenderTarget2D(MainGame.graphicsDevice, renderWidth.Value, renderHeight.Value, false, 
                MainGame.graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
        
        RenderWidth = renderWidth.Value;
        RenderHeight = renderHeight.Value;
        SpriteBatch = new SpriteBatch(MainGame.graphicsDevice);
        Debug.Log($"Created {layer} canvas, ({x}, {y}) {width} x {height}, {renderWidth} x {renderHeight}");
    }

    public void Draw()
    {
        MainGame.graphicsDevice.SetRenderTarget(RenderTarget);
        if (DrawRenderTarget)
            MainGame.graphicsDevice.Clear(Color.Transparent);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
        OnDraw?.Invoke(SpriteBatch, this);
        SpriteBatch.End();
    }

    public void DrawRenderTexture()
    {
        if (DrawRenderTarget)
        {
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            SpriteBatch.Draw(RenderTarget, Rect, Color.White);
            SpriteBatch.End();
        }
    }
}