using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WipeScript : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator m_animator;
    private Image m_image;

    public float racoonTiling = 1.0f;
    public float racoonOffset = 0.0f;


    void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();
        m_image = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        m_image.materialForRendering.SetFloat("_RacoonTiling", racoonTiling);
        m_image.materialForRendering.SetFloat("_RacoonOffset", racoonOffset);
    }
}
