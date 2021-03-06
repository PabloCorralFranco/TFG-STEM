﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject generator, compiler, wiki;
    public AudioSource playerAudio;
    public AudioClip openCloseAudio;

    public void stopTime()
    {
        openClose();
        if (menu.activeSelf == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void openClose()
    {
        playAudio();
        menu.SetActive(!menu.activeSelf);
    }
    public void openCloseGenerator()
    {
        playAudio();
        compiler.SetActive(!compiler.activeSelf);
        generator.SetActive(!generator.activeSelf);
        openClose();
    }
    public void openCloseCompiler()
    {
        playAudio();
        compiler.SetActive(!compiler.activeSelf);
        openClose();
    }
    public void openCloseWiki()
    {
        playAudio();
        wiki.SetActive(!wiki.activeSelf);
        openClose();
    }

    private void playAudio()
    {
        playerAudio.PlayOneShot(openCloseAudio);
    }
}
