using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Charactar_Controller : MonoBehaviour
{
    #region Editor Data
    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 50f;

    [Header("Dependencies")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] GameObject _pauseMenuUI;
    [SerializeField] SpriteRenderer _visualsRenderer;
    #endregion

    #region Internal Data
    private Vector2 _moveDir = Vector2.zero;
    #endregion

    // -------------------- NEW: LOAD on start --------------------
    private void Start()
    {
        StartCoroutine(SpawnPlayer());
    }

    IEnumerator SpawnPlayer()
    {
        yield return null;
        var data = SaveSystem.Load();
        if (data != null)
        {
            Vector2 savedPos = new Vector2(data.posX, data.posY);
            Collider2D hit = Physics2D.OverlapCircle(savedPos, 0.5f);

            if (hit != null)
            {
                Debug.Log("HIT");
                Debug.Log(hit);
                savedPos = new Vector2(data.checkpointX, data.checkpointY);
            }

            transform.position = savedPos;
        }
    }

    private void Update()
    {
        GatherInput();
    }

    private void FixedUpdate()
    {
        MovementUpdate();
    }

    // -------------------- NEW: SAVE on pause/quit --------------------
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) SaveNow();
    }

    private void OnApplicationQuit()
    {
        SaveSystem.Delete();
    }

    private void SaveNow()
    {
        Vector2 pos = _rb != null ? _rb.position : (Vector2)transform.position;
        // If you have multiple scenes, pass the scene name:
        // var sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        var data = SaveData.FromPosition(pos /*, sceneName*/);
        SaveSystem.Save(data);
    }

    private void GatherInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            TogglePauseMenu();
        }

        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");
    }

    public void TogglePauseMenu()
    {
        if (_pauseMenuUI != null)
        {
            bool isActive = _pauseMenuUI.activeSelf;
            _pauseMenuUI.SetActive(!isActive);

            Time.timeScale = isActive ? 1f : 0f;
        }
    }

    private void MovementUpdate()
    {
        // Sprite Flipping
        if (_moveDir.x < 0) _visualsRenderer.flipX = true;
        else if (_moveDir.x > 0) _visualsRenderer.flipX = false;

        // Movement
        if (_rb != null)
            _rb.linearVelocity = _moveDir * _moveSpeed * Time.fixedDeltaTime;
        else
            transform.position += (Vector3)(_moveDir * _moveSpeed * Time.fixedDeltaTime);
    }

    // Handy for testing (Right-click component header -> “Reset Saved Position”)
    [ContextMenu("Reset Saved Position")]
    private void ResetSavedPosition()
    {
        SaveSystem.Delete();
    }
}