using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace ProjectDiplom
{
    /// <summary>
    /// Логика взаимодействия для WPFUserControl1.xaml
    /// </summary>
    public partial class WPFUserControl1 : UserControl
    {
        private Storyboard myStoryboard;
        public WPFUserControl1()
        {
            InitializeComponent();
            image1.Source = new BitmapImage(new Uri(@"D:/papka.png"));
        }
        public void animOnUp(System.Windows.Controls.Image o)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 175.0;
            myDoubleAnimation.To = 185.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            myDoubleAnimation.AutoReverse = false;

            DoubleAnimation myDoubleAnimation2 = new DoubleAnimation();
            myDoubleAnimation2.From = 175.5;
            myDoubleAnimation2.To = 185.0;
            myDoubleAnimation2.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            myDoubleAnimation2.AutoReverse = false;
            //   myDoubleAnimation.RepeatBehavior ;

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(myDoubleAnimation);
            myStoryboard.Children.Add(myDoubleAnimation2);

            Storyboard.SetTargetName(myDoubleAnimation, o.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Image.WidthProperty));

            Storyboard.SetTargetName(myDoubleAnimation2, o.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation2, new PropertyPath(Image.HeightProperty));

            myStoryboard.Begin(this);

        }
        public void animOnDown(System.Windows.Controls.Image o)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 185.0;
            myDoubleAnimation.To = 175.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            myDoubleAnimation.AutoReverse = false;

            DoubleAnimation myDoubleAnimation2 = new DoubleAnimation();
            myDoubleAnimation2.From = 185.0;
            myDoubleAnimation2.To = 175.5;
            myDoubleAnimation2.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            myDoubleAnimation2.AutoReverse = false;


            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(myDoubleAnimation);
            myStoryboard.Children.Add(myDoubleAnimation2);

            Storyboard.SetTargetName(myDoubleAnimation, o.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Image.WidthProperty));

            Storyboard.SetTargetName(myDoubleAnimation2, o.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation2, new PropertyPath(Image.HeightProperty));

            myStoryboard.Begin(this);
        }
        private void image1_MouseEnter(object sender, MouseEventArgs e)
        {
            animOnUp(this.image1);
        }

        private void image1_MouseLeave(object sender, MouseEventArgs e)
        {
            animOnDown(this.image1);
        }

        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("OK");
        }

        private void image1_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            //Base.ActivateDoc();
        }
    }
}
