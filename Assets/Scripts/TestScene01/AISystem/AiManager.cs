using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    [SerializeField]List<carControl> controlList = new List<carControl>();
    [SerializeField] carControl carControlInstance1;
    [SerializeField] carControl carControlInstance2;
    [SerializeField] bool[] whichPickUp;

    public Transform assignTransform;
    // Start is called before the first frame update
    void Start()
    {
        whichPickUp = new bool[2];
        whichPickUp[0] = false;
        whichPickUp[1] = false;
        controlList.Add(carControlInstance1);
        controlList.Add(carControlInstance2);
        StartCoroutine(checkDeliveryStatus());
    }

    IEnumerator checkDeliveryStatus()
    {
        for (int i = 0; i < controlList.Count; i++)
        {
            if (controlList[i].hasPickedUp == true)
            {
                whichPickUp[i] = true;
                StartCoroutine(assignEnemyPosition());
                break;
            }
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(checkDeliveryStatus());
    }

    IEnumerator assignEnemyPosition()
    {
        int tmp;
        for (int i = 0; i < controlList.Count; i++)
        {
            if (whichPickUp[i] == true) //car 0 picked up
            {
                tmp = (i + 1) % (controlList.Count);// 0 + 1 % 2 = car 1
                assignTransform = controlList[i].transform; //enemy transform = car 0
                controlList[tmp].setEnemyPos(assignTransform); // car 1 now sees car 0
            }
        }
        yield return new WaitForSeconds(0.2f);
    }

}
