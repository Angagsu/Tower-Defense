using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MazeFactory", menuName = "GamePlayExample/MazeFactory")]
public class MazeFactory : ScriptableObject
{
    [SerializeField] private MazeMark merQucha;
    [SerializeField] private MazeMark koxiQucha;
    [SerializeField] private MazeMark antarakuys;
    [SerializeField] private MazeMark baxcha;
    [SerializeField] private MazeMark tshnamabad;

    public MazeMark Get(MazeSkins skinType, Vector3 spawnPosition)
    {
        MazeMark instance = Instantiate(GetPrefab(skinType), spawnPosition, Quaternion.identity, null);
        instance.Initialize();
        return instance;
    }

    private MazeMark GetPrefab(MazeSkins skinType)
    {
        switch (skinType)
        {
            case MazeSkins.MerQucha:
                return merQucha;

            case MazeSkins.Koxiqucha:
                return koxiQucha;

            case MazeSkins.Antarakuys:
                return antarakuys;

            case MazeSkins.Baxcha:
                return baxcha;

            case MazeSkins.Tshnamabad:
                return tshnamabad;


            default:
                throw new ArgumentException(nameof(skinType));
        }
    }
}
