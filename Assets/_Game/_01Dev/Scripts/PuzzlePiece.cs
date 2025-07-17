using DG.Tweening;
using UnityEngine;

namespace Puzzle.Core
{
    public class PuzzlePiece : MonoBehaviour
    {
        public float snapDistance = 1.5f;
        public Color dragColor = Color.yellow;
    
        public float dragScale = 1.1f;
        public Color snapPreviewColor = Color.green;
    
        [HideInInspector] public Vector2 correctPosition;
        [HideInInspector] public PuzzleManager puzzleManager;
    
        private bool _isDragging;
        private bool _isPlaced;
        private Vector2 _dragOffset;
        private Camera _mainCamera;
        private SpriteRenderer _spriteRenderer;
        private Color _originalColor;
        private Collider2D _pieceCollider;

        private void Start()
        {
            _mainCamera = Camera.main;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = _spriteRenderer.color;
            _pieceCollider = GetComponent<Collider2D>();
            _spriteRenderer.sortingOrder = 5;
        }

        private void OnMouseDown()
        {
            if (_isPlaced) return;
        
            _isDragging = true;
            Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _dragOffset = (Vector2)transform.position - mousePos;
        
            transform.DOScale(dragScale, 0.1f);
            _spriteRenderer.color = dragColor;
            _spriteRenderer.sortingOrder = 10;
        }

        private void OnMouseDrag()
        {
            if (!_isDragging) return;
        
            Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos + _dragOffset;
        }

        private void OnMouseUp()
        {
            if (!_isDragging) return;
            
            _isDragging = false;
            
            transform.DOScale(1f, 0.2f);
            _spriteRenderer.color = _originalColor;
            _spriteRenderer.sortingOrder = 5;
            
            var distance = Vector2.Distance(transform.position, correctPosition);
            
            if (distance <= snapDistance)
            {
                SnapToPosition();
            }
        }
        
        private void SnapToPosition()
        {
            transform.DOMove(correctPosition, 0.3f).SetEase(Ease.OutBack);
            _isPlaced = true;
            _pieceCollider.enabled = false;
            _spriteRenderer.sortingOrder = 0;

            var placementSequence = DOTween.Sequence();
            placementSequence.Append(transform.DOScale(1.1f, 0.1f))
                            .Append(transform.DOScale(1f, 0.1f));
            
            puzzleManager?.OnPiecePlaced();
        }
        
        public void ResetPiece(Vector2 newPosition)
        {
            transform.DOKill();
            
            _isDragging = false;
            _isPlaced = false;
            
            transform.position = newPosition;
            transform.localScale = Vector3.one;
            
            _spriteRenderer.color = _originalColor;
            _spriteRenderer.sortingOrder = 5;
            _pieceCollider.enabled = true;
        }
        
        private void Update()
        {
            if (!_isDragging || _isPlaced) return;
            
            var distance = Vector2.Distance(transform.position, correctPosition);
            
            if (distance <= snapDistance)
            {
                _spriteRenderer.color = Color.Lerp(dragColor, snapPreviewColor, 0.6f);
            }
            else if (_spriteRenderer.color != dragColor)
            {
                _spriteRenderer.color = dragColor;
            }
        }
    }
}