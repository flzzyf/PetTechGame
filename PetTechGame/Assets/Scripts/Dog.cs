using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public Transform target;

    public float range_vision = 8;
    public float range_interact = 4;

    public float stat_hungry = 5;
    public float speed = 3;

    public Animator animator;

    public Transform gfx;

    void Start()
    {
        StartCoroutine(BarkSometimes());
    }

    void Update()
    {
        if (GameManager.instance.interactableobjects.Count == 0)
            return;

        //无目标就搜索目标
        if (target == null)
            SearchTarget();
        else
        {
            //有目标
            Vector3 dir = target.position - transform.position;

            //设置朝向
            if (Vector3.Dot(transform.right, dir.normalized) > 0)
            {
                GetComponentInChildren<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponentInChildren<SpriteRenderer>().flipX = false;
            }
            //超过交互范围，靠近
            if (HorizontalDistance(transform.position, target.position) > range_interact)
            {
                dir.y = 0;
                transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

                animator.SetBool("walking", true);
            }
            else
            {
                animator.SetBool("walking", false);

                if (Vector3.Distance(transform.position, target.position) < range_interact)
                {
                    //目标在交互范围内
                    Eat(target.gameObject);
                }
            }
        }
    }

    float HorizontalDistance(Vector3 _origin, Vector3 _target)
    {
        _target.y = _origin.y;
        return Vector3.Distance(_origin, _target);
    }

    void SearchTarget()
    {
        Transform nearest = GameManager.instance.interactableobjects[0];

        for (int i = 1; i < GameManager.instance.interactableobjects.Count; i++)
        {
            if (Vector3.Distance(transform.position, GameManager.instance.interactableobjects[i].position) <
                Vector3.Distance(transform.position, GameManager.instance.interactableobjects[i - 1].position))
            {
                nearest = GameManager.instance.interactableobjects[i];
            }
        }

        target = nearest;
    }

    void Eat(GameObject _target)
    {
        SoundManager.instance.Play("Eat");

        target = null;
        GameManager.instance.interactableobjects.Remove(_target.transform);
        Destroy(_target);
    }

    IEnumerator BarkSometimes()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3, 5));
            SoundManager.instance.Play("Bark");
        }
    }

}
