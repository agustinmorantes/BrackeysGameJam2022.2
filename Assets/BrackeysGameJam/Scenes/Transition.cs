using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public float transitionTime = 5f;
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(0);
    }
}
