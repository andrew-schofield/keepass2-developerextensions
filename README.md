# keepass2-developerextensions

**A set of extension methods on native KeePass2 domain objects to aid plugin developers.**

This project can either be built and the dll included directly, or you can reference the csproj file in your own Visual Studio solution.

## Included functions

### ProtectedStringExtensions

#### ValueEquals

```C#
bool ValueEquals(this ProtectedString protectedString, ProtectedString compareWith)
```

Compare equality of ProtectedStrings.

### PwEntryExtensions

#### GetPasswordLastModified

```C#
DateTime GetPasswordLastModified(this PwEntry entry)
```

Returns the date of the last password modifiation. Relies on their being accurate and adequate history entries available.

#### IsDeleted

```C#
bool IsDeleted(this PwEntry entry, IPluginHost pluginHost)
```

Returns whether or not the entry is in a deleted state (in the recycle bin).

#### GetUrlDomain

```C#
string GetUrlDomain(this PwEntry entry)
```

Returns the domain (and tld) of the URL field if present.

#### GetIcon

```C#
Image GetIcon(this PwEntry entry, IPluginHost pluginHost)
```

Extract the icon from a given entry.