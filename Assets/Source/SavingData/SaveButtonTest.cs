using Shops;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveButtonTest : MonoBehaviour
{
    [SerializeField] private SaveService _saveService;
    [SerializeField] private Button _saveButton;

    private void OnEnable()
    {
        _saveButton.onClick.AddListener(OnSave);
        _saveService.Loaded += ShowPurcheses;
    }

    private void OnDisable()
    {
        _saveButton.onClick.RemoveListener(OnSave);
        _saveService.Loaded -= ShowPurcheses;
    }

    private void ShowPurcheses()
    {
        Purchases purchases = (Purchases)_saveService.GetPurchasesData();

        for (int i = 0; i < purchases.Objects.Count; i++)
        {
            Debug.Log($"Purch {i} / {purchases.Objects[i].Key} / {purchases.Objects[i].Value}");
        }
    }

    private void OnSave()
    {
        List<SerializedPair<GoodNames, int>> serializedPairs = new();
        serializedPairs.Add(new SerializedPair<GoodNames, int>(GoodNames.Grenade, 30));
        serializedPairs.Add(new SerializedPair<GoodNames, int>(GoodNames.ReloadSpeed, 23));
        serializedPairs.Add(new SerializedPair<GoodNames, int>(GoodNames.MachineGun, 3));
        Purchases purchases = new(serializedPairs);
        _saveService.SetPurchasesData(purchases);
        _saveService.Save();
        Debug.Log($"SAVE  {purchases.Objects[0].Key}, {purchases.Objects[0].Value}");
    }
}