using UnityEngine;

public class SkillMinPanel : MonoBehaviour
{
    public SkillEntry skillEntryPrefab;
    public Transform contentPanel;
    public int numSkills; // test

    private void Start()
    {
        SetSkills();
    }

    private void SetSkills()
    {
        var skills = new string[] { "Fireball", "Ice Spike", "Heal", "Lightning Bolt" };

        for (int i = 0; i < numSkills; i++)
        {
            SkillEntry newEntry = Object.Instantiate(skillEntryPrefab, contentPanel);
            string skillName = skills[Random.Range(0, skills.Length)];
            newEntry.SetSkillInfo(skillName, Random.Range(1, 10), 10, Random.Range(5, 999), Random.value > 0.5f, Random.value > 0.5f);
        }
    } 
}
