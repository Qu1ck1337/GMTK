using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("Ondrop");
        if (eventData.pointerDrag != null)
        {
            RectTransform dice = eventData.pointerDrag.GetComponent<RectTransform>();
            dice.SetParent(transform);
            dice.anchoredPosition = new Vector2(0f, 0f);
        }
    }
}
