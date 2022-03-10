using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BookHandler : MonoBehaviour
{
    public  Rigidbody2D rb2d;
    public float firstImpulse;
    public Vector3 offset;
    public Transform parentTransf;
    public Transform myTransf;
    //private Transform parentTransf;
    public  float xDist, yDist;
    public SpriteRenderer sprtRndr;
    public TextMeshProUGUI text; 
    public Book book;
    public BoxCollider2D WallLeft, WallRight, FloorTop, FloorBottom;
    public BoxCollider2D bc2d;
    public LayerMask casingLayer;
    public Vector2 bottomOfBook = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //bc2d = GetComponent<BoxCollider2D>();
        rb2d.AddForce(new Vector2(firstImpulse, 0), ForceMode2D.Impulse);
        //parentTransf = GetComponentInParent<Transform>();
        xDist = parentTransf.transform.position.x - transform.position.x;
        yDist = parentTransf.transform.position.y - transform.position.y;
        book = GetComponentInParent<Book>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        //text.SetText(book.Title);
        //sprtRndr= GetComponent<SpriteRenderer>();
    }

    public string SetBookColor()
    {
        Color tempColor = new Color(Random.Range(0.3f, 1f), Random.Range(0.3f, 1f), Random.Range(0.3f, 1f));
        sprtRndr.color = tempColor;
        return "Red: " + tempColor.r + "Green: " + tempColor.g + "Blue: " + tempColor.b;
    }

    void OnMouseDown()
    {
        //if (LibraryManager.incCam && (!EventSystem.current.is))
        if (LibraryManager.incCam && (!EventSystem.current.IsPointerOverGameObject()))
        {
            //offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset = transform.position - LibraryManager.curCam.ScreenToWorldPoint(Input.mousePosition);
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            rb2d.useFullKinematicContacts = true;
            book.SetPanelToBookInfo();
            //ResetParent();
        }
    }

    public void ResizeWidth()
    {
        LibraryManager.curcCam.spaceleft -= (book.widthSlider.value - book.width);
        book.width = book.widthSlider.value;
        parentTransf.localScale = new Vector2(book.width, book.height);
    }
    
    public void ResizeHeight()
    {
        book.height =  book.heightSlider.value;
        parentTransf.localScale = new Vector2(book.width, book.height);
    }

    public void ResetParent()
    {
        Debug.Log("Parent Reset");
        float curXDist = parentTransf.transform.position.x - transform.position.x;
        float curYDist = parentTransf.transform.position.y - transform.position.y;
        if(Mathf.Abs(curXDist - xDist) > 0 || Mathf.Abs(curYDist - yDist) > 0)
        {
            parentTransf.position = new Vector3(transform.position.x + xDist, transform.position.y + yDist,
            parentTransf.transform.position.z);
        }
    }

    void OnMouseDrag()
    {
        if (LibraryManager.incCam && (!EventSystem.current.IsPointerOverGameObject()))
        {
            //Debug.Log($"WallLeft.bounds.max: {WallLeft.bounds.max}");
            //Debug.Log($"WallLeft.bounds.max.x: {WallLeft.bounds.max.x}");
            //Debug.Log($"WallRight.bounds.min: {WallRight.bounds.min}");
            //Debug.Log($"WallRight.bounds.min.x: {WallRight.bounds.min.x}");
            //transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            //var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            var mousePos = LibraryManager.curCam.ScreenToWorldPoint(Input.mousePosition) + offset;
            transform.position = ClampBookPositions(mousePos.x, mousePos.y);
            //Debug.Log($"transform.position: {transform.position}");
            //Mathf.Clamp(transform.position.x, WallLeft.bounds.max.x, WallRight.bounds.min.x);
            //Mathf.Clamp(transform.position.y, FloorBottom.bounds.max.y, FloorTop.bounds.min.y);
            //Mathf.Clamp(bc2d.bounds.max.x, WallLeft.bounds.max.x, WallRight.bounds.min.x);
            //Mathf.Clamp(bc2d.bounds.min.y, FloorBottom.bounds.max.y, FloorTop.bounds.min.y);

            //Mathf.Clamp(bc2d.bounds.min.x, WallLeft.bounds.max.x, WallRight.bounds.min.x);
            //Mathf.Clamp(bc2d.bounds.max.x, WallLeft.bounds.max.x, WallRight.bounds.min.x);
            //Mathf.Clamp(bc2d.bounds.min.y, FloorBottom.bounds.max.y, FloorTop.bounds.min.y);
            //Mathf.Clamp(bc2d.bounds.max.y, FloorBottom.bounds.max.y, FloorTop.bounds.min.y);
        }
    }

    public Vector2 ClampBookPositions(float posX, float posY)
    {
        return new Vector2(Mathf.Clamp(posX, 0.01f + WallLeft.bounds.max.x + bc2d.bounds.extents.x, WallRight.bounds.min.x - 0.01f - bc2d.bounds.extents.x),
            Mathf.Clamp(posY, 0.01f + FloorBottom.bounds.max.y + bc2d.bounds.extents.y, FloorTop.bounds.min.y - 0.01f - bc2d.bounds.extents.y));
    }

    private void OnMouseUpAsButton()
    {
        rb2d.bodyType = RigidbodyType2D.Dynamic;
    }
    
    private void OnMouseUp()
    {
        rb2d.bodyType = RigidbodyType2D.Dynamic;
    }
    private void Update()
    {
        //bottomOfBook.x = transform.position.x;
        //bottomOfBook.y = transform.position.y - bc2d.bounds.extents.y;
        //Debug.DrawLine(bottomOfBook, new Vector2(bottomOfBook.x, bottomOfBook.y - 1f), Color.red);
    }
    private void OnCollisionEnter2D()
    {
        bottomOfBook.x = transform.position.x;
        bottomOfBook.y = transform.position.y - bc2d.bounds.extents.y;
        ///RaycastHit2D raycastHit2D = new RaycastHit2D();
        ///Debug.Log($"BottomOfThe BookCoords {bottomOfBook}");
        ///Debug.Log($"myTransf.position.x: {myTransf.position.x}");
        ///Debug.Log($"parentTransf.position.x: {parentTransf.position.x}");
        ///Debug.Log($"transform.position.x: {transform.position.x}");
        ///Debug.Log($"book.posX: {book.posX}");
        ///Debug.Log($"myTransf.position.x - book.posX: {myTransf.position.x - book.posX}");
        if (Physics2D.Raycast(bottomOfBook, Vector2.down, .25f, casingLayer) 
            && Mathf.Abs(myTransf.position.x - book.posX) > .05f)
        {
            book.posX = myTransf.position.x;
            book.posY = myTransf.position.y;
            book.SaveAndUpdateBookPos(myTransf.position.x, myTransf.position.y);
        }
        //ResetParent();
    }

    public void SetBorders(BoxCollider2D WallLeft, BoxCollider2D WallRight, BoxCollider2D FloorTop, BoxCollider2D FloorBottom)
    {
        this.WallLeft = WallLeft;
        this.WallRight = WallRight;
        this.FloorTop = FloorTop;
        this.FloorBottom = FloorBottom;
    }
}
