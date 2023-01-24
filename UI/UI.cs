using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MainGameFramework;

namespace MiniJam_Warmth;

public static class UI
{
    public static readonly UIElement Root;
    public static readonly Texture2D Pixel;
    private static IDragHandler _dragHandler;
    
    public static IEnumerable<UIElement> AllUIElements => Root.Flatten().OrderBy(element => element.Priority);
    public static List<IHasInteractiveRect> AdditionalInteractiveElements = new ();

    private static List<IHasInteractiveRect> elementsUnderPointer = new ();
    private static bool _clickHandled;
    
    static UI()
    {
        Root = new UIElement();
        
        Pixel = new Texture2D(MainGame.graphicsDevice, 1, 1);
        Pixel.SetData(new []{Color.White});
        
        MainGame.OnUpdate += Update;
        MainGame.OnDrawUI += DrawUI;
    }

    private static void Update(float deltaTime)
    {
        if(!MainGame.IsFocused) return;
        
        foreach (var uiElement in AllUIElements.OfType<IHasInteractiveRect>().Concat(AdditionalInteractiveElements))
        {
            if (uiElement.InteractiveRect.Contains(Input.MousePositionWithinViewport))
            {
                if (!elementsUnderPointer.Contains(uiElement))
                {
                    elementsUnderPointer.Add(uiElement);
                    if (uiElement is IPointerEnterHandler pointerEnterHandler)
                        pointerEnterHandler.OnPointerEnter();
                }


                if (Input.GetPressedMouseButton != -1 && uiElement is IPointerClickHandler pointerClickHandler && !_clickHandled)
                {
                    _clickHandled = true;
                    pointerClickHandler.OnPointerClick(Input.GetPressedMouseButton);
                }
                else if (_dragHandler == null && !_clickHandled)
                {
                    //Initiate Drag, Pressed && within rect
                    if (Input.LeftMouseHold && uiElement is IDragHandler dragHandler)
                    {
                        if (dragHandler.OnDrag())
                        {
                            Debug.WriteLine("Drag init");
                            _dragHandler = dragHandler;
                        }
                    }
                }
                else
                {
                    if (!Input.LeftMouseHold)
                    {
                        //If released && within rect
                        if (uiElement is IDropHandler slot)
                        {
                            Debug.WriteLine("Drag & Drop executed");
                            slot.OnDrop(_dragHandler);
                            _dragHandler = null;
                        }
                    }
                    else
                    {
                        //If mouse is held && within rect
                        if (uiElement is IDropHandler slot)
                            slot.OnPotentialDrop(_dragHandler);
                    }
                }
                if (Input.GetReleasedMouseButton != -1 &&
                    uiElement is IPointerClickReleaseHandler pointerClickReleaseHandler)
                {
                    ReleaseDrag();
                    pointerClickReleaseHandler.OnPointerClickRelease(Input.GetPressedMouseButton);
                }

            }else if (!uiElement.InteractiveRect.Contains(Input.MousePositionWithinViewport) && elementsUnderPointer.Contains(uiElement))
            {
                elementsUnderPointer.Remove(uiElement);
                if(uiElement is IPointerExitHandler pointerExitHandler)
                    pointerExitHandler.OnPointerExit();
            }
        }
        _clickHandled = false;
        ReleaseDrag();
    }

    private static void ReleaseDrag()
    {
        if (_dragHandler != null && !Input.LeftMouseHold && !Input.LeftMousePressed)
        {
            Debug.WriteLine("Drag cancel");
            _dragHandler.OnDragCancelled();
            _dragHandler = null;
        }
    }

    private static void DrawUI(float deltaTime, SpriteBatch batch)
    {
        foreach (var uiElement in AllUIElements)
        {
            if (uiElement.Texture != null)
            {
                batch.Draw(uiElement.Texture, uiElement.rect, null, uiElement.color, uiElement.rotation, Vector2.Zero,
                    SpriteEffects.None, 0);
                uiElement.AfterDraw(deltaTime, batch);
            }
        }
    }

    /// <summary>
    /// Margin structure, clockwise order
    /// </summary>
    public struct Margin
    {
        public int Top;
        public int Right;
        public int Bottom;
        public int Left;

        public Rectangle GetRect => new Rectangle(Left, Top, Resolution.gameSize.X - (Right + Left),
            Resolution.gameSize.Y - (Top + Bottom));
        
        public Margin(int top, int right, int bottom, int left)
        {
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.Left = left;
        }

        public Margin(int horizontal, int vertical)
        {
            Left = horizontal;
            Right = horizontal;
            Top = vertical;
            Bottom = vertical;
        }

        public Margin(int value)
        {
            Top = value;
            Right = value;
            Bottom = value;
            Left = value;
        }
    }
}