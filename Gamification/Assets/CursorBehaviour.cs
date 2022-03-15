using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorBehaviour : MonoBehaviour
{
    public Texture2D defaultTexture;
    public Texture2D hoverTexture;
    public Texture2D grabHoverTexture;
    public Texture2D grabClickTexture;
    public CursorMode curMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    private void Start()
    {
        Cursor.SetCursor(defaultTexture, hotSpot, curMode);
    }
    public void OnMouseEnter()
    {
        Cursor.SetCursor(hoverTexture, hotSpot, curMode);
    }
    public void OnMouseExit()
    {
        Cursor.SetCursor(defaultTexture, hotSpot, curMode);
    }
    public void OnMouseEnterGrabHover()
    {
        Cursor.SetCursor(grabHoverTexture, hotSpot, curMode);
    }

    public void OnMouseEnterGrabClick()
    {
        Cursor.SetCursor(grabClickTexture, hotSpot, curMode);
    }

    public void OnMouseEnterGrabRelease()
    {
        Cursor.SetCursor(grabHoverTexture, hotSpot, curMode);
    }
}
