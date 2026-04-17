// ============================================================
// File: ContainerComponent.cs
// Purpose:
// Unity-facing runtime component for a generic container object
// placed in the world.
//
// Responsibilities:
// - Expose container presence in the scene
// - Bridge scene object behaviour to domain/container services
// - Provide a runtime hook for interaction systems
//
// Notes:
// - Should not become the full source of business logic
// - Keep scene-specific concerns separate from rules
// ============================================================