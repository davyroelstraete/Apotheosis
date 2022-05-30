using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    WaitingForPlayerInput,
    BallsMoving,
    Infection,
    GameWon
}

public class GameManager : MonoBehaviour
{
    [SerializeField] CanvasGroup gameWonGUI;
    [SerializeField] TextMeshProUGUI turncount;

    List<Health> targets;
    List<Rigidbody2D> rbs;
    GameState state = GameState.WaitingForPlayerInput;

    int remainingHumans = 0;
    int numberOfMoves = 0;

    float delayBetweenChecks = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        remainingHumans = 0;
        targets = new List<Health>();
        var possibleTargets = GameObject.FindObjectsOfType<Health>();
        foreach (Health h in possibleTargets)
        {
            if (!h.transform.CompareTag("Player")) targets.Add(h);
            remainingHumans++;
        }

        var temp = GameObject.FindObjectsOfType<CircleCollider2D>();
        rbs = new List<Rigidbody2D>();
        foreach (var t in temp)
        {
            rbs.Add(t.GetComponent<Rigidbody2D>());
        }
    }

    public bool CanAcceptPlayerInput()
    {
        return (state == GameState.WaitingForPlayerInput);
    }

    public void StartMovementPhase()
    {
        state = GameState.BallsMoving;
        numberOfMoves++;
        StartCoroutine(CheckForMovingBalls());
    }

    private bool AreBallsStillMoving()
    {
        foreach( Rigidbody2D rb in rbs)
        {
            if (rb.velocity.sqrMagnitude > 0.01f)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator CheckForMovingBalls()
    {
        while (AreBallsStillMoving())
        {
            yield return new WaitForSeconds(delayBetweenChecks);
        }

        StartInfectionPhase();
    }

    public void StartInfectionPhase()
    {
        state = GameState.Infection;

        foreach (Health h in targets) h.InfectNearby();

        CheckGameWon();
    }

    private void CheckGameWon()
    {
        remainingHumans = 0;
        foreach (Health h in targets)
        {
            if (h.state == HealthState.Human) remainingHumans++;
        }

        if (remainingHumans == 0) GameWon();
        else state = GameState.WaitingForPlayerInput;
    }

    private void GameWon()
    {
        print("Game won in "+numberOfMoves+" turns!");
        state = GameState.GameWon;

        turncount.text = numberOfMoves.ToString();

        gameWonGUI.alpha = 1;
        gameWonGUI.blocksRaycasts = true;
        gameWonGUI.interactable = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToLevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
