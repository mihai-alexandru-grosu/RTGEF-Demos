# Demos repository instructions

Read [CODING_GUIDELINES.md](CODING_GUIDELINES.md) before changing any first-party C# code or reusable editor snippets. These owner-confirmed conventions apply to every engine project and template in this repository, not only Lecture02. Preserve serialized names, Unity .meta identities, owner edits and behavior. Exclude generated, imported/vendor code and historical backups.

Use Unity Editor tooling for scene/prefab/settings changes and SerializedObject for private serialized fields. Public access must serve a concrete runtime caller or integration, never just editor automation. Course demos use built-in Unity Inspector features; do not add NaughtyAttributes or replacement custom attributes.

Read the relevant project's README before editing and update it when behavior, setup or reset procedures change. Keep intentional faults isolated from working demonstrations. Current user instructions override standing verification guidance.

