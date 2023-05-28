using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum InputMode
{
    Select,
    Command,
    Build
}
public enum SelectionMode
{
    UI,
    Terrain
}
public class InputManager : Singleton<InputManager>
{
    private BuildingManager buildingManager;

    [SerializeField]
    private EventSystem eventSystem;

    [SerializeField]
    private InputMode mode = InputMode.Select;

    [Header("Selection Input")]
    [SerializeField] private InputAction mousePosition;
    [SerializeField] private InputAction leftClick;
    //    [SerializeField] private InputAction        select;
    [SerializeField] private SelectionMode selectionMode;
    [SerializeField] private RectTransform uISelectionArea;
    [SerializeField] private SelectionArea terrainSelectionArea;
    [SerializeField] private Details detailsPanel;

    public List<Selectable>     selected;
    private bool                selecting;
    private Camera              mainCamera;
    private Ray                 ray;
    private RaycastHit          hit;
    private Vector3             startPosition,
                                endPosition,
                                scale;

    [Header("Command Input")]
    [SerializeField] private InputAction command;

    [Header("Pan Input")]
    [SerializeField] private InputAction pan;
    [SerializeField] private float panSpeed = 1;
    private bool panning;
    private Vector3 panDirection;

    [Header("Zoom Input")]
    [SerializeField] private InputAction zoom;
    [SerializeField] private float zoomSpeed = 1;
    [SerializeField] private int minCameraView = 30,
                                            maxCameraView = 90;

    [SerializeField] private LineRenderer minimapViewportDisplay;

    private Building toBeBuilt, preview;
    private bool isValid;

    [SerializeField] private Material   valid,
                                        invalid;

    protected override void Awake()
    {
        base.Awake();
        selected = new List<Selectable>(1);
        mainCamera = Camera.main;

        uISelectionArea.gameObject.SetActive(false);
        terrainSelectionArea.gameObject.SetActive(false);

        mousePosition.performed += Hover;

        leftClick.performed += LeftClickPerformed;
        //        select.started      += StartSelecting;
        //      select.performed    += Selecting;
        //    select.canceled     += FinishSelecting;
        command.started += Command;
        zoom.performed += Zoom;
        pan.performed += PerformPanning;

        mousePosition.Enable();
        leftClick.Enable();
        //        select.Enable();
        command.Enable();
        pan.Enable();
        zoom.Enable();
    }

    private void Start()
    {
        buildingManager = BuildingManager.Instance;
    }

    private void ResetSelection()
    {
        //  tell everything currently selected that they are not selected anymore
        foreach (Selectable selectable in selected)
            selectable.Select(false);

        //  clear the list
        selected.Clear();

        //  hide details panel
        detailsPanel.ShowDetails(null);
    }

    private bool MouseOverUIElement
    {
        get
        {
            return EventSystem.current.currentSelectedGameObject != null &&
          EventSystem.current.currentSelectedGameObject.layer == LayerMask.NameToLayer("UI");
        }
    }

    private void Hover(InputAction.CallbackContext context)
    {
        if (MouseOverUIElement)
            return;

        ray = mainCamera.ScreenPointToRay(context.ReadValue<Vector2>());

        if (Physics.Raycast(ray, out hit))
        {
            Selectable selectable = hit.transform.GetComponent<Selectable>();
            if (selectable != null)
            {
                Hover(selectable);
                return;
            }

            HexTile tile = hit.transform.GetComponentInParent<HexTile>();
            if (tile != null)
            {
                if (mode == InputMode.Build)
                {
                    preview.transform.position = hit.point;
                    return;
                }
            }
        }
    }

    private void Hover(Selectable selectable)
    {
        //  if input mode is select, display a selection icon?
        //  if input mode is command, display an appropriate icon? dunno
    }
    /*
    private void Hover(HexTile tile)
    {
        if (mode == InputMode.Build)
        {
            preview.transform.position = tile.transform.position;
            //ValidatePreview(preview.canBeBuiltOn.Contains(tile.terrainType) && tile.builtHere == null && tile.resource == null);
        }
        // if input mode is command, then display a move icon?
    }*/

