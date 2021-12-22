using System;

namespace Blzr.BootstrapSelect
{
    public class BootstrapSelectOption
    {
        public string OptGroup { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
        public string Id { get; private set; }

        public BootstrapSelectOption()
        {
            Id = Guid.NewGuid().ToString();
        }

        public void ToggleSelected()
        {
            Selected = !Selected;
        }
    }

    public enum SelectedTextFormats
    {
        Values,
        Count,
        Static,
        CountGreaterThan
    }

    public enum SearchStyles
    { 
        Contains,
        StartsWith
    }

    public enum ButtonStyles 
    { 
        Default,
        Primary,
        Info,
        Success,
        Warning,
        Danger,
        Custom
    }
}