// ============================================================
// File: FishingNetworkController.cs
// Purpose:
// Handles network synchronization for fishing events such as
// casts, hooks, catches, and related state changes.
//
// Responsibilities:
// - Route fishing network events
// - Enforce authority boundaries
// - Synchronize fishing state in multiplayer
//
// Notes:
// - Catch outcomes should typically be server authoritative
// - Keep transport concerns separate from FishingRules
// ============================================================