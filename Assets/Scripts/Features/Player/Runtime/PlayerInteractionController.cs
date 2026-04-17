// ============================================================
// File: PlayerInteractionController.cs
// Purpose:
// Handles player interaction requests such as raycasts,
// usable target detection, and interaction initiation.
//
// Responsibilities:
// - Detect valid interactable objects
// - Submit interaction requests
// - Keep interaction flow separate from movement and UI
//
// Notes:
// - Should validate before invoking feature services
// - Avoid directly mutating authoritative state on clients
// ============================================================