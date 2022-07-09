using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurretChargeable
{
    bool Charging { get; set; }
    public float ChargeTime { get; set; }

    public void BeginCharge();
    public void StopCharge();


}
    
