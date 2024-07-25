using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetroyAfterSeconds : MonoBehaviour
{
    [SerializeField] float waitTime;

    IEnumerator waitToSetActive()
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.SetActive(false);

    }
}
