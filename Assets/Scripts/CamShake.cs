using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{

    private bool doneMoving = true;
    private bool doneShaking = true;

    public void OnEnable() {
        EventManager.StartListening("Player Hit", bigShake);
        EventManager.StartListening("Error", tinyShake);
        EventManager.StartListening("Box Crash", boxShake);
    }

    public void OnDisable() {
        EventManager.StopListening("Player Hit", bigShake);
        EventManager.StopListening("Error", tinyShake);
        EventManager.StopListening("Box Crash", boxShake);
    }

    public void tinyShake()
    {
        if (doneShaking) StartCoroutine(shake(5, 0.1f, 0.01f, true, Vector2.zero));
    }

    public void boxShake()
    {
        if (doneShaking) StartCoroutine(shake(15, 0.25f, 0.01f, true, Vector2.down));
    }

    public void bigShake()
    {
        if (doneShaking) StartCoroutine(shake(30, 0.75f, 0.01f, true, Vector2.zero));
    }

    private IEnumerator shake(int amount, float range, float duration, bool decay, Vector2 direction)
    {
        doneShaking = false;
        direction = direction.normalized;
        Vector3 origin = transform.localPosition;
        Vector3 newPos = new Vector3();
        float scale = 1;
        int sign = 1;

        for (int i = 0; i < amount; i++)
        {
            if (decay) scale = (amount - (float)i) / amount;

            if (direction == Vector2.zero)
                newPos = origin + new Vector3(Random.Range(-range, range) * scale, Random.Range(-range, range) * scale, 0);
            else
            {
                newPos = origin + (Vector3)direction * scale * range * sign;
                sign *= -1;
            }

            StartCoroutine(smoothMove(newPos, duration));
            while (!doneMoving) { yield return null; }
        }

        StartCoroutine(smoothMove(origin, duration));
        while (!doneMoving) { yield return null; }
        doneShaking = true;
    }

    private IEnumerator smoothMove(Vector3 targetPosition, float duration)
    {
        doneMoving = false;
        Vector3 originalPosition = transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.localPosition = new Vector3(Mathf.SmoothStep(originalPosition.x, targetPosition.x, elapsedTime / duration),
                                                  Mathf.SmoothStep(originalPosition.y, targetPosition.y, elapsedTime / duration),
                                                  originalPosition.z);
            yield return null;
        }

        transform.localPosition = new Vector3(targetPosition.x, targetPosition.y, originalPosition.z);
        doneMoving = true;
    }
}