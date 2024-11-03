using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockTutorial : MonoBehaviour
{
    private GameManager gameManager;
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI blockTutorialText;
    private Image arrowImage;
    private textStates textstate = textStates.Waiting;
    private enum textStates { Waiting, Appearing, Appeared, Final }

    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            gameManager = GameManager.Instance;
        }
        else
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        canvasGroup = GetComponent<CanvasGroup>();
        blockTutorialText = GetComponentInChildren<TextMeshProUGUI>();
        arrowImage = GetComponentInChildren<Image>();
    }

    private void Start()
    {
        if (gameManager.RoomLevel != 1)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        EnemyCombatHandler.StartingAttackWarning += ShowBlockText;
        PlayerCombatHandler.PlayerBlockStart += PlayerBlocked;
    }

    private void OnDisable()
    {
        EnemyCombatHandler.StartingAttackWarning -= ShowBlockText;
        PlayerCombatHandler.PlayerBlockStart -= PlayerBlocked;
    }

    private void ShowBlockText(PlayerCombatStates state)
    {
        if (textstate == textStates.Waiting)
        {
            textstate = textStates.Appearing;
            StartCoroutine(FadeInCanvas());
        }
    }

    private IEnumerator FadeInCanvas()
    {
        yield return new WaitForSeconds(1.5f);
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * 4;
            yield return null;
        }
        textstate = textStates.Appeared;
    }

    private void PlayerBlocked()
    {
        if (textstate == textStates.Appeared)
        {
            textstate= textStates.Final;
            StartCoroutine(ChangeAndFadeText());
        }
    }

    private IEnumerator ChangeAndFadeText()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * 4;
            yield return null;
        }
        arrowImage.gameObject.SetActive(false);
        blockTutorialText.text = 
            "But be warned...\n" +
            "Blocking only\n" +
            "<b><u>reduces</u></b> damage";
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * 4;
            yield return null;
        }
        yield return new WaitForSeconds(6);
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * 4;
            yield return null;
        }
    }
}