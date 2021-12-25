using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public void Awake()
    {
        StartCoroutine(GoTitle());
    }

    private IEnumerator GoTitle()
    {
        yield return new WaitForSeconds(1f);
        
        SceneManager.LoadScene("RealTitle");
    }
}
