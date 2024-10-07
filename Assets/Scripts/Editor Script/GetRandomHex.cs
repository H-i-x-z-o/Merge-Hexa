using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Editor
{
    public partial class GenerateLevel 
    {
        private List<NumberHexagon> availableHexa;

        public void SelectHexagonRandom(GameObject parent)
        {
            availableHexa = new List<NumberHexagon>();
            foreach (var hexa in parent.GetComponentsInChildren<NumberHexagon>())
            {
                if(!HasSixAdjacent(hexa))
                {
                    availableHexa.Add(hexa);
                }
            }
        }

        public NumberHexagon GetRandomHexagon(GameObject parent)
        {
            SelectHexagonRandom(parent);
            return availableHexa[Random.Range(0, availableHexa.Count)];
        }

        private bool HasSixAdjacent(Hexagon tile)
        {
            int count = 0;
            foreach (Transform adjacent in tile.neighbors)
            {
                if (adjacent != null)
                {
                    count++;
                }
            }
            return count >= 6;
        }
    }
}