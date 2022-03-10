using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuPanel : MonoBehaviour
{
    public TMP_InputField shelfName, shelfRow, shelfColumn;
    //TMP_Text callingButtonText;
    public GameObject shelfButton;
    //public List<> shelfButton;
    public GameObject shelfContainer;
    public Sprite defSS;
    public Image ssImage;
    public static ShelfButton curShelf;
    List<ShelfButton> shelfButtons =  new List<ShelfButton>();
    public GameObject Panel;
    public LibraryManager libraryManager;

    // Start is called before the first frame update
    void Start()
    {
        LoadShelfButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadShelfButtons()
    {
        foreach (var shelf in databasetest.GetShelves())
        {
            shelfButtons.Add(CreateShelf(shelf.Item1, shelf.Item2));
        }
        if (shelfButtons.Count > 0)
        {
            curShelf = shelfButtons[0];
            ViewAllShelfButton(curShelf);
        }
    }

    public void ViewShelf(ShelfButton callingButton, Vector2Int callingButtonRowCol)
    {
        if (callingButton != null)
        {
            curShelf = callingButton;
            //this.callingButtonText = callingButtonText;
            shelfName.text = callingButton.myName;
            shelfRow.text = callingButtonRowCol.x.ToString();
            shelfColumn.text = callingButtonRowCol.y.ToString();
        }
        else
            shelfName.text = null;
    }
    
    public void ViewSS(Sprite ss)
    {
        if(ss != null)
            this.ssImage.sprite = ss;
        else
            this.ssImage.sprite = defSS;
    }

    public bool PassesNameChecks() 
    {
        return curShelf != null && shelfName.text.Length > 0 && ShelfNameUnique(shelfName.text);
    }

    public void RenameShelf()
    {
        if(curShelf.exists && PassesNameChecks() && !curShelf.myName.Equals(shelfName.text))
        {
            databasetest.UpdateTableTimeName(shelfName.text, curShelf.myName);
            curShelf.SetMyText(shelfName.text);
        }
        else
            Debug.Log("no calling button or text not changed or name already taken");
    }
    public ShelfButton CreateShelf(string shelfName, Vector2Int rowCol)
    {
        if (shelfContainer != null)
        {
            GameObject shelfButtonObj = GameObject.Instantiate(shelfButton, shelfContainer.transform);
            ShelfButton tempNewShelf;
            if (tempNewShelf = shelfButtonObj.GetComponent<ShelfButton>())
            {
                shelfButtons.Add(tempNewShelf);
                tempNewShelf.SetMyMMP(this);
                tempNewShelf.SetMyText(shelfName);
                tempNewShelf.SetMyRowCol(rowCol);
                return tempNewShelf;
            }
            else
                Debug.Log("no name ui text for the shelf found");
        }
        else
            Debug.Log("no container found");
        return null;
    }
    public void CreateShelf()
    {
        if (shelfContainer != null)
        {
            GameObject shelfButtonObj = GameObject.Instantiate(shelfButton, shelfContainer.transform);
            ShelfButton tempNewShelf;
            if (tempNewShelf = shelfButtonObj.GetComponent<ShelfButton>())
            {
                shelfButtons.Add(tempNewShelf);
                tempNewShelf.SetMyMMP(this);
                //ViewShelf(tempNewShelf.GetMyText(), tempNewShelf.rowCol);
                ViewShelf(null, tempNewShelf.rowCol);
                ViewSS(tempNewShelf.GetMySS());
                tempNewShelf.exists = false;
                curShelf = tempNewShelf;
            }
            else
                Debug.Log("no name ui text for the shelf found");
        }
        else
            Debug.Log("no container found");
    }

    public bool ShelfNameUnique(ShelfButton tempNewShelf)
    {
        foreach (ShelfButton shelfButton in shelfButtons)
            if (!shelfButton.Equals(tempNewShelf) && shelfButton.myName.Equals(tempNewShelf.myName))
                return false;
        return true;
    }

    public bool ShelfNameUnique(string tempNewShelfName)
    {
        foreach (ShelfButton shelfButton in shelfButtons)
            if (!shelfButton.Equals(curShelf) && shelfButton.myName.Equals(tempNewShelfName))
                return false;
        return true;
    }

    public void LoadShelf()
    {
        //if (shelfName.text != null && ShelfNameUnique(shelfName.text))
        if (PassesNameChecks())
        {
            if(curShelf.exists)
                RenameShelf();
            curShelf.myName = shelfName.text;
            Vector2Int newRowCol = new Vector2Int(int.Parse(shelfRow.text), int.Parse(shelfColumn.text));
            databasetest.CreateLibraryTable(curShelf.myName, newRowCol);
            LibraryManager.curShelfName = curShelf.myName;
            LibraryManager.curShelfRowCol = newRowCol;
            Debug.Log($"LibraryManager.curShelfName : {LibraryManager.curShelfName}");
            Panel.SetActive(true);
            this.gameObject.SetActive(false);
            libraryManager.LoadLib();
        }
        else Debug.Log("TextBox is null or Name is already taken!");
    }
    public void ViewAllShelfButton(ShelfButton shelfButton)
    {
        ViewShelf(shelfButton, shelfButton.rowCol);
        ViewSS(shelfButton.GetMySS());
    }
    public void DeleteShelf()
    {
        if (curShelf != null)
        {
            DestroyShelfButton(curShelf.gameObject);
        }
        else
            Debug.Log("no calling button");
    }
    void DestroyShelfButton(GameObject shelfButton)
    {
        Destroy(shelfButton);
        ViewShelf(null, new Vector2Int(2,2));
        ViewSS(null);
    }
    public void ResetBookList()
    {
        databasetest.DropTablesTimeSchema();
        databasetest.CreateSchemaTableTime();
        List<string> schemas = databasetest.GetSchemaList();
        foreach (string schema in schemas)
        {
            DebugText.outText += $"schemaName: {schema}\n";
            databasetest.InsertNewTableTime(schema);
        }
        foreach(var shelfButton in shelfButtons)
        {
            Destroy(shelfButton.gameObject);
        }
        shelfButtons.Clear();
        curShelf = null;
        LoadShelfButtons();
    }
}
