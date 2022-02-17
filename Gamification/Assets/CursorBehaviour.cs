using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehaviour : MonoBehaviour
{
    public Texture2D defaultTexture;
    public Texture2D hoverTexture;
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
}
