using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
    private Coroutine typingCoroutine;
    public TextMeshProUGUI textDisplay;
    public float waitingSeconds = 0.05f;
    public bool isTyping;

    private void Awake()
    {
        isTyping = false;
        typingCoroutine = null;
    }

    public void StartTyping(string text)
    {
        if (typingCoroutine != null)
        {
            StopAllCoroutines();
        }
        textDisplay.text = text;
        typingCoroutine = StartCoroutine(TypeLine(text));
    }

    private IEnumerator TypeLine(string text)
    {
        isTyping = true;
        textDisplay.maxVisibleCharacters = 0;
        for (int i = 0; i < text.Length; i++)
        {
            textDisplay.maxVisibleCharacters = i + 1;
            yield return new WaitForSeconds(waitingSeconds);
        }
        isTyping = false;
    }

    public void CompleteLine()
    {
        if (typingCoroutine != null && isTyping)
        {
            StopCoroutine(typingCoroutine);
            textDisplay.maxVisibleCharacters = textDisplay.text.Length;
            isTyping = false;
        }
    }
}