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
    protected bool DrawRenderTarget => renderTarget != null;
    protected RenderTarget2D renderTarget;
    public virtual int RenderWidth { get; protected set; }
    public virtual int RenderHeight { get; protected set; }
    public virtual Vector2 ViewportOffset { get; set; }
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
            renderTarget = new RenderTarget2D(MainGame.GraphicsDevice, renderWidth.Value, renderHeight.Value, false, 
                MainGame.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
        
        RenderWidth = renderWidth.Value;
        RenderHeight = renderHeight.Value;
        SpriteBatch = new SpriteBatch(MainGame.GraphicsDevice);
    }

    public void Draw()
    {
        MainGame.GraphicsDevice.SetRenderTarget(renderTarget);
        if (DrawRenderTarget)
            MainGame.GraphicsDevice.Clear(Color.Transparent);

        ViewportOffset = (ViewportOffset * 1000f).ToPoint().ToVector2() / 1000f;
        
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
        OnDraw?.Invoke(SpriteBatch, this);
        SpriteBatch.End();
    }

    public void DrawRenderTexture()
    {
        if (DrawRenderTarget)
        {
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            var offset = new Point();
            offset.X = (int) (-((ViewportOffset.X - MathF.Truncate(ViewportOffset.X)) * ((float) Width / RenderWidth)));
            offset.Y = (int) (-((ViewportOffset.Y - MathF.Truncate(ViewportOffset.Y)) * ((float) Height / RenderHeight)));
            Debug.Log(ViewportOffset);
            SpriteBatch.Draw(renderTarget, new Rectangle(offset + Location, Size), Color.White);
            SpriteBatch.End();
        }
    }
}