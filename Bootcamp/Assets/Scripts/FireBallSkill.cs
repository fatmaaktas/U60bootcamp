using UnityEngine;

[CreateAssetMenu(fileName = "New Fireball Skill", menuName = "Skills/Fireball")]
public class FireballSkill : ScriptableObject
{
    public string skillName = "Fireball";
    public int damage = 50;
    public float cooldown = 5f;
    public GameObject fireballPrefab;
    public AudioClip castSound;
    public float fireballSpeed = 10f;

    public void Cast(Vector3 startPosition, Vector3 targetPosition)
    {
        // Fireball'ý yarat ve hedefe doðru ateþle
        GameObject fireball = Instantiate(fireballPrefab, startPosition, Quaternion.identity);
        FireballController fireballController = fireball.GetComponent<FireballController>();
        if (fireballController != null)
        {
            fireballController.SetFireballSkill(this);
        }

        Rigidbody fireballRb = fireball.GetComponent<Rigidbody>();
        fireballRb.velocity = (targetPosition - startPosition).normalized * fireballSpeed;

        AudioManager.instance.PlaySkillSound(castSound, .5f);
    }
}