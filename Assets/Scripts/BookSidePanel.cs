using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using TMPro;

public class BookSidePanel : MonoBehaviour
{
    public Animator anim;
    public TMP_InputField titleField;
    public TMP_InputField subTitleField;
    public TMP_InputField isbnField;
    void Awake()
    {
        //anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"AnimBool-Open: {anim.GetBool("Open")}");
        anim.SetBool("Open", false);
        Debug.Log($"AnimBool-Open: {anim.GetBool("Open")}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPanelState()
    {
        bool curState = anim.GetBool("Open");
        Debug.Log($"SetPanel Current State: {curState}");
        anim.SetBool("Open", !curState);
        //anim.SetTrigger("Close");
    }

    void PickLocation()
    {

    }

    void InsertBook()
    {
        //databasetest.InsertBook(titleField.text, subTitleField.text, null, null, null, null, , , , 0.0f);
    }
}
