using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using PrimeTween;

public class ViewManager : MonoBehaviour
{   
    private MainInput mainInput;
    
    public Transform handParent;
    public Transform itemParent;
    public Transform drawPoint;
    public Transform discardPoint;
    
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private int hoveredIndex;
    private bool grabbed;
    [SerializeField] private GameObject selected;
    private int selectedIndex = 0;
    private Vector3 selectedOriginalPos;
    private Quaternion selectedOriginalRot;
    public List<GameObject> handObjects;
    private IGameItem[] handItemValues;
    private Dictionary<Vector3, GameObject> itemSlots;
    [SerializeField] private List<Vector3> itemPosList;
    [SerializeField] private List<GameObject> items;
    public List<GameObject> Items => items;
    
    public static event System.Action<int, int> OnItemAdded;
    public GameObject ItemObjectPrefab;
    
    private void Awake()
    {
        handObjects = new List<GameObject>();
        itemSlots = new Dictionary<Vector3, GameObject>();
        items =  new List<GameObject>();
        DeckSystem.OnHandUpdated += OnHandUpdatedHandler;
        ItemManager.OnItemActivated += StartCombatAnimations;
        StateLogicControl.OnEnterShopState += OnEnterShopStateHandler;
    }

    private void OnDisable()
    {
        DeckSystem.OnHandUpdated -= OnHandUpdatedHandler;
        ItemManager.OnItemActivated -= StartCombatAnimations;
        StateLogicControl.OnEnterShopState -= OnEnterShopStateHandler;
        mainInput.Disable();
    }

    private void Start()
    {   
        mainInput = new MainInput();
        mainInput.Enable();
        
        mainInput.Default.Click.started += HandleMouseGrab;
        mainInput.Default.Click.canceled += HandleMouseRelease;
        
        foreach (Transform child in handParent)
        {   
            var c = child.GetComponent<HandObject>();
            c.originalPos = c.transform.position;
            c.originalRot = c.transform.rotation;
            handObjects.Add(c.gameObject);
        }

        foreach (Transform child in itemParent)
        {
            var screenPoint = Camera.main.WorldToScreenPoint(child.transform.position);
            itemSlots.Add(new Vector3(screenPoint.x, screenPoint.y, 0), child.transform.gameObject);
            itemPosList.Add(child.transform.position);
        }
        
        Debug.Log("ITEM SLOT COUNT: " +  itemSlots.Count);
    }

    private void OnHandUpdatedHandler(IGameItem[] newHand)
    {
        UpdateHandObjects(newHand);
        StartCoroutine(PlayDrawAnimation());
    }
    
    
    private void UpdateHandObjects(IGameItem[] newHand)
    {
        handItemValues = newHand;
        for (var i = 0; i < newHand.Length; i++)
        {
            if (handObjects[i] == null || newHand[i] == null) continue; 
            var objScript = handObjects[i].GetComponent<HandObject>();
            objScript.descriptionText.text = newHand[i].ItemName;
            objScript.cost = newHand[i].Cost;
            objScript.costText.text = "Cost: " + objScript.cost;
        }
    }

