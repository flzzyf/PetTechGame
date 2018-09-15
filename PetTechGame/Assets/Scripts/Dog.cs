using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Singleton<Dog>
{
    [HideInInspector]
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

    enum DogStatus { idle, busy }
    DogStatus status;

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

        if (status != DogStatus.idle)
            return;

        //无目标就搜索目标
        if (target == null)
            target = SearchTarget();

        if (target != null)
        {
            status = DogStatus.busy;
            //有目标向其移动
            StartCoroutine(MoveToTarget(target.transform));
        }
    }

    float HorizontalDistance(Vector3 _origin, Vector3 _target)
    {
        _target.y = _origin.y;
        return Vector3.Distance(_origin, _target);
    }

    Transform SearchTarget()
    {
        objectList = new List<Transform>();
        //去除离玩家太近的物体
        for (int i = 0; i < GameManager.instance.interactableobjects.Count; i++)
        {
            if (!(GameManager.instance.interactableobjects[i].gameObject.GetComponent<InteractableObject>().type == ObjectType.toy &&
                            HorizontalDistance(GameManager.instance.interactableobjects[i].position, Camera.main.transform.position) < 1))
            {
                objectList.Add(GameManager.instance.interactableobjects[i]);
            }
        }

        if (objectList.Count == 0)
            return null;

        Transform nearest = objectList[0];

        for (int i = 1; i < objectList.Count; i++)
        {
            if (Vector3.Distance(transform.position, objectList[i].position) <
                Vector3.Distance(transform.position, objectList[i - 1].position))
            {
                nearest = objectList[i];
            }
        }

        return nearest;
    }

    IEnumerator MoveToTarget(Transform _target)
    {
        animator.SetBool("walking", true);

        bool speedUp = false;
        //目标是玩具则加速
        if (_target.gameObject.GetComponent<InteractableObject>().type == ObjectType.toy)
        {
            speedUp = true;
            speed *= 2;
        }

        //目标在交互范围外
        while (Vector3.Distance(transform.position, _target.position) > range_interact)
        {
            Vector3 dir = _target.position - transform.position;

            //设置朝向
            if (Mathf.Abs(Vector3.Dot(transform.right, dir.normalized)) > 0.05f)
            {
                if (Vector3.Dot(transform.right, dir.normalized) > 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }

            if (HorizontalDistance(transform.position, _target.position) > range_interact / 2)
            {
                //移动
                dir.y = 0;
                transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
            }


            yield return null;
        }

        if (speedUp)
            speed /= 2;

        animator.SetBool("walking", false);

        Interact(target.gameObject);
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

        status = DogStatus.idle;
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

        status = DogStatus.idle;
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
