using System;
using System.Linq;
using System.Text;
using System.IO;
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
    using System.Xml.Linq;
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


        [WpfTheory]
        [InlineData("Images/Red.png", "LeftBtn")]
        [InlineData("Images/Yellow.png", "RightBtn")]
        public void ShouldHave_CorrectImageOnCorrectButton(string ImageAddress, string ButtonName)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();

            var Btn = mainWindow.FindName(ButtonName) as Button;

            Assert.IsType<Image>(Btn.Content);
            var Image = Btn.Content as Image;


            Assert.NotNull(Image);
            Assert.NotNull(Image.Source);


            Assert.Contains(ImageAddress, Image.Source.ToString());
        }
        [Fact]
        public void Xaml_ShouldHaveExpectedEvents()
        {
            var xamlPath = FindMainWindowXamlPath();
            var doc = XDocument.Load(xamlPath);

            // 1. Check Button x:Name="LeftBtn" has Click="LeftButtonClickHandle"
            var leftButton = doc.Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "Button" &&
                    e.Attributes().Any(a => a.Name.LocalName == "Name" && a.Value == "LeftBtn"));
            Assert.NotNull(leftButton);
            Assert.Equal("LeftButtonClickHandle", leftButton.Attribute("Click")?.Value);

            // 2. Check Button x:Name="RightBtn" has Click="RightButtonClickHandle"
            var rightButton = doc.Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "Button" &&
                    e.Attributes().Any(a => a.Name.LocalName == "Name" && a.Value == "RightBtn"));
            Assert.NotNull(rightButton);
            Assert.Equal("RightButtonClickHandle", rightButton.Attribute("Click")?.Value);

            // 3. Check <Image Loaded="LeftImageLoaded" ...>
            //var leftImage = doc.Descendants()
            //    .FirstOrDefault(e => e.Name.LocalName == "Image" &&
            //        e.Attribute("Loaded")?.Value == "LeftImageLoaded");
            //Assert.NotNull(leftImage);

            //// 4. Check <Image Loaded="RightImageLoaded" ...>
            //var rightImage = doc.Descendants()
            //    .FirstOrDefault(e => e.Name.LocalName == "Image" &&
            //        e.Attribute("Loaded")?.Value == "RightImageLoaded");
            //Assert.NotNull(rightImage);
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
        public void ShouldNotHaveStaticMemberVariable()
        {
            // ASK WHY?
            Type myType = typeof(MainWindow);

            // Get all static members of MyClass
            FieldInfo[] staticFields = myType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            // Display the names of all static members
            Dictionary<string, string> map = new Dictionary<string, string>();
            foreach (FieldInfo field in staticFields)
            {
                object value = field.GetValue(null);
                map.Add(field.Name, value.ToString());
            }
            // "Must have no static variables"
            Assert.Empty(map);

            //foreach (string key in map.Keys)
            //{
            //    Console.WriteLine("{0}={1}", key, map[key].ToString());
            //}

        }


        [WpfFact]
        public void ButtonsShouldSwapImagesAfter1Click()
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();

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
            mainWindow.Show();

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
            mainWindow.Show();

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
            mainWindow.Show();

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
            mainWindow.Show();

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


        public static string FindMainWindowXamlPath()
        {
            // Optionally, you could accept a parameter for the starting directory
            var startDirectory = Directory.GetCurrentDirectory();
            var dirInfo = new DirectoryInfo(startDirectory);

            while (dirInfo != null)
            {
                // 1) Look for a subfolder named "AP02023_ws01_WPF_Peekaboo" in the current directory
                string targetFolder = Path.Combine(dirInfo.FullName, "AP02023_ws01_WPF_Peekaboo");
                if (Directory.Exists(targetFolder))
                {
                    // 2) Check if "MainWindow.xaml" exists in that subfolder
                    string xamlPath = Path.Combine(targetFolder, "MainWindow.xaml");
                    if (File.Exists(xamlPath))
                    {
                        return xamlPath;
                    }
                }

                // Move up one level
                dirInfo = dirInfo.Parent;
            }

            // If we exit the loop, we never found the file
            throw new FileNotFoundException(
                $"Could not find 'MainWindow.xaml' in a folder named 'AP02023_ws01_WPF_Peekaboo' while searching upward from '{startDirectory}'.");
        }
    }

}
