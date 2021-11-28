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
            CollidedSummonedObjectList.Clear();
            MyTeam = Team.None;
        }

        private void OnDisable()
        {
            CollidedObjectList.Clear();
            CollidedSummonedObjectList.Clear();
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

        public void Attack(Action<Entity> entityAction, Team team, bool singleTarget = false, string effectName = "HitEffect", float effectXOffset = 0f, bool effectFlip = false)
        {
            AttackEntity(entityAction, team, singleTarget);

            if (CollidedSummonedObjectList.Count > 0)
            {
                foreach (var targetObject in CollidedSummonedObjectList)
                {
                    var summoned = targetObject.GetComponent<Summoned>();
                    
                    if (summoned.MyTeam != Team.None && summoned.MyTeam == team)
                    {
                        continue;
                    }

                    summoned.Damaged(targetObject.transform.position, MyTeam, effectName, effectXOffset, effectFlip);

                    if (singleTarget)
                    {
                        return;
                    }
                }
            }
        }

        public void AttackEntity(Action<Entity> entityAction, Team team, bool singleTarget = false)
        {
            if (CollidedObjectList.Count > 0)
            {
                for (int i = 0; i < CollidedObjectList.Count; i++)
                {
                    var targetObject = CollidedObjectList[i];

                    var entity = targetObject.GetComponent<Entity>();
                    
                    if (entity.MyTeam != Team.None && entity.MyTeam == team)
                    {
                        continue;
                    }

                    if (entity.IsDead)
                    {
                        CollidedObjectList.Remove(targetObject);
                    }

                    entityAction?.Invoke(entity);

                    if (singleTarget)
                    {
                        return;
                    }
                }
            }
        }

        public void AttackSummoned(Action<Summoned> summonedAction, Team team, bool singleTarget = false)
        {
            if (CollidedSummonedObjectList.Count > 0)
            {
                foreach (var targetObject in CollidedSummonedObjectList)
                {
                    var summoned = targetObject.GetComponent<Summoned>();
                    
                    if (summoned.MyTeam != Team.None && summoned.MyTeam == team)
                    {
                        continue;
                    }
                    
                    summonedAction?.Invoke(summoned);

                    if (singleTarget)
                    {
                        return;
                    }
                }
            }
        }
    }
}