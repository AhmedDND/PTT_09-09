using System.Collections;
using UnityEngine;

public class CardFlip : MonoBehaviour
{
    [SerializeField] private GameObject frontFace;
    [SerializeField] private GameObject backFace;

    private bool flipped = false;
    private bool isFlipping = false;

    private void Start()
    {
        // Ensure initial rotations
        frontFace.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        backFace.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void FlipCard()
    {
        if (!isFlipping)
        {
            flipped = !flipped;
            StartCoroutine(RotateFaces(flipped));
            Debug.Log("Flipped: " + flipped);
        }
    }

    private IEnumerator RotateFaces(bool flipped, float duration = 0.25f)
    {
        isFlipping = true;
        float time = 0f;

        Quaternion frontStart = frontFace.transform.localRotation;
        Quaternion backStart = backFace.transform.localRotation;

        // Rotate front and back relative to current rotation
        Quaternion frontEnd = frontStart * Quaternion.Euler(0f, 180f, 0f);
        Quaternion backEnd = backStart * Quaternion.Euler(0f, 180f, 0f);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            frontFace.transform.localRotation = Quaternion.Slerp(frontStart, frontEnd, t);
            backFace.transform.localRotation = Quaternion.Slerp(backStart, backEnd, t);

            yield return null;
        }

        frontFace.transform.localRotation = frontEnd;
        backFace.transform.localRotation = backEnd;

        isFlipping = false;
    }
}
