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
    [Required]
    public GameObject gameObject;

    public SpawnPositionType spawnPositonType = SpawnPositionType.none;
    public float spawnDelay = 0f;

    [ShowIf("spawnPositonType", SpawnPositionType.spawnPoints)]
    public List<GameObject> spawnPoints = new List<GameObject>();

    public RotationType rotationType = RotationType.none;
    [ShowIf("rotationType", RotationType.random)]
    [MaxValue(360)]
    [MinValue(0)]
    public int rotationFactor;
}

public class BossSequence : MonoBehaviour
{
    [SerializeField]
    private Character sender;

    [SerializeField]
    private Character target;

    [SerializeField]
    [HideLabel]
    [BoxGroup("Initialize")]
    private SequenceProperty bossSequenceProperty;

    [BoxGroup("Children")]
    [SerializeField]
    private List<SequenceProperty> properties = new List<SequenceProperty>();

    private float timeElapsed;

    private void Start()
    {
        this.transform.position = GetSpawnPosition(this.bossSequenceProperty);
        this.transform.rotation = GetRotation(this.bossSequenceProperty);
    }

    private void Update()
    {
        this.timeElapsed += Time.deltaTime;
        SpawnGameObjects();
    }

    private void SpawnGameObjects()
    {
        List<SequenceProperty> temp = new List<SequenceProperty>();
        temp.AddRange(this.properties);

        foreach (SequenceProperty property in this.properties)
        {
            if (this.timeElapsed >= property.spawnDelay)
            {                
                GameObject spawnedObject = Instantiate(property.gameObject, GetSpawnPosition(property), GetRotation(property));
                SetSkill(spawnedObject.GetComponent<Skill>());

                temp.Remove(property);
            }
        }
        this.properties = temp;

        if (this.properties.Count == 0) DestroyIt();
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

    public void Initialize(Character sender, Character target)
    {
        this.sender = sender;
        this.target = target;
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
        if (character.shadowRenderer != null) return character.shadowRenderer.transform.position;
        return character.transform.position;
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
        int rng = Random.Range(0, (divisor+1));
        float result = factor * rng;

        return Quaternion.Euler(0, 0, result);
    }

    private Quaternion SetRotationOnTarget()
    {
        Vector2 direction = (this.target.transform.position - gameObject.transform.position).normalized;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        return Quaternion.Euler(new Vector3(0, 0, angle));
    }

    #region Trigger

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }

    #endregion

}
