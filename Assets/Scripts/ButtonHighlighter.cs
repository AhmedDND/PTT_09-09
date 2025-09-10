using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonHighlighter : MonoBehaviour
{
    [SerializeField] private GameObject highlightPrefab;
    [SerializeField] private List<Button> targetButtons;

    private GameObject currentHighlight;

    private void Start()
    {
        if (highlightPrefab != null)
        {
            currentHighlight = Instantiate(highlightPrefab, targetButtons[0].transform);

            RectTransform rt = currentHighlight.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            currentHighlight.SetActive(true);
        }
    }

    public void HighlightButton(Button button)
    {
        if (currentHighlight == null || !targetButtons.Contains(button))
            return;

        currentHighlight.transform.SetParent(button.transform, false);

        RectTransform rt = currentHighlight.GetComponent<RectTransform>();
        RectTransform btnRT = button.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        currentHighlight.SetActive(true);
    }
}
