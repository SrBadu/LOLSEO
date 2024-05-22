using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private RectTransform SelectionBox;
    [SerializeField]
    private LayerMask UnitLayers;
    [SerializeField]
    private LayerMask FloorLayers;
    [SerializeField]
    private float DragDelay = 0.1f;

    private float MouseDownTime;
    private Vector2 StartMousePosition;

    private void Update()
    {
        HandleSelectionInputs();
        HandleMovementInputs();
    }

    private void HandleMovementInputs()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1) && SelectionManager.Instance.SelectedUnits.Count > 0)
        {
            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000f, FloorLayers.value))
            {
                //Debug.Log(hit.collider.name);
                foreach (SelectableUnit unit in SelectionManager.Instance.SelectedUnits)
                {
                    // Verificar si la unidad todavía existe antes de intentar moverla
                    if (unit != null)
                    {
                        unit.MoveTo(hit.point);
                    }
                }
            }
        }
    }


    private void HandleSelectionInputs()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SelectionBox.sizeDelta = Vector2.zero;
            SelectionBox.gameObject.SetActive(true);
            StartMousePosition = Input.mousePosition;
            MouseDownTime = Time.time;
        }
        else if (Input.GetKey(KeyCode.Mouse0) && MouseDownTime + DragDelay < Time.time)
        {
            ResizeSelectionBox();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            SelectionBox.sizeDelta = Vector2.zero;
            SelectionBox.gameObject.SetActive(false);

            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, UnitLayers)
                && hit.collider.TryGetComponent<SelectableUnit>(out SelectableUnit unit))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (SelectionManager.Instance.IsSelected(unit))
                    {
                        SelectionManager.Instance.Deselect(unit);
                    }
                    else
                    {
                        SelectionManager.Instance.Select(unit);
                    }
                }
                else
                {
                    SelectionManager.Instance.DeselectAll();
                    SelectionManager.Instance.Select(unit);
                }
            }
            else if (MouseDownTime + DragDelay > Time.time)
            {
                SelectionManager.Instance.DeselectAll();
            }

            MouseDownTime = 0;
            /*
            foreach(SelectableUnit newUnit in newlySelectedUnits)
            {
                SelectionManager.Instance.Select(newUnit);
            }
            foreach (SelectableUnit deselectedUnit in deselectedUnits)
            {
                SelectionManager.Instance.Deselect(deselectedUnit);
            }
            newlySelectedUnits.Clear();
            deselectedUnits.Clear();*/
        }
    }

    private void ResizeSelectionBox()
    {
        float width = Input.mousePosition.x - StartMousePosition.x;
        float height = Input.mousePosition.y - StartMousePosition.y;

        SelectionBox.anchoredPosition = StartMousePosition + new Vector2(width / 2, height / 2);
        SelectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        Bounds bounds = new Bounds(SelectionBox.anchoredPosition, SelectionBox.sizeDelta);

        for (int i = 0; i < SelectionManager.Instance.AvailableUnits.Count; i++)
        {
            if (SelectionManager.Instance.AvailableUnits[i] != null)
            {
                Vector2 unitScreenPosition = Camera.WorldToScreenPoint(SelectionManager.Instance.AvailableUnits[i].transform.position);
                if (UnitIsSelectionBox(unitScreenPosition, bounds))
                {
                    SelectionManager.Instance.Select(SelectionManager.Instance.AvailableUnits[i]);
                }
                else
                {
                    SelectionManager.Instance.Deselect(SelectionManager.Instance.AvailableUnits[i]);
                }
            }
        }
    }


    private bool UnitIsSelectionBox(Vector2 Position, Bounds bounds)
    {
        return Position.x > bounds.min.x && Position.x < bounds.max.x
            && Position.y > bounds.min.y && Position.y < bounds.max.y;
    }
}
