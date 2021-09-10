using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigid;
    private Vector3 _dir;

    public const float MoveSpeed = 8f;

    public void SetInfo(Vector3 dir)
    {
        _dir = dir;

        StartCoroutine(Go());
    }

    private IEnumerator Go()
    {
        float goTime = 0;

        while (goTime < 1f)
        {
            goTime += Time.deltaTime;
            _rigid.MovePosition(this.transform.position + _dir * MoveSpeed * Time.deltaTime);

            yield return null;
        }

        Global.PoolingManager.Despawn(this.gameObject);
    }
}
