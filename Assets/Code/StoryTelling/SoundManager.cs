using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource playerAudio;
    public AudioClip cuteTownIntroClip, cuteTownLoopClip;
    public AudioClip firstStageIntroClip, firstStageLoopClip, firstStageHouseIntroClip, firstStageHouseLoopClip;
    public AudioClip bosqueIntroClip, bosqueLoopClip, botasIntroClip, botasLoopClip, secondStageHouseIntro, secondStageHouseLoop;
    public AudioClip secondStageRevelationIntro, secondStageRevelationLoop, platformIntroClip, platformLoopClip;
    public AudioClip bossFightPhaseOneIntroClip, bossFightPhaseOneLoopClip, bossFightPhaseTwoIntroClip, bossFightPhaseTwoLoopClip, syncIntro, syncLoop;

    public void playAudioByScene(string sceneName)
    {
        StartCoroutine(playAudio(sceneName));
    }

    private IEnumerator playAudio(string sceneName)
    {
        if (sceneName.Equals("CuteTown"))
        {
            playerAudio.clip = cuteTownIntroClip;
            playerAudio.Play();
            yield return new WaitForSeconds(7);
            playerAudio.clip = cuteTownLoopClip;
            playerAudio.Play();
        }
        if (sceneName.Equals("FirstStage"))
        {
            playerAudio.clip = firstStageIntroClip;
            playerAudio.Play();
            yield return new WaitForSeconds(5);
            playerAudio.clip = firstStageLoopClip;
            playerAudio.Play();
        }
        if (sceneName.Equals("FirstStageHouse"))
        {
            playerAudio.clip = firstStageHouseIntroClip;
            playerAudio.Play();
            yield return new WaitForSeconds(4);
            playerAudio.clip = firstStageHouseLoopClip;
            playerAudio.Play();
        }
        if (sceneName.Equals("Bosque"))
        {
            playerAudio.clip = bosqueIntroClip;
            playerAudio.Play();
            yield return new WaitForSeconds(9);
            playerAudio.clip = bosqueLoopClip;
            playerAudio.Play();
        }
        if (sceneName.Equals("SecondStage"))
        {
            playerAudio.clip = botasIntroClip;
            playerAudio.Play();
            yield return new WaitForSeconds(12);
            playerAudio.clip = botasLoopClip;
            playerAudio.Play();
        }
        if (sceneName.Equals("SecondStageHouse"))
        {
            playerAudio.clip = secondStageHouseIntro;
            playerAudio.Play();
            yield return new WaitForSeconds(7);
            playerAudio.clip = secondStageHouseLoop;
            playerAudio.Play();
        }
        if (sceneName.Equals("SecondStageRevelation"))
        {
            playerAudio.clip = secondStageRevelationIntro;
            playerAudio.Play();
            yield return new WaitForSeconds(5);
            playerAudio.clip = secondStageRevelationLoop;
            playerAudio.Play();
        }
        if (sceneName.Equals("FinalBoss"))
        {
            playerAudio.clip = platformIntroClip;
            playerAudio.Play();
            yield return new WaitForSeconds(7);
            playerAudio.clip = platformLoopClip;
            playerAudio.Play();
        }
        if (sceneName.Equals("Sync"))
        {
            playerAudio.clip = syncIntro;
            playerAudio.Play();
            yield return new WaitForSeconds(28);
            playerAudio.clip = syncIntro;
            playerAudio.Play();
        }
        if (sceneName.Equals("FinalBossPhaseOne"))
        {
            playerAudio.clip = bossFightPhaseOneIntroClip;
            playerAudio.Play();
            yield return new WaitForSeconds(14);
            playerAudio.clip = bossFightPhaseOneLoopClip;
            playerAudio.Play();
        }
        if (sceneName.Equals("FinalBossPhaseTwo"))
        {
            playerAudio.clip = bossFightPhaseTwoIntroClip;
            playerAudio.Play();
            yield return new WaitForSeconds(22);
            playerAudio.clip = bossFightPhaseTwoLoopClip;
            playerAudio.Play();
        }


        yield return null;
    }

    public void stopAllAudios()
    {
        playerAudio.Stop();
        StopAllCoroutines();
    }
}
