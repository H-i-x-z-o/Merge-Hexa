using UnityEngine;

namespace Editor
{
    public partial class GenerateLevel
    {
        public Hexagon GenModHex(GameObject parent)
        {
            // get random hexagon
            NumberHexagon randomHexa = GetRandomHexagon(parent);
            
            // if number = 1, can only create sub hexagon
            // else create add/sub hexagon
            int random = Random.Range(0, 2);

            // if random = 0 create sub hexagon else random = 1 create add hexagon
            if (randomHexa.number == 1) random = 0; // only create add hexagon

            // init mod hexagon
            Hexagon modHexa;
            if (random == 1)
            {
                modHexa = Instantiate(Resources.Load<ModNumHexagon>("Prefabs/Hexagon/Add"));
                randomHexa.number--;
            }
            else
            {
                modHexa = Instantiate(Resources.Load<ModNumHexagon>("Prefabs/Hexagon/Sub"));
                randomHexa.number++;
            }
            modHexa.transform.parent = parent.transform;

            // set position of mod hexagon
            modHexa.transform.position = randomHexa.transform.position;

            // get random direction
            randomHexa.transform.position = randomNewPos();
            previousHexa = modHexa;

            // update neighbors

            return modHexa;
        }

        public Hexagon GenMoveHex(GameObject parent)
        {
            // get random hexagon
            NumberHexagon randomHexa = GetRandomHexagon(parent);

            // init move hexagon
            MoveHexArrow moveHexa = Instantiate(Resources.Load<MoveHexArrow>("Prefabs/Hexagon/Arrow"));
            moveHexa.transform.parent = parent.transform;

            // set position of move hexagon
            moveHexa.transform.position = randomHexa.transform.position;

            // get random direction
            randomHexa.transform.position = randomNewPos();

            // find direction of move hexagon
            int directionOfMove = DirOffset.IndexOf(randomHexa.transform.position - moveHexa.transform.position);
            moveHexa.direction = (Direction)((directionOfMove + 3) % 6);
            previousHexa = moveHexa;

            // update neighbors

            return moveHexa;
        }

        
    }
}