using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Book : MonoBehaviour
{
    public int ID;
    public string Title;
    public string ISBN;
    public string Subtitles;
    public string Edition;
    public int Pages;
    public string Section;
    public string Author;
    public int row;
    public int column;
    public float posX;
    public float posY;
    public float width;
    public float height = 1;
    public TMP_InputField titleTextBox;
    public TMP_InputField subTitleTextBox;
    public TMP_InputField isbnTextBox;
    public Canvas canvas;
    public Slider widthSlider;
    public Slider heightSlider;
    public BookHandler bookHandler;
    public bool isDirty = false;
    public bool isRelevant = true;

    public void SetBook(int ID, string Title, string ISBN, string Subtitles, string Edition, int Pages, string Section, 
        string Author, int row, int column, float posX, float posY, float width, float height)
    {
        this.ID = ID;
        this.Title = Title;
        this.ISBN = ISBN;
        this.Subtitles = Subtitles;
        this.Edition = Edition;
        this.Pages = Pages;
        this.Author = Author;
        this.row = row;
        this.column = column;
        this.posX = posX;
        this.posY = posY;
        this.width = width;
        this.height = height;
    }
    public void SetBook(int ID, string Title, string ISBN, string Subtitles, string Edition, int Pages, string Section, 
        string Author, int row, int column, float width, float height)
    {
        this.ID = ID;
        this.Title = Title;
        this.ISBN = ISBN;
        this.Subtitles = Subtitles;
        this.Edition = Edition;
        this.Pages = Pages;
        this.Author = Author;
        this.row = row;
        this.column = column;
        this.width = width;
        this.height = height;
    }
    public void SetBookXY(float posX, float posY)
    {
        this.posX = posX;
        this.posY = posY;
    }
    public void SetBook(Book book)
    {
        this.ID = book.ID;
        this.Title = book.Title;
        this.ISBN = book.ISBN;
        this.Subtitles = book.Subtitles;
        this.Edition = book.Edition;
        this.Pages = book.Pages;
        this.Author = book.Author;
        this.row = book.row;
        this.column = book.column;
        this.posX = book.posX;
        this.posY = book.posY;
        this.width = book.width;
        this.height = book.height;
    }

    public void SetPanelRefs(TMP_InputField titleTextBox, TMP_InputField subTitleTextBox, TMP_InputField isbnTextBox, 
        Slider widthSlider, Slider heightSlider)
    {
        this.titleTextBox = titleTextBox;
        this.subTitleTextBox = subTitleTextBox;
        this.isbnTextBox = isbnTextBox;
        this.widthSlider = widthSlider;
        this.heightSlider = heightSlider;
    }

    public void EventTriggerTest()
    {
        Debug.Log("Please.. Work FFS");
    }

    public void SetEventCamera(Camera eventCam)
    {
        canvas.worldCamera = eventCam;
    }

    public void SetPanelToBookInfo()
    {
        //Debug.Log($"BookPosInWorldSpace {transform.position}");
        //Debug.Log($"BookPosInLocalSpace {transform.localPosition}");
        Debug.Log($"Setting titleTextBox: {Title}, subTitleTextBox: {subTitleTextBox}, authorTextBox: {isbnTextBox}");
        LibraryManager.curBook = this;
        titleTextBox.text = Title;
        subTitleTextBox.text = Subtitles;
        isbnTextBox.text = ISBN;
        heightSlider.value = height;
        LibraryManager.curcCam.SetWidthSliderForBook(width);
    }
    public void SaveAndUpdateBookPos(float posX, float posY)
    {
        databasetest.UpdateBookPos(ID, posX, posY);
    }
    public void SaveAndUpdateBook()
    {   
        Debug.Log($"Saving: {Title}...");
        //if (LibraryManager.curBook == this)
        //{
            if (heightSlider.value != height)
                isDirty = true;
            else if (!titleTextBox.text.Equals(Title))
                isDirty = true;
            else if (!isbnTextBox.text.Equals(ISBN))
                isDirty = true;
            else if (!subTitleTextBox.text.Equals(Subtitles))
                isDirty = true;

            Debug.Log($"isDirty? {isDirty}...");
            if (isDirty)
                databasetest.UpdateBook(ID, titleTextBox.text, isbnTextBox.text, subTitleTextBox.text, Edition, Pages, Section, null,
                                        row, column, transform.position.x, transform.position.y, width, heightSlider.value);
        //}
    }
}
