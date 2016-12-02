using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {
    [SerializeField] private Text healthLabel;
    [SerializeField] private InventoryPopup popup;

    void Awake() {
        Messenger.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
    }
    void OnDestroy() {
        Messenger.RemoveListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
    }

	// Use this for initialization
	void Start () {
        OnHealthUpdated();

        popup.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M)) {
            bool isShowing = popup.gameObject.activeSelf;
            popup.gameObject.SetActive(!isShowing);
            popup.Refresh();
        }
	}

    private void OnHealthUpdated() {
        string message = "Health: " + Managers.Player.health + "/" + Managers.Player.maxHealth;
        healthLabel.text = message;
    }
}
