using System.Collections.Generic;
using System.Linq;
using CanvasManagement;
using MainGameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReFactory.Debugger;
namespace ReFactory.UISystem;

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
        CanvasLayer.UI.GetCanvas().OnDraw += DrawUI;
    }

    private static void Update(float deltaTime)
    {
        if(!MainGame.IsFocused) return;
        
        foreach (var uiElement in AllUIElements.OfType<IHasInteractiveRect>().Concat(AdditionalInteractiveElements))
        {
            if (uiElement.InteractiveRect.Contains(CanvasLayer.UI.GetCanvas().MousePosition))
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
                            Debug.Log("Drag init");
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
                            Debug.Log("Drag & Drop executed");
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

            }else if (!uiElement.InteractiveRect.Contains(CanvasLayer.UI.GetCanvas().MousePosition) && elementsUnderPointer.Contains(uiElement))
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
            Debug.Log("Drag cancel");
            _dragHandler.OnDragCancelled();
            _dragHandler = null;
        }
    }

    private static void DrawUI(SpriteBatch spriteBatch, Canvas canvas)
    {
        foreach (var uiElement in AllUIElements)
        {
            if (uiElement.Texture != null)
            {
                spriteBatch.Draw(uiElement.Texture, uiElement.rect, null, uiElement.color, uiElement.rotation, Vector2.Zero,
                    SpriteEffects.None, 0);
                uiElement.AfterDraw(spriteBatch);
            }
        }
    }
}