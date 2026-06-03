using RO_Flex_UI.Panels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Utils
{
    public class PrefabsMenu : MonoBehaviour
    {
        [SerializeField] private Button template;
        [SerializeField] private List<GameObject> prefabs;

        private void Start()
        {
            GenerateMenu();
        }

        private void GenerateMenu()
        {
            if (template != null)
                template.gameObject.SetActive(false);

            foreach (var prefab in prefabs)
            {
                var prefabName = prefab.name;
                var buttonPrefab = Instantiate(template, transform);
                buttonPrefab.gameObject.SetActive(true);

                buttonPrefab.GetComponentInChildren<TMP_Text>().text = prefabName;
                var buttonComponent = buttonPrefab.GetComponent<Button>();
                buttonComponent.onClick.AddListener(() =>
                {
                    prefab.GetComponent<IWindow>().ToggleVisibility();
                });
            }
        }
    }
}