    private void LeftClickPerformed(InputAction.CallbackContext context)
    {
        //  If clicking on a UI element, ignore
        if(MouseOverUIElement)
            return;

        if(mode == InputMode.Select)
        {
            //  raycast to select a single selectable object, or select nothing
            ray = mainCamera.ScreenPointToRay(mousePosition.ReadValue<Vector2>());

            if (Physics.Raycast(ray, out hit))
            {
                Selectable selectable = hit.transform.GetComponent<Selectable>();

                if (selectable != null)
                {
                    //  clicked on something selectable
                    //  if this selectable is already selected, show details
                    if (selected.Contains(selectable))
                    {
                        detailsPanel.ShowDetails(selectable);
                        return;
                    }
                    //  otherwise, remove everything from current selection and select this instead
                    ResetSelection();
                    selected.Add(selectable);
                    selectable.Select(true);
                    return;
                }
                //  if clicked on a tile, select nothing
                if (hit.transform.GetComponentInParent<HexTile>())
                {
                    ResetSelection();
                    return;
                }
            }
        }
        else if(mode == InputMode.Build)
        {
            if (isValid)
                ConfirmBuildingSite();
            else
                print("Can't build that here, bro!");
        }
    }

    private void StartSelecting(InputAction.CallbackContext context)
    {
        if (selectionMode == SelectionMode.UI)
        {
            startPosition = context.ReadValue<Vector2>();
            startPosition.x -= Screen.width / 2;
            startPosition.y -= Screen.height / 2;
            uISelectionArea.anchoredPosition = startPosition;
            uISelectionArea.sizeDelta = Vector2.zero;
            uISelectionArea.gameObject.SetActive(true);
            selecting = true;
        }
        if (selectionMode == SelectionMode.Terrain)
        {
            ray = mainCamera.ScreenPointToRay(context.ReadValue<Vector2>());
            if(Physics.Raycast(ray, out hit))
            {
                startPosition = hit.point;
                startPosition.y = 0.1f;
                terrainSelectionArea.transform.position = startPosition;
                scale = Vector3.forward;
                terrainSelectionArea.transform.localScale = scale;
                terrainSelectionArea.gameObject.SetActive(true);
                selecting = true;
            }
        }
    }

    private void Selecting(InputAction.CallbackContext context)
    {
        if (!selecting)
            return;

        if (selectionMode == SelectionMode.UI)
        {
            endPosition = context.ReadValue<Vector2>();
            endPosition.x -= Screen.width / 2;
            endPosition.y -= Screen.height / 2;
            uISelectionArea.anchoredPosition = Vector2.Lerp(startPosition, endPosition, 0.5f);
            scale.x = Mathf.Abs(startPosition.x - endPosition.x);
            scale.y = Mathf.Abs(startPosition.y - endPosition.y);
            uISelectionArea.sizeDelta = scale;
        }
        if (selectionMode == SelectionMode.Terrain)
        {
            ray = mainCamera.ScreenPointToRay(context.ReadValue<Vector2>());
            Physics.Raycast(ray, out hit);
            endPosition = hit.point;
            endPosition.y = 0.1f;
            terrainSelectionArea.transform.position = Vector3.Lerp(startPosition, endPosition, 0.5f);
            scale.x = startPosition.x - endPosition.x;
            scale.y = startPosition.z - endPosition.z;
            terrainSelectionArea.transform.localScale = scale;
        }
    }

    private void FinishSelecting(InputAction.CallbackContext context)
    {
        if (selectionMode == SelectionMode.UI)
        {
            // figure out what is inside the selection area and select it
            uISelectionArea.gameObject.SetActive(false);
        }
        if (selectionMode == SelectionMode.Terrain)
        {
            // figure out what is colliding with the selection area and select it
            selected = terrainSelectionArea.GetSelection();
            terrainSelectionArea.gameObject.SetActive(false);
        }
        selecting = false;
    }

    private void Command(InputAction.CallbackContext context)
    {
        //  Cancel build mode
        if(mode == InputMode.Build)
        {
            Destroy(preview.gameObject);
            buildingManager.OpenBuildMenu(false);
            mode = InputMode.Select;
            return;
        }

        //  Nothing selected to command
        if (selected.Count == 0)
            return;

        ray = mainCamera.ScreenPointToRay(context.ReadValue<Vector2>());

        if(Physics.Raycast(ray, out hit))
        {
            //  If right clicked on terrain
            HexTile ground = hit.transform.GetComponentInParent<HexTile>();

            if (ground)
            {
                Unit unit = selected[0].GetComponent<Unit>();

                if (unit)
                {
                    foreach (Selectable selected in this.selected)
                    {
                        unit = selected.GetComponent<Unit>();
                        StartCoroutine(unit.NavigateTo(hit.point));
                        return;
                    }
                }
            }
            Resource resource = hit.transform.GetComponent<Resource>();

            if(resource != null)
            {
                //  right clicked on a resource, check if any workers are selected, send them to harvest the resource
                Villager villager;

                foreach(Selectable selectable in selected)
                {
                    villager = selectable.GetComponent<Villager>();

                    if(villager)
                        StartCoroutine(villager.GatherResources(resource));
                }
                return;
            }

            Crab enemy = hit.transform.GetComponent<Crab>();

            if(enemy)
            {
                print("Right clicked on an enemy");
                //  right clicked on an enemy, check if any fighers are selected and command them to attack it
                Villager villager;

                foreach (Selectable selectable in selected)
                {
                    villager = selectable.GetComponent<Villager>();

                    if (villager)
                        villager.Fight(enemy);
                }
                return;
            }
        }
    }

