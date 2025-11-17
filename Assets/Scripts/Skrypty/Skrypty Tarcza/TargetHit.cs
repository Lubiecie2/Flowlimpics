using UnityEngine;

public class TargetHit : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private int _pointValue = 1;
    [SerializeField] private bool _destroyOnHit = true; 
    [SerializeField] private float _destroyDelay = 0.3f; 

    [Header("Audio")]
    [SerializeField] private AudioClip _hitSound; 
    [SerializeField] private float _volume = 1f; 

    [Header("Visual Feedback")]
    [SerializeField] private Color _hitColor = Color.green;
    [SerializeField] private float _flashDuration = 0.5f;

    private Renderer _renderer;
    private Color _originalColor;
    private bool _isFlashing = false;
    private bool _wasHit = false;
    private AudioSource _audioSource;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        if (_renderer != null)
        {
            _originalColor = _renderer.material.color;
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1f; 
    }

    public void OnArrowHit(float damage)
    {
        if (_wasHit) return;

        _wasHit = true;

        Debug.Log($"Trafienie! +{_pointValue} pkt");

        PlayHitSound();

        FlashColor();

        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.AddPoints(_pointValue);
        }

        if (_destroyOnHit)
        {
            Destroy(gameObject, _destroyDelay);
        }
    }

    private void PlayHitSound() 
    {
        if (_hitSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(_hitSound, _volume);
        }
    }

    private void FlashColor()
    {
        if (_renderer == null || _isFlashing) return;

        _isFlashing = true;
        _renderer.material.color = _hitColor;

        if (!_destroyOnHit)
        {
            Invoke(nameof(ResetColor), _flashDuration);
        }
    }

    private void ResetColor()
    {
        if (_renderer != null)
        {
            _renderer.material.color = _originalColor;
        }
        _isFlashing = false;
        _wasHit = false;
    }
}