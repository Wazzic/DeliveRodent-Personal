using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    enum pickAnimation
    {
        Waving = 0,
        Walking,
        Jumping,
        Sleeping,
        Jogging,
        Situps,
        Dead,
        rad, 
        sitting,
        sitIdle
    }

    [SerializeField]Animator anim;

    int[] hashID = 
    { 
      Animator.StringToHash("isWaving"), //0
      Animator.StringToHash("isWalking"), //1
      Animator.StringToHash("isJumping"), //2
      Animator.StringToHash("isSleeping"), //3
      Animator.StringToHash("isJogging"), //4
      Animator.StringToHash("isSitUpping"), //5
      Animator.StringToHash("isDying"),  //6
      Animator.StringToHash("isRadFlip"),  //7
      Animator.StringToHash("isSitting"),  //7
      Animator.StringToHash("isSitIdle")  //7
    };

    [SerializeField] pickAnimation initialiseState;

    int m_stateTracker = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_stateTracker = (int)initialiseState;
        anim = GetComponent<Animator>();
    } 

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(m_stateTracker);

        anim.SetBool(hashID[m_stateTracker], true);

        
        
        
    }
}
