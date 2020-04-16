using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum SpawnPositionType
{
    none,
    spawnPoints,
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
    public GameObject spawnObject;

    [ShowIf("spawnObject")]
    public SpawnPositionType spawnPositonType = SpawnPositionType.none;

    [ShowIf("spawnObject")]
    public float spawnDelay = 0f;

    [ShowIf("spawnObject")]
    [ShowIf("spawnPositonType", SpawnPositionType.spawnPoints)]
    public List<GameObject> spawnPoints = new List<GameObject>();

    [Space(10)]
    public RotationType rotationType = RotationType.none;
    [ShowIf("rotationType", RotationType.random)]
    [MaxValue(360)]
    [MinValue(0)]
    public int rotationFactor;
}

public class BossMechanicProperty : MonoBehaviour
{
    [SerializeField]
    [HideLabel]
    [BoxGroup("Initialize")]
    private SequenceProperty bossSequenceProperty;

    private Character sender;
    private Character target;
    private float timeElapsed;

    private void Start()
    {
        this.transform.rotation = GetRotation(this.bossSequenceProperty);
    }

    public void Initialize(Character sender, Character target)
    {
        this.sender = sender;
        this.target = target;
    }

    private void Update()
    {
        this.timeElapsed += Time.deltaTime;
        SpawnGameObjects();
    }

    private void SpawnGameObjects()
    {
        if (this.timeElapsed >= this.bossSequenceProperty.spawnDelay && this.bossSequenceProperty.spawnObject != null)
        {
            GameObject spawnedObject = Instantiate(this.bossSequenceProperty.spawnObject, GetSpawnPosition(this.bossSequenceProperty), this.transform.rotation);
            SetSkill(spawnedObject.GetComponent<Skill>());
            this.enabled = false;
        }
    }

    private void SetSkill(Skill skill)
    {
        if (skill != null)
        {
            skill.sender = this.sender;
            skill.target = this.target;
            skill.overridePosition = false;
        }
    }

    private Vector2 GetSpawnPosition(SequenceProperty property)
    {
        switch (property.spawnPositonType)
        {
            case SpawnPositionType.sender: return GetPositionFromCharacter(this.sender);
            case SpawnPositionType.target: return GetPositionFromCharacter(this.target);
            case SpawnPositionType.spawnPoints: return GetPositionFromSpawnPoint(property.spawnPoints);
        }
        return Vector2.zero;
    }

    private Vector2 GetPositionFromCharacter(Character character)
    {
        return character.GetGroundPosition();
    }

    private Vector2 GetPositionFromSpawnPoint(List<GameObject> spawnPoints)
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
        }

        return Quaternion.identity;
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
        Vector2 direction = (this.target.transform.position - gameObject.transform.position).normalized;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        return Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
