using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class AttackRange : MonoBehaviour
    {
        [SerializeField] private GameObject _ownerObject;
        public List<GameObject> CollidedObjectList { get; private set; } = new List<GameObject>();
        public List<GameObject> CollidedSummonedObjectList { get; private set; } = new List<GameObject>();

        public Team MyTeam { get; private set; } = Team.None;

        private void OnEnable()
        {
            CollidedObjectList.Clear();
            MyTeam = Team.None;
        }

        private void OnDisable()
        {
            CollidedObjectList.Clear();
            MyTeam = Team.None;
        }

        public void SetOwner(GameObject owner)
        {
            _ownerObject = owner;
        }

        public void SetTeam(Team team)
        {
            MyTeam = team;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (_ownerObject == null)
            {
                AddCollidedObject(collision.gameObject);
                return;
            }

            if (!_ownerObject.Equals(collision.gameObject))
            {
                AddCollidedObject(collision.gameObject);
            }
        }

        private void AddCollidedObject(GameObject gameObject)
        {
            if (!CollidedObjectList.Contains(gameObject))
            {
                if (gameObject.GetComponent<Entity>() != null &&
                    (gameObject.GetComponent<Entity>().MyTeam == Team.None ||
                     gameObject.GetComponent<Entity>().MyTeam != MyTeam) &&
                    !gameObject.GetComponent<Entity>().IsDead)
                {
                    CollidedObjectList.Add(gameObject);
                }
            }

            if (!CollidedSummonedObjectList.Contains(gameObject))
            {
                if (gameObject.GetComponent<Summoned>() != null &&
                    (gameObject.GetComponent<Summoned>().MyTeam == Team.None ||
                     gameObject.GetComponent<Summoned>().MyTeam != MyTeam))
                {
                    CollidedSummonedObjectList.Add(gameObject);
                }
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (CollidedObjectList.Contains(collision.gameObject))
            {
                CollidedObjectList.Remove(collision.gameObject);
            }

            if (CollidedSummonedObjectList.Contains(collision.gameObject))
            {
                CollidedSummonedObjectList.Remove(collision.gameObject);
            }
        }

        public void Attack(Action<Entity> entityAction, bool singleTarget = false)
        {
            AttackEntity(entityAction, singleTarget);

            if (CollidedSummonedObjectList.Count > 0)
            {
                foreach (var targetObject in CollidedSummonedObjectList)
                {
                    targetObject.GetComponent<Summoned>().Damaged(targetObject.transform.position);

                    if (singleTarget)
                    {
                        return;
                    }
                }
            }
        }

        public void AttackRandom(Action<Entity> entityAction, bool singleTarget = false)
        {
            bool isEntity = false;
            bool isSummoned = false;

            if (CollidedObjectList.Count > 0)
            {
                isEntity = true;
            }

            if (CollidedSummonedObjectList.Count > 0)
            {
                isSummoned = false;
            }

            var attackEntitiy = UnityEngine.Random.Range(0, 2) == 0;

            if (isEntity && !isSummoned)
            {
                attackEntitiy = true;
            }
            else if (!isEntity && isSummoned)
            {
                attackEntitiy = false;
            }

            if (attackEntitiy && isEntity)
            {
                int index = UnityEngine.Random.Range(0, CollidedObjectList.Count);

                var targetObject = CollidedObjectList[index];

                if (targetObject.GetComponent<Entity>().IsDead)
                {
                    CollidedObjectList.Remove(targetObject);
                }

                entityAction?.Invoke(targetObject.GetComponent<Entity>());
            }
            else if (!attackEntitiy && isSummoned)
            {
                int index = UnityEngine.Random.Range(0, CollidedSummonedObjectList.Count);

                var targetObject = CollidedSummonedObjectList[index];

                targetObject.GetComponent<Summoned>().Damaged(targetObject.transform.position);
            }
        }

        public void AttackEntity(Action<Entity> entityAction, bool singleTarget = false)
        {
            if (CollidedObjectList.Count > 0)
            {
                for (int i = 0; i < CollidedObjectList.Count; i++)
                {
                    var targetObject = CollidedObjectList[i];

                    if (targetObject.GetComponent<Entity>().IsDead)
                    {
                        CollidedObjectList.Remove(targetObject);
                    }

                    entityAction?.Invoke(targetObject.GetComponent<Entity>());

                    if (singleTarget)
                    {
                        return;
                    }
                }
            }
        }

        public void AttackSummoned(Action<Summoned> summonedAction = null, bool singleTarget = false)
        {
            if (CollidedSummonedObjectList.Count > 0)
            {
                foreach (var targetObject in CollidedSummonedObjectList)
                {
                    summonedAction?.Invoke(targetObject.GetComponent<Summoned>());

                    if (singleTarget)
                    {
                        return;
                    }
                }
            }
        }
    }
}