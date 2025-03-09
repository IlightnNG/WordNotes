using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using WordNotes.Models;
using WordNotes.Services;

namespace WordNotes.Views.Base
{
    public abstract class BaseWindow : Window
    {
        private Point _pressedPosition;
        private bool _isDragMoved;
        // 可拖动区域
        protected Rect _dragArea;


        public BaseWindow()
        {
            PreviewMouseLeftButtonDown += BaseWindow_PreviewMouseLeftButtonDown;
            PreviewMouseMove += BaseWindow_PreviewMouseMove;
            PreviewMouseLeftButtonUp += BaseWindow_PreviewMouseLeftButtonUp;
        }

        // 关闭窗口
        public void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // 拖动窗口
        public void BaseWindow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _pressedPosition = e.GetPosition(this);
        }

        public void BaseWindow_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed &&
                _pressedPosition != e.GetPosition(this) && 
                _dragArea.Contains(_pressedPosition)&& 
                !_isDragMoved)
            {
                _isDragMoved = true;
                DragMove();
            }
        }

        public void BaseWindow_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragMoved)
            {
                _isDragMoved = false;
                e.Handled = true;

                // 记录窗口位置
                SaveWindowPosition();

            }
        }

        // 存储窗口信息
        public abstract void SaveWindowPosition();
    }
}
