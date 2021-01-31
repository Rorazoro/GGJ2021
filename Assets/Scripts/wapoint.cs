using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wapoint : MonoBehaviour
{
    public Image img;
    public Transform target;

    private void Update()
    {
        img.transform.position = Camera.main.WorldToScreenPoint(target.position);
    }
}
