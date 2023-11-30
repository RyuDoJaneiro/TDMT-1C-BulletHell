using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    private RawImage _img;
    [SerializeField] private float _x = 0.1f, _y = 0f;

    private void Start()
    {
        _img = gameObject.GetComponent<RawImage>();
    }
    void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _img.uvRect.size);
    }
}
