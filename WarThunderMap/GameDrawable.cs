using System.Numerics;

namespace WarThunderMap;

public class GameDrawable : IDrawable
{
    const int GridSize = 10;
    const float Radius = 10;

    float _width;
    float _height;
    float _playSize;
    readonly float _uiHeight = 60;

    float CellSize => _playSize / GridSize;

    readonly List<Vector2> _points = [];
    int? _draggingIndex;

    string _metersStr = "200";

    Vector2 _mousePos;

    bool _isEditingMeters;
    float _cursorTimer;
    bool _cursorVisible = true;

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        _cursorTimer += 1f / 60f;

        if (_cursorTimer > 0.5f)
        {
            _cursorVisible = !_cursorVisible;
            _cursorTimer = 0;
        }

        _width = dirtyRect.Width;
        _height = dirtyRect.Height;

        _playSize = Math.Min(_width, _height - _uiHeight);

        canvas.FillColor = Colors.White;
        canvas.FillRectangle(dirtyRect);

        canvas.StrokeColor = Colors.Black;

        for (int i = 0; i <= GridSize; i++)
        {
            float x = i * CellSize;
            float y = i * CellSize;

            canvas.DrawLine(x, 0, x, _playSize);
            canvas.DrawLine(0, y, _playSize, y);
        }

        canvas.FillColor = Colors.LightGray;
        canvas.FillRectangle(0, _playSize, _width, _uiHeight);

        canvas.FillColor = Colors.Red;

        foreach (var pt in _points)
        {
            canvas.FillCircle(pt.X, pt.Y, Radius);
        }

        string distText = "Press";

        if (_points.Count == 2)
        {
            canvas.StrokeColor = Colors.Blue;
            canvas.StrokeSize = 3;

            canvas.DrawLine(_points[0], _points[1]);

            float dx = _points[1].X - _points[0].X;
            float dy = _points[1].Y - _points[0].Y;

            float pixelDist = MathF.Sqrt(dx * dx + dy * dy);

            int mPerCell = int.TryParse(_metersStr, out var m) ? m : 0;

            float finalDist = (pixelDist / CellSize) * mPerCell;

            distText = $"Distance: {(int)finalDist} м";
        }

        canvas.FontSize = 18;
        canvas.FontColor = _isEditingMeters ? Colors.Yellow : Colors.Red;
        
        string metersText = $"Meters: {_metersStr}";

        if (_isEditingMeters && _cursorVisible)
            metersText += "_";

        canvas.DrawString(
            metersText,
            10,
            _playSize + 10,
            HorizontalAlignment.Left
        );

        canvas.FontColor = Colors.Blue;

        canvas.DrawString(
            distText,
            10,
            _playSize + 30,
            HorizontalAlignment.Left
        );
    }

    public void OnDown(PointF p)
    {
        _mousePos = new Vector2(p.X, p.Y);

        if (p.Y >= _playSize && p.Y <= _playSize + _uiHeight)
        {
            _isEditingMeters = true;
        }
        else
        {
            _isEditingMeters = false;
        }

        _isEditingMeters = false;

        for (int i = 0; i < _points.Count; i++)
        {
            if (Vector2.Distance(_points[i], _mousePos) <= Radius + 5)
            {
                _draggingIndex = i;
                return;
            }
        }

        if (_points.Count >= 2)
            _points.Clear();

        _points.Add(_mousePos);
    }

    public void OnMove(PointF p)
    {
        _mousePos = new Vector2(p.X, p.Y);

        if (_draggingIndex.HasValue)
        {
            int i = _draggingIndex.Value;

            _points[i] = _mousePos with { Y = Math.Clamp(_mousePos.Y, 0, _playSize) };
        }
    }

    public void OnUp()
    {
        _draggingIndex = null;
    }

    public void OnKey(char key)
    {
        if (!_isEditingMeters) return;

        if (char.IsDigit(key))
            _metersStr += key;
    }
    
    public void OnBackspace()
    {
        if (!_isEditingMeters) return;

        if (_metersStr.Length > 0)
            _metersStr = _metersStr[..^1];
    }
    
    public void SetMeters(string value)
    {
        _metersStr = value;
    }
}
