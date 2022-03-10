using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShelfButton : MonoBehaviour
{
    public TMP_Text myNameTMPText;
    public string myName;
    public Vector2Int rowCol = new Vector2Int(2,2);
    MainMenuPanel mmP;
    public Button button;
    public Sprite screenshot;
    public bool exists = true;

    // Start is called before the first frame update
    void Start()
    {
        myName = myNameTMPText.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMyMMP(MainMenuPanel mmP)
    {
        this.mmP = mmP;
    }
    
    public TMP_Text GetMyText()
    {
        return myNameTMPText;
    }
     
    public bool SetMyText(string newText)
    {
        myNameTMPText.text = newText;
        myName = newText;
        return true;
    }
      
    public bool SetMyRowCol(Vector2Int rowCol)
    {
        this.rowCol = rowCol;
        return true;
    }
    
    public Sprite GetMySS()
    {
        return screenshot;
    }

    public void PutMyNameOutThere()
    {
        if(myNameTMPText != null)
        {
            mmP.ViewShelf(this, rowCol);
            //MainMenuPanel.curShelf = this;
        }
    }

    public void PutMyPhotoOutThere()
    {
        //if(screenshot != null)
            mmP.ViewSS(screenshot);
    }
}
