using Shops;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] protected SaveService _saveService;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        List<SerializedPair<GoodNames, int>> serializedPairs = new();
        Purchases purchases = new Purchases(serializedPairs);
        _saveService.SavePurchasesData(purchases);
        _saveService.SetCompletedTrainingData(false);
        _saveService.SetCurrencyData(0);
        _saveService.SetLevelData(1);
        _saveService.SetPlayerHelperData(0);
        Debug.Log("SAVE");
    }
}
