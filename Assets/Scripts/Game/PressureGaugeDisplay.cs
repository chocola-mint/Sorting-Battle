using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SortGame;
using UnityEngine.UI;
using SortGame.Core;

namespace SortGame
{
    [RequireComponent(typeof(Image))]
    public class PressureGaugeDisplay : MonoBehaviour
    {
        [SerializeField] Gradient gradient;
        [SerializeField] GameBoard gameBoard;
        private GamePressureState state;
        private Image image;
        [SerializeField] [Min(0.01f)]
        float fillRateOverTime = 4;
        // Start is called before the first frame update
        void Start()
        {
            state = gameBoard.state.gamePressureState;
            image = GetComponent<Image>();
            image.fillAmount = state.pressureRate;
        }

        // Update is called once per frame
        void Update()
        {
            image.fillAmount = Mathf.MoveTowards(image.fillAmount, state.pressureRate, Time.deltaTime * fillRateOverTime);
            image.color = gradient.Evaluate(image.fillAmount);
        }
    }
}
