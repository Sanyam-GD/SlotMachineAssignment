using UnityEngine;

// Different symbol types used in slot machine
public enum SymbolType
{
    Seven,
    Bell,
    Cherry,
    Bar
}

public class ReelController : MonoBehaviour
{
    [Header("Symbols")]

    // UI symbols used in reel
    [SerializeField] private RectTransform[] symbols;

    // Stores symbol type for each symbol object
    [SerializeField] private SymbolType[] symbolTypes;

    [Header("Spin Settings")]

    // Speed of reel movement
    [SerializeField] private float spinSpeed = 600f;

    // Distance between symbols
    [SerializeField] private float symbolSpacing = 200f;

    // Position where symbols recycle back to top
    [SerializeField] private float bottomLimit = -800f;

    // Controls reel spinning state
    private bool isSpinning = false;

    // Stores currently visible center symbol type
    public SymbolType CurrentSymbolType { get; private set; }

    private void Update()
    {
        // Only move symbols while spinning
        if (!isSpinning) return;

        MoveSymbols();
    }

    private void MoveSymbols()
    {
        float moveAmount = spinSpeed * Time.deltaTime;

        // Move all symbols downward
        foreach (RectTransform symbol in symbols)
        {
            symbol.anchoredPosition += Vector2.down * moveAmount;
        }

        // Recycle symbols back to top for infinite reel effect
        foreach (RectTransform symbol in symbols)
        {
            if (symbol.anchoredPosition.y <= bottomLimit)
            {
                float topY = GetTopSymbolY();

                symbol.anchoredPosition = new Vector2(
                    symbol.anchoredPosition.x,
                    topY + symbolSpacing
                );
            }
        }
    }

    // Finds highest symbol position in reel
    private float GetTopSymbolY()
    {
        float topY = symbols[0].anchoredPosition.y;

        foreach (RectTransform symbol in symbols)
        {
            if (symbol.anchoredPosition.y > topY)
            {
                topY = symbol.anchoredPosition.y;
            }
        }

        return topY;
    }

    // Starts reel spinning
    public void StartSpin()
    {
        isSpinning = true;
    }

    // Stops reel and detects visible center symbol
    public void StopSpin()
    {
        isSpinning = false;

        float closestDistance = Mathf.Abs(symbols[0].anchoredPosition.y);
        int closestIndex = 0;

        for (int i = 0; i < symbols.Length; i++)
        {
            // Snap symbols neatly into position
            float snappedY =
                Mathf.Round(symbols[i].anchoredPosition.y / symbolSpacing)
                * symbolSpacing;

            symbols[i].anchoredPosition = new Vector2(
                symbols[i].anchoredPosition.x,
                snappedY
            );

            // Find symbol closest to center
            float distanceFromCenter =
                Mathf.Abs(symbols[i].anchoredPosition.y);

            if (distanceFromCenter < closestDistance)
            {
                closestDistance = distanceFromCenter;
                closestIndex = i;
            }
        }

        // Store currently visible symbol type
        CurrentSymbolType = symbolTypes[closestIndex];
    }
}