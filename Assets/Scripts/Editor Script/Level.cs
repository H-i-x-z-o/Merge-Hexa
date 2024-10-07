using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Editor.Test
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level Test")]
    public class Level : ScriptableObject
    {
        public int size = 50;
    }
}
