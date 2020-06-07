using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class BossMechanicSpawn : MonoBehaviour
{
    public enum SpawnPositionType
    {
        none,
        spawnPoints,
        randomPoints,
        area,
        target,
        sender
    }

    public enum RotationType
    {
        none,
        random,
        identity,
        target,
        parent
    }

    [System.Serializable]
    public class ChildSequenceProperty : SequenceProperty
    {
        public Object spawnObject;

        [ShowIf("spawnObject")]
        [HideIf("spawnPositonType", SpawnPositionType.spawnPoints)]
        [MinValue(1)]
        public int amount = 1;

        [ShowIf("spawnObject")]
        [Tooltip("Time between Spawns")]
        public float spawnDelay = 0f;

        [ShowIf("spawnObject")]
        [MinValue(0)]
        public int repeat = 0;

        [ShowIf("spawnObject")]
        [Tooltip("Time between Spawns")]
        [MinValue(0.1)]
        public float repeatDelay = 1f;
    }

    [System.Serializable]
    public class SequenceProperty
    {
        public SpawnPositionType spawnPositonType = SpawnPositionType.none;
        public RotationType rotationType = RotationType.none;

        [ShowIf("rotationType", RotationType.random)]
        [MaxValue(360)]
        [MinValue(0)]
        public int rotationFactor;
    }

    [System.Serializable]
    public class SequenceObject
    {
        private int amount = 1;
        private float delay = 0;
        public bool isRunning = true;
        public bool spawnIt = false;
        public GameObject spawnPoint;
        private float elapsed;

        public SequenceObject(GameObject spawnPoint, int amount, float delay)
        {
            this.spawnPoint = spawnPoint;
            this.amount = amount;
            this.delay = delay;
        }

        public void Updating(float time)
        {
            if (this.elapsed <= 0) this.spawnIt = true;
            else this.elapsed -= time;
        }

        public void SetNext()
        {
            this.spawnIt = false;
            this.elapsed = this.delay;
            this.amount--;
            if (this.amount <= 0) this.isRunning = false;
        }
    }


    [SerializeField]
    [HideLabel]
    [BoxGroup("Main")]
    private SequenceProperty selfProperty;

    [SerializeField]
    [BoxGroup("Main")]
    private float startDelay = 0f;

    [SerializeField]
    [HideLabel]
    [BoxGroup("Children")]
    private ChildSequenceProperty childProperty;

    private Character sender;
    private Character target;
    private float timeLeftToSpawnNext;
    private int counter;
    private bool isRunning = true;

    private List<SequenceObject> sequences = new List<SequenceObject>();
    private List<GameObject> spawnPoints = new List<GameObject>();

    //Order

    private void Start()
    {
        SetSpawnPoints();
        this.transform.rotation = this.GetRotation(this.selfProperty.rotationType, this.selfProperty.rotationFactor, this.gameObject);
        this.transform.position = GetSpawnPosition(this.selfProperty.spawnPositonType).transform.position;
        this.timeLeftToSpawnNext = this.startDelay;
    }

    private void SetSpawnPoints()
    {
        foreach (Transform child in this.transform) this.spawnPoints.Add(child.gameObject);
    }

    public void Initialize(Character sender, Character target)
    {
        this.sender = sender;
        this.target = target;
    }

    private void Update()
    {
        if(this.isRunning) AddSequences(); //Add new sequence 

        UpdatingSequences(); //update existing sequences

        if (!this.isRunning && this.sequences.Count == 0) this.enabled = false; //deactivate when all sequences are done
    }

    private void UpdatingSequences()
    {
        foreach (SequenceObject sequence in this.sequences)
        {
            if (sequence.isRunning)
            {
                sequence.Updating(Time.deltaTime);

                if (sequence.spawnIt)
                {
                    Quaternion rotation = GetRotation(this.childProperty.rotationType, this.childProperty.rotationFactor, sequence.spawnPoint);
                    Instantiate(sequence.spawnPoint.transform.position, rotation);
                    sequence.SetNext();
                }
            }        
        }

        this.sequences.RemoveAll(x => x.isRunning == false);
    }

    private void AddSequences()
    {
        this.timeLeftToSpawnNext -= Time.deltaTime;
        if (this.timeLeftToSpawnNext <= 0 && this.childProperty.spawnObject != null) AddSequence();
    }

    private void AddSequence()
    {
        GameObject position = GetSpawnPosition(this.childProperty.spawnPositonType);

        this.sequences.Add(new SequenceObject(position, this.childProperty.repeat, this.childProperty.repeatDelay));
        this.timeLeftToSpawnNext = this.childProperty.spawnDelay;
        this.counter++;
        if (this.counter >= this.GetMax()) this.isRunning = false;
    }

    private int GetMax()
    {
        if (this.childProperty.spawnPositonType == SpawnPositionType.spawnPoints) return this.spawnPoints.Count;
        else return this.childProperty.amount;
    }

    private void Instantiate(Vector2 position, Quaternion rotation)
    {
        if (this.childProperty.spawnObject.GetType() == typeof(GameObject))
        {
            GameObject spawnedObject = Instantiate(this.childProperty.spawnObject, position, rotation, this.transform) as GameObject;
            SetSkill(spawnedObject.GetComponent<Skill>());
        }
        else if (this.childProperty.spawnObject.GetType() == typeof(Ability))
        {
            Ability ability = Instantiate(this.childProperty.spawnObject) as Ability;
            Skill skill = ability.InstantiateSkill(position, this.sender, rotation);
            skill.transform.SetParent(this.transform);
            Destroy(ability);
        }
    }

    private void SetSkill(Skill skill)
    {
        if (skill != null)
        {
            skill.sender = this.sender;
            skill.target = this.target;
        }
    }

    private GameObject GetSpawnPosition(SpawnPositionType type)
    {
        switch (type)
        {
            case SpawnPositionType.sender: return CreateEmptyGameObject(GetPositionFromCharacter(this.sender)); 
            case SpawnPositionType.target: return CreateEmptyGameObject(GetPositionFromCharacter(this.target));
            case SpawnPositionType.area: return CreateEmptyGameObject(UnityUtil.GetRandomVector(this.GetComponent<Collider2D>()));
            case SpawnPositionType.randomPoints: return GetRandomPositionFromSpawnPoint(spawnPoints); 
            case SpawnPositionType.spawnPoints: return GetPositionFromSpawnPoint();
        }

        return CreateEmptyGameObject(Vector2.zero);
    }

    private GameObject CreateEmptyGameObject(Vector2 position)
    {
        GameObject temp = new GameObject("spawnPoint");
        temp.transform.position = position;
        temp.transform.SetParent(this.transform);
        return temp;
    }

    private Vector2 GetPositionFromCharacter(Character character)
    {
        return character.GetGroundPosition();
    }

    private GameObject GetPositionFromSpawnPoint()
    {
        return spawnPoints[this.counter];
    }

    private GameObject GetRandomPositionFromSpawnPoint(List<GameObject> spawnPoints)
    {
        int rng = Random.Range(0, spawnPoints.Count);
        return spawnPoints[rng];
    }

    private Quaternion GetRotation(RotationType type, int rotationfactor, GameObject gameObject)
    {
        switch (type)
        {
            case RotationType.random: return SetRandomRotation(rotationfactor);
            case RotationType.target: return SetRotationOnTarget(gameObject.transform.position);
            case RotationType.identity: return Quaternion.identity;
            case RotationType.parent: return this.transform.rotation;
        }

        return gameObject.transform.rotation;
    }

    private Quaternion SetRandomRotation(int factor)
    {
        int divisor = 360 / factor;
        int rng = Random.Range(0, (divisor + 1));
        float result = factor * rng;

        return Quaternion.Euler(0, 0, result);
    }

    private Quaternion SetRotationOnTarget(Vector2 origin)
    {
        Vector2 direction = (this.target.GetGroundPosition() - origin).normalized;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        return Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
