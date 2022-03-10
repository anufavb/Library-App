using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookPreview : MonoBehaviour
{
    public RectTransform bookPreview;
    public Slider heightSlider;
    public float initialHeight;
    void Start()
    {
        initialHeight = bookPreview.rect.width;
    }

    public void SetPreviewHeight()
    {
        bookPreview.sizeDelta = new Vector2(initialHeight * heightSlider.value, bookPreview.rect.height);//;
    //    if (LibraryManager.curBook != null)
    //        LibraryManager.curBook.bookHandler.ResizeHeight();
    }
    ///public void SetPreviewWidth()
    ///{
    ///    bookPreview.sizeDelta = new Vector2(initialHeight * wi.value, bookPreview.rect.height);//;
    ///    if (LibraryManager.curBook != null)
    ///        LibraryManager.curBook.bookHandler.ResizeWidth();
    ///}
}
