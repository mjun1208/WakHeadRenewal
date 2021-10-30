using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Martine : Actor
{
    private List<Martine_Vent> _myVentList = new List<Martine_Vent>();

    private Martine_Vent _currentVent = null;
    private bool _isOnVent = false;

    private List<Collider2D> _colliedVent = new List<Collider2D>();

    private IEnumerator _selectNextVent = null;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        SelectVent();
    }

    protected override void ForceStop(bool isStun)
    {
        base.ForceStop(isStun);
        if (_selectNextVent != null)
        {
            StopCoroutine(_selectNextVent);
        }

        _selectNextVent = null;
        _renderer.enabled = true;
    }

    protected override void Active_Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        _attackRange.Attack(targetEntity =>
        {
            targetEntity.KnockBack(5, GetAttackDir(), 0.5f, 0);
        });
    }

    protected override void Active_Skill_1()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        _skill_1Range.Attack(targetEntity =>
        {
            targetEntity.KnockBack(15, GetAttackDir(), 1f, 0);
        });
    }

    private void Active_Skill_1_2()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        _skill_1Range.Attack(targetEntity =>
        {
            targetEntity.KnockBack(15, GetAttackDir(), 1f, 0);
        });
    }

    protected override void Active_Skill_2()
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }

    public override void OnSkill_2()
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

            _animator.SetBool("IsSkill_2", true);

            if (OnSkillCoroutine != null)
            {
                StopCoroutine(OnSkillCoroutine);
                OnSkillCoroutine = null;
            }

            IsDoingSkill = true;

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

        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Skill_2"))
        {
            yield return null;
        }

        yield return null;

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
        {
            yield return null;
        }

        _renderer.enabled = false;

        _selectNextVent = SelectNextVent();
        StartCoroutine(_selectNextVent);
        // 벤트 올라옴
    }

    private IEnumerator SelectNextVent()
    {
        bool isInputUp = true;

        int currentIndex = GetCurrentVentIndex();

        while (isInputUp)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentIndex = GetRightVent(currentIndex);

                this.transform.position = _myVentList[currentIndex].transform.position + new Vector3(0, 0.2f, 0);
                _smoothSync.teleport();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentIndex = GetLeftVent(currentIndex);

                this.transform.position = _myVentList[currentIndex].transform.position + new Vector3(0, 0.2f, 0);
                _smoothSync.teleport();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                isInputUp = false;
            }
            yield return null;
        }

        _myVentList[currentIndex].OnVent();

        yield return new WaitForSeconds(0.15f);

        _renderer.enabled = true;

        UpVent(currentIndex);

        _selectNextVent = null;
    }

    private int GetRightVent(int currentIndex)
    {
        if (_myVentList.Count < 1)
        {
            return GetCurrentVentIndex();
        }

        Vector2 minDistance = new Vector2(float.MaxValue, float.MaxValue);

        int nextIndex = currentIndex;
        int ventIndex = 0;

        foreach (var vent in _myVentList)
        {
            Vector2 distance = vent.transform.position;

            if (_myVentList[currentIndex].transform.position.x < distance.x)
            {
                if (minDistance.x > distance.x)
                {
                    minDistance = distance;
                    nextIndex = ventIndex;
                }
            }

            ventIndex++;
        }

        return nextIndex;
    }

    private int GetLeftVent(int currentIndex)
    {
        if (_myVentList.Count < 1)
        {
            return GetCurrentVentIndex();
        }

        Vector2 maxDistance = new Vector2(-float.MaxValue, -float.MaxValue);

        int nextIndex = currentIndex;
        int ventIndex = 0;

        foreach (var vent in _myVentList)
        {
            Vector2 distance = vent.transform.position;

            if (_myVentList[currentIndex].transform.position.x > distance.x)
            {
                if (maxDistance.x < distance.x)
                {
                    maxDistance = distance;
                    nextIndex = ventIndex;
                }
            }

            ventIndex++;
        }

        return nextIndex;
    }

    private void UpVent(int ventIndex)
    {
        var nextVent = _myVentList[ventIndex];

        this.transform.position = nextVent.transform.position + new Vector3(0, 0.2f, 0);
        _smoothSync.teleport();

        _animator.SetBool("IsSkill_2", false);
        _animator.SetBool("IsSkill_2_Reverse", true);

        if (OnSkillCoroutine != null)
        {
            StopCoroutine(OnSkillCoroutine);
            OnSkillCoroutine = null;
        }

        IsDoingSkill = true;
        OnSkillCoroutine = OnSkill("Skill_2_Reverse");
        StartCoroutine(OnSkillCoroutine);
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
