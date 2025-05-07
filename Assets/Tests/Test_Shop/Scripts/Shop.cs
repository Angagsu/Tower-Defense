using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopContent contentItems;

    [SerializeField] private ShopCategoryButton characterSkinsButton;
    [SerializeField] private ShopCategoryButton mazeSkinsButton;

    [SerializeField] private BuyButton buyButton;
    [SerializeField] private Button selectionButton;
    [SerializeField] private Image selectedText;

    [SerializeField] private ShopPanel shopPanel;
    [SerializeField] private SkinPlacement skinPlacement;

    [SerializeField] private Camera modelCamera;
    [SerializeField] private Transform characterCategoryCameraPosition;
    [SerializeField] private Transform mazeCategoryCameraPosition;

    private IDataProvider dataProvider;

    private ShopItemView previewedItem;

    private Wallet wallet;

    private SkinSelecter skinSelecter;
    private SkinUnlocker skinUnlocker;
    private OpenSkinsChecker openSkinsChecker;
    private SelectedSkinChecker selectedSkinChecker;


    public void Initialize(IDataProvider dataProvider, Wallet wallet, OpenSkinsChecker openSkinsChecker, SelectedSkinChecker selectedSkinChecker, SkinSelecter skinSelecter, SkinUnlocker skinUnlocker)
    {
        this.wallet = wallet;
        this.openSkinsChecker = openSkinsChecker;
        this.selectedSkinChecker = selectedSkinChecker;
        this.skinSelecter = skinSelecter;
        this.skinUnlocker = skinUnlocker;
        this.dataProvider = dataProvider;

        shopPanel.Initialize(openSkinsChecker, selectedSkinChecker);

        shopPanel.ItemViewClicked += OnItemViewClicked;

        OnCharacterSkinsButton_Click();
    }

    private void OnItemViewClicked(ShopItemView item)
    {
        previewedItem = item;
        skinPlacement.InstantiateModel(previewedItem.Model);

        openSkinsChecker.Visit(previewedItem.Item);

        if (openSkinsChecker.IsOpened)
        {
            selectedSkinChecker.Visit(previewedItem.Item);

            if (selectedSkinChecker.IsSelected)
            {
                ShowSelectedText();
                return;
            }

            ShowSelectionButton();
        }
        else
        {
            ShowBuyButton(previewedItem.Price);
        }
    }

    private void OnBuyButtonClick()
    {
        if (wallet.IsEnough(previewedItem.Price))
        {
            wallet.Spend(previewedItem.Price);
            skinUnlocker.Visit(previewedItem.Item);

            SelectSkin();

            previewedItem.UnLock();
            dataProvider.Save();
        }
    }

    private void OnSelectionButtonClick()
    {
        SelectSkin();
        dataProvider.Save();
    }

    private void OnMazeSkinsButtons_Click()
    {
        mazeSkinsButton.Select();
        characterSkinsButton.Unselect();

        UpdateCameraTransform(mazeCategoryCameraPosition);

        shopPanel.Show(contentItems.MazeSkinItems.Cast<ShopItem>());
    }

    private void OnCharacterSkinsButton_Click()
    {
        mazeSkinsButton.Unselect();
        characterSkinsButton.Select();

        UpdateCameraTransform(characterCategoryCameraPosition);

        shopPanel.Show(contentItems.CharacterSkinItems.Cast<ShopItem>());
    }

    private void UpdateCameraTransform(Transform transform)
    {
        modelCamera.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }

    private void SelectSkin()
    {
        skinSelecter.Visit(previewedItem.Item);
        shopPanel.Select(previewedItem);
        ShowSelectedText();
    }

    private void ShowSelectionButton()
    {
        selectionButton.gameObject.SetActive(true);
        HideBuyButton();
        HideSelectedText();
    }

    private void ShowSelectedText()
    {
        selectedText.gameObject.SetActive(true);
        HideSelectionButton();
        HideBuyButton();
    }

    private void ShowBuyButton(int price)
    {
        buyButton.gameObject.SetActive(true);
        buyButton.UpdateText(price);

        if (wallet.IsEnough(price))
        {
            buyButton.Unlock();
        }
        else
        {
            buyButton.Lock();
        }

        HideSelectedText();
        HideSelectionButton();
    }

    private void HideBuyButton() => buyButton.gameObject.SetActive(false);
    private void HideSelectionButton() => selectionButton.gameObject.SetActive(false);
    private void HideSelectedText() => selectedText.gameObject.SetActive(false);

    private void OnEnable()
    {
        characterSkinsButton.Click += OnCharacterSkinsButton_Click;
        mazeSkinsButton.Click += OnMazeSkinsButtons_Click;
        
        buyButton.Click += OnBuyButtonClick;
        selectionButton.onClick.AddListener(OnSelectionButtonClick);
    }

    private void OnDisable()
    {
        characterSkinsButton.Click -= OnCharacterSkinsButton_Click;
        mazeSkinsButton.Click -= OnMazeSkinsButtons_Click;
        shopPanel.ItemViewClicked -= OnItemViewClicked;
        buyButton.Click -= OnBuyButtonClick;
        selectionButton.onClick.RemoveListener(OnSelectionButtonClick);
    }
}
