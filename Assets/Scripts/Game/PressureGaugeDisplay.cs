using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SortGame;
using UnityEngine.UI;

namespace SortGame
{
    [RequireComponent(typeof(Image))]
    public class PressureGaugeDisplay : MonoBehaviour
    {
        [SerializeField] Gradient gradient;
        [SerializeField] GameBoard gameBoard;
        private GamePressureState state;
        private Image image;
        // Start is called before the first frame update
        void Start()
        {
            state = gameBoard.state.gamePressureState;
            image = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            image.fillAmount = state.pressureRate;
            image.color = gradient.Evaluate(image.fillAmount);
        }
    }
}
