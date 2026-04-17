// ============================================================
// File: TickSystem.cs
// Purpose:
// Provides a controlled update/tick mechanism for systems that
// should run on a managed schedule instead of scattered Update calls.
//
// Responsibilities:
// - Run subscribed systems on a consistent tick
// - Centralise simulation timing
// - Reduce duplicated frame-based scheduling logic
//
// Notes:
// - Useful for simulation and server-authoritative timing
// - Avoid mixing tick control with feature-specific rules
// ============================================================