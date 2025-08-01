using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : Singleton<FadeManager>
{
    public Image fadeImage;
    
    protected override void Awake()
    {
        base.Awake(); // Call Singleton's Awake first
        
        // Create UI elements if not assigned
        if (fadeImage == null)
        {
            CreateFadeImage();
        }
    }
    
    private void CreateFadeImage()
    {
        // Create canvas if it doesn't exist
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("FadeCanvas");
            canvasObj.transform.SetParent(transform);
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = -1; // not positive to ensure it renders behind other UI elements

            // Add required components
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        
        // Create image if it doesn't exist
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvas.transform, false);
        fadeImage = imageObj.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0); // Start transparent
        
        // Make image fill the screen
        RectTransform rect = fadeImage.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;
    }
    
    public void FadeToBlack(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(0f, 1f, duration));
    }
    
    public void FadeFromBlack(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(1f, 0f, duration));
    }
    
    private IEnumerator FadeRoutine(float startAlpha, float targetAlpha, float duration)
    {
        // Ensure the fade image exists
        if (fadeImage == null)
        {
            CreateFadeImage();
        }
        
        // Set initial alpha
        Color currentColor = fadeImage.color;
        currentColor.a = startAlpha;
        fadeImage.color = currentColor;
        
        // Perform the fade
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(timer / duration);
            
            currentColor.a = Mathf.Lerp(startAlpha, targetAlpha, normalizedTime);
            fadeImage.color = currentColor;
            
            yield return null;
        }
        
        // Ensure we end at exactly the target alpha
        currentColor.a = targetAlpha;
        fadeImage.color = currentColor;
    }
}