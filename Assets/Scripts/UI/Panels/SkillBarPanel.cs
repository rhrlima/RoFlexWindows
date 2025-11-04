using UnityEngine;

public class SkillBarPanel : MonoBehaviour
{
    [SerializeField] private ItemEntry skillSlot;
    public int numSlots;

    private void Start() {
        for (int i = 0; i < numSlots - 1; i++) {
            var bar = Instantiate(skillSlot, transform);
        }
    }
}
