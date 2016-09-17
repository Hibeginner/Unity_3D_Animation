using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour, IGameManager {
    public ManagerStatus status {
        get;
        private set;
    }

    public void Startup() {

    }
}
