using Photon.Pun;
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
        if (!photonView.IsMine)
        {
            return;
        }

        base.Update();

        SelectVent();

        for (int i = 0; i < _myVentList.Count; i++)
        {
            if (_myVentList[i].IsDead)
            {
                var vent = _myVentList[i];

                photonView.RPC("SpawnDeadEffect", RpcTarget.All, vent.transform.position);

                _myVentList.Remove(vent);

                PhotonNetwork.Destroy(vent.gameObject);
            }
        }
    }

    protected override void ForceStop()
    {
        base.ForceStop();
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
            targetEntity.KnockBack(15, -GetAttackDir(), 1f, 0);
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

    [PunRPC]
    private void Hide(bool isTrue)
    {
        _renderer.enabled = !isTrue;
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

        photonView.RPC("Hide", RpcTarget.All, true);

        // 벤트 선택

        _selectNextVent = SelectNextVent();
        StartCoroutine(_selectNextVent);
    }

    private IEnumerator SelectNextVent()
    {
        bool isInputUp = true;

        int currentIndex = GetCurrentVentIndex();

        Vector3 lastVentPos = _currentVent.transform.position + new Vector3(0, 0.2f, 0);

        while (isInputUp)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentIndex = GetRightVent(currentIndex);

                _currentVent = _myVentList[currentIndex];

                lastVentPos = _currentVent.transform.position + new Vector3(0, 0.2f, 0);

                this.transform.position = lastVentPos;
                _smoothSync.teleport();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentIndex = GetLeftVent(currentIndex);

                _currentVent = _myVentList[currentIndex];

                lastVentPos = _currentVent.transform.position + new Vector3(0, 0.2f, 0);

                this.transform.position = lastVentPos;
                _smoothSync.teleport();
            }

            if (Input.GetKeyDown(KeyCode.C) || _currentVent == null)
            {
                isInputUp = false;
            }
            yield return null;
        }

        // 벤트 올라옴
        if (_currentVent == null)
        {
            _currentVent.OnVent();
        }

        yield return new WaitForSeconds(0.15f);

        photonView.RPC("Hide", RpcTarget.All, false);

        UpVent(lastVentPos);

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

    private void UpVent(Vector3 lastVentPos)
    {
        // var nextVent = _myVentList[ventIndex];

        if (_currentVent != null)
        {
            this.transform.position = _currentVent.transform.position + new Vector3(0, 0.2f, 0);
        }
        else
        {
            this.transform.position = lastVentPos;
        }
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
            if (collision.GetComponent<Martine_Vent>().photonView.IsMine)
            {
                _colliedVent.Add(collision);
            }
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

    [PunRPC]
    public void SpawnDeadEffect(Vector3 pos)
    {
        Global.PoolingManager.LocalSpawn("DeathEffect", pos, Quaternion.identity, true);
    }

    protected override void Dead()
    {
        base.Dead();

        _currentVent = null;
        _colliedVent.Clear();
    }
}
