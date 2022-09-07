using UnityEngine.EventSystems;
using UnityEngine;

public class TowerBuildManager : MonoBehaviour
{
    public static TowerBuildManager Instance;


    private GroundBehavior selectedGround;
    [SerializeField] private TowersBuildUI towersBuildUI;
    [SerializeField] private TowerUpgradeUI towerUpgradeUI;
    [SerializeField] private Hero hero;
    
    [HideInInspector]
    public TowerBlueprint towerToBuild;

    [HideInInspector] public bool SelectMissileLauncherTower = false;
    [HideInInspector] public bool SelectStandardTower = false;
    [HideInInspector] public bool SelectLaserTower = false;

    public bool CanBuild { get { return towerToBuild != null; } }
    public bool HasManey { get { return PlayerStats.Money >= towerToBuild.Cost; } }

    private Camera mainCamera;
    private int groundLayer;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one TowerBuilManager in scene!");
            return;
        }

        Instance = this;

        towerUpgradeUI = GameObject.Find("TowerUpgradeUI").GetComponent<TowerUpgradeUI>();

        mainCamera = Camera.main;
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    private void Update()
    {
        DeselectAllUIAndHeroesThenClickGround();
    }

    public void SelectedGround(GroundBehavior groundBehavior)
    {
        if (selectedGround == groundBehavior)
        {
            DeselectGround();
            return;
        }

        hero.DeselectHeroes();
        towersBuildUI.towerBuildUI.SetActive(!towersBuildUI.towerBuildUI.activeSelf);

        if (towersBuildUI.towerBuildUI.activeSelf)
        {
            towersBuildUI.towerBuildUI.SetActive(false);
        }

        selectedGround = groundBehavior;
        towerToBuild = null;


        towerUpgradeUI.SetTargetGround(groundBehavior);
    }

    public void SelectedGroundForBuildTowerUI(GroundBehavior groundBehavior)
    {
        if (selectedGround == groundBehavior)
        {
            DeselectGround();
            return;
        }

        hero.DeselectHeroes();
        towerUpgradeUI.towerUpgradeUI.SetActive(!towerUpgradeUI.towerUpgradeUI.activeSelf);

        if (towerUpgradeUI.towerUpgradeUI.activeSelf)
        {
            towerUpgradeUI.towerUpgradeUI.SetActive(false);
        }
        selectedGround = groundBehavior;
        towerToBuild = null;
        towersBuildUI.SetTargetGroundForBuilding(groundBehavior);
    }

    private void DeselectAllUIAndHeroesThenClickGround()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
        {
            if (Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider &&
                raycastHit.collider.gameObject.layer.CompareTo(groundLayer) == 0)
            {
                DeselectGround();
                hero.DeselectHeroes();
            }
        }

    }
    public void DeselectGround()
    {
        selectedGround = null;
        towerUpgradeUI.HideCanvas();
        towersBuildUI.HideCanvas();
    }
    public void SelectTowerToBuild(TowerBlueprint tower)
    {
        towerToBuild = tower;
        DeselectGround();
    }

    public TowerBlueprint GetTowerToBuild()
    {
        return towerToBuild;
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
