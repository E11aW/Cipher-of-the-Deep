# Save/Load Implementation — Top-Down 2D Prototype

**Video demo:** https://www.youtube.com/watch?v=yTt5RLHGruM

## What counts as the “Save State”
For this assignment, my save state includes:
- **Player world position** (x, y)
- **Current scene name** (for future multi-scene support)
- **Metadata**: save version and timestamp (to support upgrades later)

This is enough to satisfy “resume where I left off” and gives room to expand (health, inventory, quests, options).

---

## How Save/Load works (overview)

**Files added**
- `Assets/_GAME_/Core/SaveSystem/SaveData.cs`
- `Assets/_GAME_/Core/SaveSystem/SaveSystem.cs`

**Where the data lives**
- Saved to `Application.persistentDataPath/save1.json` (platform-standard per Unity).

**When it saves/loads**
- **Load**: on game start (`Awake()` in `Charactar_Controller`) reads `save1.json` and places the player.
- **Save**: on app pause/quit (`OnApplicationPause`, `OnApplicationQuit`) writes `save1.json`.
- Dev helpers (Editor): context-menu buttons to Save/Load/Delete on demand.

**Data format (example)**
```json
{
  "posX": -3.25,
  "posY": 7.10,
  "scene": "Dungeon01",
  "playerHealth": 100,
  "inventoryItemIds": [],
  "version": "1.0.0",
  "savedAtIsoUtc": "2025-10-27T03:19:21.532Z"
}
```

**Corruption-resistance**
- Save uses a temp file then replaces the old file (simple “atomic-ish” write).

---

## Evidence for the rubric

#### 1) Key/Value store (Unity **PlayerPrefs**)
- **Pros:** trivial API, great for small scalar values; cross-platform; no file I/O code.
- **Cons:** not ideal for complex/structured data; portability/inspection is harder; versioning and schema changes get messy.
- **Fit here:** fine for *just* two floats, but not for future inventory/quests/settings.

#### 2) **JSON file** (object serialization) — *Chosen*
- **Pros:** human-readable; simple versioning (add fields, keep defaults); engine-agnostic; easy to diff in PRs; works in Editor and builds; no extra packages required.
- **Cons:** naïve writes can corrupt on crash (mitigated with temp file); Unity’s built-in `JsonUtility` doesn’t handle dictionaries/polymorphism (can switch to Newtonsoft later).
- **Why chosen:** best balance of simplicity, clarity, and future growth for a student project. Easy to extend with more fields without database overhead.

#### 3) **Database** (SQLite / embedded NoSQL)
- **Pros:** scales for lots of records (logs, telemetry, inventories), fast queries, good data integrity options.
- **Cons:** higher setup & code complexity; overkill for a single save slot; queries add cognitive overhead.
- **Fit here:** premature complexity; could be useful later for analytics or large histories.

---

## Implementation details (quick reference)

- **Save entry point**
  - `Charactar_Controller.SaveNow()` → `SaveSystem.Save(SaveData.FromPosition(pos, sceneName))`
- **Load entry point**
  - `Awake()` → `var data = SaveSystem.Load(); if (data != null) apply position`
- **File path**
  - macOS: `~/Library/Application Support/<CompanyName>/<ProductName>/save1.json`
- **Builds**
  - Works unchanged in macOS builds; same persistent path rules apply.

---

## Future extensions

- Add fields: health, inventory, quest flags, options, unlocked levels.
- **Multi-slot** support: parameterize filename (`save2.json`, etc.) or store a manifest.
- **Migrate to Newtonsoft** if/when we need dictionaries or polymorphic items.
- **Checksum** or hash to detect tampering; **backups** (e.g., `save1.bak`) for recovery.
- **Cloud sync** later (e.g., Steam Cloud, iCloud), with conflict resolution policy.
