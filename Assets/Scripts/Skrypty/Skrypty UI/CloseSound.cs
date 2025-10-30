using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonSoundEffect : MonoBehaviour, IPointerClickHandler
{
    [Header("Sound Settings")]
    public AudioClip clickSound;

    [Range(0f, 1f)]
    public float volume = 1f;

    public bool playOnClick = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (playOnClick)
        {
            PlaySound();
        }
    }

    public void PlaySound()
    {
        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position, volume);
        }
    }
}