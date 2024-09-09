using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour
{
    public Section[] prefabs; // ������ � ����� ����������� �������
    public Section[] start; // ��������� ������
    public Section[] stop; // �������� ������
    public int sections = 20; // ����. ��������� ����� ������
    public float sectionSize = 1; // ������ ������, ��� ������� ������ ���� �����

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

    private bool Check() // ��������, ���� �� �� ���� ����� ��������� ������
    {
        Vector3 position = current.endPoint.position + current.endPoint.forward * sectionSize / 2;
        Collider[] colliders = Physics.OverlapSphere(position, sectionSize / 4);
        foreach (Collider hit in colliders) if (hit) return true;
        return false;
    }
}