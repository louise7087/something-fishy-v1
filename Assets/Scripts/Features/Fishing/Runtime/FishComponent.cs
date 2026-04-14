// ============================================================
// File: FishComponent.cs
// Purpose:
// Unity-facing runtime component for a fish instance in the
// world, bridging scene behaviour to fishing systems.
//
// Responsibilities:
// - Represent fish in the scene
// - Expose state to other runtime systems
// - Connect fish object lifecycle to gameplay flow
//
// Notes:
// - Keep fish rules in services/domain code where possible
// - Avoid mixing visuals, networking, and rules into one class
// ============================================================