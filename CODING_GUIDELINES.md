# C# coding guidelines

Owner-confirmed conventions. Apply throughout the course workspace and demos repository to all new and modified first-party C# code and reusable editor snippets. Do not rewrite imported/vendor code. Preserve unrelated owner edits and behavior during formatting changes. Historical backups and archived examples are records, not reusable tooling.

## Statements and braces

- One statement per line. Never combine assignments, calls, return or continue on one line. Even `voice.Play(); return;` must occupy two lines.
- Every opening and closing curly brace goes on its own line (Allman style), including methods, control blocks and initializers.
- Single-statement if/loop bodies may omit braces, but the body goes on the next line, indented. Never use `if (condition) return;` or a loop and its body on the same line.
- Ternary expressions may stay on one line when readable; this is not permission to combine statements. For-loop header semicolons remain in the header.
- Use four-space indentation, spaces after commas and around operators. Keep generic type syntax intact, e.g. `GetComponent<AudioSource>()`.
- Prefer ordinary method bodies for behavior rather than compressing methods into expression-bodied one-liners. Concise computed properties are fine.

## Fields and access

- Inspector configuration defaults to `[SerializeField] private`, not public. Inspector visibility is not a reason for public access.
- Public fields/methods/properties require a concrete external caller or integration purpose. Prefer read-only properties or focused methods for runtime access.
- Debug Inspector fields stay private. Course demos use built-in Unity attributes only; use `[SerializeField]` when runtime state needs to be visible and explain that these fields are for observation. Do not add a custom read-only attribute or Inspector package.
- Keep existing serialized names when changing access so prefab/scene values survive. Update editor tooling to use SerializedObject rather than making fields public for convenience.
- Keep related fields under Headers with a blank line between groups; put runtime state in a separate group. One field declaration per line. Use a runtime-state comment for private, nonserialized state that has no Inspector header.
- Course demos do not use NaughtyAttributes. This owner decision supersedes the earlier Inspector-package convention. Use built-in Unity Inspector features and keep dependencies minimal.

## Logical grouping

- Separate methods with one blank line. Keep attributes attached to their declaration (no intervening blank line).
- Usually put a blank line before if/for/foreach/while when preceding work exists in the block. Do not add a leading blank line after an opening brace.
- After an early-return/continue guard, leave a blank line before the remaining work.
- Consecutive simple if statements may be adjacent without blank lines.
- Group assignments configuring the same object directly underneath one another. Insert a blank line when moving to another object, a control-flow block, playback/action, or an unrelated operation.
- Do not insert blank lines mechanically between every statement; use them to show purpose.

```csharp
[Header("Engine clips - seamless loops")]
[SerializeField] private AudioClip idleLoop;
[SerializeField] private AudioClip drivingLoop;

private void PlayImpact(AudioClip clip)
{
    if (!clip)
        return;

    foreach (var voice in voices)
    {
        if (voice.isPlaying)
            continue;

        voice.clip = clip;
        voice.volume = impactVolume;
        voice.pitch = Random.Range(.94f, 1.06f);

        voice.Play();
        return;
    }
}
```

## Editor configuration example

Use SerializedObject for private serialized settings, retaining their field names. This is an Editor-only snippet; scene dirty/save handling belongs to the calling tool.

```csharp
var serializedPlayer = new UnityEditor.SerializedObject(player);
serializedPlayer.Update();
serializedPlayer.FindProperty("speed").floatValue = 5f;
serializedPlayer.ApplyModifiedProperties();
```

