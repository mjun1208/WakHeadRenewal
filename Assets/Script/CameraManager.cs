using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Transform TargetTransform { get; set; }

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetTransform != null)
        {
            MoveToTarget();
        }
    }

    public void SetTarget(Transform target)
    {
        TargetTransform = target;
    }

    private void MoveToTarget()
    {
        Vector3 targetPos = TargetTransform.position;
        targetPos.z = this.transform.position.z;

        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, 10f * Time.deltaTime);
    }
}
