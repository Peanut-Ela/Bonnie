using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChestRandomDropList<T>
{
    [System.Serializable]
    public struct Pair
    {
        public T item;
        public float weight;

        public Pair(T item, float weight)
        {
            this.item = item;
            this.weight = weight;
        }
    }

    public List<Pair> list = new List<Pair>();

    public int count
    {
        get => list.Count;
    }

    public T GetRandom()
    {
        float totalWeight = 0f;

        foreach (Pair p in list)
        {
            totalWeight += p.weight;
        }

        float value = Random.value * totalWeight;

        float sumWeight = 0;

        foreach (Pair p in list)
        {
            sumWeight += p.weight;

            if (sumWeight >= value)
            {
                return p.item;
            }
        }

        return default(T);
    }
}
