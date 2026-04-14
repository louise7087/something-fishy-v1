// ============================================================
// File: InventoryNetworkController.cs
// Purpose:
// Handles server-authoritative inventory synchronization and
// inventory-related networking flow.
//
// Responsibilities:
// - Route inventory RPC/network actions
// - Synchronize inventory state where required
// - Enforce multiplayer authority boundaries
//
// Notes:
// - Inventory integrity should normally be server authoritative
// - Avoid embedding inventory UI logic here
// ============================================================