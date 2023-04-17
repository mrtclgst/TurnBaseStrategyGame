using System.Collections.Generic;

public class GridObject
{
    private GridSystemHex<GridObject> _gridSystemHex;
    private GridPosition _gridPosition;
    private List<Unit> _unitList;
    private IInteractable _interactable;

    public GridObject(GridSystemHex<GridObject> gridSystemHex, GridPosition gridPosition)
    {
        _gridSystemHex = gridSystemHex;
        _gridPosition = gridPosition;
        _unitList = new();
    }

    public override string ToString()
    {
        string unitString = string.Empty;
        foreach (Unit unit in _unitList)
        {
            unitString += unit + "\n";
        }

        return _gridPosition.ToString() + "\n" + unitString;
    }

    public void AddUnit(Unit unit)
    {
        _unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        _unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return _unitList;
    }

    public bool HasAnyUnit()
    {
        return _unitList.Count > 0;
    }

    public Unit GetUnit()
    {
        if (HasAnyUnit())
        {
            return _unitList[0];
        }
        else
        {
            return null;
        }
    }

    public IInteractable GetInteractable()
    {
        return _interactable;
    }

    public void SetInteractable(IInteractable interactable)
    {
        _interactable = interactable;
    }
}