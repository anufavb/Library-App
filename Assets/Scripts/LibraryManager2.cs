using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class LibraryManager : MonoBehaviour
{
    public CubixCam[,] cCams;
    public TMP_InputField titleTextBox, isbnTextBox, subTitleTextBox;
    private string title, subTitle, isbn;
    public int row, col; 
    public float posX, posY;
    public static CubixCam curcCam;
    public static Camera curCam;
    //public RectTransform bookPreview;

    public void AddBook()
    {
        if (titleTextBox.text.Length > 0 && subTitleTextBox.text.Length > 0 && isbnTextBox.text.Length > 0)
        {
            title = titleTextBox.text;
            subTitle = subTitleTextBox.text;
            isbn = isbnTextBox.text;
            if (curcCam && curcCam.wait)
            {
                curcCam.SpawnBookAtClick();
            }
            else if (curcCam)
            {
                //float spaceleft;
                //if ((spaceleft = maxXScale - bookLengths[curcCam.rowCol.x, curcCam.rowCol.y]) > minBookWidth)
                curcCam.spaceleft = maxXScale - bookLengths[curcCam.rowCol.x, curcCam.rowCol.y];
                if (curcCam.spaceleft > minBookWidth)
                {
                    Debug.Log($"spaceleft: {curcCam.spaceleft}");
                    SetWidthSliderMinMax(curcCam.spaceleft);
                    curcCam.wait = true;
                }
                else Debug.Log($"No more space here!");
            }
            else
            {
                EnterSelectionMode();
            }
        }
        else
            Debug.Log("Fill in all input fields first before you hit \"Add\"");
    }

    public void AddBookRowColPos(int row, int column, float posX, float posY)
    {
        GameObject gO = new GameObject();
        Book book = gO.AddComponent<Book>();
        book.SetBook(databasetest.InsertBook(title, isbn, subTitle, null, 0, null, null, row, column, posX, posY, widthSlider.value, heightSlider.value),
            title, isbn, subTitle,
            null, 0, null, null, row, column, posX, posY, widthSlider.value, heightSlider.value);
        //Vector2 clampPos = book.bookHandler.ClampBookPositions(posX, posY);
        //book.SetBookXY(clampPos.x, clampPos.y);
        ///this.row = row;
        ///this.col = column;
        ///this.posX = posX;
        ///this.posY = posY;
        //databasetest.;
        MakeBook(2f, book);
        Destroy(gO);
        ResetHeightSlider();
    }

    public void EnterSelectionMode()
    {
        foreach (var cCam in cCams)
            cCam.EnterSelectionMode();
    }

    public void ExitSelectionMode()
    {
        foreach (var cCam in cCams)
            cCam.ExitSelectionMode();
    }
}
