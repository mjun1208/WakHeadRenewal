using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Smooth;

public abstract class Actor : Entity, IPunObservable
{
    [SerializeField] protected Animator _animator;
    [SerializeField] protected SmoothSyncPUN2 _smoothSync;
    [SerializeField] protected SpriteRenderer _renderer;

    [SerializeField] protected AttackRange _attackRange;
    [SerializeField] protected AttackRange _skill_1Range;
    [SerializeField] protected AttackRange _skill_2Range;

    protected float _attackMoveSpeed = 5f;
    protected float _moveSpeed = 10f;

    protected bool _isMove = false;
    protected bool _isMoveInput = false;
    protected bool _isAttack = false;
    protected bool _isAttackInput = false;

    private Vector3 _Movedir = Vector3.zero;

    protected bool _isSkill_1Input = false;
    protected bool _isSkill_2Input = false;

    protected bool _isSkill_1 = false;
    protected bool _isSkill_2 = false;

    protected Vector3 _originalScale = Vector3.zero;

    protected bool _isDoingSkill = false;

    protected IEnumerator _onSkillCoroutine = null;

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);

        if (stream.IsWriting)
        {
            stream.SendNext(this.transform.localScale.x);
        }
        else
        {
            var scale_x = (float)stream.ReceiveNext();
            this.transform.localScale = new Vector3(scale_x, this.transform.localScale.y, this.transform.localScale.z);
        }
    }

    protected virtual void Start()
    {
        if (!photonView.IsMine)
        {
            this.gameObject.layer = 9; // Enemy;
            return;
        }
        this.gameObject.layer = 8; // Player;

        CameraManager.instance.SetTarget(this.transform);
        _originalScale = this.transform.localScale;
    }

    protected virtual void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            _isAttack = true;
        }
        else
        {
            _isAttack = false;
        }

        KeyInput();

        if (!_isDoingSkill)
        {
            Move();

            Attack();

            if (!_isAttackInput)
            {
                if (_isSkill_1Input)
                {
                    OnSkill_1();
                    return;
                }

                if (_isSkill_2Input)
                {
                    OnSkill_2();
                    return;
                }
            }
        }
    }

    protected virtual void KeyInput()
    {
        MoveInput();

        AttackInput();

        if (!_isAttackInput)
        {
            Skill_1Input();
            Skill_2Input();
        }
    } 

    private void MoveInput()
    {
        _isMoveInput = false;

        _Movedir = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _Movedir = Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _Movedir = Vector3.right;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            _Movedir += Vector3.up * 0.6f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            _Movedir += Vector3.down * 0.6f;
        }

        if (_Movedir != Vector3.zero)
        {
            _isMoveInput = true;
        }
    }

    private void Move()
    {
        _isMove = false;

        if (_Movedir != Vector3.zero)
        {
            _isMove = true;
        }

        if (_isMove && _Movedir.x != 0)
        {
            float rotationScale = _originalScale.x * _Movedir.x;
            this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);
        }

        float moveSpeed = _isAttack ? _attackMoveSpeed : _moveSpeed;

        _rigid.MovePosition(transform.position + _Movedir * moveSpeed * Time.deltaTime);
        _animator.SetBool("IsWalk", _isMove);
    }

    protected virtual void AttackInput()
    {
        _isAttackInput = false;

        if (Input.GetKey(KeyCode.Z))
        {
            _isAttackInput = true;
        }
    }

    protected virtual void Attack()
    {
        _animator.SetBool("IsAttack", _isAttackInput);
    }


    protected virtual bool Skill_1Input()
    {
        _isSkill_1Input = false;

        if (Input.GetKeyDown(KeyCode.X))
        {
            _isSkill_1Input = true;
        }

        return _isSkill_1Input;
    }

    protected virtual void OnSkill_1()
    {
        if (_isSkill_1Input)
        {
            _isSkill_1 = true;

            if (_onSkillCoroutine == null)
            {
                _isDoingSkill = true;
                _onSkillCoroutine = OnSkill("Skill_1");
                StartCoroutine(_onSkillCoroutine);

                _animator.SetBool("IsSkill_1", true);
            }
            else
            {
                _isSkill_1 = false;
            }
        }
    }

    protected virtual bool Skill_2Input()
    {
        _isSkill_2Input = false;

        if (Input.GetKeyDown(KeyCode.C))
        {
            _isSkill_2Input = true;
        }

        return _isSkill_2Input;
    }

    protected virtual void OnSkill_2()
    {
        if (_isSkill_2Input)
        {
            _isSkill_2 = true;

            if (_onSkillCoroutine == null)
            {
                _isDoingSkill = true;
                _onSkillCoroutine = OnSkill("Skill_2");
                StartCoroutine(_onSkillCoroutine);

                _animator.SetBool("IsSkill_2", true);
            }
            else
            {
                _isSkill_2 = false;
            }
        }
    }

    protected IEnumerator OnSkill(string name)
    {
        _isDoingSkill = true;

        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            yield return null;
        }

        yield return null;

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        // end

        _isDoingSkill = false;
        _isSkill_1 = false;
        _isSkill_2 = false;

        _animator.SetBool("Is" + name, false);

        _onSkillCoroutine = null;
    }

    protected void SkillCancle()
    {
        _isDoingSkill = false;
        _isSkill_1 = false;
        _isSkill_2 = false;

        _animator.SetBool("IsSkill_1", false);
        _animator.SetBool("IsSkill_2", false);

        if (_onSkillCoroutine != null)
        {
            StopCoroutine(_onSkillCoroutine);
            _onSkillCoroutine = null;
        }
    }

    protected virtual void Active_Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }

    protected virtual void Active_Skill_1()
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }

    protected virtual void Active_Skill_2()
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }

    protected Vector3 GetAttackDir()
    {
        float dir = transform.localScale.x > 0 ? 1 : -1;

        Vector3 attackDir = new Vector3(dir, 0, 0);

        return attackDir;
    }
}
