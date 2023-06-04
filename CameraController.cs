using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    [SerializeField] protected Vector3 offset;

    [SerializeField] protected float followSpeed;

    [SerializeField] protected bool isXLocked = false;

    [SerializeField] protected bool isYLocked = false;
    private bool shake = false;
    public Camera cam;
    public float shakeDuration = 1f;
    public float zoomSpeed = 1f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 150f;
    public float decreaseFactor = 5.0f;
    Vector3 originalPos;
    private float defaultOrthographicSize;


    void Start()
    {
        shake = false;
        defaultOrthographicSize = cam.orthographicSize;
        Debug.Log("Camera Reference: " + cam);
    }

    void Update()
    {
        float xTarget = player.position.x + offset.x;
        float yTarget = player.position.y + offset.y;
        float xNew = Mathf.Lerp(transform.position.x, xTarget, Time.deltaTime * followSpeed);
        float yNew = Mathf.Lerp(transform.position.y, yTarget, Time.deltaTime * followSpeed);
        transform.position = new Vector3(xNew, yNew, offset.z);

        if (shake == true)
        {
            // shake = false;
            originalPos = transform.position;
            transform.position = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
            Invoke("StopShake", 1.5f);
            // StartCoroutine(Shake(0.7f, 2f));
        }
        else
        {
            shakeDuration = 0f;
            transform.position = new Vector3(xNew, yNew, offset.z);
        }
    }

    public void CauseShake(bool value)
    {
        shake = value;
    }

    public void StopShake()
    {
        shake = false;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 0f) * magnitude;
            float y = Random.Range(-0.5f, 0f) * magnitude;
            transform.position += new Vector3(x, y, 0);

            x = Random.Range(0f, 1f) * magnitude;
            y = Random.Range(0f, 1f) * magnitude;
            transform.position += new Vector3(x, y, 0);

            x = Random.Range(-1f, 0f) * magnitude;
            y = Random.Range(-0.5f, 0f) * magnitude;
            transform.position += new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }

    public void ZoomOut()
    {
        StartCoroutine(ZoomCoroutine(cam.orthographicSize, 1.3f * defaultOrthographicSize));
    }

    public void ZoomBackIn()
    {
        StartCoroutine(ZoomCoroutine(cam.orthographicSize, defaultOrthographicSize));
    }

    private IEnumerator ZoomCoroutine(float startSize, float targetSize)
    {
        float elapsedTime = 0f;
        float currentSize = startSize;

        while (elapsedTime < zoomSpeed)
        {
            currentSize = Mathf.Lerp(startSize, targetSize, elapsedTime / zoomSpeed);
            cam.orthographicSize = currentSize;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cam.orthographicSize = targetSize; // Ensure that the target size is reached exactly
    }
}
