using UnityEngine;
using SA.ScriptableData;
using DG.Tweening;

public class CameraController : TemporalSingleton<CameraController>
{
    #region Variables

    [SerializeField]
    private Vector3Value playerPosition;
    [SerializeField]
    private Vector3Value aimPosition;

    [Header("Camera target")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float offsetFromPlayerToAim = 0.5f;
    [SerializeField]
    private float maxOffsetFromPlayerToAim = 10.0f;

    [Header("Camera setup")]
    [SerializeField]
    private float distanceModifier = 0.5f;
    [SerializeField]
    private float minDistance = 20.0f;
    [SerializeField]
    private float maxDistance = 30.0f;
    [SerializeField]
    [Range(-90, 90)]
    private float cameraTilt = 45.0f;
    [SerializeField]
    [Range(-360, 360)]
    private float cameraAngle = -45.0f;
    [SerializeField]
    private float movingSpeed = 3.0f;

    private Tween cameraTween;

	#endregion Variables

	#region Properties

    public Camera Camera { get; private set; }

	#endregion

	#region MonoBehaviour Methods

	protected override void Initialize()
	{
		base.Initialize();
     
        if (playerPosition != null && aimPosition != null)
        {
            playerPosition.ValueChanged += UpdateCameraPosition;
            aimPosition.ValueChanged += UpdateCameraPosition;
        }
        else
        {
            Debug.LogError($"{nameof(playerPosition)} or {nameof(aimPosition)} properties of {nameof(CameraController)} are not set at GameObject: {this.gameObject.name}");
        }

        Camera = GetComponent<Camera>();
    }

    #endregion MonoBehaviour Methods

    #region Methods

    private void UpdateCameraPosition()
    {
        Vector3 playerToAimDirection = aimPosition.Value - playerPosition.Value;
        float distance = playerToAimDirection.magnitude;

        Vector3 targetPosition = playerPosition.Value + Vector3.ClampMagnitude(playerToAimDirection, Mathf.Clamp(distance * 0.5f, 0, maxOffsetFromPlayerToAim));

        Vector3 direction = Quaternion.Euler(cameraTilt, cameraAngle, 0.0f) * Vector3.back;

        distance = Mathf.Clamp(minDistance + distance * distanceModifier, minDistance, maxDistance);

        if (cameraTween != null)
        {
            cameraTween.Kill(false);
        }
        cameraTween = transform?.DOMove(targetPosition + direction * distance, 1.0f / movingSpeed);
        transform.rotation = Quaternion.LookRotation(-direction, Vector3.up);
    }

    /// <summary>Overrides camera target and moves camera to the new target.</summary>
    /// <param name="newTargetPosition">New target position.</param>
    public void OverrideCameraTarget(Vector3 newTargetPosition)
    {
        playerPosition.ValueChanged -= UpdateCameraPosition;
        aimPosition.ValueChanged -= UpdateCameraPosition;

        float distance = minDistance;

        Vector3 direction = Quaternion.Euler(cameraTilt, cameraAngle, 0.0f) * Vector3.back;

        distance = Mathf.Clamp(minDistance + distance * distanceModifier, minDistance, maxDistance);

        if (cameraTween != null)
        {
            cameraTween.Kill(false);
        }
        cameraTween = transform.DOMove(newTargetPosition + direction * distance, 1.0f / movingSpeed);
    }

    /// <summary>Disables camera target override and moves camera back to player.</summary>
    public void DisableCameraTargetOverride()
    {
        playerPosition.ValueChanged += UpdateCameraPosition;
        aimPosition.ValueChanged += UpdateCameraPosition;
        UpdateCameraPosition();
    }

    private void OnDestroy()
    {
        playerPosition.ValueChanged -= UpdateCameraPosition;
        aimPosition.ValueChanged -= UpdateCameraPosition;
    }

    #endregion Methods
}
