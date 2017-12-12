using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace CribbageAI
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return Int32.Parse((string)value);
            }
            catch
            {
                return 0;
            }
        }
    }

    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return Double.Parse((string)value);
            }
            catch
            {
                return 0;
            }
        }
    }


    //
    //  pass in a list like "1,2,3,4" and convert it to a List<int> {1, 2, 3, 4}
    public class StringToIntListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string s = "";
            foreach (int val in (List<int>)value)
            {
                s += val.ToString() + ",";
            }

            return s;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                List<int> list = new List<int>();
                string s = value as string;
                string[] values = s.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string val in values)
                {
                    list.Add(Int32.Parse(val));
                }

                return list;
            }
            catch
            {
                return 0;
            }
        }
    }

   

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value.GetType() == typeof(bool))
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((Visibility)value == Visibility.Visible)
                return true;
            else
                return false;
        }
    }

    public class StringToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (string.IsNullOrEmpty(value as string))
            {
                return null;
            }
            else return new BitmapImage(new Uri(value as string, UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            return ((BitmapImage)value).UriSource.ToString();
        }
    }

    public class StringToImageBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (string.IsNullOrEmpty(value as string))
            {
                return null;
            }

            ImageBrush brush = new ImageBrush();
            BitmapImage bitmapImage = new BitmapImage(new Uri(value as string, UriKind.RelativeOrAbsolute));
            bitmapImage.DecodePixelHeight = 125;
            bitmapImage.DecodePixelWidth = 125;
            bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            brush.ImageSource = bitmapImage;
            brush.Stretch = Stretch.UniformToFill;



            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            return ((BitmapImage)value).UriSource.ToString();
        }
    }

    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            try
            {
                if (value.GetType() == typeof(string))
                {

                    if (targetType == typeof(Brush))
                    {
                        Color c = Colors.HotPink;
                        if (StaticHelpers.StringToColorDictionary.TryGetValue((string)value, out c))
                        {
                            return new SolidColorBrush(c);
                        }
                    }

                }

                if (value.GetType() == typeof(Color))
                {
                    if (targetType == typeof(Brush))
                    {

                        return new SolidColorBrush((Color)value);
                    }
                }



            }
            catch
            {
                this.TraceMessage($"Exception thrown in ColorToBrushConvert: {value.ToString()}");
                return new SolidColorBrush(Colors.HotPink);
            }

            return new SolidColorBrush(Colors.HotPink);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {

                if (value.GetType() == typeof(SolidColorBrush))
                {
                    SolidColorBrush br = (SolidColorBrush)value;
                    if (targetType == typeof(string))
                    {
                        return StaticHelpers.ColorToStringDictionary[br.Color];
                    }
                    if (targetType == typeof(Color))
                    {
                        return br.Color;
                    }

                    return null;
                }



            }
            catch
            {
                return "HotPink";

            }

            return null;
        }
    }



    public class ScoreIntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return "Score: " + value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                string s = ((string)value).TrimStart(new char[] { 'S', 'c', 'o', 'r', 'e', ':', ' ' });
                return Int32.Parse(s);
            }
            catch
            {
                return 0;
            }
        }
    }

    public class AnimationSpeedValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              string language)
        {

            // val goes 1-4

            string[] Names = new string[] { "Slow - Fun to watch", "Medium - pretty normal", "Faster - if you are in a hurry", "Super Fast -- if you are debugging" };

            int val = System.Convert.ToInt32(value);

            return Names[val - 1];


        }
        public object ConvertBack(object value, Type targetType,
                                  object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ObjectToObjectValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              string language)
        {

            return value;


        }
        public object ConvertBack(object value, Type targetType,
                                  object parameter, string language)
        {
            return value;
        }
    }

    public class StorageFileToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              string language)
        {
            if (value == null)
                return value;
            ObservableCollection<string> col = new ObservableCollection<string>();
            foreach (StorageFile f in value as ObservableCollection<StorageFile>)
            {
                col.Add(f.DisplayName);
            }

            return col;


        }
        public object ConvertBack(object value, Type targetType,
                                  object parameter, string language)
        {
            if (parameter.GetType() != typeof(ComboBox))
                return null;

            ComboBox bx = parameter as ComboBox;
            ObservableCollection<StorageFile> list = bx.Tag as ObservableCollection<StorageFile>;
            foreach (var f in list)
            {
                if (f.DisplayName == (string)value)
                    return f;
            }

            return null;
        }
    }

    public class TileOrientationToObjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              string language)
        {

            return value;


        }
        public object ConvertBack(object value, Type targetType,
                                  object parameter, string language)
        {
            return value;
        }
    }



    //
    //  used to bind to IsEnabled - e.g. "if the CurrentTile == null, the control is disabled"
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              string language)
        {

            if (value == null)
                return false;

            return true;


        }
        public object ConvertBack(object value, Type targetType,
                                  object parameter, string language)
        {
            return value;
        }
    }

    public class EnumToStringValueConverter<T>
    {
        public object Convert(object value)
        {
            return value.ToString();
        }
        public object ConvertBack(object value)
        {
            T t = (T)Enum.Parse(typeof(T), value.ToString());
            return t;
        }

    }

    
    public class GameStateValueConverter : IValueConverter
    {
        EnumToStringValueConverter<GameState> _converter = new EnumToStringValueConverter<GameState>();
        public object Convert(object value, Type targetType, object parameter,
                              string language)
        {

            return _converter.Convert(value);


        }
        public object ConvertBack(object value, Type targetType,
                                  object parameter, string language)
        {
            return _converter.ConvertBack(value);
        }
    }

    

    public class StorageFileValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