    private IEnumerator PlayDrawAnimation()
    {
        foreach (var item in handObjects)
        {
            item.SetActive(false);
            item.transform.position = drawPoint.position;
        }

        foreach (var item in handObjects)
        {   
            item.SetActive(true);
            var itemScript = item.GetComponent<HandObject>();
            yield return Tween
                .Position(item.transform, endValue: itemScript.originalPos, duration: 0.2f, ease: Ease.InOutSine)
                .ToYieldInstruction();
        }
    }
    
    
    private void HandleMouseGrab(InputAction.CallbackContext context)
    {   
        grabbed = true;
        Debug.Log("grab");
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform.name);
            selected = hit.transform.gameObject;
            selectedOriginalPos = selected.transform.position;
            selectedOriginalRot = selected.transform.rotation;
            selected.transform.rotation = Quaternion.identity;
            selectedIndex = handObjects.IndexOf(selected);
        }
    }

    private void HandleMouseRelease(InputAction.CallbackContext context)
    {
        if (hoveredIndex != -1 && selected != null && selected.TryGetComponent<HandObject>(out var obj))
        {
            if (PlayerState.Instance.AdjustPlayerMoneyHasEnough(obj.cost))
            {
                AddItem(selectedIndex, hoveredIndex);
            }
            else
            {
                ResetObjectPosition(selected.GetComponent<HandObject>());
            }
        }
        else if (hoveredIndex == -1 && selected != null && selected.TryGetComponent<HandObject>(out var obj2))
        {
            ResetObjectPosition(obj2);
        }
        grabbed = false;
        selected = null;
    }

    private void OnEnterShopStateHandler()
    {
        StopCombatAnimations();
    }
    
    private void AddItem(int fromIndex, int toIndex)
    {
        OnItemAdded?.Invoke(fromIndex, toIndex);
        var newItem = Instantiate(ItemObjectPrefab, selected.transform.position, selected.transform.rotation);
        selected.transform.position = selectedOriginalPos;
        selected.transform.rotation = selectedOriginalRot;
        items.Add(newItem);
        if (handObjects[fromIndex] != null)
        {
            var nS = handObjects[fromIndex].GetComponent<HandObject>();
            newItem.GetComponent<ItemObject>().descriptionText.text = nS.descriptionText.text;
        }
        selected.gameObject.SetActive(false);
    }
    
    private void Update()
    {   
        mousePos = Mouse.current.position.ReadValue();
        var positions = itemSlots.Keys.ToList();
        
        if (Vector3.Distance(positions[0], mousePos) < 100f)
        {
            hoveredIndex = 0;
        }
        else if (Vector3.Distance(positions[1], mousePos) < 100f)
        {
            hoveredIndex = 1;
        }
        else if (Vector3.Distance(positions[2], mousePos) < 100f)
        {
            hoveredIndex = 2;
        }
        else if (Vector3.Distance(positions[3], mousePos) < 100f)
        {
            hoveredIndex = 3;
        }
        else if (Vector3.Distance(positions[4], mousePos) < 100f)
        {
            hoveredIndex = 4;
        }
        else
        {
            hoveredIndex = -1;
        }
        if (selected != null && grabbed)
        {   
            var z = Camera.main.WorldToScreenPoint(selected.transform.position).z;
            var worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, z));
            int index = - 1;
            if (Vector3.Distance(positions[0], mousePos) < 100f)
            {
                index = 0;
            }
            else if (Vector3.Distance(positions[1], mousePos) < 100f)
            {
                index = 1;
            }
            else if (Vector3.Distance(positions[2], mousePos) < 100f)
            {
                index = 2;
            }
            else if (Vector3.Distance(positions[3], mousePos) < 100f)
            {
                index = 3;
            }
            else if (Vector3.Distance(positions[4], mousePos) < 100f)
            {
                index = 4;
            }
            else
            {   
                selected.transform.position = worldPoint;
            }
            if (index != -1)
            {
                selected.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(positions[index].x, positions[index].y, z));
                ShiftItem(index);
            }
        }
    }
    
    private void ShiftItem(int newIndex)
    {
        var oldIndex = items.IndexOf(selected);
        if (oldIndex == -1)
        {
            return;
        }
        items.RemoveAt(oldIndex);
        items.Insert(newIndex, selected);
        for (var i = 0; i < items.Count; i++)
        {
            if (items[i] == null) continue;
            
            if (Vector3.Distance(items[i].transform.position, itemPosList[i]) < 1)
            {
                continue;
            }
            Tween.Position(items[i].transform, itemPosList[i], 0.1f);
        }
    }

    private void ResetObjectPosition(HandObject obj)
    {
        obj.transform.position = obj.originalPos;
        obj.transform.rotation = obj.originalRot;
    }
    
    private void StartCombatAnimations(int index, IGameItem itm)
    {
        var i = items[index];
        if (i == null) return;
        var itemScript = i.GetComponent<ItemObject>();
        if (itemScript == null) return;
        itemScript.AnimateSlider(itm.Cooldown);
    }

    private void StopCombatAnimations()
    {
        foreach (var item in items)
        {   
            if (item == null) continue;
            var itemScript = item.GetComponent<ItemObject>();
            if (itemScript == null) continue;
            itemScript.StopAnimation();
        }
    }
}
