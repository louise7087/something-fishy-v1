// ============================================================
// File: NetworkObjectRegistry.cs
// Purpose:
// Tracks important spawned network objects and provides safe
// lookup support for other systems.
//
// Responsibilities:
// - Register and unregister tracked network objects
// - Provide lookup access to shared networked entities
// - Reduce fragile direct scene searching
//
// Notes:
// - Keep lifecycle handling robust
// - Avoid using this as a substitute for proper ownership design
// ============================================================