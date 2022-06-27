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

    private QTEManager _qte;
    private readonly Dictionary<KeyCode, GameObject> _qteDict = new();

    private Random _rnd;
    public TaskUi _playerTask;

    public MenuControls HUD;

    private void Start()
    {
        _qteDict.Add(
            KeyCode.E,
            transform.GetChild(0).gameObject
        );

        _qte = GetComponent<QTEManager>();
        
        _qte.Init(
            _qteDict, fA:
            2f, dfA:
            0f, sA:
            0f
        );

        _qte.enabled = false;
        
        _rnd = new Random();

        foreach (Transform child in spawnPointsGameObject.transform)
        {
            _spawnPoints.Add(child.gameObject);
        }

        _playerInventory = player.Inventory;

        _dialogues.Add(0, new[] {"Приходи, когда соберешь все колокольчики!"});

        _dialogues.Add(1, new[] {"Да! Это именно то, что было нужно!"});

        _dialogues.Add(2,
            new[]
            {
                "Приветствую, путник! Ты наверняка напуган и хочешь спросить, где оказался, но..",
                "..могу ли я прежде попросить тебя об одной услуге? Прошу, будь так добр, принеси мне колокольчиков.",
                "В награду я отвечу на все твои вопросы!",
                "...",
                "Что говоришь? Спрашиваешь, где они находятся?",
                "Так откуда мне знать?! Если бы мне было известно, я бы не стал просить их найти!!!",
                "...прошу, принеси колокольчики."
            });

        _dialogues.Add(3,
            new[]
            {
                "Что такое, путник?",
                "Ох.. Кажется я знаю, в чем дело. Я не дал тебе обещанную награду? Но согласись, поиски ведь не составили для тебя большого труда?",
                "...",
                "Да.. Да, ты прав! Я всё же обещал. Тогда давай так... Ты достанешь мне еще немного колокольчиков, а я дам тебе ещё кое-что полезное! По рукам?"
            });

        _dialogues.Add(4,
            new[]
            {
                "Да! Это именно то, что быо нужно!",
                "Да-да, я знаю, что обещал ответить на твои вопросы, дать награду и всё такое, но...",
                "...мне всё ещё нужна твоя помощь! Найди мне больше этих чёртовых колокольчиков!!!",
                "Зачем? Не твоего ума дело!.. Я расскажу об этом позже.",
                "...",
                "Что значит 'не хочу' ?..",
                "Что. Это. ЗНАЧИТ?!",
                "То есть, мало того, что за такое плёвое поручение я даю награду, а ты шевелишься как черепаха... Теперь ты и вовсе отказываешься выполнить мою просьбу?!",
                "ТАКОВА ТВОЯ БЛАГОДАРНОСТЬ?! Я НЕ ОСТАВЛЮ ЭТО ПРОСТО ТАК!!!",
                "...",
                "...даже не пытайся убежать."
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
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        
        Debug.Log("entered");
        
        _qte.enabled = true;
        _qte.EnableQte();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _qte.enabled = false;
        _qte.DisableQte();
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