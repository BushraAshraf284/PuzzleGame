using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnim : MonoBehaviour
{
    public Color32 color1, color2;
    int rv, bv, gv;
    public int alpha;

    private void Start()
    {
        gameObject.GetComponent<Image>().color = color2;
        rv = color2.r;
        gv = color2.g;
        bv = color2.b;
    }
    // Update is called once per frame
    void Update()
    {
        if (rv < color1.r)
        {
            rv++;
            gv++;
            bv++;
            GetComponent<Image>().color = new Color32((byte)rv, (byte)gv, (byte)bv, (byte)alpha);
        }
        else
        {
            gameObject.GetComponent<Image>().color = color2;
            rv = color2.r;
            gv = color2.g;
            bv = color2.b;
        }
        
    }
}
