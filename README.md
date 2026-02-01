A 3D Unity game featuring resource management, storage systems, and factory production. The player collects resources from stocks, carries them in an inventory, and factories consume ingredients to produce new items via configurable recipes.

## Requirements

- **Unity**: 2022.3.62f3
- **Platform**: Windows, macOS, or Linux

## Features

- **Player movement** — Joystick-based control (Joystick Pack) for moving and rotating the character
- **Player inventory** — Limited-capacity inventory for carrying resources
- **Stocks** — Storage buildings that accept specific resource types; players can deposit or withdraw by entering trigger zones
- **Factories** — Buildings that consume ingredients from input storage and produce results into output storage using recipes
- **Recipes** — Configurable ingredients, result, and production time per factory
- **Resource pooling** — Object pooling for resource prefabs (N1, N2, N3) via `InstanceCreator`

## Project Structure

```
SaintTest/
├── Assets/
│   ├── GameResources/
│   │   ├── Building/     # Building prefabs (Stock, Factories, Triggers)
│   │   ├── Plane/        # Ground/plane
│   │   ├── Player/       # Player sprites
│   │   └── Resources/    # Resource prefabs, materials, config (ResourceItems.asset)
│   ├── Joystick Pack/    # Third-party joystick UI & examples
│   ├── Scenes/
│   │   └── SampleScene.unity
│   └── Scripts/
│       ├── PlayerController.cs    # Joystick-driven movement
│       ├── PlayerInventory.cs     # Player storage (StorageBase)
│       ├── Stock.cs               # Stock storage logic
│       ├── StockInteraction.cs    # Trigger-based give/take at stocks
│       ├── Factory.cs             # Recipe-based production loop
│       ├── StorageBase.cs         # Abstract storage + ItemsPlacer hooks
│       ├── ItemsPlacer.cs         # Places resources visually in storage
│       ├── Recipe.cs / ResourceModel.cs / ResourceType.cs
│       ├── InstanceCreator.cs     # Resource pooling
│       ├── ResourceView.cs / ResourceMover.cs
│       └── ...
├── Packages/             # Unity package manifest
└── ProjectSettings/
```

## Core Scripts

| Script | Purpose |
|--------|---------|
| `PlayerController` | Moves player with `FixedJoystick`; rotates toward movement direction |
| `PlayerInventory` | Extends `StorageBase`; finite capacity, add/remove by `ResourceType` or `ResourceModel` |
| `Stock` | Storage with per-type capacity; accepts only configured `ResourceType`s |
| `StockInteraction` | Trigger-based; when player is inside, either gives items from stock to player or takes from player to stock (`_isOutput` flips mode) |
| `Factory` | Coroutine production loop: consumes ingredients from `_inputStorage`, produces `_recipe.Result` into `_outputStorage` |
| `StorageBase` | Abstract storage API (`CanAdd`, `HasItems`, `TryAdd`, `TryGet`) and `OnItemAdded` / `OnItemRemoved` for `ItemsPlacer` |
| `InstanceCreator` | Singleton; object pools for `ResourceView` by `ResourceType`; `CreateObject` / `ReturnToPool` |

## Dependencies

- **Joystick Pack** — Included under `Assets/Joystick Pack` for mobile-style joystick input.
- Unity packages from `Packages/manifest.json` (e.g. UGUI, TextMeshPro, Input, Physics).

---

