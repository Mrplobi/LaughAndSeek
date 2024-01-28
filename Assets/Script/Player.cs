using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    //Event to trigger laugh when light is turned off
    public UnityEvent<bool> flashlightChanged;
    public UnityEvent transitionStart;
    public UnityEvent onJumpscare;

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
    [SerializeField]
    Material fadeMat;
    [SerializeField]
    Animator animator;

    bool isActive = false;

    bool hittingClown = false;
    Clown currentTarget = null;

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

            flashlightChanged.Invoke(isActive);
        }

        if (!isActive || turningOff) return;
        
        RaycastHit hit;
        Physics.Raycast(RaycastStart.position, RaycastStart.forward, out hit);
        Debug.DrawLine(RaycastStart.position, RaycastStart.position + RaycastStart.forward * 10, Color.magenta, 0.1f);
        if (hit.collider != null && hit.collider.tag == "Clown")
        {
            hittingClown = true;
            if(currentTarget == null)
            {
                currentTarget = hit.collider.GetComponentInParent<Clown>();
            }
        }
        else
        {
            hittingClown = false;
            currentTarget = null;
        }

        if(hittingClown)
        {
            validateTimer += Time.deltaTime;
            Debug.LogWarning(validateTimer);
            if (validateTimer > validateTime)
            {
                Debug.LogWarning(validateTimer); 
                if (currentTarget == null)
                {
                    currentTarget = hit.collider.GetComponentInParent<Clown>();
                }
                TurnOffAnim(currentTarget);
            }
        }
        else
        {
            validateTimer = 0;
        }
    }

    public void TurnOffAnim(Clown clown)
    {
        transitionStart.Invoke();
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
        lightCone.intensity = 1;
        lightParent.SetActive(false);
        Fade(false);
        clown.Hit();
    }

    public void Fade(bool fadeIn)
    {
        if(fadeIn)
        {
            turningOff = false;
            lightParent.SetActive(isActive);
            fadeMat.DOColor(Color.clear, 0.2f).SetEase(Ease.OutQuart);
        }
        else
        {
            fadeMat.DOColor(Color.black, 0.2f).SetEase(Ease.OutQuart);
        }
    }

    public void Jumpscare()
    {
        animator.SetTrigger("Jumpscare");
        onJumpscare.Invoke();
    }

    public void Reset()
    {
        isActive = false;
        lightParent.SetActive(false);
    }
}