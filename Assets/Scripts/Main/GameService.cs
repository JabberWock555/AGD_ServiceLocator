using UnityEngine;
using ServiceLocator.Utilities;
using ServiceLocator.Player;
using ServiceLocator.Wave;
using ServiceLocator.UI;
using ServiceLocator.Sound;
using ServiceLocator.Map;
using ServiceLocator.Events;

public class GameService : GenericMonoSingleton<GameService>
{
    public PlayerService PlayerService { get; private set; }
    public WaveService WaveService { get; private set; }
    public SoundService SoundService { get; private set; }
    public MapService MapService { get; private set; }
    public EventService EventService { get; private set; }
    public UIService UIService => uIService;

    [Header("UI Service Refs")]
    [SerializeField] private UIService uIService;

    [Header("Player Service Refs")]
    [SerializeField] public PlayerScriptableObject playerScriptableObject;

    [Header("Wave Service Refs")]
    [SerializeField] private WaveScriptableObject waveScriptableObject;

    [Header("Sound Service Refs")]
    [SerializeField] private SoundScriptableObject soundScriptableObject;
    [SerializeField] private AudioSource audioEffects;
    [SerializeField] private AudioSource backgroundMusic;

    [Header("Sound Service Refs")]
    [SerializeField] private MapScriptableObject mapScriptableObject;

    private void Start()
    {
        EventService = new();
        PlayerService = new(playerScriptableObject);
        WaveService = new(waveScriptableObject);
        SoundService = new(soundScriptableObject, audioEffects, backgroundMusic);
        MapService = new(mapScriptableObject);
    }

    private void Update()
    {
        PlayerService.Update();
    }
}
