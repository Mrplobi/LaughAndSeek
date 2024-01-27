using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    GameObject clownPrefab;
    Clown _clown;

    [SerializeField]
    List<LevelScriptable> levelScripts;

    [SerializeField]
    Transform playerStart;
    [SerializeField]
    Player playerObject;

    [SerializeField]
    Transform darkSphere;

    int currentLevel = 0;

    bool transitioning = false;
    bool inDarkness = false;


    #region Death
    float globalDeathTimer = 0;
    [SerializeField]
    float globalDeathTime = 120;
    float darknessDeathTimer = 0;
    [SerializeField]
    float darknessDeathTime = 30;
    #endregion


    #region MainMenu
    bool gameStarted = false;
    bool lightOn = false;
    bool titleCardSeen = false;

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
                Death();
            }
        }
    }

    void StartRound()
    {
        if (currentLevel >= levelScripts.Count)
        {
            StartCoroutine(WinCoroutine());
            return;
        }

        if(_clown == null)
        {
            _clown = Instantiate(clownPrefab).GetComponent<Clown>();
            _clown.onHit += ClownFound;
        }

        var spawn = levelScripts[currentLevel].GetRandomSpawn();
        SpawnClown(spawn);

        StartCoroutine(StartRoundCoroutine());
    }

    IEnumerator StartRoundCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        ResetAllTimers();
        transitioning = false;
        PlacePlayer(playerStart);
        playerObject.GetComponent<CharacterController>().enabled = true;
        ResetAllTimers();
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
            StopLaugh();
            inDarkness = false;
            darknessDeathTimer = 0;
            if(!gameStarted && !lightOn)
            {
                lightOn = true;
                SetUpTutorial();
            }
        }
        else
        {
            PlayLaugh();
            if(gameStarted)
            {
                inDarkness = true;
            }
        }
    }

    private void PlayLaugh()
    {
        if(!gameStarted && outsideClown != null)
        {
            outsideClown.PlayLaugh();
        }
        else if(_clown != null)
        {
            _clown.PlayLaugh();
        }
    }

    private void StopLaugh()
    {
        if (!gameStarted && outsideClown != null)
        {
            outsideClown.StopLaugh();
        }
        else if (_clown != null)
        {
            _clown.StopLaugh();
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
        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        PlacePlayer(darkSphere);
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
        yield return new WaitForSeconds(0.2f);

        gameStarted = false;
        lightOn = false;
        playerObject.Reset();
        ResetAllTimers();
        currentLevel = 0;
        SetUpMainMenu();
    }
}
