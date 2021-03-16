# blzr.bootstrap-select

This Blazor bootstap select component is inspired by the js [bootstrap-select](https://github.com/snapappointments/bootstrap-select/), rewritten using C# as a Razor Component.

There is no dependency with JavaScript.

## Getting Setup

Adding a project reference to the `Blzr.BootstrapSelect` library

### 1. Register Services
You will need to register the Blzr.BootstrapSelect service in your application

#### Blazor WebAssembly
Add the following line to your applications `Program.Main` method.

```csharp
builder.Services.AddBootstrapSelect();
```

#### Blazor Server
Add the following line to your applications `Startup.ConfigureServices` method.

```csharp
services.AddBootstrapSelect();
```

### 2. Add Imports
Add the following to your *_Imports.razor*

```csharp
@using Blzr.BootstrapSelect
```

### 3. Add reference to style sheet(s)
Add the following line to the `head` tag of your `_Host.cshtml` (Blazor Server app) or `index.html` (Blazor WebAssembly).

We ship both minified and unminified CSS.

For minified use:

```HTML
<link href="_content/Blzr.BootstrapSelect/blzr-bootstrap-select.min.css" rel="stylesheet" />
```

For unminified use:
```HTML
<link href="_content/Blzr.BootstrapSelect/blzr-bootstrap-select.css" rel="stylesheet" />
```

Presumably, you already have bootstrap css referenced in your project. If not, use:
```HTML
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.5.3/dist/css/bootstrap.min.css" integrity="sha384-TX8t27EcRE3e/ihU7zmQxVncDAy5uIKz4rEkgIXeMed4M0jlfIDPvg6uqKI2xXr2" crossorigin="anonymous">
```

## Defaults
The following system wide defaults can be configured as part of the service registration 

- `ShowSearch` (Default: `false`) - Determines if the search box should be displayed. When true, adds a search box to the top of the drop down (works in conjunction with `ShowSearchThreshold`)
- `ShowSearchThreshold` (Default: `0`) - The threshold to determine the number of options that must exists before the search box is displayed
- `SearchPlaceholderText` (Default: `Search`) - The placeholder text displayed in the search box 
- `SearchNotFoundText` (Default: `No matching results`) - The text displayed if no options match a search term
- `DelayValueChangedCallUntilClose` (Default: `false`) - For multi's only, whether to delay calling ValueChanged until after the select is closed (default will fire after each option is selected/deselected)
- `SelectedTextFormat` (Default: `values`) - Specifies how the selection is displayed with a multi select. `values` displays a list of the selected options (separated by a ,). `static` simply displays the select element's placeholder text. `count` displays the total number of selected options
- `MultiSelectedText` (Default: `{0} of {1} selected`) - Specifies the text to display when the `SelectedTextFormat` is `count`. `{0}` is replaced with the number of selected items. `{1}` is replaced with the total number of options  
- `ShowPlaceholder` (Default: `false`) - For singles only, determines if the placeholder text should be displayed
- `MultiPlaceholderText` (Default: `Nothing selected`) - The text to display as the placeholder for multi's
- `SinglePlaceholderText` (Default: `Select...`) - The text to display as the placeholder for singles

### Example
```csharp
builder.Services.AddBootstrapSelect(defaults =>
                {
                    defaults.ShowSearch = true;
                    defaults.SearchPlaceholderText = "Find";
                    defaults.ShowSearchThreshold = 10;
                    defaults.SearchNotFoundText = "Can't find any";
                    defaults.DelayValueChangedCallUntilClose = true;
                    defaults.SelectedTextFormat = SelectedTextFormats.Count;
                    defaults.MultiSelectedText = "{0} selected";
                    defaults.ShowPlaceholder = true;
                    defaults.MultiPlaceholderText = "Pick some";
                    defaults.SinglePlaceholderText = "Pick one";
                });
```

## Usage

### Basic Example
```csharp
@page "/"

<BootstrapSelect TItem="Country" Data="@countries" TextField="@((item) => item.Name)" ValueField="@((item) => item.Id.ToString())" TType="string" />

@code {
    private IList<Country> countries;

    protected override void OnInitialized()
    {
        countries = new List<Country> {
            new Country { Id = 1, Name = "United Kingdom" },
            new Country { Id = 2, Name = "United States" },
            new Country { Id = 3, Name = "Germany" },
            new Country { Id = 4, Name = "France" },
            new Country { Id = 5, Name = "China" }
        };
    }

    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
```
### Parameters

- `TItem` (Required) - The underlying type of the objects used in the Data collection
- `TType` (Required) - The underlying type of the Value field.  Currently supported is: `string`, `int`, `IEnumerable<string>`, `IEnumerable<int>`
- `Data` (Required) - The Data to use to build the drop down options from
- `TextField` (Required) - The `Func` to select the Text value from each item within `Data`
- `ValueField` (Required) - The `Func` to select the Value value from each item within `Data`
- `OptGroupField` (Optional) - The 'Func' to select the Opt Group value from each item within `Data`. If this is supplied, opt groups will be displayed, and its assumed that the `Data` will be sorted so that all items from the same opt group are positioned together  
- `Id` (Optional) - Html Id to be added to the element
- `Value` (Optional) - An initial value for the select.  Can be used for 2 way binding using `@bind-value`
- `ValueChanged` (Optional) - An `EventCallback` to be called when the value changes 
- `IsMultiple` (Optional. Default `false`) - Determines if the select should be a single or multi
- `ShowSearch` (Optional. Default: Uses system wide Defaults) - Determines if the search box should be displayed. When true, adds a search box to the top of the drop down (works in conjunction with `ShowSearchThreshold`)
- `ShowSearchThreshold` (Optional. Default: Uses system wide Defaults) - The threshold to determine the number of options that must exists before the search box is displayed
- `DelayValueChangedCallUntilClose` (Optional. Default: Uses system wide Defaults) - For multi's only, whether to delay calling ValueChanged until after the select is closed (default will fire after each option is selected/deselected)
- `SelectedTextFormat` (Optional. Default: Uses system wide Defaults) - Specifies how the selection is displayed with a multi select. `values` displays a list of the selected options (separated by a ,). `static` simply displays the select element's placeholder text. `count` displays the total number of selected options
- `ShowPlaceholder` (Optional. Default: Uses system wide Defaults) - For singles only, determines if the placeholder text should be displayed
- `PlaceholderText` (Optional. Default: Uses system wide Defaults) - The placeholder text
- `Width` (Optional) - If supplied, will be used to add a width to the element
- `CssClass` (Optional) - Additional classes to be added to the element
- `Label` (Optional) - A label to added to the element
- `ValidationFor` (Optional) - A `Expression` to provide the validation information. Can only be used if component is within an `EditForm`

See the code in the index page within samples for more examples
