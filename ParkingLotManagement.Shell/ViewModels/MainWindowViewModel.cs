using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ParkingLotManagement.Shell.Helper;

namespace ParkingLotManagement.Shell.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Private Member

        /// <summary>
        /// The window this view model controls
        /// </summary>
        private Window _window;

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        private int _outerMarginSize = 10;

        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition _dockPosition = WindowDockPosition.Undocked;

        /// <summary>
        /// The title of the application
        /// </summary>
        private string _title = "Parking Lot Management";

        #endregion

        #region Public Properties

        /// <summary>
        /// The smallest width the window can go to
        /// </summary>
        public double WindowMinWidth { get; set; } = 400;

        /// <summary>
        /// The smallest height the window can go to
        /// </summary>
        public double WindowMinHeight { get; set; } = 400;

        /// <summary>
        /// True if the window should be borderless because it is docked or maximized
        /// </summary>
        public bool Borderless { get { return (_window.WindowState == WindowState.Maximized || _dockPosition != WindowDockPosition.Undocked); } }

        /// <summary>
        /// The size of the resize border around the window
        /// </summary>
        public int ResizeBorder { get; set; } = 6;

        /// <summary>
        /// The size of the resize border around the window, taking into account the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder + OuterMarginSize); } }

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public int OuterMarginSize
        {
            get
            {
                // If it is maximized or docked, no border
                return Borderless ? 0 : _outerMarginSize;
            }
            set
            {
                _outerMarginSize = value;
            }
        }

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public Thickness OuterMarginSizeThickness { get { return new Thickness(OuterMarginSize); } }

        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public int TitleHeight { get; set; } = 30;
        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public GridLength TitleHeightGridLength { get { return new GridLength(TitleHeight + ResizeBorder); } }

        /// <summary>
        /// The title of the application
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                SetProperty(ref _title, value);
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// The command to minimize the window
        /// </summary>
        public DelegateCommand MinimizeCommand { get; set; }

        /// <summary>
        /// The command to maximize the window
        /// </summary>
        public DelegateCommand MaximizeCommand { get; set; }

        /// <summary>
        /// The command to close the window
        /// </summary>
        public DelegateCommand CloseCommand { get; set; }

        /// <summary>
        /// The command to show the system menu of the window
        /// </summary>
        public DelegateCommand MenuCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindowViewModel()
        {
            _window = Application.Current.MainWindow;

            // Listen out for the window resizing
            _window.StateChanged += (sender, e) =>
            {
                // Fire off events for all properties that are affected by a resize
                WindowResized();
            };

            // Create commands
            MinimizeCommand = new DelegateCommand(() => _window.WindowState = WindowState.Minimized);
            MaximizeCommand = new DelegateCommand(() => _window.WindowState ^= WindowState.Maximized);
            CloseCommand = new DelegateCommand(() => _window.Close());
            MenuCommand = new DelegateCommand(() => SystemCommands.ShowSystemMenu(_window, GetMousePosition()));

            // Fix window resize issue
            var resizer = new WindowResizer(_window);

            // Listen out for dock changes
            resizer.WindowDockChanged += (dock) =>
            {
                // Store last position
                _dockPosition = dock;

                // Fire off resize events
                WindowResized();
            };
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Gets the current mouse position on the screen
        /// </summary>
        /// <returns></returns>
        private Point GetMousePosition()
        {
            // Position of the mouse relative to the window
            var position = Mouse.GetPosition(_window);

            // Add the window position so its a "ToScreen"
            return new Point(_window.WindowState == WindowState.Maximized ? position.X : position.X + _window.Left, _window.WindowState == WindowState.Maximized ? position.Y : position.Y + _window.Top);
        }

        /// <summary>
        /// If the window resizes to a special position (docked or maximized)
        /// this will update all required property change events to set the borders and radius values
        /// </summary>
        private void WindowResized()
        {
            // Fire off events for all properties that are affected by a resize
            RaisePropertyChanged(nameof(Borderless));
            RaisePropertyChanged(nameof(ResizeBorderThickness));
            RaisePropertyChanged(nameof(OuterMarginSize));
            RaisePropertyChanged(nameof(OuterMarginSizeThickness));
        }

        #endregion
    }
}
