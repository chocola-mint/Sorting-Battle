using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    public class GamePressureState
    {
        public int pressure { get; private set; } = 0;
        private readonly int maxPressure;
        public float pressureRate => (float) pressure / (float) maxPressure;
        public GamePressureState(int maxPressure = 40)
        {
            this.maxPressure = maxPressure;
        }
        public int ConsumePressure(int amount)
        {
            int before = pressure;
            pressure = Mathf.Max(0, pressure - amount);
            return before - pressure;
        }
        public void AddPressure(int amount)
        {
            pressure = Mathf.Min(maxPressure, pressure + amount);
        }
        public void Attack(GamePressureState other, int attackPower)
        {
            attackPower -= ConsumePressure(attackPower);
            other.AddPressure(attackPower);
        }
    }
}