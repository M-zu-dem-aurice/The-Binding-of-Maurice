using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private PlayerController _player;
    private bool _coroutineAllowed;


    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _coroutineAllowed = true;
    }


    private void Update()
    {
        if (_coroutineAllowed)
        {
            StartCoroutine("StartPulsing");
        }
    }


    private IEnumerator StartPulsing()
    {
        _coroutineAllowed = false;

        for (float i = 0f; i <= 1f; i += 0.1f)
        {
            transform.localScale = new Vector2(
                (Mathf.Lerp(transform.localScale.x, transform.localScale.x + 0.025f, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(transform.localScale.y, transform.localScale.y + 0.025f, Mathf.SmoothStep(0f, 1f, i)))
                );
            yield return new WaitForSeconds(0.015f);
        }

        for (float i = 0f; i <= 1f; i += 0.1f)
        {
            transform.localScale = new Vector2(
                (Mathf.Lerp(transform.localScale.x, transform.localScale.x - 0.025f, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(transform.localScale.y, transform.localScale.y - 0.025f, Mathf.SmoothStep(0f, 1f, i)))
                );
            yield return new WaitForSeconds(0.015f);
        }

        _coroutineAllowed = true;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_player._health < _player._amountOfHearts)
            {
                _player._health += 1;
                Destroy(gameObject);
            }
        }
    }
}
