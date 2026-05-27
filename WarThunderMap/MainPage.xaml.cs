using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;

namespace WarThunderMap;

public partial class MainPage
{
    readonly GameDrawable _drawable;

    public MainPage()
    {
        InitializeComponent();

        _drawable = new GameDrawable();
        CanvasView.Drawable = _drawable;
    }

    void OnStart(object sender, TouchEventArgs e)
    {
        var p = e.Touches[0];

        _drawable.OnDown(p);
        CanvasView.Invalidate();
    }

    void OnDrag(object sender, TouchEventArgs e)
    {
        _drawable.OnMove(e.Touches[0]);
        CanvasView.Invalidate();
    }

    void OnEnd(object sender, TouchEventArgs e)
    {
        _drawable.OnUp();
        CanvasView.Invalidate();
    }

    // Обработчик кнопки увеличения метров
    void OnIncreaseMeters(object sender, EventArgs e)
    {
        _drawable.IncreaseMeters();
        CanvasView.Invalidate();
    }

    // Обработчик кнопки уменьшения метров
    void OnDecreaseMeters(object sender, EventArgs e)
    {
        _drawable.DecreaseMeters();
        CanvasView.Invalidate();
    }

    // Обработчик кнопки увеличения зума
    void OnZoomIn(object sender, EventArgs e)
    {
        _drawable.ZoomIn();
        CanvasView.Invalidate();
    }

    // Обработчик кнопки уменьшения зума
    void OnZoomOut(object sender, EventArgs e)
    {
        _drawable.ZoomOut();
        CanvasView.Invalidate();
    }

    // Обработчик кнопки сброса зума
    void OnResetZoom(object sender, EventArgs e)
    {
        _drawable.ResetZoom();
        CanvasView.Invalidate();
    }
}
