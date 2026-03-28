using System.Windows;
using System.Windows.Controls;

namespace PromptForge.App.Controls;

public partial class SliderFlyout : UserControl
{
    public SliderFlyout()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
        nameof(Label), typeof(string), typeof(SliderFlyout), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty HelperTextProperty = DependencyProperty.Register(
        nameof(HelperText), typeof(string), typeof(SliderFlyout), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ValueTextProperty = DependencyProperty.Register(
        nameof(ValueText), typeof(string), typeof(SliderFlyout), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty GuideTextProperty = DependencyProperty.Register(
        nameof(GuideText), typeof(string), typeof(SliderFlyout), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value), typeof(int), typeof(SliderFlyout), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
        nameof(Minimum), typeof(double), typeof(SliderFlyout), new PropertyMetadata(0d));

    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
        nameof(Maximum), typeof(double), typeof(SliderFlyout), new PropertyMetadata(100d));

    public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register(
        nameof(ButtonWidth), typeof(double), typeof(SliderFlyout), new PropertyMetadata(130d));

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string HelperText
    {
        get => (string)GetValue(HelperTextProperty);
        set => SetValue(HelperTextProperty, value);
    }

    public string ValueText
    {
        get => (string)GetValue(ValueTextProperty);
        set => SetValue(ValueTextProperty, value);
    }

    public string GuideText
    {
        get => (string)GetValue(GuideTextProperty);
        set => SetValue(GuideTextProperty, value);
    }

    public int Value
    {
        get => (int)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public double ButtonWidth
    {
        get => (double)GetValue(ButtonWidthProperty);
        set => SetValue(ButtonWidthProperty, value);
    }
}
