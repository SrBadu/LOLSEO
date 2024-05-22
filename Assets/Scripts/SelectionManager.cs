using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private static SelectionManager _instance;

    public static SelectionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Busca la instancia existente en la escena
                _instance = FindObjectOfType<SelectionManager>();

                // Si no hay una instancia existente, crea una nueva
                if (_instance == null)
                {
                    GameObject selectionManagerObject = new GameObject("SelectionManager");
                    _instance = selectionManagerObject.AddComponent<SelectionManager>();
                }
            }
            return _instance;
        }
    }

    public HashSet<SelectableUnit> SelectedUnits = new HashSet<SelectableUnit> ();
    public List<SelectableUnit> AvailableUnits = new List<SelectableUnit>();

    private SelectionManager() { }

    public void Select(SelectableUnit Unit)
    {
        SelectedUnits.Add(Unit);
        Unit.OnSelected();
    }

    public void Deselect(SelectableUnit Unit)
    {
        Unit.OnDeselected();
        SelectedUnits.Remove(Unit);
    }

    public void DeselectAll()
    {
        foreach(SelectableUnit unit in SelectedUnits)
        {
            unit.OnDeselected();
        }
        SelectedUnits.Clear();
    }

    public bool IsSelected(SelectableUnit Unit)
    {
        return SelectedUnits.Contains(Unit);
    }
}
