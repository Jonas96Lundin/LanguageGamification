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

	private bool isGrabbing;

	private void Start()
	{
		Cursor.SetCursor(defaultTexture, hotSpot, curMode);
	}
	public void OnMouseEnter()
	{
		if (!isGrabbing)
			Cursor.SetCursor(hoverTexture, hotSpot, curMode);
	}
	public void OnMouseExit()
	{
		if (!isGrabbing)
			Cursor.SetCursor(defaultTexture, hotSpot, curMode);
	}
	public void OnMouseEnterGrabHover()
	{
		if (!isGrabbing)
			Cursor.SetCursor(grabHoverTexture, hotSpot, curMode);
	}

	public void OnMouseEnterGrabClick()
	{
		if (!isGrabbing)
		{
			isGrabbing = true;
			Cursor.SetCursor(grabClickTexture, hotSpot, curMode);
		}
	}

	public void OnMouseEnterGrabRelease()
	{
		if (isGrabbing)
		{
			isGrabbing = false;
			Cursor.SetCursor(grabHoverTexture, hotSpot, curMode);
		}
	}
}
