using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum SortingMode { itemAmount, itemName};

public class InventoryManager : MonoBehaviour
{
    public delegate bool CompareItems(InventoryItemScript a, InventoryItemScript b);

    #region member variables

    // Parent object inventory item
    public Transform parentPanel;
    // Item info to build inventory items
    public List<Sprite> itemSprites;
    public List<string> itemNames;
    public List<int> itemAmounts;
    // Starting template item
    public GameObject startItem;    GameObject inventoryItem;
    List<InventoryItemScript> inventoryList;
    #endregion

    void Start()
    {
    
    }

    public void AddItem(Sprite img, string name, int amount)
    {
        //check if the item in in the list, if not, we add it, if so, we increase the amount
        if (itemNames.Contains(name))
        {
            int id = itemNames.FindIndex(found => found==name); //using predicate
            itemAmounts[id]++;
        }
        else
        {
            itemNames.Add(name);
            itemSprites.Add(img);
            itemAmounts.Add(amount);
        }
    }

    public void RefreshInventory()
    {
        inventoryList = new List<InventoryItemScript>();
        for (int i = 0; i < itemNames.Count; i++)
        {
            // Create a duplicate of the starter item
            inventoryItem = (GameObject)Instantiate(startItem);
            // UI items need to parented by the canvas or an objec within the canvas
            inventoryItem.transform.SetParent(parentPanel);
            // Original start item is disabled – so the duplicate must be enabled
            inventoryItem.SetActive(true);
            // Get InventoryItemScript component so we can set the data
            InventoryItemScript iis = inventoryItem.GetComponent<InventoryItemScript>();
            iis.itemSprite.sprite = itemSprites[i];
            iis.itemNameText.text = itemNames[i];
            iis.itemName = itemNames[i];
            iis.itemAmountText.text = itemAmounts[i].ToString();
            iis.itemAmount = itemAmounts[i];
            // Keep a list of the inventory items
            inventoryList.Add(iis);
        }

        DisplayListInOrder();
    }

    void DisplayListInOrder()
    {
        // Height of item plus space between each
        float yOffset = 55f;
        // Use the start position for the first item
        Vector3 startPosition = startItem.transform.position;
        foreach (InventoryItemScript iis in inventoryList)
        {
            iis.transform.position = startPosition;
            //set position of next item using offset
            startPosition.y -= yOffset;
        }
    }

    public void SelectionSortInventory()
    {
        // iterate through every item in the list except last
        for (int i = 0; i < inventoryList.Count - 1; i++)
        {
            int minIndex = i;
            // iterate through unsorted portion of the list
            for (int j = i; j < inventoryList.Count; j++)
            {
                if (inventoryList[j].itemAmount < inventoryList[minIndex].itemAmount)
                {
                    minIndex = j;
                }
            }
            // Swap the minimum item into position
            if (minIndex != i)
            {
                InventoryItemScript iis = inventoryList[i];
                inventoryList[i] = inventoryList[minIndex];
                inventoryList[minIndex] = iis;
            }
        }
        // Display the list in the new correct order
        DisplayListInOrder();
    }

    List<InventoryItemScript> QuickSort(List<InventoryItemScript> listIn)
    {
        if (listIn.Count <= 1)
        {
            return listIn;
        }
        // Naive pivot selection
        int pivotIndex = 0;
        // Left and right lists
        List<InventoryItemScript> leftList = new List<InventoryItemScript>();
        List<InventoryItemScript> rightList = new List<InventoryItemScript>();
        for (int i = 1; i < listIn.Count; i++)
        {
            // Compare amounts to determine list to add to
            if (listIn[i].itemAmount > listIn[pivotIndex].itemAmount)
            {
                // Greater than pivot to left list
                leftList.Add(listIn[i]);
            }
            else
            {
                // Smaller than pivot to right list
                rightList.Add(listIn[i]);
            }
        }

        // Recurse left list
        leftList = QuickSort(leftList);
        //Recurse right list
        rightList = QuickSort(rightList);
        // Concatenate lists: left + pivot + right
        leftList.Add(listIn[pivotIndex]);
        leftList.AddRange(rightList);
        return leftList;
    }

    public void StartQuickSort()
    {
        inventoryList = QuickSort(inventoryList);
        DisplayListInOrder();
    }    public void BubbleSort()
    {
        bool sorted = false;
        bool swapOccurred = false;
        while (!sorted)
        {
            sorted = true; //we set sorted to true, then to false everytime we swap elements
            swapOccurred = false;
            //sorting iteration
            for (int i = 1; i < inventoryList.Count; i++)
            {
                InventoryItemScript temp;
                if (inventoryList[i - 1].itemAmount > inventoryList[i].itemAmount)
                {
                    temp = inventoryList[i];
                    inventoryList[i] = inventoryList[i - 1];
                    inventoryList[i - 1] = temp;
                    swapOccurred = true;
                }
            }
            if (swapOccurred)
            {
                sorted = false;
            }
        }
        DisplayListInOrder();
    }    public void InsertionSort()
    {
        for (int i = 1; i < inventoryList.Count; i++)
        {
            InventoryItemScript current = inventoryList[i];
            int prec = i - 1;
            while (prec >= 0 && inventoryList[prec].itemAmount > current.itemAmount)
            {
                inventoryList[prec + 1] = inventoryList[prec];
                prec -= 1;
            }
            inventoryList[prec + 1] = current;
        }        DisplayListInOrder();    }    List<InventoryItemScript> MergeSort(List<InventoryItemScript> list)
    {
        //recursion exit point
        if (list.Count <= 1)
            return list;

        //recursive step
        List<InventoryItemScript> leftList = new List<InventoryItemScript>();
        List<InventoryItemScript> rightList = new List<InventoryItemScript>();
        for (int i = 0; i < list.Count; i++)
        {
            if (i < list.Count / 2)
            {
                leftList.Add(list[i]);
            }
            else
            {
                rightList.Add(list[i]);
            }
        }
        //print(leftList.Count);
        //print("------");
        //print(rightList.Count);
        //print("######");

        leftList = MergeSort(leftList);
        rightList = MergeSort(rightList);

        //post recursion step
        list = Merge(leftList, rightList);
        //print("Merged list");
        return list;
    }
    List<InventoryItemScript> Merge(List<InventoryItemScript> leftList, List<InventoryItemScript> rightList)// ,DelegatePointer)
    {
        List<InventoryItemScript> merged = new List<InventoryItemScript>();
        while (leftList.Count > 0 && rightList.Count > 0)
        {
            if (leftList[0].itemAmount <= rightList[0].itemAmount) //if (Order)
            {
                merged.Add(leftList[0]);
                leftList.RemoveAt(0);
            }
            else
            {
                merged.Add(rightList[0]);
                rightList.RemoveAt(0);
            }
        }

        while (leftList.Count > 0)
        {
            merged.Add(leftList[0]);
            leftList.RemoveAt(0);
        }
        while (rightList.Count > 0)
        {
            merged.Add(rightList[0]);
            rightList.RemoveAt(0);
        }

        return merged;
    }

    public void MergeSortInventory()
    {
        inventoryList = MergeSort(inventoryList);
        DisplayListInOrder();
    }
}
