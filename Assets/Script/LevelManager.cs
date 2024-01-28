using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    GameObject clownPrefab;
    Clown _clown;

    [SerializeField]
    List<LevelScriptable> levelScripts;

    [SerializeField]
    List<Trap> traps;
    [SerializeField]
    Trap whoopie;
    [SerializeField]
    Trap jumpscare;

    [SerializeField]
    Transform playerStart;
    [SerializeField]
    Player playerObject;

    [SerializeField]
    Transform darkSphere;

    int currentLevel = 0;

    bool transitioning = false;
    bool inDarkness = false;

    #region Sound Variables
    [Header("Sound")]
    [SerializeField]
    AudioSource globalGameVoice;
    [SerializeField]
    AudioSource RdmSFX;
    [SerializeField]
    AudioSource heartbeat;

    [SerializeField]
    List<AudioClip> deathClips;
    [SerializeField]
    List<AudioClip> winClips;
    [SerializeField]
    List<AudioClip> welcomeClips;
    [SerializeField]
    List<AudioClip> halfTimeClips;
    #endregion


    #region Death
    float globalDeathTimer = 0;
    [Header("Death")]
    [SerializeField]
    float globalDeathTime = 60;
    float darknessDeathTimer = 0;
    [SerializeField]
    float darknessDeathTime = 10;
    [SerializeField]
    Animator deathClown;
    #endregion


    #region MainMenu
    bool gameStarted = false;
    bool lightOn = false;
    bool titleCardSeen = false;

    [Header("Menu")]
    [SerializeField]
    Transform menuStart;
    [SerializeField]
    GameObject outsideLights;

    Clown outsideClown;
    [SerializeField]
    Transform outsideClownPosition;
    [SerializeField]
    GameObject flashlightInstruction;
    [SerializeField]
    GameObject gameInstructions;

    [SerializeField]
    GameObject titleCard;
    #endregion

    private void Start()
    {
        SetUpMainMenu();
        playerObject.transitionStart.AddListener(StartTransition);
        playerObject.onJumpscare.AddListener(PlayTrapLaugh);
    }

    private void Update()
    {
        if(gameStarted && !transitioning)
        {
            if(inDarkness)
            {
                darknessDeathTimer += Time.deltaTime;
            }
            globalDeathTimer += Time.deltaTime;
            if( darknessDeathTimer > darknessDeathTime || globalDeathTimer > globalDeathTime )
            {
                if(_clown)
                {
                    _clown.StopDarknessLaugh();
                }
                gameStarted = false;
                Death();
            }
        }
    }

    void StartRound()
    {
        if(currentLevel == 0)
        {
            PlayGlobalSound(welcomeClips);
        }
        if (currentLevel >= levelScripts.Count)
        {
            PlayGlobalSound(winClips);
            StartCoroutine(WinCoroutine());
            return;
        }

        if(_clown == null)
        {
            _clown = Instantiate(clownPrefab).GetComponent<Clown>();
            _clown.onHit += ClownFound;
        }

        var spawn = levelScripts[currentLevel].GetRandomSpawn();
        if(spawn == null)
        {
            Debug.LogError("NoSpawnFOund");
        }
        Debug.LogWarning(spawn.spawnName);
        SpawnClown(spawn);


        // Handle traps 
        Trap trap;

        whoopie.Activate();
        jumpscare.Activate();

        List<int> usedIndexes = new List<int>();
        // Activate 2 random ones
        while (usedIndexes.Count!=1)
        {
            int index = Random.Range(0, traps.Count);
            if (!usedIndexes.Contains(index))
            {
                trap = traps[index];
                trap.Activate();
                usedIndexes.Add(index);
            }     
        }
        // Hide the others
        for (int i=0; i<traps.Count; i++)
        {
            if (!usedIndexes.Contains(i))
            {
                traps[i].Hide();
            }
        }        
       
        StartCoroutine(StartRoundCoroutine());
    }

    IEnumerator StartRoundCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        transitioning = false;
        PlacePlayer(playerStart);
        playerObject.GetComponent<CharacterController>().enabled = true;
    }

    public void SpawnClown(SpawnInfo spawn)
    {
        _clown.Spawn(spawn);
    }

    public void ClownFound()
    {
        if(gameStarted)
        {
            currentLevel++;
            StartRound();
        }
        else
        {
            if(titleCardSeen)
            {
                gameStarted = true;
                StartRound();
            }
            else
            {
                StartCoroutine(TitleCardCoroutine());
            }
        }
    }

    public void OnFlashlightChanged(bool isOn)
    {
        if (isOn)
        {
            StopDarknessLaugh();
            inDarkness = false;
            darknessDeathTimer = 0;
            heartbeat.Stop();
            if(!gameStarted && !lightOn)
            {
                lightOn = true;
                SetUpTutorial();
            }
        }
        else
        {
            PlayDarknessLaugh();
            heartbeat.Play();
            if(gameStarted)
            {
                inDarkness = true;
            }
        }
    }

    private void PlayDarknessLaugh()
    {
        if(!gameStarted && outsideClown != null)
        {
            outsideClown.PlayDarknessLaugh();
        }
        else if(_clown != null)
        {
            _clown.PlayDarknessLaugh();
        }
    }

    private void StopDarknessLaugh()
    {
        if (!gameStarted && outsideClown != null)
        {
            outsideClown.StopDarknessLaugh();
        }
        else if (_clown != null)
        {
            _clown.StopDarknessLaugh();
        }
    }

    public void PlayTrapLaugh()
    {
        if(_clown != null)
        {
            _clown.PlayTrapLaugh();
        }
    }

    private void PlacePlayer(Transform newPlace)
    {
        playerObject.GetComponent<CharacterController>().enabled = false;
        playerObject.transform.position = newPlace.position;
        playerObject.transform.rotation = newPlace.rotation;
        playerObject.Fade(true);
    }

    void SetUpMainMenu()
    {
        PlacePlayer(menuStart);
        flashlightInstruction.SetActive(true);
        outsideLights.SetActive(true);
        gameInstructions.SetActive(false);
    }

    void SetUpTutorial()
    {
        flashlightInstruction.SetActive(false);
        outsideLights.SetActive(false);
        gameInstructions.SetActive(true);
        outsideClown = Instantiate(clownPrefab).GetComponent<Clown>();
        outsideClown.transform.SetParent(outsideClownPosition, false);
        outsideClown.transform.localPosition = Vector3.zero;
        outsideClown.transform.localRotation = Quaternion.identity;
        outsideClown.onHit += ClownFound;
    }

    void StartTransition()
    {
        transitioning = true;
    }

    void ResetAllTimers()
    {
        globalDeathTimer = 0;
        darknessDeathTimer = 0;
    }

    void Death()
    {
        playerObject.GetComponent<CharacterController>().enabled = false;
        playerObject.Fade(false);
        PlayGlobalSound(deathClips);
        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(1);

        PlacePlayer(darkSphere);

        deathClown.SetTrigger("Dead");

        yield return new WaitForSeconds(4.5F);

        playerObject.Fade(false);

        yield return new WaitForSeconds(4);

        ReturnToMenu();
    }

    IEnumerator TitleCardCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        titleCardSeen = true;
        PlacePlayer(darkSphere);
        titleCard.SetActive(true);

        yield return new WaitForSeconds(5f);
        playerObject.Fade(false);
        titleCard.SetActive(false);

        gameStarted = true;

        StartRound();
    }

    IEnumerator WinCoroutine()
    {        
        yield return new WaitForSeconds(globalGameVoice.clip.length);

        ReturnToMenu();
    }

    void ReturnToMenu()
    {
        gameStarted = false;
        lightOn = false;
        playerObject.Reset();
        ResetAllTimers();
        currentLevel = 0;
        SetUpMainMenu();
    }



    #region Sound Methods

    void PlayGlobalSound(List<AudioClip> clips)
    {
        var clip = clips[Random.Range(0, clips.Count)];
        globalGameVoice.clip = clip;
        globalGameVoice.Play();
    }

    void PlayRandomSFX(AudioClip clip)
    {
        RdmSFX.clip = clip;
        RdmSFX.Play();
    }

    #endregion
}
