using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Automation;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using Xunit;

namespace AP02023_ws01_WPF_Peekaboo.UITests
{
    using AP02023_ws01_WPF_Peekaboo;
    using System.Reflection;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using Xunit;



    public class MainWindowTests
    {


        [WpfTheory]
        [InlineData("LeftBtn")]
        [InlineData("RightBtn")]
        public void ShouldHave_Buttons(string ButtonName)
        {
            var window = new MainWindow();

            var Btn = window.FindName(ButtonName) as Button;
            Assert.NotNull(Btn);
        }



        [WpfFact]
        public void ScoreShouldStartFromZero()
        {
            var window = new MainWindow();
            var obj = window.FindName("ScoreNumber");
            Assert.NotNull(obj);

            Assert.IsType<TextBlock>(obj);

            var Textblock = window.FindName("ScoreNumber") as TextBlock;
            Assert.Equal("0", Textblock.Text);

        }

        //[WpfTheory]
        //[InlineData("LeftBtn")]
        //[InlineData("RightBtn")]
        //public void ShouldAttach_ClickHandlersToButtons(string ButtonName)
        //{
        //    var window = new MainWindow();

        //    var Btn = window.FindName(ButtonName) as Button;
        //    Assert.NotNull(Btn.Click);

        //}





        [WpfTheory]
        [InlineData(["Images/Red.png", "LeftBtn"])]
        [InlineData(["Images/Yellow.png", "RightBtn"])]
        public void ShouldHave_CorrectImageOnCorrectButton(string ImageAddress, string ButtonName)
        {
            var mainWindow = new MainWindow();
            var Btn = mainWindow.FindName(ButtonName) as Button;

            Assert.IsType<Image>(Btn.Content);
            var Image = Btn.Content as Image;


            Assert.NotNull(Image);
            Assert.NotNull(Image.Source);


            Assert.Contains(ImageAddress, Image.Source.ToString());
        }


        [WpfFact]
        public void ShouldAttachClickHandlerToRightBtn()
        {
            object mainWindow = new MainWindow();
            Assert.True(mainWindow.GetType().GetMethod("RightButtonClickHandle") != null);
            // Make Sure You have Created "public" void RightButtonClickHandle(object sender, RoutedEventArgs e)
        }

        [WpfFact]
        public void ShouldAttachClickHandlerToLeftBtn()
        {
            object mainWindow = new MainWindow();
            Assert.True(mainWindow.GetType().GetMethod("LeftButtonClickHandle") != null);
            // Make Sure You have Created "public" void LeftButtonClickHandle(object sender, RoutedEventArgs e)

        }



        [WpfFact]
        public void ButtonsShouldSwapImagesAfter1Click()
        {
            var mainWindow = new MainWindow();
            //mainWindow.Show();

            var Btn = mainWindow.FindName("RightBtn") as Button;
            Assert.NotNull(Btn);

            var Image = Btn.Content as Image;
            Assert.NotNull(Image);

            Assert.Contains("Images/Yellow.png", Image.Source.ToString());



            mainWindow.RightButtonClickHandle(null, null);

            Btn = mainWindow.FindName("RightBtn") as Button;
            Image = Btn.Content as Image;
            Assert.Contains("Images/Red.png", Image.Source.ToString());

            Btn = mainWindow.FindName("LeftBtn") as Button;
            Image = Btn.Content as Image;
            Assert.Contains("Images/Yellow.png", Image.Source.ToString());


        }

        [WpfFact]
        public void ButtonsShouldSwapImagesAfter2Clicks()
        {
            var mainWindow = new MainWindow();
            //mainWindow.Show();

            var Btn = mainWindow.FindName("RightBtn") as Button;
            Assert.NotNull(Btn);

            var Image = Btn.Content as Image;
            Assert.NotNull(Image);

            Assert.Contains("Images/Yellow.png", Image.Source.ToString());



            mainWindow.RightButtonClickHandle(null, null);
            mainWindow.LeftButtonClickHandle(null, null);

            Btn = mainWindow.FindName("RightBtn") as Button;
            Image = Btn.Content as Image;
            Assert.Contains("Images/Yellow.png", Image.Source.ToString());


            Btn = mainWindow.FindName("LeftBtn") as Button;
            Image = Btn.Content as Image;
            Assert.Contains("Images/Red.png", Image.Source.ToString());
        }





        [WpfFact]
        public void ShouldScoreUpAfter1Click()
        {
            var mainWindow = new MainWindow();
            //mainWindow.Show();

            var Btn = mainWindow.FindName("RightBtn") as Button;
            Assert.NotNull(Btn);

            var Image = Btn.Content as Image;
            Assert.NotNull(Image);

            Assert.Contains("Images/Yellow.png", Image.Source.ToString());

            mainWindow.RightButtonClickHandle(null, null);
            var Textblock = mainWindow.FindName("ScoreNumber") as TextBlock;
            Assert.Equal("1", Textblock.Text);


        }


        [WpfFact]
        public void ShouldScoreUpAfter2Clicks()
        {
            var mainWindow = new MainWindow();
            //mainWindow.Show();

            var Btn = mainWindow.FindName("RightBtn") as Button;
            Assert.NotNull(Btn);

            var Image = Btn.Content as Image;
            Assert.NotNull(Image);

            Assert.Contains("Images/Yellow.png", Image.Source.ToString());



            mainWindow.RightButtonClickHandle(null, null);
            var Textblock = mainWindow.FindName("ScoreNumber") as TextBlock;
            Assert.Equal("1", Textblock.Text);



            mainWindow.LeftButtonClickHandle(null, null);
            Textblock = mainWindow.FindName("ScoreNumber") as TextBlock;
            Assert.Equal("2", Textblock.Text);
        }


        [WpfFact]
        public void ShouldNotActAfterClickRedButton()
        {
            var mainWindow = new MainWindow();
            //mainWindow.Show();

            var Btn = mainWindow.FindName("RightBtn") as Button;
            Assert.NotNull(Btn);

            var Image = Btn.Content as Image;
            Assert.NotNull(Btn);

            Assert.Contains("Images/Yellow.png", Image.Source.ToString());



            mainWindow.RightButtonClickHandle(null, null);
            mainWindow.RightButtonClickHandle(null, null);
            mainWindow.RightButtonClickHandle(null, null);
            mainWindow.RightButtonClickHandle(null, null);
            mainWindow.RightButtonClickHandle(null, null);


            Btn = mainWindow.FindName("RightBtn") as Button;
            Image = Btn.Content as Image;
            Assert.Contains("Images/Red.png", Image.Source.ToString());

            var Textblock = mainWindow.FindName("ScoreNumber") as TextBlock;
            Assert.Equal("1", Textblock.Text);



            mainWindow.LeftButtonClickHandle(null, null);
            mainWindow.LeftButtonClickHandle(null, null);
            mainWindow.LeftButtonClickHandle(null, null);
            mainWindow.LeftButtonClickHandle(null, null);
            mainWindow.LeftButtonClickHandle(null, null);
            mainWindow.LeftButtonClickHandle(null, null);

            Btn = mainWindow.FindName("LeftBtn") as Button;
            Image = Btn.Content as Image;
            Assert.Contains("Images/Red.png", Image.Source.ToString());

            Textblock = mainWindow.FindName("ScoreNumber") as TextBlock;
            Assert.Equal("2", Textblock.Text);
        }

    }

}
