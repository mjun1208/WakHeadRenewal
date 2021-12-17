using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WakHead;

public class Kakashi_ChainLightning : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;

    public void SetInfo(Vector3 carrierPos, Vector3 targetPos)
    {
        _lineRenderer.SetPosition(0, carrierPos);
        _lineRenderer.SetPosition(1, targetPos);
        
        this.gameObject.SetActive(true);
        
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.5f);
        
        Global.PoolingManager.LocalDespawn(this.gameObject);
    }
}
