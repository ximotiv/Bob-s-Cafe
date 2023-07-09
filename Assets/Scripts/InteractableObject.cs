using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

[RequireComponent(typeof(Outline))]
public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    private VisualDataConfig _visualDataConfig;
    private Outline _outline;

    public bool IsSelected { get; protected set; }
    public bool IsAnimationActived { get; protected set; }
    public bool IsOnMouseEnter { get; private set; }

    protected virtual void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.OutlineWidth = 0;
    }

    private void OnMouseEnter()
    {
        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

        if (!isPointerOverUI)
        {
            ShowOutline();
            IsOnMouseEnter = true;
        }
    }

    private void OnMouseExit()
    {
        IsOnMouseEnter = false;

        if (IsSelected) return;

        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

        if (!isPointerOverUI)
        {
            HideOutline();
        }
    }

    private void OnMouseUp()
    {
        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

        if (!isPointerOverUI)
        {
            HandleClick();
        }
    }

    [Inject]
    public void Construct(VisualDataConfig visualDataConfig)
    {
        _visualDataConfig = visualDataConfig;
    }

    public void Select()
    {
        if (IsSelected) return;

        IsSelected = true;

        ShowOutline();
        ChangeOutlineColor(VisualDataConfig.OutlineType.Selected);

        OnSelect();
    }

    public void UnSelect()
    {
        if (IsSelected == false) return;

        IsSelected = false;

        ChangeOutlineColor(VisualDataConfig.OutlineType.MouseStay);

        if (IsOnMouseEnter == false)
        {
            HideOutline();
        }

        OnUnSelect();
    }

    protected abstract void OnSelect();
    protected abstract void OnUnSelect();

    public void ShowOutline()
    {
        if (IsSelected) return;
        _outline.OutlineWidth = 5;
    }

    public void HideOutline()
    {
        if (IsSelected) return;
        _outline.OutlineWidth = 0;
    }

    protected void ChangeOutlineColor(VisualDataConfig.OutlineType type)
    {
        if (_outline == null) return;
        _outline.OutlineColor = _visualDataConfig.OutlineColors[(int)type];
    }

    protected abstract void HandleClick();
}