    private void PerformPanning(InputAction.CallbackContext context)
    {
        if(!panning)
            StartCoroutine(Panning());
    }

    private IEnumerator Panning()
    {
        panning = true;
        do
        {
            panDirection = pan.ReadValue<Vector3>();
            mainCamera.transform.position += panDirection * panSpeed;
            UpdateMinimapViewportDisplay();
            yield return null;
        }
        while (panDirection != Vector3.zero);

        panning = false;
    }

    private void Zoom(InputAction.CallbackContext context)
    {
        if (zoomSpeed == 0)
            return;

        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView - context.ReadValue<float>() * zoomSpeed, minCameraView, maxCameraView);
        UpdateMinimapViewportDisplay();
    }
    /*
    public void Select(Selectable selecting)
    {
        //  if this is already selected, do nothing
        if (selected.Contains(selecting))
            return;

        ResetSelection();

        //  finally, select the thing
        selected.Add(selecting);
        selecting.Select(true);
        detailsPanel.ShowDetails(selecting);
    }
    */

    public void SelectBuildingSite(Building toBeBuilt)
    {
        this.toBeBuilt = toBeBuilt;
        preview = Instantiate(toBeBuilt, buildingManager.transform);
        preview.SetAsPreview();
        ValidatePreview(isValid = false);
        mode = InputMode.Build;
        EventSystem.current.SetSelectedGameObject(preview.gameObject);
    }
    public void ValidatePreview(bool isValid)
    {
        this.isValid = isValid;

        foreach (MeshRenderer renderer in preview.GetComponentsInChildren<MeshRenderer>())
            renderer.material = isValid ? valid : invalid;
    }
    public void ConfirmBuildingSite()
    {
        Building beingBuilt = Instantiate(toBeBuilt, preview.transform.position, Quaternion.identity, buildingManager.transform);
        beingBuilt.Initialize();
        Destroy(preview.gameObject);

        foreach(Villager villager in GetSelectedBuilders())
            StartCoroutine(villager.Build(beingBuilt));

        mode = InputMode.Select;
    }
    public List<Villager> GetSelectedBuilders()
    {
        List<Villager> villagers = new List<Villager>();

        Villager villager;

        foreach(Selectable selectable in selected)
        {
            villager = selectable.GetComponent<Villager>();

            if (villager != null && villager.CanBuild())
                villagers.Add(villager);
        }
        return villagers;
    }

    public void UpdateMinimapViewportDisplay()
    {
        Vector3[] corners = new Vector3[4];
        Vector3 screenCoordinates = Vector3.zero;
        Ray ray = mainCamera.ScreenPointToRay(screenCoordinates);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            corners[0] = hit.point;

        screenCoordinates.y = Screen.height;
        ray = mainCamera.ScreenPointToRay(screenCoordinates);

        if (Physics.Raycast(ray, out hit))
            corners[1] = hit.point;

        screenCoordinates.x = Screen.width;
        ray = mainCamera.ScreenPointToRay(screenCoordinates);

        if (Physics.Raycast(ray, out hit))
            corners[2] = hit.point;

        screenCoordinates.y = 0;
        ray = mainCamera.ScreenPointToRay(screenCoordinates);

        if (Physics.Raycast(ray, out hit))
            corners[3] = hit.point;

        minimapViewportDisplay.SetPositions(corners);
    }

    private void OnValidate()
    {
        if (panSpeed <= 0)
            panSpeed = 0.001f;

        if (zoomSpeed < 0)
            zoomSpeed = 0;

        if (minCameraView < 1)
            minCameraView = 1;

        if (maxCameraView > 179)
            maxCameraView = 179;

        if (maxCameraView < minCameraView)
            minCameraView = maxCameraView;
    }
}