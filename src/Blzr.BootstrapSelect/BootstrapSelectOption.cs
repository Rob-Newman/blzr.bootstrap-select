namespace Blzr.BootstrapSelect
{
    public class BootstrapSelectOption
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }

        public void ToggleSelected()
        {
            Selected = !Selected;
        }
    }

    public enum SelectedTextFormats
    {
        Values,
        Count,
        Static
    }
}
