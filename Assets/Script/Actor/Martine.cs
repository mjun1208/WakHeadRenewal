using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Martine : Actor
{
    private List<Martine_Vent> _myVentList = new List<Martine_Vent>();

    private Martine_Vent _currentVent = null;
    private bool _isOnVent = false;

    private List<Collider2D> _colliedVent = new List<Collider2D>();

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        SelectVent();
    }

    protected override void Active_Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        var targetList = _attackRange.CollidedObjectList;

        foreach (var target in targetList)
        {
            var targetEntity = target.GetComponent<Entity>();
            targetEntity.KnockBack(GetAttackDir(), 10, 0);
        }
    }

    protected override void Active_Skill_1()
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }

    protected override void Active_Skill_2()
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }

    protected override void OnSkill_2()
    {
        if (_currentVent == null)
        {
            var newVent = Global.PoolingManager.Spawn("Martine_Vent", this.transform.position, this.transform.rotation);
            var newVentScript = newVent.GetComponent<Martine_Vent>();
            _myVentList.Add(newVentScript);
        }
        else
        {
            this.transform.position = _currentVent.transform.position + new Vector3(0, 0.2f, 0);
            base.OnSkill_2();

            _currentVent.OnVent();

            StartCoroutine(Venting());
        }
    }

    private int GetCurrentVentIndex()
    {
        int ventIndex = 0;
        int currentVentIndex = 0;

        foreach (var vent in _myVentList)
        {
            if (vent == _currentVent)
            {
                currentVentIndex = ventIndex;
            }

            ventIndex++;
        }

        return currentVentIndex;
    }

    private IEnumerator Venting()
    {
        int currentVentIndex = GetCurrentVentIndex();

        if (currentVentIndex + 1 == _myVentList.Count)
        {
            currentVentIndex = -1;
        }

        var nextVent = _myVentList[currentVentIndex + 1];

        bool up = false;

        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Skill_2"))
        {
            yield return null;
        }

        yield return null;

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f && !up)
            {
                nextVent.OnVent();
                up = true;
            }

            yield return null;
        }

        // 벤트 올라옴

        this.transform.position = nextVent.transform.position + new Vector3(0, 0.2f, 0);
        _smoothSync.teleport();

        _animator.SetBool("IsSkill_2", false);
        _animator.SetBool("IsSkill_2_Reverse", true);

        if (_onSkillCoroutine != null)
        {
            StopCoroutine(_onSkillCoroutine);
            _onSkillCoroutine = null;
        }

        _isDoingSkill = true;
        _onSkillCoroutine = OnSkill("Skill_2_Reverse");
        StartCoroutine(_onSkillCoroutine);
    }

    private void SelectVent()
    {
        if (_currentVent == null && _colliedVent.Count > 0)
        {
            _currentVent = _colliedVent[0].GetComponent<Martine_Vent>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vent"))
        {
            _colliedVent.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Vent"))
        {
            if (_currentVent != null)
            {
                if (collision.gameObject == _currentVent.gameObject)
                {
                    _currentVent = null;
                }
            }

            _colliedVent.Remove(collision);
        }
    }
}
