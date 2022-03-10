using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class LibraryManager : MonoBehaviour
{
    public int pubRowNo, pubColNo, noOfBooksPer;
    private int rowNo, colNo;
    private int rowNoPlus, colNoPlus;
    public GameObject rowSprite, colSprite, book;
    public List<BoxCollider2D> rowColliders = new List<BoxCollider2D>(), colColliders = new List<BoxCollider2D>();
    public Transform startPoint;
    private readonly float width = 16f, height = 9f, multiplierDef = 1.2f, multiplier = 1.2f;
    private float cubixWidth, cubixHeight, mainRatio;//, multiplier = 1.20f;
    public GameObject cubixCam;
    public Camera mainLobbyCamera;
    public static Camera mainLibCamera;
    public static bool incCam = false;
    //private databasetest databasetest;
    private GameObject[] bookObjs;
    public List<Book> bookList;
    public Camera[,] cams;
    [SerializeField]
    public GameObject blockingPanel;
    public float minBookHeight = 0.375f, minBookWidth = 0.75f;
    public Slider heightSlider, widthSlider;
    public float maxXScale, maxYScale;
    public static Book curBook;
    public static string curShelfName;
    public static Vector2Int curShelfRowCol;
    public float[,] bookLengths;

    // Start is called before the first frame update
    ///void Start()
    ///{
    ///    curShelfName = "Testing Shelf";
    ///    LoadLib();
    ///}
    
    public void LoadLib()
    {
        pubRowNo = curShelfRowCol.x;
        pubColNo = curShelfRowCol.y;
        mainRatio = multiplier / multiplierDef;
        if (pubRowNo > 0 && pubColNo > 0)
        {
            bookLengths = new float[pubRowNo, pubColNo];
            cams = new Camera[pubRowNo, pubColNo];
            cCams = new CubixCam[pubRowNo, pubColNo];
            StartLib();
        }
        else Debug.Log("Error Number of Rows or Columns is 0");
        mainLobbyCamera.enabled = false;
    }

    public void SetWidthSliderMinMax(float max)
    {
        if (max > minBookWidth)
        {
            widthSlider.minValue = minBookWidth;
            widthSlider.maxValue = max;
            widthSlider.value = .75f * max;
        }
        else
        {
            widthSlider.maxValue = 0;
            widthSlider.maxValue = 0;
            widthSlider.value = 0;
        }
    }
    public void SetWidthSliderMinMax(bool value, float max)
    {
        if (max > minBookWidth)
        {
            widthSlider.minValue = minBookWidth;
            widthSlider.maxValue = max;
            if(value)
                widthSlider.value = .75f * max;
        }
        else
        {
            widthSlider.maxValue = 0;
            widthSlider.maxValue = 0;
            widthSlider.value = 0;
        }
    }
    
    public void SetWidthSliderDisabled()
    {
        widthSlider.interactable = false;
    }
    public void SetWidthSliderEnabled()
    {
        widthSlider.interactable = true;
    }
    public void SetHeightSliderMinMax(float min, float max)
    {
        heightSlider.minValue = min;
        heightSlider.maxValue = max;
        ResetHeightSlider();
    }
    public void ResetHeightSlider()
    {
        heightSlider.value = 1f;
    }

    void StartLib()
    {
        cubixWidth = width * multiplier;
        cubixHeight = height * multiplier;
        rowNo = pubColNo;
        colNo = pubRowNo;
        rowNoPlus = rowNo + 1;
        colNoPlus = colNo + 1;
        //float colSize=0, rowSize=0;
        float libRatio=0,lastColLen=0, lastRowLen=0;

        Vector3 rowInitialPos = startPoint.position;
        Vector3 colInitialPos = startPoint.position;

        GameObject libPar = new GameObject("Casing");
        libPar.transform.SetParent(startPoint);
        libPar.transform.position = startPoint.position;

        for (int i = 0; i < colNoPlus; i++)
        {
            GameObject rowObject = GameObject.Instantiate(rowSprite, 
                rowInitialPos, rowSprite.transform.rotation, libPar.transform);
            rowColliders.Add(rowObject.GetComponentInChildren<BoxCollider2D>());
            //Debug.Log("row:");
            PutInShape(rowObject, rowSprite, rowNo, cubixWidth /** multiplier*/);

            rowInitialPos.y -= cubixHeight - 1;
            if (i+1 == colNoPlus)
            {
                rowObject.transform.localScale += new Vector3(mainRatio, 0f, 0f);
                lastRowLen = rowObject.transform.localScale.x;
                //Debug.Log("row size: " + (lastRowLen));
            } 
            //rowSize =  lastRowLen;
            //Debug.Log("row size: " + rowSize);
            
            //Debug.Log(((colNo * cubixHeight) - colNo) / colNo);       
        }
        for (int i = 0; i < rowNoPlus; i++)
        {
            GameObject colObject = GameObject.Instantiate(colSprite, 
                colInitialPos, colSprite.transform.rotation, libPar.transform);
            colColliders.Add(colObject.GetComponentInChildren<BoxCollider2D>());
            //Debug.Log("column:");
            PutInShape(colObject, colSprite, colNo, cubixHeight /** multiplier*/);

            colInitialPos.x += cubixWidth - 1;
            if (i+1 == rowNoPlus)
            {
                colObject.transform.localScale += new Vector3(mainRatio, 0f, 0f);
                lastColLen = colObject.transform.localScale.x;
                //Debug.Log("col size: " + (lastColLen));
            }
            //colSize =  lastColLen;      
            //Debug.Log("col size: " + colSize);
            //Debug.Log(((rowNo * cubixWidth) - rowNo) / rowNo); 
        }

        SpawnCams();

        //libRatio = rowSize / colSize;
        libRatio = lastRowLen/ lastColLen;
        SpawnMainCam(libRatio, lastRowLen, lastColLen);

        SpawnBooks();
        SetHeightSliderMinMax(minBookHeight, maxYScale);
    }

    void PutInShape(GameObject objectIn, GameObject spriteObj, int objNo, float objLen)
    {
        //Debug.Log((objNo * objLen) - objNo);
        objectIn.transform.localScale *= 0;
        objectIn.transform.localScale += new Vector3((objNo * objLen) - objNo,
                    mainRatio, spriteObj.transform.localScale.z);

        //SpriteRenderer objectInChildSR = objectIn.GetComponentInChildren<SpriteRenderer>();
        //if (objectInChildSR.drawMode == SpriteDrawMode.Tiled)
        //{
        //    objectInChildSR.size = new Vector3((objNo * objLen) - objNo, mainRatio);
        //}
    }           

    private void SpawnCams()
    {
        float tempRatio = 0;//mainRatio;
        Vector3 camLocalPos = new Vector3(((cubixWidth + tempRatio) / 2), -((cubixHeight + tempRatio) / 2), -10f);
        
        GameObject camPar = new GameObject("CamHolder");
        camPar.transform.SetParent(startPoint);
        camPar.transform.position = startPoint.position;
        
        for (int i = 0; i < colNo; i++)
        {
            for (int j = 0; j < rowNo; j++)
            {  
                GameObject cubixCamObj = GameObject.Instantiate(cubixCam, camPar.transform);
                cubixCamObj.name = "CubixCam. Row: " + i + " Col: " + j;
                cubixCamObj.transform.localPosition = camLocalPos;
                Camera cam;
                if (cam = cubixCamObj.GetComponent<Camera>())
                {
                    cam.orthographicSize = cubixHeight/2;
                    cams[i, j] = cam;
                }
                camLocalPos.x += (((rowNo * cubixWidth) - rowNo) / rowNo);
                CubixCam cCam = cubixCamObj.GetComponent<CubixCam>();
                cCams[i, j] = cCam;
                //cCam.TurnOnLibBack();
                cCam.blockingPanel = blockingPanel;
                cCam.SetRowCol(i, j);
                cCam.SetLibraryManager(this);
            }
            camLocalPos.x = ((cubixWidth + tempRatio) / 2);
            camLocalPos.y -= (((colNo * cubixHeight) - colNo) / colNo);
        }
    }

    void SpawnMainCam(float libRatio, float lastRowLen, float lastColLen)
    {
        //Debug.Log("libRatio: " + libRatio);
        float orthoCamSize = 1.1f;
        if (libRatio != 0)
        {
            if (libRatio > 1 && pubRowNo < pubColNo)
            {
                orthoCamSize *= (lastRowLen * (height/width))/2;
                //Debug.Log("(1a) lasRowLen: " + lastRowLen + " height/width: " + (width/height) + "\n pubColNo/pubRowNo: " + (pubColNo / pubRowNo)
                //    + " pubColNo: " + pubColNo + " pubRowNo: " + pubRowNo);
                //Debug.Log("(1b) mainCam ortho size: " + orthoCamSize);
            }
            else
            {
                //orthoCamSize = (lastRowLen * (height / width) * (pubColNo / pubRowNo)) / 2;
                orthoCamSize *= lastColLen / 2;
                //Debug.Log("(2a) lastColLen: " + lastColLen);
                //Debug.Log("(2b) mainCam ortho size: " + orthoCamSize);
            }
        }
        else
        {
            Debug.LogError("There's an issue, libRatio is zero!");
        }
        GameObject cubixCamObj = GameObject.Instantiate(cubixCam, startPoint);
        Camera cam;
        if ((cam = cubixCamObj.GetComponent<Camera>()) && orthoCamSize != 0)
        {
            //Debug.Log("mainCam ortho size: "+orthoCamSize);
            cam.orthographicSize = orthoCamSize;
        }
        Collider2D c2d;
        if (c2d = cubixCamObj.GetComponent<Collider2D>())
        {
            c2d.enabled = false;
        }
        cubixCamObj.name = "Main Library Camera";
        Vector3 mainCamPos = new Vector3(lastRowLen/2, -lastColLen/2, -20f);
        cubixCamObj.transform.localPosition = mainCamPos;
        CubixCam cCam = cubixCamObj.GetComponent<CubixCam>();
        cCam.TurnOffBackButton();
        cCam.TurnOnWallPaper();
        cCam.blockingPanel = blockingPanel;
        cCam.SetCameraActive(true);
        mainLibCamera = cam;

    }

    public static void SetMainLibCamDepth(int newDepth)
    {
        mainLibCamera.depth = newDepth;
    }

    public static void SetMainCameraActive(bool active)
    {
        mainLibCamera.enabled = active;
    }

    public GameObject bookHoldPar;
    void SpawnBooks()
    {
        maxYScale = .95f * (rowColliders[0].bounds.min.y - rowColliders[1].bounds.max.y) / (book.GetComponent<Book>().bookHandler.myTransf.localScale.y);
        maxXScale = .95f * colColliders[1].bounds.min.x - colColliders[0].bounds.max.x;
        float heightSpacing = .4f, minSpacing = .3f, minBookWidth = 0.3f, maxBookWidth = 2f;
        float defBookLocPosX = rowSprite.transform.localScale.z + minSpacing;
        float defBookLocPosY = -(rowSprite.transform.localScale.y + heightSpacing);
        Vector2 bookLocalPos = new Vector2(defBookLocPosX, defBookLocPosY);
        GameObject bookPosTester = new GameObject("BookPosTester");
        bookPosTester.transform.SetParent(startPoint);
        bookPosTester.transform.position = startPoint.position;
        int noOfBooksPerToAttempt;
        float bookSpace = (cubixWidth - ((noOfBooksPer + 1) * minSpacing));                                                                                                                                                                                                                                                                                                                                                                
        float bookWidth = bookSpace / noOfBooksPer; 
        
        bookWidth = Mathf.Clamp(bookWidth, minBookWidth, maxBookWidth);
        noOfBooksPerToAttempt = noOfBooksPer;
        int maxNoOfBooksAttemptable = (int) (cubixWidth / (minBookWidth+minSpacing));
        
        noOfBooksPerToAttempt = Mathf.Clamp(noOfBooksPerToAttempt, noOfBooksPerToAttempt, maxNoOfBooksAttemptable);

        bookHoldPar = new GameObject("MainBookHolder");
        bookHoldPar.transform.SetParent(startPoint);
        bookHoldPar.transform.position = startPoint.position;
        //int bl = 0, btl = databasetest.GetBookLength();
        
        for (int i = 0; i < pubRowNo; i++)
        {
            for (int j = 0; j < pubColNo; j++)
            {
                List<Book> books = databasetest.GetBooksFromRowCol(i, j);
                noOfBooksPerToAttempt = books.Count;
                for (int k = 0; k < noOfBooksPerToAttempt; k++)
                {
                    ///GameObject bookPar = new GameObject("Book Holder. Row: " + i + " Col: " + j + " No: " + k);
                    ///bookPar.transform.SetParent(bookHoldPar.transform);
                    ///bookPar.transform.position = bookHoldPar.transform.position;

                    MakeBook(bookWidth, books[k]);

                    //Debug.Log("Book"+ "Row: " + i + " Col: " + j + "Color: " + bookColor);

                    //float bookWidth = bookObj.GetComponentInChildren<Transform>().localScale.x;

                    //Debug.Log("bookWidth: " + bookWidth);

                    bookLocalPos.x += minSpacing + bookWidth;
                    
                    //Debug.Log("bookLocalPos.x: " + bookLocalPos.x + ", bookLocalPos.y: " + bookLocalPos.y
                    //            + ", i: " + i + ", k: " + k);
                }

                float addToBookXPos = (cubixWidth * (j + 1)) - ((j+1) - rowSprite.transform.localScale.z);
                bookLocalPos.x = addToBookXPos + minSpacing;
                //Debug.Log("add to book X position: " + addToBookXPos);
            }
            bookLocalPos.x = defBookLocPosX;
            bookLocalPos.y = -(cubixHeight * (i+1)) - heightSpacing;
        }
        //dbt.SetBookList(bookList, bookList.Count);
    }
    public Book MakeBook(float width, Book booksk)
    {
        
        GameObject bookObj = GameObject.Instantiate(book, new Vector2(booksk.posX, booksk.posY), new Quaternion());

        //bookObj.GetComponentInChildren<Transform>().transform.localScale = new Vector2(width, 0.25f);//cubixHeight - (2 * heightSpacing));
        //bookObj.GetComponent<Transform>().transform.localScale = new Vector2(width, 0.25f);

        //bookObj.transform.localPosition = bookLocalPos;

        Book bookObjBook = bookObj.GetComponent<Book>();
        BookHandler bkHndlr = bookObjBook.bookHandler;
        bookObjBook.SetBook(booksk);

        bkHndlr.SetBorders(colColliders[booksk.column], colColliders[booksk.column + 1], rowColliders[booksk.row], rowColliders[booksk.row + 1]);
        bookObj.transform.localScale = new Vector2(
            Mathf.Clamp(booksk.width, minBookWidth, maxXScale-bookLengths[booksk.row, booksk.column]), 
            Mathf.Clamp(booksk.height, minBookHeight, maxYScale));
        bookLengths[booksk.row, booksk.column] += bookObj.transform.localScale.x;
        bookObj.transform.position = bkHndlr.ClampBookPositions(booksk.posX, booksk.posY);
        //float maxYScale = .95f * (bkHndlr.FloorTop.bounds.min.y - bkHndlr.FloorBottom.bounds.max.y) / (bkHndlr.bc2d.bounds.size.y);
        //bookObj.transform.localScale = new Vector2(width, Mathf.Clamp(bookObjBook.height, minBookHeight, maxYScale));
        //bookObj.GetComponent<Transform>().transform.localScale.Set(0.5f, 0.5f, 1f);
        ///(bookObj.transform.localScale.x, 
        ///Mathf.Clamp(bookObjBook.height, 0.25f, maxYScale*.95f), 
        ///bookObj.transform.localScale.z);
        string bookColor = bkHndlr.SetBookColor();
        bookObjBook.SetEventCamera(cams[booksk.row, booksk.column]);
        bookObjBook.SetPanelRefs(titleTextBox, subTitleTextBox, isbnTextBox, widthSlider, heightSlider);
        //bookList.Add(bookObj.GetComponent<Book>());
        bookObj.transform.SetParent(bookHoldPar.transform);
        bookList.Add(bookObjBook);
        return bookObjBook;
    }

    public void SaveAndUpdateAllBooks()
    {
        foreach (var book in bookList)
        {
            if (book.isDirty)
                book.SaveAndUpdateBook();
        }
    }

    public void SaveAndUpdateCurBook()
    {
        if(curBook)
            curBook.SaveAndUpdateBook();
    }

}
