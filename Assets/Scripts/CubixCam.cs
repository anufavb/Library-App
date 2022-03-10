using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CubixCam : MonoBehaviour
{
    //public static int maxCamDepth = 0;
    private readonly int maxCamDepth = 10, camDepth = 0;
    private Camera cam;
    public Button backButton;
    public GameObject libBack, wallPaper;
    private float camSize;
    public GameObject blockingPanel;
    public Canvas canvas;
    public Image panelImage;
    public Vector2Int rowCol;
    public bool selectionMode = false, wait = false;
    public LibraryManager libraryManager;
    public Vector2 pointToSpawn;
    public float spaceleft, transparency;

    // Start is called before the first frame update
    void Awake()
    {
        cam = GetComponent<Camera>();
        camSize = cam.orthographicSize;
        SetCameraActive(false);
        transparency = panelImage.color.a;
    }

    void Start()
    {
    }

    public void SetRowCol(int x, int y)
    {
        rowCol = new Vector2Int(x, y);
    }

    public void SetLibraryManager(LibraryManager libraryManager)
    {
        this.libraryManager = libraryManager;
    }

    public void SetCameraActive(bool active)
    {
        cam.enabled = active;
    }

    //void OnMouseUp()
    void OnMouseUpAsButton()
    {
        if (selectionMode)
        {
            //float spaceleft;
            //if ((spaceleft=libraryManager.maxXScale-libraryManager.bookLengths[rowCol.x, rowCol.y]) > libraryManager.minBookWidth)
            if (spaceleft > libraryManager.minBookWidth)
            {
                Debug.Log($"Camera {rowCol} selected");
                Debug.Log($"maxSpace Allowed {libraryManager.maxXScale}");
                Debug.Log($"Space Used in Camera {rowCol}: {libraryManager.bookLengths[rowCol.x, rowCol.y]}");
                Debug.Log($"spaceleft: {spaceleft}");
                GoIntoCam();
                wait = true;
                libraryManager.SetWidthSliderMinMax(spaceleft);
                libraryManager.ExitSelectionMode();
            }
            else Debug.Log($"No more room here!"); ;
        }
        else if (!EventSystem.current.IsPointerOverGameObject())
        {
            GoIntoCam();
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.T))
            Debug.Log($"LibraryManager.curcCam.spaceleft: {LibraryManager.curcCam.spaceleft}");
            //Debug.Log($"spaceleft: {spaceleft}");
        if (Input.GetMouseButtonDown(0) && wait && !EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("1");
            //SpawnBookAtClick(cam.ScreenToWorldPoint(Input.mousePosition));
            pointToSpawn = cam.ScreenToWorldPoint(Input.mousePosition);
            //wait = false;
        }
    }

    void GoIntoCam()
    {
        Debug.Log("clicked cam - " + rowCol);
        LibraryManager.SetMainLibCamDepth(camDepth);
        SetCameraActive(true);
        LibraryManager.SetMainCameraActive(false);
        cam.depth = maxCamDepth;
        blockingPanel.SetActive(false);
        LibraryManager.incCam = true;
        LibraryManager.curcCam = this;
        spaceleft = libraryManager.maxXScale - libraryManager.bookLengths[rowCol.x, rowCol.y];
        libraryManager.SetWidthSliderMinMax(spaceleft);
        LibraryManager.curCam = cam;
    }

    public void SpawnBookAtClick()
    {
        if (pointToSpawn != null)
        {
            libraryManager.AddBookRowColPos(rowCol.x, rowCol.y, pointToSpawn.x, pointToSpawn.y);
            wait = false;
        }
        else
            Debug.Log($"Error adding book, pointToSpawn in null");
        //libraryManager.AddBookRowColPos(rowCol.x, rowCol.y, mouseClick.x, mouseClick.y);
        //libraryManager.MakeBook(mouseClick.x, mouseClick.y, 2f, libraryManager.heightSlider.value, rowCol.x, rowCol.y, book);
    }

    public void BackOutOfCubixCam()
    {
        if (!wait)
        {
            cam.depth = camDepth;
            LibraryManager.SetMainCameraActive(true);
            SetCameraActive(false);
            LibraryManager.SetMainLibCamDepth(maxCamDepth);
            blockingPanel.SetActive(true);
            LibraryManager.incCam = false;
            LibraryManager.curcCam = null;
            LibraryManager.curBook = null;
            libraryManager.ResetPanels();
            LibraryManager.curCam = LibraryManager.mainLibCamera;
        }
    }

    public void TurnOffBackButton()
    {
        backButton.gameObject.SetActive(false);
    }

    public void TurnOnLibBack()
    {
        libBack.gameObject.SetActive(true);
    }

    public void TurnOnWallPaper()
    {
        float newCamSize = GetComponent<Camera>().orthographicSize / camSize;

        //Debug.Log("Old Cam Size: " + GetComponent<Camera>().orthographicSize);
        //Debug.Log("Current Cam Size: " + camSize);
        //Debug.Log("newCam Size: " + newCamSize);

        newCamSize *= 1.1f;
        wallPaper.gameObject.SetActive(true);
        wallPaper.transform.localScale *= 0;
        wallPaper.transform.localScale += new Vector3(newCamSize, newCamSize, 1f);
    }

    public void EnterSelectionMode()
    {
        selectionMode = true;
        panelImage.enabled = true;
        canvas.renderMode = RenderMode.WorldSpace;
        spaceleft = libraryManager.maxXScale - libraryManager.bookLengths[rowCol.x, rowCol.y];
    }

    public void ExitSelectionMode()
    {
        selectionMode = false;
        panelImage.enabled = false;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
    }
    void OnMouseEnter()
    {
        if (selectionMode)
        {
            panelImage.color = Color.yellow;
            //panelImage.canvasRenderer.SetAlpha(transparency);
            panelImage.canvasRenderer.SetAlpha(0);
        }
    }
    void OnMouseExit()
    {
        if (selectionMode)
        {
            panelImage.color = Color.white;
            panelImage.canvasRenderer.SetAlpha(transparency);
        }
    }
    public void SetWidthSliderForBook(float value)
    {
        libraryManager.widthSlider.minValue = libraryManager.minBookWidth;
        libraryManager.widthSlider.maxValue = value + LibraryManager.curcCam.spaceleft;
        libraryManager.widthSlider.SetValueWithoutNotify(value);
    }
}
