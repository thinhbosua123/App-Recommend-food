using Project.Model;
using Project.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Project.Pages;
using Project.Pages.SubCalorieBurnPage;
using System.Media;
using System.IO;

namespace Project.Pages
{
    /// <summary>
    /// Interaction logic for CalorieBurnPage.xaml
    /// </summary>
    public partial class CalorieBurnPage : Page
    {
        public List<Exercise> ExerciseList { get; set; }
        public List<UserExercise> ExerciseUser { get; set; }

        private CollectionView _view;
        private DispatcherTimer _countdownTimer = new DispatcherTimer();
        private DispatcherTimer _toggleTimer = new DispatcherTimer();
        

        private int _borderFlag = 0;
        private int _remainingTime = 0;
        private double _totalCalo = 0;
        private double _caloBurnedPerSec = 0;
        private double _burnedCalo = 0;

        public CalorieBurnPage()
        {
            InitializeComponent();
            this.DataContext = this;

            _countdownTimer.Interval = TimeSpan.FromSeconds(1);

            _toggleTimer.Interval = TimeSpan.FromSeconds(0.5);
            _toggleTimer.Tick += Flicker_Tick;
            _toggleTimer.Start();


            ExerciseUser = new List<UserExercise>();
            ExerciseList = new List<Exercise>();

            Play_btn.IsEnabled = false;
            Pause_btn.IsEnabled = false;
        }

        private void Flicker_Tick(object sender, EventArgs e)
        {
            switch (_borderFlag)
            {
                case 0:
                    {
                        CaloBox.BorderThickness = new Thickness(2);
                        CaloBox.BorderBrush = Brushes.Yellow;
                        _borderFlag = 1;
                    }
                    break;
                case 1:
                    {
                        CaloBox.BorderThickness = new Thickness(1);
                        CaloBox.BorderBrush = Brushes.Black;
                        _borderFlag = 0;
                    }
                    break;
                case 2:
                    {
                        CaloBox.BorderThickness = new Thickness(1);
                        CaloBox.BorderBrush = Brushes.Black;
                        _toggleTimer.Stop();
                    }
                    break;

            }
        }

        private void Count_Tick(object sender, EventArgs e)
        {
            if (_remainingTime-- <= 0)
            {
                // dung dong ho, vo hieu nut pause, lam day vong tron
                _countdownTimer.Stop();
                Pause_btn.IsEnabled = false;
                Gauge_Kcal.Value = Convert.ToInt32(CaloBox.Text);

                // reset cac bien
                _remainingTime = 0;
                _totalCalo = 0;
                _caloBurnedPerSec = 0;
                _burnedCalo = 0;

                // phat nhac
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
                string path = projectDirectory + "\\HomNayAnGi\\Assets\\Exercises\\complete_sound.wav";

                MediaPlayer mediaPlayer = new MediaPlayer();
                mediaPlayer.Open(new Uri(path));
                mediaPlayer.Play();

                _borderFlag = 0;
                _toggleTimer.Start();

                CaloBox.Text = "";

                CongratsWindow congratsWindow = new CongratsWindow();
                congratsWindow.Owner = Window.GetWindow(this);
                congratsWindow.ShowDialog();

                Gauge_Kcal.Value = 0;
                return;
            }

            // tang so calo dot moi giay
            _burnedCalo += _caloBurnedPerSec;
            Gauge_Kcal.Value = (int)_burnedCalo;

            // giam tong so calo can tinh
            _totalCalo -= _caloBurnedPerSec;
            Clock_block.Text = TimeSpan.FromSeconds(_remainingTime).ToString();
        }

        private void CaloBurnPage_Loaded(object sender, RoutedEventArgs e)
        {
            ExerciseUser = DataProvider.Ins.DB.UserExercise.Where(p => p.UserID == DataProvider.Ins.Current_UserID).ToList();
            Exercise exercise = new Exercise();
            ExerciseList = new List<Exercise>();

            foreach (UserExercise user in ExerciseUser)
            {
                exercise = DataProvider.Ins.DB.Exercise.SingleOrDefault(p => p.ExID == user.ExID);
                ExerciseList.Add(exercise);
            }

            lvCaloriesBurned.ItemsSource = ExerciseList;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(lvCaloriesBurned.ItemsSource);
            _view.Filter = UserFilter;
        }


        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            lvCaloriesBurned.Items.Filter = UserFilter;
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return ((item as Exercise).ExName.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }


