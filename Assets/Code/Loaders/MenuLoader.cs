using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoader : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    public void loadLevel()
    {
        source.PlayOneShot(clip);
        SceneManager.LoadScene("CuteTown");
    }
}
