using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class BackgroundObject : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            this.transform.localScale = Random.Range(0, 2) == 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        }
    }
}