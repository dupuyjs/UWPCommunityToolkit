﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents the control that redistributes space between columns or rows of a Grid control.
    /// </summary>
    public partial class GridSplitter
    {
        private static bool IsStarColumn(ColumnDefinition definition)
        {
            return ((GridLength)definition.GetValue(ColumnDefinition.WidthProperty)).IsStar;
        }

        private static bool IsStarRow(RowDefinition definition)
        {
            return ((GridLength)definition.GetValue(RowDefinition.HeightProperty)).IsStar;
        }

        private void SetColumnWidth(ColumnDefinition columnDefinition, double horizontalChange, GridUnitType unitType)
        {
            var newWidth = columnDefinition.ActualWidth + horizontalChange;
            if (newWidth > ActualWidth)
            {
                columnDefinition.Width = new GridLength(newWidth, unitType);
            }
        }

        private void SetRowHeight(RowDefinition rowDefinition, double verticalChange, GridUnitType unitType)
        {
            var newHeight = rowDefinition.ActualHeight + verticalChange;
            if (newHeight > ActualHeight)
            {
                rowDefinition.Height = new GridLength(newHeight, unitType);
            }
        }

        private void InitControl()
        {
            if (_resizeDirection == GridResizeDirection.Columns)
            {
                // setting the Column min width to the width of the GridSplitter
                var currentIndex = Grid.GetColumn(this);
                if ((currentIndex >= 0)
                       && (currentIndex < Resizable.ColumnDefinitions.Count))
                {
                    var splitterColumn = Resizable.ColumnDefinitions[currentIndex];
                    splitterColumn.MinWidth = ActualWidth;
                }
            }
            else if (_resizeDirection == GridResizeDirection.Rows)
            {
                // setting the Row min height to the height of the GridSplitter
                var currentIndex = Grid.GetRow(this);
                if ((currentIndex >= 0)
                       && (currentIndex < Resizable.RowDefinitions.Count))
                {
                    var splitterRow = Resizable.RowDefinitions[currentIndex];
                    splitterRow.MinHeight = ActualHeight;
                }
            }
        }

        // Return the targeted Column based on the resize behavior
        private int GetTargetedColumn()
        {
            var currentIndex = Grid.GetColumn(this);
            return GetTargetIndex(currentIndex);
        }

        // Return the sibling Row based on the resize behavior
        private int GetTargetedRow()
        {
            var currentIndex = Grid.GetRow(this);
            return GetTargetIndex(currentIndex);
        }

        // Return the sibling Column based on the resize behavior
        private int GetSiblingColumn()
        {
            var currentIndex = Grid.GetColumn(this);
            return GetSiblingIndex(currentIndex);
        }

        // Return the sibling Row based on the resize behavior
        private int GetSiblingRow()
        {
            var currentIndex = Grid.GetRow(this);
            return GetSiblingIndex(currentIndex);
        }

        // Gets index based on resize behavior for first targeted row/column
        private int GetTargetIndex(int currentIndex)
        {
            switch (_resizeBehavior)
            {
                case GridResizeBehavior.CurrentAndNext:
                    return currentIndex;
                case GridResizeBehavior.PreviousAndNext:
                    return currentIndex - 1;
                case GridResizeBehavior.PreviousAndCurrent:
                    return currentIndex - 1;
                default:
                    return -1;
            }
        }

        // Gets index based on resize behavior for second targeted row/column
        private int GetSiblingIndex(int currentIndex)
        {
            switch (_resizeBehavior)
            {
                case GridResizeBehavior.CurrentAndNext:
                    return currentIndex + 1;
                case GridResizeBehavior.PreviousAndNext:
                    return currentIndex + 1;
                case GridResizeBehavior.PreviousAndCurrent:
                    return currentIndex;
                default:
                    return -1;
            }
        }

        // Checks the control alignment and Width/Height to detect the control resize direction columns/rows
        private GridResizeDirection GetResizeDirection()
        {
            GridResizeDirection direction = ResizeDirection;

            if (direction == GridResizeDirection.Auto)
            {
                // When HorizontalAlignment is Left, Right or Center, resize Columns
                if (HorizontalAlignment != HorizontalAlignment.Stretch)
                {
                    direction = GridResizeDirection.Columns;
                }

                // When VerticalAlignment is Top, Bottom or Center, resize Rows
                else if (VerticalAlignment != VerticalAlignment.Stretch)
                {
                    direction = GridResizeDirection.Rows;
                }

                // Check Width vs Height
                else if (ActualWidth <= ActualHeight)
                {
                    direction = GridResizeDirection.Columns;
                }
                else
                {
                    direction = GridResizeDirection.Rows;
                }
            }

            return direction;
        }

        // Get the resize behavior (Which columns/rows should be resized) based on alignment and Direction
        private GridResizeBehavior GetResizeBehavior()
        {
            GridResizeBehavior resizeBehavior = ResizeBehavior;

            if (resizeBehavior == GridResizeBehavior.BasedOnAlignment)
            {
                if (_resizeDirection == GridResizeDirection.Columns)
                {
                    switch (HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            resizeBehavior = GridResizeBehavior.PreviousAndCurrent;
                            break;
                        case HorizontalAlignment.Right:
                            resizeBehavior = GridResizeBehavior.CurrentAndNext;
                            break;
                        default:
                            resizeBehavior = GridResizeBehavior.PreviousAndNext;
                            break;
                    }
                }

                // resize direction is vertical
                else
                {
                    switch (VerticalAlignment)
                    {
                        case VerticalAlignment.Top:
                            resizeBehavior = GridResizeBehavior.PreviousAndCurrent;
                            break;
                        case VerticalAlignment.Bottom:
                            resizeBehavior = GridResizeBehavior.CurrentAndNext;
                            break;
                        default:
                            resizeBehavior = GridResizeBehavior.PreviousAndNext;
                            break;
                    }
                }
            }

            return resizeBehavior;
        }
    }
}
