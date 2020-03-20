using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUtil : MonoBehaviour
{
    public static void SetAnimDirection(Vector2 direction, Animator animator)
    {
        if (animator != null)
        {
            SetAnimatorParameter(animator, "moveX", direction.x);
            SetAnimatorParameter(animator, "moveY", direction.y);
        }
    }

    public static void SetAnimatorParameter(List<Animator> animators, string parameter, bool value)
    {
        foreach (Animator animator in animators)
        {
            SetAnimatorParameter(animator, parameter, value);
        }
    }

    public static void SetAnimatorParameter(Animator animator, string parameter, bool value)
    {
        if (animator != null)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == parameter) animator.SetBool(parameter, value);
            }
        }
    }

    public static void SetAnimatorParameter(List<Animator> animators, string parameter)
    {
        foreach (Animator animator in animators)
        {
            SetAnimatorParameter(animator, parameter);
        }
    }

    public static void SetAnimatorParameter(Animator animator, string parameter)
    {
        if (animator != null)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == parameter) animator.SetTrigger(parameter);
            }
        }
    }

    public static void SetAnimatorParameter(List<Animator> animators, string parameter, float value)
    {
        foreach (Animator animator in animators)
        {
            SetAnimatorParameter(animator, parameter, value);
        }
    }

    public static void SetAnimatorParameter(Animator animator, string parameter, float value)
    {
        if (animator != null)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == parameter) animator.SetFloat(parameter, value);
            }
        }
    }

    public static void enableAnimator(List<Animator> animators, bool value)
    {
        foreach (Animator animator in animators)
        {
            animator.enabled = value;
        }
    }

    public static void SetAnimatorSpeed(List<Animator> animators, float value)
    {
        foreach (Animator animator in animators)
        {
            animator.speed = value;
        }
    }

    public static bool HasParameter(Animator animator, string parameter)
    {
        if (animator != null)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == parameter) return true;
            }
        }

        return false;
    }
}
