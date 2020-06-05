using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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
    target
}

[System.Serializable]
public class SequenceProperty
{
    public Object spawnObject;

    [ShowIf("spawnObject")]
    public SpawnPositionType spawnPositonType = SpawnPositionType.none;

    [ShowIf("spawnObject")]
    public float startDelay = 0f;

    [ShowIf("spawnObject")]
    [Tooltip("Time between Spawns")]
    public float spawnDelay = 0f;

    [ShowIf("spawnObject")]
    [MinValue(1)]
    public int amount = 1;

    [ShowIf("spawnObject")]
    [HideIf("spawnPositonType", SpawnPositionType.none)]
    [HideIf("spawnPositonType", SpawnPositionType.area)]
    [HideIf("spawnPositonType", SpawnPositionType.target)]
    [HideIf("spawnPositonType", SpawnPositionType.sender)]
    public List<GameObject> spawnPoints = new List<GameObject>();

    [Space(10)]
    public RotationType rotationType = RotationType.none;

    [ShowIf("rotationType", RotationType.random)]
    [MaxValue(360)]
    [MinValue(0)]
    public int rotationFactor;


    public int GetMax()
    {
        if (this.spawnPositonType == SpawnPositionType.spawnPoints) return this.amount * this.spawnPoints.Count;
        else return this.amount;
    }
}

public class BossMechanicProperty : MonoBehaviour
{
    [SerializeField]
    [HideLabel]
    [BoxGroup("Initialize")]
    private SequenceProperty bossSequenceProperty;

    private Character sender;
    private Character target;
    private float timeLeft;
    private int counter;

    //Self Positioning and Rotation
    //Order

    private void Start()
    {
        this.transform.rotation = GetRotation(this.bossSequenceProperty);
        this.timeLeft = this.bossSequenceProperty.startDelay;
    }

    public void Initialize(Character sender, Character target)
    {
        this.sender = sender;
        this.target = target;
    }

    private void Update()
    {
        SpawnGameObjects();
    }


    private void SpawnGameObjects()
    {
        this.timeLeft -= Time.deltaTime;

        if (this.timeLeft <= 0 && this.bossSequenceProperty.spawnObject != null)
        {
            Vector2 position = GetSpawnPosition(this.bossSequenceProperty);

            if (this.bossSequenceProperty.spawnObject.GetType() == typeof(GameObject))
            {
                GameObject spawnedObject = Instantiate(this.bossSequenceProperty.spawnObject, position, this.transform.rotation) as GameObject;
                SetSkill(spawnedObject.GetComponent<Skill>());
            }
            else if (this.bossSequenceProperty.spawnObject.GetType() == typeof(Ability))
            {
                Ability ability = Instantiate(this.bossSequenceProperty.spawnObject) as Ability;
                ability.InstantiateSkill(position, this.sender, this.transform.rotation);
                Destroy(ability);
            }

            this.timeLeft = this.bossSequenceProperty.spawnDelay;
            this.counter++;
        }

        if (this.counter >= this.bossSequenceProperty.GetMax()) this.enabled = false;
    }

    private void SetSkill(Skill skill)
    {
        if (skill != null)
        {
            skill.sender = this.sender;
            skill.target = this.target;
        }
    }

    private Vector2 GetSpawnPosition(SequenceProperty property)
    {
        switch (property.spawnPositonType)
        {
            case SpawnPositionType.sender: return GetPositionFromCharacter(this.sender);
            case SpawnPositionType.target: return GetPositionFromCharacter(this.target);
            case SpawnPositionType.randomPoints: return GetRandomPositionFromSpawnPoint(property.spawnPoints);
            case SpawnPositionType.spawnPoints: return GetPositionFromSpawnPoint(property.spawnPoints);
            case SpawnPositionType.area: return UnityUtil.GetRandomVector(this.GetComponent<Collider2D>());
        }
        return Vector2.zero;
    }

    private Vector2 GetPositionFromCharacter(Character character)
    {
        return character.GetGroundPosition();
    }

    private Vector2 GetPositionFromSpawnPoint(List<GameObject> spawnPoints)
    {
        int index = this.counter % this.bossSequenceProperty.spawnPoints.Count;
        return spawnPoints[index].transform.position;
    }

    private Vector2 GetRandomPositionFromSpawnPoint(List<GameObject> spawnPoints)
    {
        int rng = Random.Range(0, spawnPoints.Count);
        return spawnPoints[rng].transform.position;
    }

    private Quaternion GetRotation(SequenceProperty property)
    {
        switch (property.rotationType)
        {
            case RotationType.random: return SetRandomRotation(property.rotationFactor);
            case RotationType.target: return SetRotationOnTarget();
            case RotationType.identity: return Quaternion.identity;
        }

        return this.transform.rotation;
    }

    private Quaternion SetRandomRotation(int factor)
    {
        int divisor = 360 / factor;
        int rng = Random.Range(0, (divisor + 1));
        float result = factor * rng;

        return Quaternion.Euler(0, 0, result);
    }

    private Quaternion SetRotationOnTarget()
    {
        Vector2 direction = (this.target.GetGroundPosition() - (Vector2)gameObject.transform.position).normalized;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        return Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
