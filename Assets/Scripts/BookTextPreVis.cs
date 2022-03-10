using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BookTextPreVis : MonoBehaviour
{
    public TMP_Text titleText;
    public void SetText(TMP_InputField titleText)
    {
        if (titleText.text != null)
            this.titleText.text = titleText.text;
        else
            this.titleText.text = "Title";
    }
}
