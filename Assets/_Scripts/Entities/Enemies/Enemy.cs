using System.Collections;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    abstract public IEnumerator DestroyThatEnemy();
}
