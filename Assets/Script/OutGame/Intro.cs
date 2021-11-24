using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WakHead
{
    public class Intro : MonoBehaviour
    {
        public void GoTitle()
        {
            SceneManager.LoadScene("RealTitle");
        }
    }
}
