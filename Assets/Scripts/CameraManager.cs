using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineTransposer framingTransposer;
    private float currentXOffset = 0f; // Current x-offset value
    private float transitionSpeed = 5f; // Speed of the transition

    private void Awake()
    {
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        CameraTrigger.OnPlayerEnterCollider += UpdateEnterOffset;
        CameraTrigger.OnPlayerExitCollider += UpdateExitOffset;
    }

    private void OnDestroy()
    {
        CameraTrigger.OnPlayerEnterCollider -= UpdateEnterOffset;
        CameraTrigger.OnPlayerExitCollider -= UpdateExitOffset;
    }

    private void UpdateEnterOffset(float xOffset)
    {
        // Gradually update the x-offset to the specified value for smooth transition
        StartCoroutine(TransitionToOffset(xOffset));
    }

    private void UpdateExitOffset()
    {
        // Gradually update the x-offset to 0 for smooth transition back to original position
        StartCoroutine(TransitionToOffset(0f));
    }

    private System.Collections.IEnumerator TransitionToOffset(float targetOffset)
    {
        float elapsedTime = 0f;
        float initialOffset = currentXOffset;

        // Smoothly interpolate between the current offset and the target offset over time
        while (elapsedTime < 1f)
        {
            currentXOffset = Mathf.Lerp(initialOffset, targetOffset, elapsedTime);
            framingTransposer.m_FollowOffset.x = currentXOffset;

            // Update the elapsed time based on the transition speed
            elapsedTime += Time.deltaTime * transitionSpeed;
            yield return null;
        }

        // Ensure the final offset is exactly equal to the target offset
        currentXOffset = targetOffset;
        framingTransposer.m_FollowOffset.x = currentXOffset;
    }
}
