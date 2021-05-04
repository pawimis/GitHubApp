using GitHubApp.Model;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GitHubApp.CustomViews.Contribution
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContributionView : ContentView
    {
        public ContributionView()
        {
            InitializeComponent();
            Content.BindingContext = this;
            datePicker.SelectedIndexChanged += DatePicker_SelectedIndexChanged;

        }

        private void DatePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (datePicker.SelectedIndex == -1)
            {
                return;
            }

            selectedYear = Contributions.Keys.ToList()[datePicker.SelectedIndex];
            daysInYear = (new DateTime(selectedYear, 12, 31) - new DateTime(selectedYear, 1, 1)).TotalDays;
            absenceTable.WidthRequest = 5 * daysInYear;
            absenceTable.MinimumWidthRequest = 5 * daysInYear;
            absenceTable.HeightRequest = _boxSize * 8;
            absenceTable.MinimumHeightRequest = _boxSize * 8;
            absenceTable.InvalidateSurface();

        }

        private const int _boxSize = 20;
        private int selectedYear = DateTime.Now.Year;
        private double daysInYear = 365;


        public static readonly BindableProperty ContributionsProperty = BindableProperty
         .Create(nameof(Contributions), typeof(Dictionary<int, List<ContributionContainer>>), typeof(ContributionView), null, propertyChanged: ContributionsPropertyChanged);


        public Dictionary<int, List<ContributionContainer>> Contributions { get => (Dictionary<int, List<ContributionContainer>>)GetValue(ContributionsProperty); set => SetValue(ContributionsProperty, value); }

        private static void ContributionsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != null)
            {
                ((ContributionView)bindable).DrawTable();
            }
        }
        private void DrawTable()
        {
            if (Contributions == null || Contributions.Keys.Count < 1)
            {
                return;
            }

            datePicker.ItemsSource = Contributions.Keys.Select(x => x.ToString()).ToList();

            datePicker.SelectedIndex = 0;

        }
        private void absenceTable_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            if (Contributions == null || Contributions.Keys.Count < 1)
            {
                return;
            }

            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear();
            SKPaint rectPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1,
                Color = ((Color)Xamarin.Forms.Application.Current.Resources["BorderFrame"]).ToSKColor(),
            };
            SKPaint contrPaint = new SKPaint
            {
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = 1,
                Color = Color.Green.ToSKColor(),
            };
            SKPaint textPaint = new SKPaint
            {
                Color = SKColors.Black
            };
            float FrameHeight = info.Height / (float)8;
            float height = FrameHeight;
            float width = 0;
            DateTime date = new DateTime(selectedYear, 1, 1);
            float textWidth = textPaint.MeasureText("ddd");
            textPaint.TextSize = 0.9f * FrameHeight * textPaint.TextSize / textWidth;
            canvas.DrawText(date.ToString("ddd"), width, height + FrameHeight, textPaint);
            date = date.AddDays(2);
            canvas.DrawText(date.ToString("ddd"), width, height + (3 * FrameHeight), textPaint);
            date = date.AddDays(2);
            canvas.DrawText(date.ToString("ddd"), width, height + (5 * FrameHeight), textPaint);
            height = FrameHeight;
            width = FrameHeight;
            int daysInCurrentMonth = DateTime.DaysInMonth(selectedYear, 1);
            date = date.AddDays(-4);
            daysInCurrentMonth = DateTime.DaysInMonth(selectedYear, date.Month);
            canvas.DrawText(date.ToString("MMM"), width, (float)(0.5 * FrameHeight), textPaint);
            List<ContributionContainer> contr = Contributions[selectedYear];
            for (int i = 1; i <= daysInYear; i++)
            {
                if (i == daysInCurrentMonth)
                {
                    date = date.AddMonths(1);
                    daysInCurrentMonth = DateTime.DaysInMonth(selectedYear, date.Month) + daysInCurrentMonth;
                    canvas.DrawText(date.ToString("MMM"), width, (float)(0.5 * FrameHeight), textPaint);

                }
                int contrCount = contr.Count(x => x.ContributionDate.Day == i);
                if (contrCount > 0)
                {
                    canvas.DrawRoundRect(width, height, FrameHeight, FrameHeight, 1, 1, contrPaint);
                }
                else
                {
                    canvas.DrawRoundRect(width, height, FrameHeight, FrameHeight, 1, 1, rectPaint);
                }

                height += FrameHeight;
                if (i % 7 == 0)
                {
                    height = FrameHeight;
                    width += FrameHeight;
                }

            }
            canvas.Restore();
        }
    }
}