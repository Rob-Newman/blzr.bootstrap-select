# blzr.bootstrap-select

This Blazor bootstap select component is inspired by the js [bootstrap-select](https://github.com/snapappointments/bootstrap-select/), rewritten using C# as a Razor Component.

There is no dependency with JavaScript.

## Getting Setup

### 1. Add Reference
Adding a project reference to the `Blzr.BootstrapSelect` library

### 2. Add Imports
Add the following to your *_Imports.razor*

```csharp
@using Blzr.BootstrapSelect
```

### 3. Add reference to style sheet(s)
Add the following line to the `head` tag of your `_Host.cshtml` (Blazor Server app) or `index.html` (Blazor WebAssembly).

We ship both minified and unminified CSS.

For minifed use:

```
<link href="_content/Blzr.BootstrapSelect/blzr-bootstrap-select.min.css" rel="stylesheet" />
```

For unminifed use:
```
<link href="_content/Blzr.BootstrapSelect/blzr-bootstrap-select.css" rel="stylesheet" />
```

Presumably, you already have bootstrap css referenced in your project. If not, use:
```
TBC
```

## Usage
TBC
