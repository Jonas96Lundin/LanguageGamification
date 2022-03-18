using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class CargoController : MonoBehaviour
{
	EventTrigger eventTrigger;
	CursorBehaviour cursorBehaviour;

	CargoPosition cargoPosition;
	CargoPosition newCargoPosition = null;

	TrainPlayerController player;
	private bool isGrabbed;
	[SerializeField] Color grabColor;

	private void Start()
	{
		cargoPosition = GetComponentInParent<CargoPosition>();
		cargoPosition.HasCargo = true;
		player = FindObjectOfType<TrainPlayerController>();
		AddCursorBehaviour();
	}

	private void AddCursorBehaviour()
	{
		cursorBehaviour = FindObjectOfType<CursorBehaviour>();
		eventTrigger = GetComponent<EventTrigger>();

		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener((data) => { OnMouseEnterHover(); });
		eventTrigger.triggers.Add(entry);

		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerExit;
		entry.callback.AddListener((data) => { OnMouseExitHover(); });
		eventTrigger.triggers.Add(entry);

		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener((data) => { OnGrabCargo((PointerEventData)data); });
		eventTrigger.triggers.Add(entry);

		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerUp;
		entry.callback.AddListener((data) => { OnReleaseCargo((PointerEventData)data); });
		eventTrigger.triggers.Add(entry);
	}

	private void OnMouseEnterHover()
	{
		if (!player.IsGrabbingCargo)
			cursorBehaviour.OnMouseEnterGrabHover();
	}

	private void OnMouseExitHover()
	{
		if (!player.IsGrabbingCargo)
			cursorBehaviour.OnMouseExit();
	}

	private void OnGrabCargo(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			cursorBehaviour.OnMouseEnterGrabClick();
			transform.SetParent(player.transform);
			player.IsGrabbingCargo = true;
			isGrabbed = true;
		}
	}

	private void OnReleaseCargo(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (newCargoPosition)
			{
				cargoPosition.HasCargo = false;

				cargoPosition = newCargoPosition;
				cargoPosition.HasCargo = true;
				newCargoPosition = null;
			}
			transform.SetParent(cargoPosition.transform);
			cursorBehaviour.OnMouseEnterGrabRelease();
			GetComponent<Image>().color = Color.white;
			player.IsGrabbingCargo = false;
			isGrabbed = false;
			transform.DOMove(cargoPosition.transform.position, 1);
		}
	}

	private void Update()
	{
		if (isGrabbed)
		{
			transform.position = Input.mousePosition;
		}
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (isGrabbed)
		{
			if (collision.collider.tag == "CargoPosition")
			{
				if (!collision.collider.GetComponent<CargoPosition>().HasCargo)
				{
					GetComponent<Image>().color = grabColor;
					newCargoPosition = collision.collider.GetComponent<CargoPosition>();
				}
			}
		}
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (isGrabbed)
		{
			if (collision.collider.tag == "CargoPosition" && newCargoPosition == collision.collider.GetComponent<CargoPosition>())
			{
				GetComponent<Image>().color = Color.white;
				newCargoPosition = null;
			}
		}	
	}
}
