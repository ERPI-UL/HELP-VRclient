using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SimpleSlerp : MonoBehaviour
{
    public Vector3 startRotation;

    public Vector3 endRotation;

    private Quaternion _startQuaternion;
    private Quaternion _endQuaternion;
    // Start is called before the first frame update
    void Start()
    {
        _startQuaternion = Quaternion.Euler(startRotation);
        _endQuaternion = Quaternion.Euler(endRotation);
        DOTween.To(() => 0f, (x) =>
            {
                transform.rotation = Quaternion.Slerp(_startQuaternion, _endQuaternion, x);
                Debug.Log(x);
            },
            1f,
            10f).SetEase(Ease.OutExpo);
    }

    // Update is called once per frame
    void Update()
    {
        // Slerp between the start and end positions
        //transform.rotation = Quaternion.Slerp(_startQuaternion, _endQuaternion, Mathf.PingPong(Time.time, 1.0f));

    }
}
