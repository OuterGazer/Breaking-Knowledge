using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAffordance : MonoBehaviour
{
    public void OnMouseEnter()
    {
        this.gameObject.GetComponent<CanvasRenderer>().SetColor(Color.yellow);
    }

    public void OnMouseExit()
    {
        this.gameObject.GetComponent<CanvasRenderer>().SetColor(Color.white);
    }

    public void OnMouseDown()
    {
        this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.90f, 0.90f, 0.90f);
    }

    public void OnMouseUp()
    {
        this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}
