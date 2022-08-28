using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static BrackeysGameJam.Globals;
public class PlayerDeath : MonoBehaviour
{
    public float waitBeforeTransition = 2f;
    
    public void OnPlayerDeath()
    {
        gameManager.StartCoroutine(DeathCoroutine());
    }

    public IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(waitBeforeTransition);
        SceneManager.LoadScene(1);
    }
}
