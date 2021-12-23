using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Blzr.BootstrapSelect
{
    public partial class BootstrapSelect<TItem, TType> : ComponentBase
    {
        #region Fields

        private bool isActive = false;

        private readonly IList<BootstrapSelectOption> options = new List<BootstrapSelectOption>();

        private string searchTerm;

        private TType initialValue;

        private FieldIdentifier fieldIdentifier;

        private bool? showSearch;

        private int? showSearchThreshold;

        private bool? showPlaceholder;

        private bool? showTick;

        private SelectedTextFormats? selectedTextFormat;

        private int? selectedTextFormatCount;

        private bool? delayValueChangedCallUntilClose;

        private Timer countdown;

        private bool showMaxSelectedMessage = false;

        private SearchStyles? searchStyle;

        private bool? showActions;

        private ButtonStyles? buttonStyle;

        private string innerId;

        #endregion

        #region Properties

        [CascadingParameter] private EditContext CascadedEditContext { get; set; }

        [Inject] protected BootstrapSelectDefaults Defaults { get; set; }
        
        [Parameter] public IEnumerable<TItem> Data { get; set; }

        [Parameter] public Func<TItem, string> TextField { get; set; }

        [Parameter] public Func<TItem, string> ValueField { get; set; }
        
        [Parameter] public Func<TItem, string> OptGroupField { get; set; }

        [Parameter] public Func<TItem, IEnumerable<string>> KeyWordsField { get; set; }

        [Parameter] public TType Value { get; set; }

        [Parameter] public EventCallback<TType> ValueChanged { get; set; }
        
        [Parameter] public Expression<Func<TType>> ValueExpression { get; set; }

        [Parameter] public bool IsMultiple { get; set; }

        [Parameter] public bool? DelayValueChangedCallUntilClose
        {
            get { return delayValueChangedCallUntilClose.GetValueOrDefault(Defaults.DelayValueChangedCallUntilClose); }
            set { delayValueChangedCallUntilClose = value; }
        }

        [Parameter] public string Width { get; set; }

        [Parameter] public bool? ShowSearch 
        {
            get { return showSearch.GetValueOrDefault(Defaults.ShowSearch); }
            set { showSearch = value; }
        }

        [Parameter] public int? ShowSearchThreshold 
        { 
            get { return showSearchThreshold.GetValueOrDefault(Defaults.ShowSearchThreshold); } 
            set { showSearchThreshold = value; } 
        }

        [Parameter] public SelectedTextFormats? SelectedTextFormat 
        {
            get { return selectedTextFormat.GetValueOrDefault(Defaults.SelectedTextFormat); }
            set { selectedTextFormat = value; }
        }

        [Parameter] public int? SelectedTextFormatCount
        {
            get { return selectedTextFormatCount.GetValueOrDefault(Defaults.SelectedTextFormatCount); }
            set { selectedTextFormatCount = value; }
        }

        [Parameter] public bool? ShowPlaceholder 
        {
            get { return showPlaceholder.GetValueOrDefault(Defaults.ShowPlaceholder); }
            set { showPlaceholder = value; }
        }

        [Parameter] public bool? ShowTick
        {
            get { return showTick.GetValueOrDefault(Defaults.ShowTick); }
            set { showTick = value; }
        }

        [Parameter] public string PlaceholderText { get; set; }

        [Parameter] public string Id { get; set; }

        [Parameter] public string CssClass { get; set; }

        [Parameter] public string Label { get; set; }

        [Parameter] public int? MaxSelections { get; set; }

        [Parameter] public Expression<Func<TType>> ValidationFor { get; set; }

        [Parameter] public SearchStyles? SearchStyle
        {
            get { return searchStyle.GetValueOrDefault(Defaults.SearchStyle); }
            set { searchStyle = value; }
        }

        [Parameter] public bool? ShowActions
        {
            get { return showActions.GetValueOrDefault(Defaults.ShowActions); }
            set { showActions = value; }
        }

        [Parameter] public ButtonStyles? ButtonStyle
        {
            get { return buttonStyle.GetValueOrDefault(Defaults.ButtonStyle); }
            set { buttonStyle = value; }
        }

        protected IList<BootstrapSelectOption> FilteredOptions 
        {
            get 
            {
                return DisplaySearch && !string.IsNullOrEmpty(searchTerm) 
                    ? SearchStyle == SearchStyles.Contains 
                        ? options.Where(x => x.Text.ToLower().Contains(searchTerm.ToLower()) || x.KeyWords.Any(k => k.ToLower().Contains(searchTerm.ToLower()))).ToList() 
                        : options.Where(x => x.Text.ToLower().StartsWith(searchTerm.ToLower()) || x.KeyWords.Any(k => k.ToLower().StartsWith(searchTerm.ToLower()))).ToList()
                    : options;
            }
        } 

        protected IEnumerable<BootstrapSelectOption> SelectedOptions => options.Where(x => x.Selected);

        protected bool DisplaySearch => ShowSearch.Value && options.Count >= ShowSearchThreshold;

        protected string FieldCssClasses => CascadedEditContext?.FieldCssClass(fieldIdentifier) ?? "";

        protected bool ShowOptGroups => OptGroupField != null;

        protected string ButtonText
        {
            get
            {
                if (IsMultiple)
                {
                    if (!SelectedOptions.Any() || SelectedTextFormat.Value == SelectedTextFormats.Static)
                    {
                        return string.IsNullOrEmpty(PlaceholderText) ? Defaults.MultiPlaceholderText : PlaceholderText;
                    }

                    return SelectedTextFormat.Value == SelectedTextFormats.Count || 
                            (SelectedTextFormat.Value == SelectedTextFormats.CountGreaterThan && SelectedOptions.Count() > SelectedTextFormatCount) 
                        ? string.Format(Defaults.MultiSelectedText, SelectedOptions.Count(), options.Count) 
                        : string.Join($"{Defaults.MultiSeparator}", SelectedOptions.Select(x => x.Text));
                }

                if (ShowPlaceholder.Value && !SelectedOptions.Any())
                {
                    return string.IsNullOrEmpty(PlaceholderText) ? Defaults.SinglePlaceholderText : PlaceholderText;
                }

                return SelectedOptions.Any() ? SelectedOptions.First().Text : options.First().Text;
            }
        }

        protected string ButtonClass 
        {
            get 
            {
                string buttonClass = "btn-light";

                switch (ButtonStyle) {
                    case ButtonStyles.Primary:
                        buttonClass = "btn-primary";
                        break;
                    case ButtonStyles.Info:
                        buttonClass = "btn-info";
                        break;
                    case ButtonStyles.Success:
                        buttonClass = "btn-success";
                        break;
                    case ButtonStyles.Warning:
                        buttonClass = "btn-warning";
                        break;
                    case ButtonStyles.Danger:
                        buttonClass = "btn-danger";
                        break;
                    case ButtonStyles.Custom:
                        buttonClass = "btn-custom";
                        break;
                }

                return buttonClass;
            }
        }

        protected Dictionary<string, object> ConditionalAriaAttributes
        {
            get
            {
                var dict = new Dictionary<string, object>();
                if (IsMultiple)
                {
                    dict.Add("aria-multiselectable", true);
                }
                else
                {
                    var selected = SelectedOptions.FirstOrDefault(x => x.Selected)?.Id;
                    if (string.IsNullOrEmpty(selected)) {
                        selected = FilteredOptions.First().Id;
                    }

                    dict.Add("aria-activedescendant", selected);
                }

                return dict;
            }
        }

        #endregion

        #region Methods

        protected override void OnInitialized()
        {
            if (ValueExpression != null)
            {
                fieldIdentifier = FieldIdentifier.Create(ValueExpression);
            }

            innerId = Guid.NewGuid().ToString();
        }

        protected override void OnParametersSet()
        {
            if (Data != null)
            {
                options.Clear();

                var valueArray = Value != null ? GetValue() : new string[0];
                foreach (var item in Data)
                {
                    var value = ValueField?.Invoke(item);
                    var text = TextField?.Invoke(item);
                    var optGroup = OptGroupField?.Invoke(item);
                    var keywords = KeyWordsField?.Invoke(item);
                    options.Add(new BootstrapSelectOption { Value = value, Text = text, OptGroup = optGroup, KeyWords = keywords != null ? keywords : new List<string>(), Selected = valueArray.Any(x => x == value) });;
                }
            }

            base.OnParametersSet();
        }

        private async Task ToggleDropDown()
        {
            isActive = !isActive;

            if (!isActive) 
            {
                if (DelayValueChangedCallUntilClose.Value && !EqualityComparer<TType>.Default.Equals(initialValue, Value))
                { 
                    await InvokeValueChanged();
                }
            }
            else
            {
                initialValue = Value;
            }
        }

        private async Task HandleOptionClick(BootstrapSelectOption value)
        {
            if (!IsMultiple)
            {
                foreach (var selected in SelectedOptions)
                {
                    selected.Selected = false;
                }
            }

            options.First(x => x.Value == value.Value).ToggleSelected();

            if (IsMultiple && MaxSelections.HasValue && SelectedOptions.Count() > MaxSelections.Value)
            {
                options.First(x => x.Value == value.Value).ToggleSelected();
                ShowMaxSelectionMessage();
            }

            Value = SetValue();

            if (!IsMultiple || !DelayValueChangedCallUntilClose.Value)
            {
                await InvokeValueChanged();
            }

            if (!IsMultiple)
            {
                isActive = false;
            }
        }

        private async Task InvokeValueChanged()
        {
            await ValueChanged.InvokeAsync(Value);
            CascadedEditContext?.NotifyFieldChanged(fieldIdentifier);
        }

        private void PerformSearch(ChangeEventArgs a)
        {
            searchTerm = (string)a.Value;
        }

        private TType SetValue()
        {

            if (typeof(TType) == typeof(string))
            {
                return (TType)(object)string.Join(",", SelectedOptions.Select(x => x.Value));
            }
            else if (typeof(TType) == typeof(int))
            {
                return (TType)(object)int.Parse(string.Join(",", SelectedOptions.Select(x => x.Value)));
            }
            else if (typeof(TType) == typeof(IEnumerable<string>))
            {
                return (TType)SelectedOptions.Select(x => x.Value).ToList().AsEnumerable();
            }
            else if (typeof(TType) == typeof(IEnumerable<int>))
            {
                return (TType)SelectedOptions.Select(x => int.Parse(x.Value)).ToList().AsEnumerable();
            }
            else
            {
                // Not currently supported
                throw new NotImplementedException();
            }
        }

        private string[] GetValue()
        {
            if (typeof(TType) == typeof(string))
            {
                return new string[] { Value.ToString() };
            }
            else if (typeof(TType) == typeof(int))
            {
                return new string[] { Value.ToString() };
            }
            else if (typeof(TType) == typeof(IEnumerable<string>))
            {
                return ((IEnumerable<string>)Value).ToArray();
            }
            else if (typeof(TType) == typeof(IEnumerable<int>))
            {
                return ((IEnumerable<int>)Value).Select(x => x.ToString()).ToArray();
            }
            else
            {
                // Not currently supported
                throw new NotImplementedException();
            }
        }

        private void StartCountdown() 
        {
            if (countdown == null)
            {
                countdown = new Timer(1500);
                countdown.Elapsed += HideMaxSelectionMessage;
                countdown.AutoReset = false;
            }

            if (countdown.Enabled)
            {
                countdown.Stop();
                countdown.Start();
            }
            else 
            {
                countdown.Start();
            }
        }

        private void ShowMaxSelectionMessage()
        {
            showMaxSelectedMessage = true;
            StartCountdown();
            StateHasChanged();
        }

        private void HideMaxSelectionMessage(object source, ElapsedEventArgs args)
        {
            showMaxSelectedMessage = false;
            StateHasChanged();
        }

        private void SelectAll()
        {
            if (MaxSelections.HasValue && FilteredOptions.Count() > MaxSelections.Value)
            {
                ShowMaxSelectionMessage();
            }
            else
            {
                foreach (var option in FilteredOptions)
                {
                    option.Selected = true;
                }
            }
        }

        private void DeselectAll()
        {
            foreach (var option in FilteredOptions)
            {
                option.Selected = false;
            }
        }

        #endregion
    }
}
