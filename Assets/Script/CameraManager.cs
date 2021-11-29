using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WakHead
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager instance;
        public Transform TargetTransform { get; set; }

        [SerializeField] private Camera _myCamera;
        private bool _isDeadAction = false;
        private Entity _deadEntity = null;
        
        void Awake()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (TargetTransform != null && !_isDeadAction)
            {
                MoveToTarget();
            }
            else if (_isDeadAction)
            {
                MoveToDeadTarget();
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
            targetPos.y = this.transform.position.y;

            if (Mathf.Abs(targetPos.x) > 10)
            {
                targetPos.x = targetPos.x > 0f ? 10f : -10f;
            }

            this.transform.position = Vector3.Lerp(this.transform.position, targetPos, 10f * Time.deltaTime);
        }

        private void MoveToDeadTarget()
        {
            if (_deadEntity != null)
            {
                Vector3 targetPos = _deadEntity.transform.position;
                targetPos.z = this.transform.position.z;

                this.transform.position = targetPos; // Vector3.Lerp(this.transform.position, targetPos, 25f * Time.deltaTime);
            }
        }

        public void Dead(Action deadAction, Entity targetEntity)
        {
            _isDeadAction = true;
            Vector3 originalPos = this.transform.position;
            
            Vector3 targetPos = targetEntity.transform.position;
            targetPos.z = this.transform.position.z;

            Time.timeScale = 0.2f;
            this.transform.position = targetPos;

            DOTween.Sequence()
                .Append(_myCamera.DOFieldOfView(15, 0.5f))
                // .Join(transform.DOMoveX(targetPos.x, 0.5f).OnUpdate(() =>
                // {
                //     targetPos = targetEntity.transform.position;
                //     targetPos.z = this.transform.position.z;
                // }))
                // .Join(transform.DOMoveY(targetPos.y, 0.5f).OnUpdate(() =>
                // {
                //     targetPos = targetEntity.transform.position;
                //     targetPos.z = this.transform.position.z;
                // }))
                .Join(_myCamera.DOShakePosition(0.1f, 0.1f, 180).SetLoops(5, LoopType.Restart)
                    .OnUpdate(() => {
                        targetPos = targetEntity.transform.position;
                        targetPos.z = this.transform.position.z;

                        Time.timeScale = 0.2f;
                        this.transform.position = targetPos;
                    })
                    .OnComplete(() =>
                    {
                        deadAction?.Invoke();
                        _myCamera.DOFieldOfView(60, 0.5f);

                        Time.timeScale = 1f;
                    }))
                .Append(_myCamera.DOShakePosition(0.75f, 0.5f, 30, 90f, false).OnComplete(() =>
                {
                    _isDeadAction = false;
                    _deadEntity = null;
                    
                    this.transform.position = originalPos;
                }))
                .Append(_myCamera.DOShakePosition(0.75f, 0.5f, 30));

            var blood_1 = Global.PoolingManager.LocalSpawn("Blood_1",new Vector3(targetPos.x, targetPos.y, 0), 
                Quaternion.Euler(new Vector3(0, Random.Range(0, 2) == 0 ? 0 : -180)), true);
            var blood_2 = Global.PoolingManager.LocalSpawn("Blood_2",new Vector3(targetPos.x, targetPos.y, 0), 
                Quaternion.Euler(new Vector3(0, Random.Range(0, 2) == 0 ? 0 : -180)), true);
            var blood_3 = Global.PoolingManager.LocalSpawn("Blood_3", new Vector3(targetPos.x, targetPos.y, 0),
                Quaternion.Euler(new Vector3(0, Random.Range(0, 2) == 0 ? 0 : -180)),true);
            var blood_4 = Global.PoolingManager.LocalSpawn("Blood_7",new Vector3(targetPos.x, targetPos.y, 0),
                Quaternion.Euler(new Vector3(0, Random.Range(0, 2) == 0 ? 0 : -180)), true);

            blood_1.transform.parent = targetEntity.transform;
            blood_2.transform.parent = targetEntity.transform;
            blood_3.transform.parent = targetEntity.transform;
            blood_4.transform.parent = targetEntity.transform;
        }
    }
}