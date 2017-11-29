# KeyLocks

Run code with locks based on different object-types, like strings, numbers &amp; dates.

Selectively lock code on specific values, not globally for all executing code.

```
private static KeyLock<string> _keyLock = new KeyLock<string>();

public void Main()
{
    Parallel.Invoke(
        () => { UpdateData("entity-123", "First new value"); },
        () => { UpdateData("entity-123", "Second new value"); }, // This will await line above
        () => { UpdateData("another-entity-456", "Another new value"); },        
        () => { UpdateData("yet-another-entity-789", "Yet another new value"); });
}

private void UpdateData(string id, string value)
{
    _keyLock.RunWithLock(id, () =>
    {
        // Execute locked code
    });
}
```

Method `1` and `2` executed inside the `Parallel.Invoke`, which are executing the `UpdateData`-method, will await eachother and not run simultainously, while all other will run in parallel.

## NameLock

The type `NameLock` is a short-hand term for `KeyLock<string>`.

It defaults to being case-sensitive, but that can be changed by passing `StringComparer.InvariantCultureIgnoreCase` as a constructor-argument.
