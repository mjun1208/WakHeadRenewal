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

        public void Dead(Action deadAction, Vector3 targetPosition)
        {
            _isDeadAction = true;
            Vector3 originalPos = this.transform.position;
            
            Vector3 targetPos = targetPosition;
            targetPos.z = this.transform.position.z;

            Time.timeScale = 0.2f;
            this.transform.position = targetPos;

            _myCamera.DOFieldOfView(15, 0.5f);
            transform.DOMoveX(targetPos.x, 0.5f);
            transform.DOMoveY(targetPos.y, 0.5f);
            
            Global.PoolingManager.LocalSpawn("Blood_1",new Vector3(targetPos.x, targetPos.y, 0), 
                Quaternion.Euler(new Vector3(0, Random.Range(0, 2) == 0 ? 0 : -180)), true);
            Global.PoolingManager.LocalSpawn("Blood_2",new Vector3(targetPos.x, targetPos.y, 0), 
                Quaternion.Euler(new Vector3(0, Random.Range(0, 2) == 0 ? 0 : -180)), true);
            Global.PoolingManager.LocalSpawn("Blood_3", new Vector3(targetPos.x, targetPos.y, 0),
                Quaternion.Euler(new Vector3(0, Random.Range(0, 2) == 0 ? 0 : -180)),true);
            Global.PoolingManager.LocalSpawn("Blood_7",new Vector3(targetPos.x, targetPos.y, 0),
                Quaternion.Euler(new Vector3(0, Random.Range(0, 2) == 0 ? 0 : -180)), true);

            _myCamera.DOShakePosition(0.4f, 0.1f).OnComplete(() =>
            {
                deadAction?.Invoke();
                
                _myCamera.DOFieldOfView(60, 0.5f);
                this.transform.position = originalPos;
                Time.timeScale = 1f;

                _myCamera.DOShakePosition(1.5f, 0.5f).OnComplete(() =>
                {
                    this.transform.position = originalPos;
                    
                    _isDeadAction = false;
                });
                _myCamera.DOShakeRotation(1.5f, 0.5f).OnComplete(() =>
                {
                    this.transform.rotation = Quaternion.identity;
                });
            });
        }
    }
}