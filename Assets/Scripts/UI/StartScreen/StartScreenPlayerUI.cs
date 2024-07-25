using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenPlayerUI : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.ChangeScene(1);
    }
}
