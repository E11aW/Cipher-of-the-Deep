using UnityEngine;

[SelectionBase]
public class Charactar_Controller : MonoBehaviour
{
    #region Editor Data
    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 50f;

    [Header("Dependencies")]
    [SerializeField] Rigidbody2D _rb;
    #endregion

    #region Internal Data
    private Vector2 _moveDir = Vector2.zero;
    #endregion

    // -------------------- NEW: LOAD on start --------------------
    private void Awake()
    {
        var data = SaveSystem.Load();
        if (data != null)
        {
            var pos = data.ToVector2();
            if (_rb != null)
                _rb.position = pos; // physics-safe
            else
                transform.position = new Vector3(pos.x, pos.y, transform.position.z);
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
        SaveNow();
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
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");
    }

    private void MovementUpdate()
    {
        // If your Unity doesn't have linearVelocity, use _rb.velocity
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