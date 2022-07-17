using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBar : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _damageLabel;
    [SerializeField]
    private Animator _animator;

    private Transform _unit;

    private void OnEnable()
    {
        _unit = transform.parent;
        transform.SetParent(null);
        _animator.Play("Damage got");
        StartCoroutine(DamageBarWork());
    }

    public void SetDamage(int damage)
    {
        _damageLabel.text = "-" + damage.ToString() + " HP";
    }

    private IEnumerator DamageBarWork()
    {
        yield return new WaitForSeconds(1f);
        transform.SetParent(_unit);
        gameObject.SetActive(false);
    }
}
