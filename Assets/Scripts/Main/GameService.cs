using UnityEngine;
using System.Collections;
using ServiceLocator.Utilities;
using ServiceLocator.Player;

public class GameService : GenericMonoSingleton<GameService>
{
    public PlayerService PlayerService { get; private set; }


    [SerializeField] public PlayerScriptableObject playerScriptableObject;

    private void Start()
    {
        PlayerService = new(playerScriptableObject);
    }

    private void Update()
    {
        PlayerService.Update();
    }
}
