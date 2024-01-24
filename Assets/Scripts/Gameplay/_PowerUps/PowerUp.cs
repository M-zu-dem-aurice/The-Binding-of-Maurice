using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _height;
    [SerializeField] private GameObject _textCanvas;

    [SerializeField] private PowerUpEffect[] _powerUpEffects;


    private void Awake()
    {
        _speed = 1.05f;
        _height = 0.3f;
    }

    
    private void Update()
    {
        // Make power Up fly up and down
        float y = Mathf.PingPong(Time.time * _speed, 1) * _height;
        transform.position = new Vector2(transform.position.x, transform.parent.transform.position.y + 1 + y);

        
    }


    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Apply every power up effect that is attached to this game object script
            foreach (PowerUpEffect effect in _powerUpEffects)
            {
                effect.Apply(other.gameObject);
            }

            gameObject.GetComponent<SpriteRenderer>().enabled = false;


            // Display collected power up on canvas
            GameObject _text = Instantiate(_textCanvas, transform);

            string _effectsText = "";
            foreach (PowerUpEffect effect in _powerUpEffects)
            {
                _effectsText += effect.ToString().Split(' ')[0] + " " + effect.ToString().Split(' ')[1] + " " + effect.ToString().Split(' ')[2] + "\n";
            }
            _text.GetComponentInChildren<TextMeshProUGUI>().SetText(gameObject.name.Split(' ')[0] + " " + gameObject.name.Split(' ')[1] + ":\n" + _effectsText);

            yield return new WaitForSeconds(4f);

            Destroy(_text);
            Destroy(gameObject);
        }
    }
}
