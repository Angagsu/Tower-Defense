using UnityEngine;


public class TowerBuildUI : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectRoot;
    [SerializeField] private TowersMap towersMap;
    [SerializeField] private BuildsController buildsController;
    [SerializeField] private Vector3 positionOffset;

    private BuildingArea selectedBuildingArea;
    private Vector3 startPosition;
    


    private void Start()
    {
        startPosition = transform.position;
    }

    public void SelectArrowTower()
    {
        buildsController.BuildTowerByType(towersMap.ArrowTower);
    }

    public void SelectGolemTower()
    {
        buildsController.BuildTowerByType(towersMap.GolemTower);
    }
    public void SelectFireTower()
    {
        buildsController.BuildTowerByType(towersMap.FireTower);
    }

    public void SelectThunderTower()
    {
        buildsController.BuildTowerByType(towersMap.LightningTower);
    }
    public void SelectIceTower()
    {
        buildsController.BuildTowerByType(towersMap.IceTower);
    }

    public void SelectDefenderTower()
    {
        buildsController.BuildTowerByType(towersMap.DefenderTower);
    }

    public void SetTargetGroundForBuilding(BuildingArea selectedBuildingArea)
    {
        this.selectedBuildingArea = selectedBuildingArea;
        transform.position = selectedBuildingArea.gameObject.transform.position + positionOffset;
        gameObjectRoot.SetActive(true);
    }

    public void Hide()
    {
        gameObjectRoot.SetActive(false);
        transform.position = startPosition;
    }

    public bool IsGameObjectActive()
    {
        return gameObjectRoot.activeSelf;
    }
}
