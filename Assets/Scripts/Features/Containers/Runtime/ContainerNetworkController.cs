// ============================================================
// File: ContainerNetworkController.cs
// Purpose:
// Synchronizes container state across the network and routes
// server-authoritative container actions.
//
// Responsibilities:
// - Handle container-related network requests
// - Synchronize shared container state
// - Apply ownership/authority rules in multiplayer
//
// Notes:
// - Shared container state should normally be server-controlled
// - Keep networking concerns separate from core container rules
// ============================================================