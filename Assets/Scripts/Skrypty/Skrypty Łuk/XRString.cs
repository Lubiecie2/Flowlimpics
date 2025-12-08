using System;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace UnityEngine.XR.Interaction.Toolkit.Interactables
{
    public class XRPullInteractable : XRBaseInteractable
    {
        public event Action<float> PullActionReleased;
        public event Action<float> PullUpdated;
        public event Action PullStarted;
        public event Action PullEnded;

        [Header("Pull Settings")]
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;
        [SerializeField] private GameObject _notchPoint;

        [Header("String Curve")]
        [SerializeField, Range(0.1f, 1f)] private float _stringCurvature = 0.5f;

        [Header("Arrow Settings")]
        [SerializeField] private bool _enableArrowSpawning = true;
        [SerializeField] private GameObject _arrowPrefab;
        [SerializeField] private Transform _arrowSpawnPoint;
        [SerializeField] private float _minLaunchForce = 1f;
        [SerializeField] private float _maxLaunchForce = 5f;
        [SerializeField] private AudioClip _launchSound;

        public float pullAmount { get; private set; } = 0.0f;

        private LineRenderer _lineRenderer;
        private IXRSelectInteractor _pullingInteractor = null;
        private Vector3 _grabStartPosition;
        private GameObject _currentArrow;

        protected override void Awake()
        {
            base.Awake();
            _lineRenderer = GetComponent<LineRenderer>();
            ResetString();
        }

        public void SetPullInteractor(SelectEnterEventArgs args)
        {
            _pullingInteractor = args.interactorObject;
            _grabStartPosition = _pullingInteractor.GetAttachTransform(this).position;
            pullAmount = 0f;
            ResetString();

            if (_enableArrowSpawning)
            {
                SpawnArrow();
            }

            PullStarted?.Invoke();
        }

        public void Release()
        {
            PullActionReleased?.Invoke(pullAmount);

            if (_enableArrowSpawning && _currentArrow != null && pullAmount > 0.1f)
            {
                LaunchArrow();
            }
            else if (_currentArrow != null)
            {
                Destroy(_currentArrow);
            }

            PullEnded?.Invoke();
            _pullingInteractor = null;
            pullAmount = 0f;
            ResetString();
        }

        private void SpawnArrow()
        {
            if (_arrowPrefab == null || _arrowSpawnPoint == null) return;

            if (_currentArrow != null)
            {
                Destroy(_currentArrow);
            }

            _currentArrow = Instantiate(_arrowPrefab, _arrowSpawnPoint.position, _arrowSpawnPoint.rotation);
            _currentArrow.transform.SetParent(_arrowSpawnPoint);

            Rigidbody arrowRb = _currentArrow.GetComponent<Rigidbody>();
            if (arrowRb != null)
            {
                arrowRb.isKinematic = true;
                arrowRb.useGravity = false;
            }
        }

        private void LaunchArrow()
        {
            if (_launchSound != null)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.PlayOneShot(_launchSound);
                }
            }
            _currentArrow.transform.SetParent(null);

            Vector3 launchDirection = (_startPoint.position - _endPoint.position).normalized;
            float launchForce = Mathf.Lerp(_minLaunchForce, _maxLaunchForce, pullAmount);
            Vector3 launchVelocity = launchDirection * launchForce;

            Collider arrowCollider = _currentArrow.GetComponent<Collider>();
            Collider[] bowColliders = GetComponentsInChildren<Collider>();

            foreach (Collider bowCollider in bowColliders)
            {
                if (arrowCollider != null && bowCollider != null)
                {
                    Physics.IgnoreCollision(arrowCollider, bowCollider, true);
                }
            }

            Arrow arrowScript = _currentArrow.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                arrowScript.Launch(launchVelocity);
            }
            else
            {
                Rigidbody arrowRb = _currentArrow.GetComponent<Rigidbody>();
                if (arrowRb != null)
                {
                    arrowRb.isKinematic = false;
                    arrowRb.useGravity = true;
                    arrowRb.linearVelocity = launchVelocity;
                }
            }

            _currentArrow = null;
        }

        private void ResetString()
        {
            if (_notchPoint != null)
            {
                _notchPoint.transform.position = _startPoint.position;
            }

            if (_lineRenderer != null)
            {
                _lineRenderer.SetPosition(1, _startPoint.localPosition);
            }
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                if (isSelected && _pullingInteractor != null)
                {
                    Vector3 pullPosition = _pullingInteractor.GetAttachTransform(this).position;
                    float previousPull = pullAmount;
                    pullAmount = CalculatePull(pullPosition);

                    if (previousPull != pullAmount)
                    {
                        PullUpdated?.Invoke(pullAmount);
                    }

                    UpdateStringAndNotch();
                }
            }
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            SetPullInteractor(args);
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            Release();
        }

        private float CalculatePull(Vector3 currentHandPosition)
        {
            Vector3 targetDirection = _endPoint.position - _startPoint.position;
            float maxLength = targetDirection.magnitude;
            targetDirection.Normalize();

            Vector3 handMovement = currentHandPosition - _grabStartPosition;
            float pullDistance = Vector3.Dot(handMovement, targetDirection);
            float pullValue = pullDistance / maxLength;

            return Mathf.Clamp(pullValue, 0, 1);
        }

        private void UpdateStringAndNotch()
        {
            float curvedPull = pullAmount * _stringCurvature;

            Vector3 linePosition = Vector3.Lerp(_startPoint.position, _endPoint.position, curvedPull);
            _notchPoint.transform.position = linePosition;
            _lineRenderer.SetPosition(1, _notchPoint.transform.localPosition);

            if (_enableArrowSpawning && _currentArrow != null && _arrowSpawnPoint != null)
            {
                _arrowSpawnPoint.position = _notchPoint.transform.position;

                Vector3 stringDirection = (_startPoint.position - _endPoint.position).normalized;

                if (stringDirection.magnitude > 0.01f)
                {
                    _arrowSpawnPoint.rotation = Quaternion.LookRotation(stringDirection);
                }
            }
        }

        public void EnableArrowSpawning(bool enable)
        {
            _enableArrowSpawning = enable;
        }

        public bool IsArrowSpawningEnabled()
        {
            return _enableArrowSpawning;
        }

        public Vector3 GetStringPosition()
        {
            return _notchPoint.transform.position;
        }

        public Vector3 GetStringDirection()
        {
            Vector3 direction = _endPoint.position - _notchPoint.transform.position;
            return direction.normalized;
        }
    }
}