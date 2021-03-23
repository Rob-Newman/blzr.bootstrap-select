using System;

namespace Blzr.BootstrapSelect
{
    public class BootstrapSelectDefaults
    {
        private readonly Action<BootstrapSelectDefaults> defaultOptions;

        public BootstrapSelectDefaults()
        {
        }

        public BootstrapSelectDefaults(Action<BootstrapSelectDefaults> defaultOptions)
            :this()
        {
                this.defaultOptions = defaultOptions;
                this.defaultOptions?.Invoke(this);
        }

        public string SearchPlaceholderText { get; set; } = "Search";

        public string SearchNotFoundText { get; set; } = "No matching results";

        public string MultiPlaceholderText { get; set; } = "Nothing selected";

        public string SinglePlaceholderText { get; set; } = "Select...";

        public string MultiSelectedText { get; set; } = "{0} of {1} selected";

        public string MultiSeparator { get; set; } = ", ";

        public bool ShowSearch { get; set; } = false;

        public int ShowSearchThreshold { get; set; } = 0;

        public bool ShowPlaceholder { get; set; } = false;

        public bool DelayValueChangedCallUntilClose { get; set; } = false;

        public SelectedTextFormats SelectedTextFormat { get; set; } = SelectedTextFormats.Values;

        public int SelectedTextFormatCount { get; set; } = 0;

        public bool ShowTick { get; set; } = false;

        public string MaxSelectionsText { get; set; } = "Limit reached ({0} items max)";

        public SearchStyles SearchStyle { get; set; } = SearchStyles.Contains;

        public bool ShowActions { get; set; } = false;

        public string SelectAllText { get; set; } = "Select All";
        
        public string DeselectAllText { get; set; } = "Deselect All";

        public ButtonStyles ButtonStyle { get; set; } = ButtonStyles.Default;
    }
}
