using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    public void TryAgain()
    {
        SceneManager.LoadScene(1);
    }




}
