using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GENERALNOTES : MonoBehaviour
{
    [TextArea(100,10000)]
    public string Notes = "WHEN ADDING A WEAPON MAKE SURE TO...\n 1.Create physical weapon under 'Weapon' in player object \n 2. add the weapon to the weapon manager array \n 3. Change weapon assignment function accordingly";
}
