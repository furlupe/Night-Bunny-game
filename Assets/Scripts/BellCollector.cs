using System.Collections.Generic;
using System.Linq;
using ui;
using UnityEngine;
using Random = System.Random;

public class BellCollector : MonoBehaviour
{
    public Player player;
    public DialogueManager dialogueManager;

    public GameObject bellPrefab;

    private int _bellsAmount = 1, _id = 2;
    private const int IDNotEnoughBells = 0;
    private const int IDEnoughBells = 1;

    public new string name = "bell";

    private readonly Dictionary<int, string[]> _dialogues = new();
    private Dictionary<string, int> _playerInventory = new();

    private readonly Dialogue _dialogue = new();

    private bool _taskCompleted, _taskGiven, _bellsSpawned;

    [SerializeField] private GameObject spawnPointsGameObject;
    private readonly List<GameObject> _spawnPoints = new();
    private readonly HashSet<int> _beenSpawned = new();

    private Random _rnd;
    public TaskUi _playerTask;

    public MenuControls HUD;

    private void Start()
    {
        _rnd = new Random();

        foreach (Transform child in spawnPointsGameObject.transform)
        {
            _spawnPoints.Add(child.gameObject);
        }

        _playerInventory = player.Inventory;

        _dialogues.Add(0, new[] {"Приходи, когда соберешь все колокольчики!"});

        _dialogues.Add(1, new[] {"Да! Это то, что мне было нужно!"});

        _dialogues.Add(2,
            new[]
            {
                "Приветствую, путник!",
                "Могу ли я попросить тебя об одной просьбе? Принеси мне пожалуйста, колокольчиков, прошу.",
                "Я не оставлю тебя без награды!",
                "...",
                "Что говоришь? Где они находятся?",
                "...",
                "Откуда мне знать? Если бы оно было так, я бы тебя не просил их поискать. >:C"
            });

        _dialogues.Add(3,
            new[]
            {
                "Что такое, путник?",
                "А...Я не дал тебе обещанной награды? Но согласись же, достать эти колокольчики не составило тебе большего труда, да?.",
                "...",
                "Да, ты прав, я же все же обещал. Тогда...давай ты достанешь мне еще несколько колокольчиков, и тогда я тебе точно что-то дам!"
            });

        _dialogues.Add(4,
            new[]
            {
                "Да-да, я знаю, что я обещал дать тебе награду и все такое, но...",
                "...мне все еще нужна твоя помощь по поиску этих чертовых колокольчиков!",
                "Зачем? Не могу пока сказать",
                "...",
                "Что значит 'НЕ ХОЧУ' ?!",
                "Что. Это. Значит. ?!",
                "То есть ты мало того, что ты делаешь эту работу непростительно долго, так теперь и вовсе не хочешь ее делать?!",
                "И ТЫ ДУМАЕШЬ Я ОСТАВЛЮ ЭТО ПРОСТО ТАК?!!!",
                "НУ УЖ НЕТ. ДАЖЕ НЕ ПЫТАЙСЯ УБЕЖАТЬ.",
                "А-А-А-А!"
            });

        _dialogue.name = "Bell Collector";
        _dialogue.sentences = _dialogues[_id];
        GetComponent<NPC>().dialogue = _dialogue;
    }

    private void Update()
    {
        if (_playerInventory.Keys.ToList().Contains(name))
        {
            _taskCompleted = _playerInventory[name] >= _bellsAmount;

            if (_taskGiven)
            {
                _playerTask.UpdateTask(
                    $"Найти колокольчики: {_playerInventory[name]}/{_bellsAmount}"
                );
            }
        }

        switch (_taskCompleted)
        {
            case true:
                _playerTask.SetTask(
                    "Помощь Сборщику",
                    "Вернуться к Сборщику"
                );
                _dialogue.sentences = _dialogues[IDEnoughBells];
                break;
            case false when _taskGiven:
                _dialogue.sentences = _dialogues[IDNotEnoughBells];
                GetComponent<NPC>().dialogue = _dialogue;
                break;
        }

        if (!dialogueManager.isEnded) return;

        if (_id == 4)
        {
            HUD.EnwhiteShadow();
            player.Die();
        }

        _taskGiven = true;
        dialogueManager.isEnded = false;
        player.GetComponent<Player>().Enable();

        if (!_bellsSpawned && _taskGiven)
        {
            for (var i = 0; i < _bellsAmount; i++)
            {
                var bell = Instantiate(bellPrefab);

                var pos = GetRandomSpawnPointPosition();

                bell.transform.position = pos;
            }

            _playerTask.SetTask(
                "Помощь Сборщику",
                $"Найти колокольчики: 0/{_bellsAmount}"
            );

            _bellsSpawned = true;
        }

        if (_taskCompleted)
        {
            _bellsAmount += 2;
            _playerInventory[name] = 0;

            _taskCompleted = false;
            _taskGiven = false;
            _bellsSpawned = false;

            _id++;
            _dialogue.sentences = _dialogues[_id];

            _playerTask.SetTask(
                "Где я?",
                "Поговорить со Сборщиком"
            );
        }

        GetComponent<NPC>().dialogue = _dialogue;
    }

    private Vector2 GetRandomSpawnPointPosition()
    {
        var range = Enumerable.Range(0, _spawnPoints.Count - 1)
            .Where(
                i => !_beenSpawned.Contains(i)
            );

        var index = range.ElementAt(
            _rnd.Next(0, _spawnPoints.Count - _beenSpawned.Count - 1)
        );

        _beenSpawned.Add(index);

        return _spawnPoints[index].transform.position;
    }
}