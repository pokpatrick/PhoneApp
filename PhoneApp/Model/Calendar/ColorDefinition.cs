namespace PhoneApp.Model.Data
{
    public class ColorDefinition
    {

        private string _background;

        private string _foreground;

        /// <summary>The background color associated with this color definition.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("background")]
        public string Background
        {
            get
            {
                return this._background;
            }
            set
            {
                this._background = value;
            }
        }

        /// <summary>The foreground color that can be used to write on top of a background with &apos;background&apos; color.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("foreground")]
        public string Foreground
        {
            get
            {
                return this._foreground;
            }
            set
            {
                this._foreground = value;
            }
        }
    }
}
