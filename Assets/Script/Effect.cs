using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] protected Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator.Rebind();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
        {
            Global.PoolingManager.Despawn(this.gameObject);
        }
    }
}
