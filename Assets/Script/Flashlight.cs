using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Flashlight : MonoBehaviour
{
    [SerializeField]
    Light lightCone;
    [SerializeField]
    GameObject lightParent;
    [SerializeField]
    AudioSource clickSound;
    [SerializeField]
    Transform RaycastStart;
    [SerializeField]
    AudioClip onSound;
    [SerializeField]
    AudioClip offSound;

    bool isActive = true;

    bool hittingClown = false;

    [SerializeField]
    float validateTime = 0.5f;
    float validateTimer = 0;

    bool turningOff = false;

    void Update()
    {
        if(!turningOff && (Input.GetKeyDown(KeyCode.L) || OVRInput.GetDown(OVRInput.Button.One)))
        {
            isActive = !isActive;
            lightParent.SetActive(isActive);
            clickSound.clip = isActive? onSound : offSound;
            clickSound.Play();
        }

        if (!isActive || turningOff) return;
        
        RaycastHit hit;
        Physics.Raycast(RaycastStart.position, RaycastStart.forward, out hit);
        Debug.DrawLine(RaycastStart.position, RaycastStart.position + RaycastStart.forward * 10, Color.magenta, 0.1f);
        if (hit.collider != null && hit.collider.tag == "Clown")
        {
            hittingClown = true;
        }
        else
        {
            hittingClown = false;
        }

        if(hittingClown)
        {
            validateTimer += Time.deltaTime;
            Debug.LogWarning(validateTimer);
            if (validateTimer > validateTime)
            {
                Debug.LogWarning(validateTimer);
                TurnOffAnim(hit.collider.GetComponent<Clown>());
            }
        }
        else
        {
            validateTimer = 0;
        }
    }

    public void TurnOffAnim(Clown clown)
    {
        turningOff = true;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(lightCone.DOIntensity(0, 0.2f).SetEase(Ease.OutQuart));
        sequence.Append(lightCone.DOIntensity(1, 0.3f).SetEase(Ease.OutQuart));
        sequence.AppendInterval(0.3f);
        sequence.Append(lightCone.DOIntensity(0, 0.4f).SetEase(Ease.OutQuart));
        sequence.Append(lightCone.DOIntensity(1, 0.2f).SetEase(Ease.OutQuart));
        sequence.Append(lightCone.DOIntensity(0, 0.2f).SetEase(Ease.OutQuart));
        sequence.OnComplete(() => EndLevel(clown));
        sequence.Play();
    }

    public void EndLevel(Clown clown)
    {
        //Fade out
        clown.Hit();
    }
}