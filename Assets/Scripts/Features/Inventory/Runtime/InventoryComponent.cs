// ============================================================
// File: InventoryComponent.cs
// Purpose:
// Unity-facing component that exposes an inventory in the
// scene and bridges domain state to runtime systems.
//
// Responsibilities:
// - Hold inventory-related scene state
// - Connect Unity objects to inventory logic
// - Provide a runtime anchor for inventory access
//
// Notes:
// - Should stay thinner than the service/rules layer
// - Avoid absorbing UI and transfer rules
// ============================================================