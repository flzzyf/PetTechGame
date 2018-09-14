using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Singleton<Dog>
{
    public Transform target;

    public float range_vision = 8;
    public float range_interact = 4;

    public float stat_hungry = 1000;
    float stat_hungry_current;
    public float stat_happiness = 1000;
    float stat_happiness_current;

    public float speed = 3;

    public Animator animator;

    public Transform gfx;

    public Vector2 randomBarkInterval;

    GameObject carryingObject;

    public Transform carryPos;

    List<Transform> objectList;

    void Start()
    {
        stat_hungry_current = stat_hungry * 0.7f;
        stat_happiness_current = stat_happiness * 0.4f;

        if (randomBarkInterval != Vector2.zero)
            StartCoroutine(BarkSometimes());
    }

    void Update()
    {
        ChangeStat(Stat.hungry, -1);
        if (stat_hungry_current / stat_hungry < 0.5)
        {
            ChangeStat(Stat.happiness, -1);
        }
        else
        {
            ChangeStat(Stat.happiness, 1);
        }

        if (carryingObject != null)
            return;

        objectList = GameManager.instance.interactableobjects;

        for (int i = objectList.Count - 1; i >= 0; i--)
        {
            if (objectList[i].gameObject.GetComponent<InteractableObject>().type == ObjectType.toy &&
                            HorizontalDistance(objectList[i].position, Camera.main.transform.position) < 1)
            {
                objectList.Remove(objectList[i]);
            }
        }

        if (objectList.Count == 0)
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
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            //超过交互范围，靠近
            if (HorizontalDistance(transform.position, target.position) > range_interact / 2)
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
                    Interact(target.gameObject);
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
        Transform nearest = objectList[0];

        for (int i = 1; i < objectList.Count; i++)
        {
            if (Vector3.Distance(transform.position, objectList[i].position) <
                Vector3.Distance(transform.position, objectList[i - 1].position))
            {
                nearest = objectList[i];
            }
        }

        target = nearest;
    }

    //交互
    void Interact(GameObject _target)
    {
        if (_target.GetComponent<InteractableObject>().type == ObjectType.food)
        {
            Eat(_target);
        }
        else if (_target.GetComponent<InteractableObject>().type == ObjectType.toy)
        {
            Carry(_target);
        }
    }
    //吃掉
    void Eat(GameObject _target)
    {
        SoundManager.instance.Play("Eat");

        ChangeStat(Stat.hungry, 200);

        target = null;
        GameManager.instance.interactableobjects.Remove(_target.transform);
        Destroy(_target);
    }
    //叼起
    void Carry(GameObject _target)
    {
        carryingObject = _target;

        _target.transform.parent = carryPos;
        _target.transform.localPosition = Vector3.zero;

        _target.GetComponent<InteractableObject>().Drag();

        StartCoroutine(ReturnToy());
    }

    IEnumerator ReturnToy()
    {
        animator.SetBool("walking", true);

        //朝玩家走，小于范围则放下球
        while (HorizontalDistance(transform.position, Camera.main.transform.position) > 0.5f)
        {
            Vector3 dir = Camera.main.transform.position - transform.position;
            dir.y = 0;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
            yield return null;
        }

        animator.SetBool("walking", false);
        Discard();
        target = null;

    }
    //放下
    void Discard()
    {
        GameObject obj = carryingObject;
        carryingObject = null;

        obj.transform.parent = null;
        obj.GetComponent<InteractableObject>().Left();

    }

    IEnumerator BarkSometimes()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(randomBarkInterval.x, randomBarkInterval.y));
            SoundManager.instance.Play("Bark");
        }
    }

    enum Stat { hungry, happiness }
    void ChangeStat(Stat _stat, int _amount)
    {
        if (_stat == Stat.hungry)
        {
            stat_hungry_current += _amount;
            GameManager.instance.slider_hungry.value = stat_hungry_current / stat_hungry;
        }
        else if (_stat == Stat.happiness)
        {
            stat_happiness_current += _amount;
            GameManager.instance.slider_happiness.value = stat_happiness_current / stat_happiness;
        }
    }
}
