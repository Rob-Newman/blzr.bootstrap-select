using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Blzr.BootstrapSelect
{
    public partial class BootstrapSelect<TItem, TType> : ComponentBase
    {
        [CascadingParameter] private EditContext CascadedEditContext { get; set; }

        [Parameter] public IEnumerable<TItem> Data { get; set; }

        [Parameter] public Func<TItem, string> TextField { get; set; }

        [Parameter] public Func<TItem, string> ValueField { get; set; }

        [Parameter] public TType Value { get; set; }

        [Parameter] public EventCallback<TType> ValueChanged { get; set; }
        
        [Parameter] public Expression<Func<TType>> ValueExpression { get; set; }

        [Parameter] public bool IsMultiple { get; set; }

        [Parameter] public bool DelayValueChangedCallUntilClose { get; set; } = false;

        [Parameter] public string Width { get; set; }

        [Parameter] public bool ShowSearch { get; set; } = false;

        [Parameter] public int ShowSearchThreshold { get; set; } = 0;

        [Parameter] public SelectedTextFormat SelectedTextFormat { get; set; } = SelectedTextFormat.Values;

        [Parameter] public bool ShowPlaceholder { get; set; } = false;

        [Parameter] public string PlaceholderText { get; set; }

        [Parameter] public string Id { get; set; }

        [Parameter] public string CssClass { get; set; }

        [Parameter] public string Label { get; set; }

        [Parameter] public Expression<Func<TType>> ValidationFor { get; set; }

        private IList<BootstrapSelectOption> FilteredOptions => DisplaySearch && !string.IsNullOrEmpty(searchTerm) ? options.Where(x => x.Text.ToLower().Contains(searchTerm.ToLower())).ToList() : options;

        private IEnumerable<BootstrapSelectOption> SelectedOptions => options.Where(x => x.Selected);
        
        private bool DisplaySearch => ShowSearch && options.Count >= ShowSearchThreshold;

        private string FieldCssClasses => CascadedEditContext?.FieldCssClass(fieldIdentifier) ?? "";

        private string ButtonText
        {
            get
            {
                if (IsMultiple)
                {
                    if (!SelectedOptions.Any() || SelectedTextFormat == SelectedTextFormat.Static)
                    {
                        return string.IsNullOrEmpty(PlaceholderText) ? "Nothing selected" : PlaceholderText;
                    }

                    return SelectedTextFormat == SelectedTextFormat.Count ? $"{SelectedOptions.Count()} of {options.Count} selected" : string.Join(", ", SelectedOptions.Select(x => x.Text));
                }

                if (ShowPlaceholder && !SelectedOptions.Any())
                {
                    return string.IsNullOrEmpty(PlaceholderText) ? "Select..." : PlaceholderText;
                }

                return SelectedOptions.Any() ? SelectedOptions.First().Text : options.First().Text;
            }
        }

        private bool isActive = false;

        private readonly IList<BootstrapSelectOption> options = new List<BootstrapSelectOption>();

        private string searchTerm;        

        private TType initialValue;

        private FieldIdentifier fieldIdentifier;

        protected override void OnInitialized()
        {
            if (ValueExpression != null)
            {
                fieldIdentifier = FieldIdentifier.Create(ValueExpression);
            }
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
                    options.Add(new BootstrapSelectOption { Value = value, Text = text, Selected = valueArray.Any(x => x == value) });
                }
            }

            base.OnParametersSet();
        }

        private async Task ToggleDropDown()
        {
            isActive = !isActive;

            if (!isActive) 
            {
                if (DelayValueChangedCallUntilClose && !EqualityComparer<TType>.Default.Equals(initialValue, Value))
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

            Value = SetValue();

            if (!DelayValueChangedCallUntilClose)
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
    }
}
