using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class EventManager : MonoBehaviour
{
    public Dictionary<GameObject, int> npcsPhases;
    public Koke Koke;
    public Barbara Barbara;
    public bool isTaskPending = true;
    public GameObject punishEffect, bossSpawnEffect, arcaelum, playerPrison, awakenDash, awakenTime, awakenBullet;

    private Player player;
    private GameObject loadScreen;
    private bool canActivatePuzzle;
    private Vector3 barbHouse, kokeHouse;
    private int[] kokePhases, barbaraPhases;
    private bool barbExtinted, kokeExtinted, firstTime;
    private int puzzleGen;
    private SoundManager soundManager;

    private void Start()
    {
        //Añadimos al inicio del juego la fase en la que se encuentra cada npc, para mostrar las conversaciones pertinentes
        player = GameObject.FindObjectOfType<Player>();
        Koke = GameObject.FindObjectOfType<Koke>();
        Barbara = GameObject.FindObjectOfType<Barbara>();
        loadScreen = player.transform.Find("LoadScreen").gameObject;
        soundManager = FindObjectOfType<SoundManager>();
        firstTime = true;
    }

    public void oliviaHouse()
    {
        StartCoroutine(oliviaTask());
    }

    private IEnumerator oliviaTask()
    {
        player.stopFromMoving();
        NPC npc = GameObject.FindGameObjectWithTag("Respawn").GetComponent<NPC>();
        npc.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        GameObject.FindGameObjectWithTag("spawnLocation").GetComponent<BoxCollider2D>().enabled = true;
        player.continueMoving();
        yield return null;
    }

    public void LoadCuteTownLevel()
    {
        StartCoroutine(transitionToNewLevel("CuteTown"));
    }

    public void alcaldeCuteTown()
    {
        StartCoroutine(ayudaAlcalde());
    }

    private IEnumerator ayudaAlcalde()
    {
        player.stopFromMoving();
        NPC npc = GameObject.FindGameObjectWithTag("Respawn").GetComponent<NPC>();
        npc.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        GameObject.FindGameObjectWithTag("FirstWall").GetComponent<BoxCollider2D>().enabled = true;
        player.continueMoving();
        yield return null;
    }
    public void loadFirstStage()
    {
        StartCoroutine(transitionToNewLevel("FirstStageFirstTime"));
    }


    public void kokeFirstAct()
    {
        //Congelamos el movimiento del jugador
        player.stopFromMoving();
        Koke npc = Koke.GetComponent<Koke>();
        StartCoroutine(waitForTaskEndFirstAct(npc));

    }

    public IEnumerator waitForTaskEndFirstAct(Koke npc)
    {
        //Triggereamos la primera conversación de koke.
        npc.popUpMeeting();
        //Esperamos a que acabe la conversacion
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        //Cuando acabe le cambiamos la fase de conversacion
        npc.conversationPhase += 1;
        yield return new WaitForSeconds(1f);
        npc.destroyAndScalate();
        yield return new WaitForSeconds(1f);
        npc.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        npc.conversationPhase += 1;
        //Lo movemos fuera del mapa
        npc.MoveToPosition();
        yield return new WaitForSeconds(6f);
        npc.gameObject.SetActive(false);
        Destroy(GameObject.FindGameObjectWithTag("FirstWall"));
        player.continueMoving();
        isTaskPending = true;
    }

    public void Wall(string tag)
    {
        //Congelamos el movimiento del jugador
        player.stopFromMoving();
        Debug.Log(tag);
        //Debug.Log(npc.name);
        StartCoroutine(cantGoThroughWall(tag));
    }

    private IEnumerator cantGoThroughWall(string tag)
    {
        GameObject[] npc = GameObject.FindGameObjectsWithTag("FirstWall");
        npc[1].GetComponent<NPC>().popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        isTaskPending = false;
        player.continueMoving();
        yield return null;
    }

    public void kokeFirstActHouse()
    {
        //Congelamos el movimiento del jugador
        player.stopFromMoving();
        Koke npc = Koke.GetComponent<Koke>();
        Barbara barbnpc = Barbara.GetComponent<Barbara>();
        StartCoroutine(waitForTaskEndFirstActHouse(npc,barbnpc));

    }

    private IEnumerator waitForTaskEndFirstActHouse(Koke npc, Barbara barbnpc)
    {
        //Triggereamos la primera conversación de koke.
        npc.popUpMeeting();
        //Esperamos a que acabe la conversacion
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        //Cuando acabe le cambiamos la fase de conversacion
        npc.conversationPhase += 1;
        npc.extinted = false;
        //Movemos a Koke y Barbara a sus posiciones
        barbnpc.MoveToTable();
        npc.MoveToBed();
        canActivatePuzzle = true;
        yield return new WaitForSeconds(3f);
        //Cogemos sus posiciones para la carga.
        kokeHouse = npc.transform.position;
        barbHouse = barbnpc.transform.position;
        //Activamos el menu para la wiki
        GameObject.FindGameObjectWithTag("Movement").transform.Find("MenuButton").gameObject.SetActive(true);
        GameObject.FindGameObjectWithTag("Movement").transform.Find("Life").gameObject.SetActive(true);
        player.continueMoving();
        player.canAttack = true;
        //Termina la conversacion conjunta y ahora para desactivar las puertas de las granjas Olivia tiene que hablar con el Barbara
        yield return null;
    }

    public void genPuzzleFinished()
    {
        GameObject whereToSpawn = GameObject.FindGameObjectWithTag("spawnLocation");
        whereToSpawn.transform.Find("Koke").gameObject.SetActive(true);
        Koke kokenpc = whereToSpawn.transform.Find("Koke").GetComponent<Koke>();
        whereToSpawn.transform.Find("Barbara").gameObject.SetActive(true);
        Barbara barbnpc = whereToSpawn.transform.Find("Barbara").GetComponent<Barbara>();
        StartCoroutine(genPuzzleEvent(kokenpc,barbnpc));
    }

    private IEnumerator genPuzzleEvent(Koke kokenpc, Barbara barbnpc)
    {
        player.stopFromMoving();
        kokenpc.conversationPhase = 4;
        yield return new WaitForSeconds(2);
        kokenpc.MoveToGenerator();        
        yield return new WaitForSeconds(1.5f);
        barbnpc.MoveToGenerator();
        yield return new WaitForSeconds(1f);
        kokenpc.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        kokenpc.conversationPhase += 1;
        kokenpc.extinted = false;
        //Metemos a Barbara en casa. Cerramos la puerta, abrimos la del bosque y mandamos a Koke al Bosque. Ponemos las habilidades activas
        GameObject abilities = GameObject.FindGameObjectWithTag("AbilitieManager");
        for(int i = 0; i < abilities.transform.childCount; i++)
        {
            abilities.transform.GetChild(i).gameObject.SetActive(true);
        }
        barbnpc.backToHouse();
        kokenpc.MoveToForest();
        yield return new WaitForSeconds(3f);
        barbnpc.gameObject.SetActive(false);
        kokenpc.gameObject.SetActive(false);
        GameObject barreras = GameObject.FindGameObjectWithTag("barrerasHolder");
        for(int i = 0; i < barreras.transform.childCount; i++)
        {
            barreras.transform.GetChild(i).gameObject.SetActive(true);
        }
        GameObject.FindGameObjectWithTag("BarreraBosque").SetActive(false);
        player.continueMoving();
        //Cuando acabemos le damos los modulos de generador y compilador
        GameObject generalMenu = player.transform.Find("GeneralMenu").gameObject;
        Button generator = generalMenu.transform.Find("BotonGenerador").GetComponent<Button>();
        generator.interactable = true;
        generator.GetComponentInChildren<TextMeshProUGUI>().text = "GENERADOR";
        Button compilador = generalMenu.transform.Find("BotonCompilador").GetComponent<Button>();
        compilador.interactable = true;
        compilador.GetComponentInChildren<TextMeshProUGUI>().text = "COMPILADOR";

    }


    public void LoadHouseLevel()
    {
        StartCoroutine(transitionToNewLevel("FirstStageHouse"));
    }
    public void LoadSlimesLevel()
    {
        StartCoroutine(transitionToNewLevel("FirstStage"));
    }

    public IEnumerator transitionToNewLevel(string lvlName)
    {
        player.stopFromMoving();
        soundManager.stopAllAudios();
        //Antes de salir cogemos en que estado de conversacion se han quedado
        if (lvlName.Equals("FirstStage"))
        {
            Koke = GameObject.FindObjectOfType<Koke>();
            Barbara = GameObject.FindObjectOfType<Barbara>();
            kokePhases = new int[] { Koke.conversationPhase, Koke.startingPhase, Koke.repeatedPhase, Koke.negationPhase };
            kokeExtinted = Koke.extinted;
            barbaraPhases = new int[] { Barbara.conversationPhase, Barbara.startingPhase, Barbara.repeatedPhase, Barbara.negationPhase };
            barbExtinted = Barbara.extinted;
        }
        if (lvlName.Equals("FirstStageHouse"))
        {
            puzzleGen = FindObjectOfType<GeneticManager>().genPhase;
        }
        if (lvlName.Equals("FirstStageFirstTime"))
        {
            lvlName = "FirstStage";
        }
        yield return new WaitForSeconds(1f);
        loadScreen.SetActive(!loadScreen.activeSelf);
        SceneManager.LoadSceneAsync(lvlName, LoadSceneMode.Single);
        SceneManager.sceneLoaded += findActualNPCs;
        yield return new WaitForSeconds(3f);
        //Ponemos la musica segun el nivel
        soundManager.playAudioByScene(lvlName);
        loadScreen.SetActive(!loadScreen.activeSelf);
        player.continueMoving();
        yield return null;
    }

    private void findActualNPCs(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("CuteTown")) {
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().InvalidatePathCache();
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.FindGameObjectWithTag("Confiner").GetComponent<PolygonCollider2D>();
        }
        if (scene.name.Equals("FirstStage") && firstTime)
        {
            Koke = GameObject.FindObjectOfType<Koke>();
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().InvalidatePathCache();
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.FindGameObjectWithTag("Confiner").GetComponent<PolygonCollider2D>();
        }
        //Con esto mantenemos el estado de la escena abierta
        if (canActivatePuzzle && scene.name.Equals("FirstStage") && !firstTime)
        {
            //Reactivamos los colisionadores para los pilares
            Debug.Log("Conseguimos los pilares");
            PilarMatrix[] pilars = FindObjectsOfType<PilarMatrix>();
            for (int i = 0; i < pilars.Length; i++)
            {
                pilars[i].reactivateCollider();
            }
            //Activamos el collider para el generador
            BoxCollider2D genManagerCollider = FindObjectOfType<GeneticManager>().gameObject.GetComponent<BoxCollider2D>();
            genManagerCollider.enabled = true;
            GameObject[] barrerasGranja = GameObject.FindGameObjectsWithTag("barreraGranja");
            for(int i = 0; i < barrerasGranja.Length; i++)
            {
                barrerasGranja[i].SetActive(!barrerasGranja[i].gameObject.activeSelf);
            }
            GameObject[] storyTriggers = GameObject.FindGameObjectsWithTag("DontComeBack");
            for (int i = 0; i < storyTriggers.Length; i++)
            {
                storyTriggers[i].SetActive(!storyTriggers[i].gameObject.activeSelf);
            }
            GameObject[] firstWalls = GameObject.FindGameObjectsWithTag("FirstWall");
            for(int i = 0; i < firstWalls.Length; i++)
            {
                firstWalls[i].SetActive(false);
            }
            //Koke = GameObject.FindObjectOfType<Koke>();
            //Koke.gameObject.SetActive(false);
            FindObjectOfType<GeneticManager>().genPhase = puzzleGen;
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.FindGameObjectWithTag("Confiner").GetComponent<PolygonCollider2D>();
            //Ponemos a koke en su sitio para el evento de finalizacion
            Koke = GameObject.FindObjectOfType<Koke>();
            Koke.gameObject.SetActive(false);
            GameObject whereToSpawn = GameObject.FindGameObjectWithTag("spawnLocation");
            Koke.gameObject.transform.position = whereToSpawn.transform.position;
            Koke.gameObject.transform.SetParent(whereToSpawn.transform);
            //canActivatePuzzle = false;

        }
        if(canActivatePuzzle && scene.name.Equals("FirstStageHouse"))
        {
            Koke = GameObject.FindObjectOfType<Koke>();
            Barbara = GameObject.FindObjectOfType<Barbara>();
            Koke.transform.position = kokeHouse;
            Barbara.transform.position = barbHouse;
            Koke.GetComponent<Animator>().SetFloat("Horizontal", -1);
            Koke.GetComponent<Animator>().SetFloat("Vertical", 0);
            GameObject.FindGameObjectWithTag("DontComeBack").SetActive(false);
            Koke.extinted = kokeExtinted;
            Barbara.extinted = barbExtinted;
            kokePhases[0] = Koke.conversationPhase;
            kokePhases[1] = Koke.startingPhase;
            kokePhases[2] = Koke.repeatedPhase;
            kokePhases[3] = Koke.negationPhase;
            barbaraPhases[0] = Barbara.conversationPhase;
            barbaraPhases[1] = Barbara.startingPhase;
            barbaraPhases[2] = Barbara.repeatedPhase;
            barbaraPhases[3] = Barbara.negationPhase;
        }
        if (!canActivatePuzzle && scene.name.Equals("FirstStageHouse"))
        {
            Koke = GameObject.FindObjectOfType<Koke>();
            Barbara = GameObject.FindObjectOfType<Barbara>();
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().InvalidatePathCache();
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.FindGameObjectWithTag("Confiner").GetComponent<PolygonCollider2D>();
            firstTime = false;
        }
        if(!canActivatePuzzle && scene.name.Equals("FirstStage") && !firstTime)
        {
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().InvalidatePathCache();
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.FindGameObjectWithTag("Confiner").GetComponent<PolygonCollider2D>();
            GameObject[] storyTriggers = GameObject.FindGameObjectsWithTag("DontComeBack");
            for (int i = 0; i < storyTriggers.Length; i++)
            {
                storyTriggers[i].SetActive(!storyTriggers[i].gameObject.activeSelf);
            }
            GameObject[] firstWalls = GameObject.FindGameObjectsWithTag("FirstWall");
            for (int i = 0; i < firstWalls.Length; i++)
            {
                firstWalls[i].SetActive(false);
            }
            Koke = GameObject.FindObjectOfType<Koke>();
            Koke.gameObject.SetActive(false);
        }
        if (scene.name.Equals("Bosque"))
        {
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().InvalidatePathCache();
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.FindGameObjectWithTag("Confiner").GetComponent<PolygonCollider2D>();
            Koke = FindObjectOfType<Koke>();
            Koke.GetComponent<Animator>().SetFloat("Horizontal", 1);
            Koke.GetComponent<Animator>().SetFloat("Vertical", 0);
        }
        if (scene.name.Equals("SecondStage"))
        {
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().InvalidatePathCache();
            PolygonCollider2D collider = GameObject.FindGameObjectWithTag("Confiner").transform.Find("confinerBotas").GetComponent<PolygonCollider2D>();
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().m_BoundingShape2D = collider;
        }
        if (scene.name.Equals("FinalBoss"))
        {
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().InvalidatePathCache();
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.FindGameObjectWithTag("Confiner").GetComponent<PolygonCollider2D>();
            player.transform.Find("VirtualCam").GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 1.8f;
        }
        player.transform.position = GameObject.FindGameObjectWithTag("spawnLocation").gameObject.transform.position;
        if(scene.name.Equals("FirstStage") && firstTime)
        {
            player.transform.position = GameObject.FindGameObjectWithTag("Respawn").gameObject.transform.position;
        }
    }

    public void transportToMaria()
    {
        StartCoroutine(transitionToNewLevel("SecondStage"));
    }

    public void transportToEnemyZone()
    {
        StartCoroutine(enemyZone());
    }

    private IEnumerator enemyZone()
    {
        player.stopFromMoving();
        Destroy(Instantiate(punishEffect, player.transform.position, Quaternion.identity),1f);
        yield return new WaitForSeconds(1f);
        Vector3 myLastPosition = player.transform.position;
        primEnemySpawner spawner = GameObject.FindGameObjectWithTag("enemySpawnZoneForest").GetComponent<primEnemySpawner>();
        loadScreen.SetActive(!loadScreen.activeSelf);
        player.transform.position = spawner.gameObject.transform.position;
        yield return new WaitForSeconds(1f);
        loadScreen.SetActive(!loadScreen.activeSelf);
        spawner.canSpawn = true;
        player.continueMoving();
        //Hacemos que la jugadora este 30 segundos peleando.
        yield return new WaitForSeconds(30f);
        spawner.canSpawn = false;
        player.stopFromMoving();
        loadScreen.SetActive(!loadScreen.activeSelf);
        yield return new WaitForSeconds(1f);
        player.transform.position = myLastPosition;
        yield return new WaitForSeconds(1f);
        loadScreen.SetActive(!loadScreen.activeSelf);
        player.continueMoving();

    }

    public void kokeInTheWoods()
    {
        StartCoroutine(woodsConversation());
    }

    private IEnumerator woodsConversation()
    {
        player.stopFromMoving();
        Koke kokenpc = FindObjectOfType<Koke>();
        kokenpc.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        kokenpc.conversationPhase += 1;
        kokenpc.extinted = false;
        //Ahora se iria y desapareceria
        kokenpc.exitForest();
        GameObject wiki = player.transform.Find("Wiki").gameObject;
        wiki.transform.Find("Circuito").gameObject.SetActive(true);
        wiki.transform.Find("Grafos").gameObject.SetActive(true);
        wiki.transform.Find("Prim").gameObject.SetActive(true);
        wiki.transform.Find("MariaButton").gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        kokenpc.gameObject.SetActive(false);
        player.continueMoving();
        yield return null;
    }

    public void LoadBosque()
    {
        StartCoroutine(transitionToNewLevel("Bosque"));
    }

    public void botasConversation()
    {
        StartCoroutine(botasDialogue());
    }

    private IEnumerator botasDialogue()
    {
        player.stopFromMoving();
        NPC botas = GameObject.FindGameObjectWithTag("Botas").GetComponent<NPC>(); ;
        botas.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        //Ahora transportamos a Olivia a la zona de la casa de Maria
        Destroy(Instantiate(punishEffect, player.transform.position + new Vector3(0,0.1f,0), Quaternion.identity), 1f);
        yield return new WaitForSeconds(1f);
        loadScreen.SetActive(!loadScreen.activeSelf);
        Vector3 nposition = GameObject.FindGameObjectWithTag("enemySpawnZoneForest").transform.position;
        player.transform.position = nposition;
        Inventory inv = FindObjectOfType<Inventory>();
        inv.blueEsence += 50;
        inv.redEsence += 50;
        inv.greenEsence += 50;
        GameObject wiki = player.transform.Find("Wiki").gameObject;
        wiki.transform.Find("Botas").gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().InvalidatePathCache();
        PolygonCollider2D collider = GameObject.FindGameObjectWithTag("Confiner").transform.Find("confinerMaria").GetComponent<PolygonCollider2D>();
        player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().m_BoundingShape2D = collider;
        soundManager.playAudioByScene("SecondStageHouse");
        loadScreen.SetActive(!loadScreen.activeSelf);
        player.continueMoving();
        yield return null;
    }

    public void houseOnFire()
    {
        StartCoroutine(SecondActHouseFire());
    }

    private IEnumerator SecondActHouseFire()
    {
        //Conversacion con Koke a las puertas de la casa en llamas.
        player.stopFromMoving();
        Koke = FindObjectOfType<Koke>();
        Koke.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        //Moveriamos a Koke
        Koke.enterBurnedHouse();
        yield return new WaitForSeconds(1f);
        //Ahora es conversacion sola Olivia.
        Koke.conversationPhase += 1;
        Koke.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Koke.conversationPhase += 1;
        soundManager.stopAllAudios();
        soundManager.playAudioByScene("SecondStageRevelation");
        //Teletransportamos a Olivia con un gradiente blanco.
        //Reaprovechamos Loading Screen y lo ponemos en blanco a 0 de transparencia
        loadScreen.transform.Find("LoadingSprite").gameObject.SetActive(false);
        Image fondoBlanco = loadScreen.transform.Find("FondoNegro").GetComponent<Image>();
        fondoBlanco.color = new Color(255, 255, 255, 0);
        loadScreen.SetActive(!loadScreen.activeSelf);
        float transparency = 0.05f;
        while (fondoBlanco.color.a < 1)
        {
            fondoBlanco.color = new Color(255, 255, 255, transparency);
            transparency += 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
        //Termina el fade y transportamos a Olivia
        player.transform.position = GameObject.FindGameObjectWithTag("revelation").transform.position;
        //Buscamos el confiner
        PolygonCollider2D confinador = GameObject.FindGameObjectWithTag("Confiner").transform.Find("confinerRevelation").GetComponent<PolygonCollider2D>();
        player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().m_BoundingShape2D = confinador;
        yield return new WaitForSeconds(2f);
        Debug.Log("Llegamos");
        while (fondoBlanco.color.a > 0)
        {
            transparency -= 0.05f;
            fondoBlanco.color = new Color(255, 255, 255, transparency);
            yield return new WaitForSeconds(0.1f);
        }
        loadScreen.SetActive(!loadScreen.activeSelf);
        player.continueMoving();
        yield return null;
    }

    public void mariaReturn()
    {
        StartCoroutine(SecondActMaria());
    }

    private IEnumerator SecondActMaria()
    {
        player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().InvalidatePathCache();
        PolygonCollider2D collider = GameObject.FindGameObjectWithTag("Confiner").transform.Find("confinerMaria").GetComponent<PolygonCollider2D>();
        player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().m_BoundingShape2D = collider;
        GameObject[] marias = GameObject.FindGameObjectsWithTag("Maria");
        marias[0].transform.position = new Vector3(-4.779f,4.418f,0);
        marias[1].transform.position = new Vector3(-4.779f, 4.418f, 0);
        Animator anim = player.GetComponent<Animator>();
        anim.SetFloat("IdleState", 1);
        anim.SetFloat("IdleStateY", 0.5f);
        Image fondoBlanco = loadScreen.transform.Find("FondoNegro").GetComponent<Image>();
        float transparency = 1f;
        //Triggereamos conversacion nueva con Koke
        Koke.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            if (fondoBlanco.color.a > 0)
            {
                transparency -= 0.05f;
                fondoBlanco.color = new Color(255, 255, 255, transparency);
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(0.1f);
        }
        Koke.conversationPhase += 1;
        loadScreen.SetActive(!loadScreen.activeSelf);
        loadScreen.transform.Find("LoadingSprite").gameObject.SetActive(true);
        fondoBlanco.color = new Color(0, 0, 0, 255);
        //Despertamos a Maria y conversacion con Koke
        for(int  i = 0; i < marias.Length; i++)
        {
            if (marias[i].name.Equals("cifradoMaria"))
            {
                marias[i].gameObject.SetActive(false);
            }
            else
            {
                while(marias[i].transform.localScale.x < 0.75)
                {
                    marias[i].transform.localScale = new Vector3(marias[i].transform.localScale.x + 0.1f, marias[i].transform.localScale.y + 0.1f, 0);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        Koke.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        //Teletransportamos a Olivia a la plataforma
        StartCoroutine(transitionToNewLevel("FinalBoss"));
        //player.continueMoving();
        yield return null;
    }

    public void revelationArc()
    {
        StartCoroutine(revelation());
    }

    private IEnumerator revelation()
    {
        player.stopFromMoving();
        NPC revelation = GameObject.FindGameObjectWithTag("revelationNPC").GetComponent<NPC>();
        revelation.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        loadScreen.transform.Find("LoadingSprite").gameObject.SetActive(false);
        Image fondoBlanco = loadScreen.transform.Find("FondoNegro").GetComponent<Image>();
        fondoBlanco.color = new Color(255, 255, 255, 0);
        loadScreen.SetActive(!loadScreen.activeSelf);
        float transparency = 0.05f;
        while (fondoBlanco.color.a < 1)
        {
            fondoBlanco.color = new Color(255, 255, 255, transparency);
            transparency += 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
        //Colocamos al player en su posicion nueva
        player.transform.position = new Vector3(-5.089f, 4.146f, 0);
        Koke.transform.position = new Vector3(-4.51f, 4.155f, 0);
        PolygonCollider2D confinador = GameObject.FindGameObjectWithTag("Confiner").transform.Find("confinerMaria").GetComponent<PolygonCollider2D>();
        player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().m_BoundingShape2D = confinador;
        yield return new WaitForSeconds(2f);
        mariaReturn();
        yield return null;
    }

    public void finalBossAct()
    {
        StartCoroutine(finalBoss());
    }

    private IEnumerator finalBoss()
    {
        player.stopFromMoving();
        Koke = FindObjectOfType<Koke>();
        Koke.moveToArcaelum();
        yield return new WaitForSeconds(1f);
        Koke.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Koke.conversationPhase += 1;
        Destroy(Instantiate(punishEffect, Koke.transform.position, Quaternion.identity),1f);
        yield return new WaitForSeconds(1f);
        Koke.transform.position = new Vector2(1000, 1000);
        yield return new WaitForSeconds(2f);
        Koke.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Koke.conversationPhase += 1;
        GameObject finalBoss = GameObject.FindGameObjectWithTag("FinalBoss");
        finalBoss.SetActive(false);
        Destroy(Instantiate(bossSpawnEffect, finalBoss.transform.position, Quaternion.identity),1.1f);
        yield return new WaitForSeconds(1f);
        Instantiate(arcaelum, finalBoss.transform.position, Quaternion.identity);
        soundManager.playAudioByScene("FinalBossPhaseOne");
        backgroundRepetion br = FindObjectOfType<backgroundRepetion>();
        br.ascensionSpeed = 1f;
        br.canStartScrolling = true;
        player.continueMoving();
        yield return new WaitForSeconds(3f);
        br.ascensionSpeed = 5f;
    }

    public void awakenEvent()
    {
        StartCoroutine(Awaken());
    }

    private IEnumerator Awaken()
    {
        player.stopFromMoving();
        player.transform.Find("VirtualCam").GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 1.4f;
        yield return new WaitForSeconds(1f);
        Koke.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        soundManager.stopAllAudios();
        Koke.conversationPhase += 1;
        GameObject revelationNPCS = GameObject.FindGameObjectWithTag("revelationNPC");
        revelationNPCS.transform.GetChild(0).gameObject.SetActive(true);
        GameObject prison = Instantiate(playerPrison, player.transform.position, Quaternion.identity);
        prison.transform.SetParent(player.transform);
        player.transform.position = GameObject.FindGameObjectWithTag("revelation").transform.position;
        revelationNPCS.transform.position = player.transform.position + new Vector3(-0.177f, 0.327f, 0);
        int childCount = revelationNPCS.transform.childCount;
        Koke.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Koke.conversationPhase += 1;
        yield return new WaitForSeconds(1f);
        soundManager.stopAllAudios();
        soundManager.playAudioByScene("Sync");
        for (int i = 1; i < childCount-1; i++)
        {
            revelationNPCS.transform.GetChild(i).gameObject.SetActive(true);
            Koke.popUpMeeting();
            isTaskPending = true;
            while (isTaskPending)
            {
                yield return new WaitForSeconds(0.1f);
            }
            Koke.conversationPhase += 1;
        }
        //Invocamos el movil
        
        Koke.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Koke.conversationPhase += 1;
        soundManager.stopAllAudios();
        soundManager.playAudioByScene("FinalBossPhaseTwo");
        revelationNPCS.transform.GetChild(childCount - 1).gameObject.SetActive(true);
        Koke.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Koke.conversationPhase += 1;
        //Podemos añadir efectos de power up para darle mas potencia
        yield return new WaitForSeconds(7);
        revelationNPCS.SetActive(false);
        Destroy(prison);
        player.transform.Find("VirtualCam").GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 1.8f;
        player.continueMoving();
        FindObjectOfType<Arcaelum>().dead = false;
        //Le damos el nuevo estado sync Mode
        GameObject abilities = GameObject.FindGameObjectWithTag("AbilitieManager");
        Instantiate(awakenDash, abilities.transform.GetChild(0));
        Instantiate(awakenBullet, abilities.transform.GetChild(1));
        Instantiate(awakenTime, abilities.transform.GetChild(2));
        InvokeRepeating("updateLife", 0, 5);
        yield return null;
    }

    public void updateLife()
    {
        player.setLife(+5);
    }

    public void credits()
    {
        StartCoroutine(startCredits());
    }

    private IEnumerator startCredits()
    {
        soundManager.stopAllAudios();
        loadScreen.transform.Find("LoadingSprite").gameObject.SetActive(false);
        Image fondoBlanco = loadScreen.transform.Find("FondoNegro").GetComponent<Image>();
        fondoBlanco.color = new Color(255, 255, 255, 0);
        loadScreen.SetActive(!loadScreen.activeSelf);
        float transparency = 0.05f;
        while (fondoBlanco.color.a < 1)
        {
            fondoBlanco.color = new Color(255, 255, 255, transparency);
            transparency += 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
        //Transportamos a Olivia y Koke
        player.transform.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        player.transform.Find("VirtualCam").GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 1.4f;
        player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().InvalidatePathCache();
        player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.FindGameObjectWithTag("barrerasHolder").GetComponent<PolygonCollider2D>();
        Animator playerAnim = player.GetComponent<Animator>();
        player.transform.Find("MnACanvas").gameObject.SetActive(false);
        player.transform.Find("Abilities").gameObject.SetActive(false);
        playerAnim.SetFloat("IdleState", 1);
        playerAnim.SetFloat("IdleStateY", 0);
        Koke Koke = FindObjectOfType<Koke>();
        Koke.transform.position = new Vector3(22.147f, 23.322f, -526.798f);
        Animator kokeAnim = Koke.GetComponent<Animator>();
        kokeAnim.SetFloat("Horizontal", -1);
        kokeAnim.SetFloat("Vertical", 0);
        yield return new WaitForSeconds(3);
        while (fondoBlanco.color.a > 0)
        {
            fondoBlanco.color = new Color(255, 255, 255, transparency);
            transparency -= 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
        player.setDashing(true);
        Rigidbody2D olivRb = player.GetComponent<Rigidbody2D>();
        olivRb.drag = 0;
        playerAnim.SetFloat("Speed", 1);
        playerAnim.SetFloat("Horizontal", 1);
        playerAnim.SetFloat("Vertical", 1);
        olivRb.AddForce(new Vector2(40, 0));
        Rigidbody2D kokeRb = Koke.GetComponent<Rigidbody2D>();
        kokeRb.AddForce(new Vector2(-40, 0));
        yield return new WaitForSeconds(3.3f);
        playerAnim.SetFloat("Speed", 0);
        olivRb.velocity = Vector3.zero;
        kokeRb.velocity = Vector3.zero;
        yield return new WaitForSeconds(2f);
        playerAnim.SetFloat("IdleState", 0);
        playerAnim.SetFloat("IdleStateY", 1);
        kokeAnim.SetFloat("Horizontal", 0);
        kokeAnim.SetFloat("Vertical", 1);
        //Activamos las letras de credito
        GameObject canvasFinish = GameObject.FindGameObjectWithTag("Finish");
        canvasFinish.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        canvasFinish.transform.GetChild(0).gameObject.SetActive(false);
        canvasFinish.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        canvasFinish.transform.GetChild(1).gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        Application.Quit();
        yield return null;
    }


}
