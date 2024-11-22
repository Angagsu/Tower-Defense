using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PointerOverUI : MonoBehaviour
{
    public static PointerOverUI Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }

    public  bool IsPointerOverUIObject(Vector2 touchPosition)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(touchPosition.x, touchPosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.layer == 5) //5 = UI layer
            {
                return true;
            }
        }

        return false;
    }
}
