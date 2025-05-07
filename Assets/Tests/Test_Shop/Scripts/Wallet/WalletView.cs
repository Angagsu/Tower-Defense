using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private TMP_Text value;

    private Wallet wallet;

    public void Initialize(Wallet wallet)
    {
        this.wallet = wallet;

        UpdateValue(this.wallet.GetCurrentCoins());

        wallet.CoinsChanged += UpdateValue;
    }

    private void OnDestroy() => wallet.CoinsChanged -= UpdateValue;

    private void UpdateValue(int value) => this.value.text = value.ToString();
}
