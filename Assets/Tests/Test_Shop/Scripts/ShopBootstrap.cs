using UnityEngine;

public class ShopBootstrap : MonoBehaviour
{
    [SerializeField] private Shop shop;
    [SerializeField] private WalletView walletView;

    private IDataProvider dataProvider;
    private IPersistentData persistentPlayerData;

    private Wallet wallet;

    public void Awake()
    {
        Debug.Log(Application.persistentDataPath);
        InitializeData();
        InitializeWallet();
        InitializeShop();
    }

    private void InitializeData()
    {
        persistentPlayerData = new PersistentData();
        dataProvider = new DataLocalProvider(persistentPlayerData);

        LoadDataOrInit();
    }

    private void InitializeWallet()
    {
        wallet = new Wallet(persistentPlayerData);

        walletView.Initialize(wallet);
    }

    private void InitializeShop()
    {
        OpenSkinsChecker openSkinsChecker = new OpenSkinsChecker(persistentPlayerData);
        SelectedSkinChecker selectedSkinChecker = new SelectedSkinChecker(persistentPlayerData);
        SkinSelecter skinSelecter = new SkinSelecter(persistentPlayerData);
        SkinUnlocker skinUnlocker = new SkinUnlocker(persistentPlayerData);

        shop.Initialize(dataProvider, wallet, openSkinsChecker, selectedSkinChecker, skinSelecter, skinUnlocker);
    }

    private void LoadDataOrInit()
    {
        if (dataProvider.TryLoad() == false)
        {
            persistentPlayerData.PlayerData = new PlayerData();
        }
    }
}
