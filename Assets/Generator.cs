using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour
{
    public Section[] prefabs; // секции с двумя контактными точками
    public Section[] start; // стартовые секции
    public Section[] stop; // конечные секции
    public int sections = 20; // макс. возможное число секций
    public float sectionSize = 1; // размер секции, все стороны должны быть равны

    private Section current, previous;
    private int index;

    private void Awake()
    {
        Generate();
    }

    private void InstSection(Section[] arr)
    {
        current = Instantiate(arr[Random.Range(0, arr.Length)]) as Section;
        current.gameObject.name = "Platform_0" + index;
        current.transform.parent = transform;

        if (previous)
        {
            current.transform.forward = previous.endPoint.forward;
            current.transform.position += previous.endPoint.position - current.startPoint.position;
        }
    }

    private void Generate()
    {
        InstSection(start);
        previous = current;

        Section tmp = null;

        for (int i = 0; i < sections; i++)
        {
            index = i;

            if (!Check())
            {
                tmp = current;
                InstSection(prefabs);
            }
            else
            {
                Destroy(current.gameObject);
                previous = tmp;
                InstSection(stop);
                return;
            }

            previous = current;
        }

        InstSection(stop);
    }

    private bool Check() // проверка, есть ли на пути ранее созданные секции
    {
        Vector3 position = current.endPoint.position + current.endPoint.forward * sectionSize / 2;
        Collider[] colliders = Physics.OverlapSphere(position, sectionSize / 4);
        foreach (Collider hit in colliders) if (hit) return true;
        return false;
    }
}