using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SlotMachineManager : MonoBehaviour
{
    [Header("Reels")]

    // Reel references
    [SerializeField] private ReelController reel1;
    [SerializeField] private ReelController reel2;
    [SerializeField] private ReelController reel3;

    [Header("UI")]

    // Spin button reference
    [SerializeField] private Button spinButton;

    // UI text references
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private TMP_Text betText;

    [Header("Bet Buttons")]

    // Bet control buttons
    [SerializeField] private Button increaseBetButton;
    [SerializeField] private Button decreaseBetButton;

    [Header("Lever")]

    // Lever animation objects
    [SerializeField] private GameObject leverIdle;
    [SerializeField] private GameObject leverPulled;

    [Header("Popup")]

    // Popup shown when player has insufficient balance
    [SerializeField] private GameObject notEnoughMoneyPopup;

    [Header("Money Settings")]

    // Initial player balance
    [SerializeField] private int startingBalance = 10000;

    // Minimum allowed bet
    [SerializeField] private int minBet = 50;

    // Maximum allowed bet
    [SerializeField] private int maxBet = 500;

    // Bet increment/decrement value
    [SerializeField] private int betStep = 50;

    // Current player balance
    private int currentBalance;

    // Current selected bet amount
    private int currentBet;

    private void Start()
    {
        // Initialize balance and bet
        currentBalance = startingBalance;
        currentBet = minBet;

        // Set default lever state
        leverIdle.SetActive(true);
        leverPulled.SetActive(false);

        // Hide insufficient balance popup initially
        notEnoughMoneyPopup.SetActive(false);

        UpdateUI();

        // Register button listeners
        increaseBetButton.onClick.AddListener(IncreaseBet);
        decreaseBetButton.onClick.AddListener(DecreaseBet);
    }

    // Updates balance and bet UI text
    private void UpdateUI()
    {
        balanceText.text = "$" + currentBalance;
        betText.text = "Bet : $" + currentBet;
    }

    // Increase current bet amount
    public void IncreaseBet()
    {
        AudioManager.Instance.PlayBetClick();

        if (currentBet < maxBet)
        {
            currentBet += betStep;
        }

        UpdateUI();
    }

    // Decrease current bet amount
    public void DecreaseBet()
    {
        AudioManager.Instance.PlayBetClick();

        if (currentBet > minBet)
        {
            currentBet -= betStep;
        }

        UpdateUI();
    }

    // Starts slot machine spin
    public void StartSpin()
    {
        AudioManager.Instance.PlayButtonClick();

        // Prevent spin if player has insufficient balance
        if (currentBalance < currentBet)
        {
            notEnoughMoneyPopup.SetActive(true);

            Time.timeScale = 0f;

            return;
        }

        StartCoroutine(SpinRoutine());
    }

    // Controls full reel spinning sequence
    private IEnumerator SpinRoutine()
    {
        spinButton.interactable = false;

        // Clear previous result text
        resultText.text = "";

        // Start spinning sound loop
        AudioManager.Instance.StartReelSpin();

        // Deduct bet amount before spin
        currentBalance -= currentBet;

        UpdateUI();

        // Play lever pull animation
        leverIdle.SetActive(false);
        leverPulled.SetActive(true);

        yield return new WaitForSeconds(0.15f);

        leverIdle.SetActive(true);
        leverPulled.SetActive(false);

        // Start all reels spinning
        reel1.StartSpin();
        reel2.StartSpin();
        reel3.StartSpin();

        // Stop reels sequentially for realistic slot feel
        yield return new WaitForSeconds(1f);

        reel1.StopSpin();

        AudioManager.Instance.PlayReelStop();

        yield return new WaitForSeconds(0.7f);

        reel2.StopSpin();

        AudioManager.Instance.PlayReelStop();

        yield return new WaitForSeconds(0.7f);

        reel3.StopSpin();

        AudioManager.Instance.PlayReelStop();

        // Stop reel spinning audio loop
        AudioManager.Instance.StopReelSpin();

        // Check final slot result
        CheckWin();

        spinButton.interactable = true;
    }

    // Checks if all visible symbols match
    private void CheckWin()
    {
        SymbolType r1 = reel1.CurrentSymbolType;
        SymbolType r2 = reel2.CurrentSymbolType;
        SymbolType r3 = reel3.CurrentSymbolType;

        // Player wins if all symbols match
        if (r1 == r2 && r2 == r3)
        {
            AudioManager.Instance.PlayWin();

            int payout = GetPayout(r1);

            // Add winnings to player balance
            currentBalance += payout;

            StartCoroutine(ShowResultText("YOU WIN $" + payout));
        }
        else
        {
            AudioManager.Instance.PlayLose();

            StartCoroutine(
                ShowResultText("BETTER LUCK NEXT TIME")
            );
        }

        UpdateUI();
    }

    // Returns payout amount based on winning symbol type
    private int GetPayout(SymbolType symbolType)
    {
        switch (symbolType)
        {
            case SymbolType.Seven:
                return currentBet * 10;

            case SymbolType.Bell:
                return currentBet * 5;

            case SymbolType.Cherry:
                return currentBet * 3;

            case SymbolType.Bar:
                return currentBet * 2;

            default:
                return currentBet;
        }
    }

    // Displays result text temporarily
    private IEnumerator ShowResultText(string message)
    {
        resultText.text = message;

        yield return new WaitForSeconds(2f);

        resultText.text = "";
    }
}