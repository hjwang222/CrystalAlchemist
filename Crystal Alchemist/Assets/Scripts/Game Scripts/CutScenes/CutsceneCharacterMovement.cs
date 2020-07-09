using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;

public class CutsceneCharacterMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 position;

    [SerializeField]
    private float duration;

    [Required]
    [SerializeField]
    Rigidbody2D rigidbody;

    public void Play()
    {
        this.rigidbody?.DOMove(position, this.duration);
    }
}
