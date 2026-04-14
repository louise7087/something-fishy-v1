// ============================================================
// File: ContainerSession.cs
// Purpose:
// Tracks an active interaction session between a player and a
// container.
//
// Responsibilities:
// - Store who is currently interacting with a container
// - Represent session ownership or locking state
// - Support safe open/close interaction flow
//
// Notes:
// - Useful for preventing conflicting interactions
// - Keep session rules explicit and simple
// ============================================================