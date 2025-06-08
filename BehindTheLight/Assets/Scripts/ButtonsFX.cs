using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonsFX : MonoBehaviour
{
    private TMP_Text buttonText;

    private void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
    }

    public void OnMouseEnter()
    {
        buttonText.color = Color.white;
    }

    public void OnMouseExit()
    {
        buttonText.color = Color.gray;
    }
}
