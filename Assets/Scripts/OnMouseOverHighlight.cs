using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnMouseOverHighlight : MonoBehaviour
{
    Outline _outline;

    private TextMeshProUGUI _text;
    private float t_OriginalSize;

    public GameObject[] toEnable;

    void Start()
    {
        _outline = transform.GetComponent<Outline>();
        _text = transform.GetComponentInChildren<TextMeshProUGUI>();
        t_OriginalSize = _text.fontSize;
    }

    void OnMouseOver()
    {
        // Change the color of the GameObject to red when the mouse is over GameObject
        _outline.enabled = true;
        _text.fontSize = t_OriginalSize*1.1f;
        _text.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-35f);
        _text.enableWordWrapping = false;
        _text.overflowMode = TextOverflowModes.Overflow;

        foreach (GameObject obj in toEnable)
        {
            obj.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        // Reset the color of the GameObject back to normal
        _outline.enabled = false;
        _text.fontSize = t_OriginalSize;
        _text.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
        _text.enableWordWrapping = true;
        _text.overflowMode = TextOverflowModes.Ellipsis;
        
        foreach (GameObject obj in toEnable)
        {
            obj.SetActive(false);
        }
    }
}
