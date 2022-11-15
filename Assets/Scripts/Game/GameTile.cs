using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SortGame
{
    public class GameTile : MonoBehaviour
    {
        private System.Lazy<GameBoard> board;
        private Vector2Int coord;
        public Vector2Int gridCoord => coord;
        private void Awake() 
        {
            board = new(GetComponentInParent<GameBoard>);
            coord = new Vector2Int{
                x = transform.parent.GetSiblingIndex(),
                y = transform.GetSiblingIndex()
            };
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }

}