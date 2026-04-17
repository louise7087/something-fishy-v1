// ============================================================
// File: ServiceLocator.cs
// Purpose:
// Provides central access to shared services when direct
// dependency injection is not practical in Unity.
//
// Responsibilities:
// - Register shared services at startup
// - Resolve shared services at runtime
// - Reduce hard-coded scene/service lookups
//
// Notes:
// - Use sparingly to avoid hidden dependencies
// - Prefer explicit dependencies where practical
// ============================================================