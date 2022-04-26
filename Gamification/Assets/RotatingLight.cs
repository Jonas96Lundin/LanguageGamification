using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatingLight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMove(new Vector3(100, -80, -108), 4).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        //transform.DOLocalMove(new Vector3(750, -160, -108), 4).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(transform.parent.position, Vector3.forward, -180 * Time.deltaTime);
        
    }
}
