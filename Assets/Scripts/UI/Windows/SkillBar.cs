using UnityEngine;

public class SkillBar : MonoBehaviour
{
    [SerializeField] private SkillBarPanel skillBarPanel;
    public int numBars;

    private void Start() {
        for (int i = 0; i < numBars - 1; i++) {
            var bar = Instantiate(skillBarPanel, transform);
        }
    }
}
