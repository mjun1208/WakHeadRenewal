using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] private TrailRenderer _trail;
    private GameObject _owner;
    private Vector3 _dir;

    public const float GoSpeed = 10f;
    public const float BackSpeed = 15f;

    public void SetInfo(GameObject owner, Vector3 dir)
    {
        this.transform.position = owner.transform.position;

        _trail.Clear();

        _owner = owner;
        _dir = dir;

        StartCoroutine(Go());
    }

    private IEnumerator Go()
    {
        float goTime = 0;

        while (goTime < 0.5f)
        {
            goTime += Time.deltaTime;
            _rigid.MovePosition(this.transform.position + _dir * GoSpeed * Time.deltaTime);

            yield return null;
        }

        StartCoroutine(Back());
    }

    private IEnumerator Back()
    {
        float ownerDistance = Vector3.Distance(this.transform.position, _owner.transform.position);

        while (ownerDistance > 0.2f)
        {
            _dir = this.transform.position - _owner.transform.position;
            _rigid.MovePosition(this.transform.position - _dir.normalized * BackSpeed * Time.deltaTime);

            ownerDistance = Vector3.Distance(this.transform.position, _owner.transform.position);

            yield return null;
        }

        _trail.Clear();
        this.gameObject.SetActive(false);
    }
}
