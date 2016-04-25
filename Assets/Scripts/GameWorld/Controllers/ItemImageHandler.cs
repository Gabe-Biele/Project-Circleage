using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ItemImageHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IDropHandler
{
    bool isHovered;
    public Item thisItem;
    public GameUI ourUI;
    public GameObject hoverbox;
    bool hoverboxExists;

    public void OnDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {

        hoverboxExists = false;
        isHovered = false;

	}
	
	// Update is called once per frame
	void Update () {
	
        if(isHovered&&!hoverboxExists)
        {
            hoverbox = ourUI.drawHoverBox(thisItem);
            hoverboxExists = true;
        }
        else if(isHovered&&hoverboxExists)
        {
            hoverbox.transform.position = Input.mousePosition + new Vector3(-95, 47);
        }
        if(!isHovered)
        {
            Destroy(hoverbox);
            hoverboxExists = false;
        }

	}
}
