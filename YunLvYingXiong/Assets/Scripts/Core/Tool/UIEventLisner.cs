//===================================================
//作    者：周连康 
//创建时间：2018-12-06 08:37:07
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void PointerClickHandler(PointerEventData obj);

public class UIEventLisner : MonoBehaviour, IPointerClickHandler {

    public event PointerClickHandler PointerClickHandler;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PointerClickHandler != null)
        {
            PointerClickHandler(eventData);
        }
    }
}
