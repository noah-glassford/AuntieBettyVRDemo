using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMenuScript : MonoBehaviour
{
    
    public void ContinueToExperience()
    {
        SceneManager.LoadScene(1);
    }
}
