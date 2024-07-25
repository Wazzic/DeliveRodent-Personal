using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyItemChanger<T>
{
    private int currentIndex;

    List<T> listOfItems = new();


    public LobbyItemChanger()
    {

    }

    public LobbyItemChanger(List<T> list)
    {
        currentIndex = 0;
        listOfItems = list;
    }

    public void IncreaseMeshIndex()
    {
        Debug.Log("Increase Mesh Index");
        if (currentIndex >= listOfItems.Count - 1)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }

    }
    public void DecreaseMeshIndex()
    {
        if (currentIndex <= 0)
        {
            currentIndex = listOfItems.Count - 1;
        }
        else
        {
            currentIndex--;
        }

    }


    public T GetCurrentItem()
    {
        return listOfItems[currentIndex];
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }
}