        private void Play_btn_Click(object sender, RoutedEventArgs e)
        {
            _countdownTimer.Tick += Count_Tick;
            _countdownTimer.Start();
            (sender as Button).IsEnabled = false;
            Pause_btn.IsEnabled = true;
        }

        private void Pause_btn_Click(object sender, RoutedEventArgs e)
        {
            _countdownTimer.Stop();
            _countdownTimer.Tick -= Count_Tick;
            (sender as Button).IsEnabled = false;
            Play_btn.IsEnabled = true;
        }

        private void Calculate_btn_Click(object sender, RoutedEventArgs e)
        {
            // nhan so calo moi, kiem tra hop le
            if (!int.TryParse(CaloBox.Text, out int totalCalo) || totalCalo <= 0)
            {
                MessageBox.Show("Số calo nhập vào phải nguyên dương");
                return;
            }

            _totalCalo = totalCalo;

            // reset calo da dot, item da chon, tat nhap nhay caloBox
            _burnedCalo = 0;
            lvCaloriesBurned.SelectedIndex = -1;
            _borderFlag = 2;

            // reset dong ho
            _countdownTimer.Stop();

            MessageBox.Show("Đã tính xong, mời bạn chọn bài tập");
            _countdownTimer.Tick -= Count_Tick;
            Clock_block.Text = TimeSpan.FromSeconds(0).ToString();

            // reset dong ho calo
            Gauge_Kcal.To = (int)_totalCalo;
            Gauge_Kcal.Value = 0;

        }

        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            InsertExerciseWindow insertExerciseWindow = new InsertExerciseWindow();
            insertExerciseWindow.Owner = Window.GetWindow(this);
            insertExerciseWindow.ShowDialog();
            _view.Refresh();
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show
                ("Bạn chắc chắn muốn xóa bài tập này không ?", 
                "Thông báo", 
                MessageBoxButton.YesNoCancel, 
                MessageBoxImage.Question, 
                MessageBoxResult.Cancel) 
                == MessageBoxResult.Yes)
            {
                Button button = (Button)sender;
                Exercise exercise = button.DataContext as Exercise;

                // xoa userEx khoi DB
                DataProvider.Ins.DB.UserExercise.Remove(DataProvider.Ins.DB.UserExercise.SingleOrDefault(p => p.ExID == exercise.ExID && p.UserID == DataProvider.Ins.Current_UserID));

                // khong xoa nhung bai tap mac dinh
                if (exercise.ExID > 15)
                    DataProvider.Ins.DB.Exercise.Remove(exercise);

                DataProvider.Ins.DB.SaveChanges();

                ExerciseList.Remove(exercise);
                _view.Refresh();
            }
        }


        private void lvCaloriesBurned_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_totalCalo <= 0 || lvCaloriesBurned.SelectedIndex == -1)
                return;

            _countdownTimer.Stop();
            if (MessageBox.Show("Bạn muốn chọn bài tập này?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // xoa event tick cu de khong bi nap chong len nhau
                _countdownTimer.Tick -= Count_Tick;

                // kich hoat nut play
                Play_btn.IsEnabled = true;
                Pause_btn.IsEnabled = false;

                Exercise exercise = (Exercise)lvCaloriesBurned.SelectedItem;

                // tinh tong thoi gian va calo dot moi giay
                _remainingTime = (int)_totalCalo * 3600 / (int)exercise.Kph;
                _caloBurnedPerSec = (double)exercise.Kph / 3600;

                // hien thi thoi gian
                Clock_block.Text = TimeSpan.FromSeconds(_remainingTime).ToString();
            }
            else
                _countdownTimer.Start();

        }
    }

}

