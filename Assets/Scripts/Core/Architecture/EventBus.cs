// ============================================================
// File: EventBus.cs
// Purpose:
// Allows systems to publish and subscribe to events without
// tightly coupling directly to one another.
//
// Responsibilities:
// - Dispatch events between systems
// - Reduce direct references between features
// - Support loosely coupled architecture
//
// Notes:
// - Event flow should stay understandable and traceable
// - Do not use events to hide critical control flow
// ============================================================