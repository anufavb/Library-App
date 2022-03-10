using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class LibraryManager : MonoBehaviour
{
    public TMP_InputField searchField;

    public void SearchBooks()
    {
        List<int> searchResultList = databasetest.SearchBooks(searchField.text, false);
        foreach (var book in bookList)
        {
            if (searchResultList.Contains(book.ID))
            {
                if (!book.isRelevant)
                {
                    Color color = book.bookHandler.sprtRndr.material.color;
                    color.a = 1;
                    book.bookHandler.sprtRndr.material.color = color;
                    book.isRelevant = true;
                }
            }
            else
            {
                if (book.isRelevant)
                {
                    Color color = book.bookHandler.sprtRndr.material.color;
                    color.a = 50;
                    book.bookHandler.sprtRndr.material.color = color;
                    book.isRelevant = false;
                }

            }
        }
    }

    public List<int> searchResults()
    {
        return databasetest.SearchBooks(searchField.text, false);
    }

    public void SetPreviewHeight()
    {
        if (curBook != null)
            curBook.bookHandler.ResizeHeight();
    }
    public void SetPreviewWidth()
    {
        if (curBook != null)
            curBook.bookHandler.ResizeWidth();
    }
    public void ResetPanels()
    {
        titleTextBox.text = null;
        subTitleTextBox.text = null;
        isbnTextBox.text = null;
    }
}
