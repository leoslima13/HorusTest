using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Horus.Helpers;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
using Xamarin.Forms.Xaml;

namespace Horus.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomEntry : ContentView
    {
        public CustomEntry()
        {
            InitializeComponent();
            
            entryContainer.Focused += (o, e) =>
            {
                if (e.IsFocused)
                    entry.Focus();
            };
            entry.TextChanged += (o, e) =>
            {
                Text = e.NewTextValue;
            };
        }

        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(CustomEntry), false,
                propertyChanged: OnIsPasswordPropertyChanged);

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomEntry),
                defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnTextPropertyChanged);

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CustomEntry), Colors.SecondaryDarkColor,
                propertyChanged: OnTextColorPropertyChanged);

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CustomEntry),
                propertyChanged: OnPlaceholderPropertyChanged);

        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(CustomEntry), Colors.SecondaryColor,
                propertyChanged: OnPlaceholderColorPropertyChanged);

        public static readonly BindableProperty FloatLabelTextProperty =
            BindableProperty.Create(nameof(FloatLabelText), typeof(string), typeof(CustomEntry),
                propertyChanged: OnFloatLabelTextPropertyChanged);

        public static readonly BindableProperty FloatLabelTextColorProperty =
            BindableProperty.Create(nameof(FloatLabelTextColor), typeof(Color), typeof(CustomEntry),
                Colors.SecondaryColor, propertyChanged: OnFloatLabelTextColorPropertyChanged);

        public static readonly BindableProperty KeyboardTypeProperty =
            BindableProperty.Create(nameof(KeyboardType), typeof(Keyboard), typeof(CustomEntry),
                propertyChanged: OnKeyboardTypePropertyChanged);

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(CustomEntry),
                new CornerRadius(5),
                propertyChanged: OnCornerRadiusPropertyChanged);
        
        public new static readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(CustomEntry), Colors.SecondaryColor,
                propertyChanged: OnBackgroundColorPropertyChanged);

        public bool IsPassword
        {
            get => (bool) GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Color TextColor
        {
            get => (Color) GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public string Placeholder
        {
            get => (string) GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public Color PlaceholderColor
        {
            get => (Color) GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        public string FloatLabelText
        {
            get => (string) GetValue(FloatLabelTextProperty);
            set => SetValue(FloatLabelTextProperty, value);
        }

        public Color FloatLabelTextColor
        {
            get => (Color) GetValue(FloatLabelTextColorProperty);
            set => SetValue(FloatLabelTextColorProperty, value);
        }

        public Keyboard KeyboardType
        {
            get => (Keyboard) GetValue(KeyboardTypeProperty);
            set => SetValue(KeyboardTypeProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        private static void OnIsPasswordPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not CustomEntry customEntry) return;

            var bValue = (bool) newValue;
            customEntry.entry.IsPassword = bValue;
            customEntry.icon.IsVisible = bValue;

            if (!bValue) return;
            
            var command = new Command(() =>
            {
                var value = !customEntry.entry.IsPassword;
                customEntry.entry.IsPassword = !customEntry.entry.IsPassword;
                customEntry.icon.Source = value ? "eye_b.png" : "eye_none_b.png";
            });
            customEntry.icon.GestureRecognizers.Clear();
            customEntry.icon.GestureRecognizers.Add(new TapGestureRecognizer {Command = command});
        }

        private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not CustomEntry customEntry) return;

            customEntry.entry.Text = (string) newValue;
        }

        private static void OnTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not CustomEntry customEntry) return;

            customEntry.entry.TextColor = (Color) newValue;
        }

        private static void OnPlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not CustomEntry customEntry) return;

            customEntry.entry.Placeholder = (string) newValue;
        }

        private static void OnPlaceholderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not CustomEntry customEntry) return;

            customEntry.entry.PlaceholderColor = (Color) newValue;
        }

        private static void OnFloatLabelTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not CustomEntry customEntry) return;

            customEntry.floatLabel.Text = (string) newValue;
        }

        private static void OnFloatLabelTextColorPropertyChanged(BindableObject bindable, object oldValue,
            object newValue)
        {
            if (bindable is not CustomEntry customEntry) return;

            customEntry.floatLabel.TextColor = (Color) newValue;
        }

        private static void OnKeyboardTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not CustomEntry customEntry) return;

            customEntry.entry.Keyboard = (Keyboard) newValue;
        }

        private static void OnCornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not CustomEntry customEntry) return;

            customEntry.entryContainer.CornerRadius = (CornerRadius) newValue;
        }
        
        private static void OnBackgroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not CustomEntry customEntry) return;

            customEntry.entryContainer.BackgroundColor = (Color) newValue;
        }
    }
}