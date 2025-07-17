using System.Collections.Generic;
using UnityEngine;

namespace Puzzle.Core
{
    public class PuzzleManager : MonoBehaviour
    {
        public Sprite[] puzzleSprites;
        public GameObject puzzlePiecePrefab;
    
        private readonly List<PuzzlePiece> _puzzlePieces = new();
        private int _placedPieces;
    
        private readonly Vector2[] _desiredPositions = {
            new(-2.67f, 3.77f),  // Piece 0
            new(-0.91f, 3.69f),  // Piece 1
            new(0.92f, 3.62f),   // Piece 2
            new(2.50f, 3.70f),   // Piece 3
            new(-2.57f, 2.02f),  // Piece 4
            new(-0.90f, 1.74f),  // Piece 5
            new(0.79f, 1.79f),   // Piece 6
            new(2.49f, 1.94f),   // Piece 7
            new(-2.74f, 0.00f),  // Piece 8
            new(-1.08f, -0.01f), // Piece 9
            new(0.88f, -0.20f),  // Piece 10
            new(2.68f, -0.03f),  // Piece 11
            new(-2.56f, -2.08f), // Piece 12
            new(-0.80f, -1.74f), // Piece 13
            new(0.88f, -1.94f),  // Piece 14
            new(2.53f, -1.81f),  // Piece 15
            new(-2.65f, -3.72f), // Piece 16
            new(-0.91f, -3.66f), // Piece 17
            new(0.89f, -3.62f),  // Piece 18
            new(2.52f, -3.72f)   // Piece 19
        };

        private void Start()
        {
            CreatePuzzlePieces();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetPuzzle();
            }
        }

        private void CreatePuzzlePieces()
        {
            for (var i = 0; i < puzzleSprites.Length && i < _desiredPositions.Length; i++)
            {
                if (puzzleSprites[i] == null) continue;
                
                var pieceObj = Instantiate(puzzlePiecePrefab);
                var piece = pieceObj.GetComponent<PuzzlePiece>();
                
                pieceObj.GetComponent<SpriteRenderer>().sprite = puzzleSprites[i];
                piece.correctPosition = _desiredPositions[i];
                piece.puzzleManager = this;
                piece.transform.position = GetRandomPosition();
                
                _puzzlePieces.Add(piece);
            }
        }

        private static Vector2 GetRandomPosition()
        {
            return new Vector2(
                Random.Range(-5, 5),
                Random.Range(-8, 8)
            );
        }
        
        public void OnPiecePlaced()
        {
            _placedPieces++;
            
            if (_placedPieces >= _puzzlePieces.Count)
            {
                OnPuzzleComplete();
            }
        }
        
        private void ResetPuzzle()
        {
            _placedPieces = 0;
            
            foreach (var piece in _puzzlePieces)
            {
                piece.ResetPiece(GetRandomPosition());
            }
        }
        
        private static void OnPuzzleComplete()
        {
            Debug.Log("Puzzle complete");
        }
    }
}