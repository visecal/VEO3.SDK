using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VEO3.SDK;

namespace VEO3.Plugins.ImageGenV2
{
    /// <summary>
    /// Image Generation V2 Plugin - Advanced image generation with presets
    /// </summary>
    public class ImageGenV2Plugin : PluginBase
    {
        public override string Name => "Image Gen V2";
        public override string Version => "1.0.0";
        public override string Description => "Tạo hình ảnh với AI - Phiên bản nâng cao với nhiều preset và style";
        public override string Author => "VEO3 Team";
        public override string Icon => "🎨";

        public override UserControl CreateUI()
        {
            return new ImageGenV2Control(Context);
        }
    }

    /// <summary>
    /// UI Control cho Image Generation V2
    /// </summary>
    public class ImageGenV2Control : UserControl
    {
        private readonly IPluginContext _context;

        // UI Controls
        private ComboBox _styleComboBox;
        private ComboBox _subjectComboBox;
        private ComboBox _aspectRatioComboBox;
        private TextBox _promptTextBox;
        private TextBox _negativePromptTextBox;
        private Button _generateButton;
        private ProgressBar _progressBar;
        private TextBlock _statusText;
        private Image _previewImage;
        private StackPanel _presetsPanel;

        // Presets Data
        private readonly string[] _artStyles = new[]
        {
            "Realistic Photo",
            "Anime Style",
            "Oil Painting",
            "Watercolor",
            "3D Render",
            "Cyberpunk",
            "Studio Ghibli",
            "Pixel Art",
            "Comic Book",
            "Minimalist"
        };

        private readonly string[] _subjects = new[]
        {
            "Portrait",
            "Landscape",
            "Animal",
            "Food",
            "Architecture",
            "Vehicle",
            "Fantasy Character",
            "Sci-Fi Scene",
            "Nature",
            "Abstract"
        };

        private readonly string[] _aspectRatios = new[]
        {
            "1:1",
            "16:9",
            "9:16",
            "4:3",
            "3:4"
        };

        public ImageGenV2Control(IPluginContext context)
        {
            _context = context;
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Main Container
            var mainGrid = new Grid
            {
                Margin = new Thickness(20)
            };

            // Define columns
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Left Panel - Controls
            var leftPanel = CreateControlsPanel();
            Grid.SetColumn(leftPanel, 0);
            mainGrid.Children.Add(leftPanel);

            // Right Panel - Preview
            var rightPanel = CreatePreviewPanel();
            Grid.SetColumn(rightPanel, 2);
            mainGrid.Children.Add(rightPanel);

            this.Content = mainGrid;
        }

        private StackPanel CreateControlsPanel()
        {
            var panel = new StackPanel { Margin = new Thickness(0) };

            // Title
            var title = new TextBlock
            {
                Text = "🎨 Image Generation V2",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 20),
                Foreground = new SolidColorBrush(Color.FromRgb(100, 100, 255))
            };
            panel.Children.Add(title);

            // Quick Presets Section
            panel.Children.Add(CreateSectionTitle("⚡ Quick Presets"));
            _presetsPanel = CreatePresetsPanel();
            panel.Children.Add(_presetsPanel);

            // Art Style
            panel.Children.Add(CreateSectionTitle("🎭 Art Style"));
            _styleComboBox = new ComboBox
            {
                Margin = new Thickness(0, 5, 0, 15),
                FontSize = 14,
                Height = 35
            };
            foreach (var style in _artStyles)
            {
                _styleComboBox.Items.Add(style);
            }
            _styleComboBox.SelectedIndex = 0;
            panel.Children.Add(_styleComboBox);

            // Subject
            panel.Children.Add(CreateSectionTitle("📸 Subject"));
            _subjectComboBox = new ComboBox
            {
                Margin = new Thickness(0, 5, 0, 15),
                FontSize = 14,
                Height = 35
            };
            foreach (var subject in _subjects)
            {
                _subjectComboBox.Items.Add(subject);
            }
            _subjectComboBox.SelectedIndex = 0;
            panel.Children.Add(_subjectComboBox);

            // Aspect Ratio
            panel.Children.Add(CreateSectionTitle("📐 Aspect Ratio"));
            _aspectRatioComboBox = new ComboBox
            {
                Margin = new Thickness(0, 5, 0, 15),
                FontSize = 14,
                Height = 35
            };
            foreach (var ratio in _aspectRatios)
            {
                _aspectRatioComboBox.Items.Add(ratio);
            }
            _aspectRatioComboBox.SelectedIndex = 1; // 16:9
            panel.Children.Add(_aspectRatioComboBox);

            // Custom Prompt
            panel.Children.Add(CreateSectionTitle("✏️ Custom Prompt (Optional)"));
            _promptTextBox = new TextBox
            {
                Margin = new Thickness(0, 5, 0, 15),
                Height = 80,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                FontSize = 13
            };
            panel.Children.Add(_promptTextBox);

            // Negative Prompt
            panel.Children.Add(CreateSectionTitle("🚫 Negative Prompt (Optional)"));
            _negativePromptTextBox = new TextBox
            {
                Margin = new Thickness(0, 5, 0, 15),
                Height = 60,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                FontSize = 13,
                Text = "blurry, low quality, distorted, ugly"
            };
            panel.Children.Add(_negativePromptTextBox);

            // Generate Button
            _generateButton = new Button
            {
                Content = "🎨 Generate Image",
                Height = 45,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 10, 0, 10),
                Background = new SolidColorBrush(Color.FromRgb(100, 100, 255)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                Cursor = System.Windows.Input.Cursors.Hand
            };
            _generateButton.Click += GenerateButton_Click;
            panel.Children.Add(_generateButton);

            // Progress Bar
            _progressBar = new ProgressBar
            {
                Height = 25,
                Margin = new Thickness(0, 10, 0, 5),
                Visibility = Visibility.Collapsed,
                IsIndeterminate = true
            };
            panel.Children.Add(_progressBar);

            // Status Text
            _statusText = new TextBlock
            {
                Margin = new Thickness(0, 5, 0, 0),
                FontSize = 12,
                Foreground = Brushes.Gray,
                TextWrapping = TextWrapping.Wrap
            };
            panel.Children.Add(_statusText);

            return panel;
        }

        private StackPanel CreatePresetsPanel()
        {
            var panel = new StackPanel
            {
                Margin = new Thickness(0, 5, 0, 15)
            };

            var presets = new[]
            {
                new { Name = "🌅 Sunset", Style = "Realistic Photo", Subject = "Landscape" },
                new { Name = "😺 Cute Cat", Style = "Anime Style", Subject = "Animal" },
                new { Name = "🏰 Fantasy Castle", Style = "Oil Painting", Subject = "Architecture" },
                new { Name = "🤖 Cyberpunk", Style = "Cyberpunk", Subject = "Sci-Fi Scene" },
                new { Name = "🌸 Watercolor", Style = "Watercolor", Subject = "Nature" },
                new { Name = "🎮 Pixel Game", Style = "Pixel Art", Subject = "Fantasy Character" }
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10) });
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            int row = 0;
            for (int i = 0; i < presets.Length; i++)
            {
                var preset = presets[i];
                var button = new Button
                {
                    Content = preset.Name,
                    Height = 35,
                    Margin = new Thickness(0, 0, 0, 8),
                    FontSize = 13,
                    Background = new SolidColorBrush(Color.FromRgb(240, 240, 250)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 220)),
                    Cursor = System.Windows.Input.Cursors.Hand,
                    Tag = preset
                };
                button.Click += PresetButton_Click;

                if (i % 2 == 0)
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, 0);
                }
                else
                {
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, 2);
                    row++;
                }

                grid.Children.Add(button);
            }

            panel.Children.Add(grid);
            return panel;
        }

        private void PresetButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            dynamic preset = button.Tag;

            // Set style and subject from preset
            _styleComboBox.SelectedItem = preset.Style;
            _subjectComboBox.SelectedItem = preset.Subject;

            _context.ShowNotification($"Applied preset: {preset.Name}", NotificationType.Info);
        }

        private StackPanel CreatePreviewPanel()
        {
            var panel = new StackPanel();

            // Preview Title
            var title = new TextBlock
            {
                Text = "🖼️ Preview",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 15),
                Foreground = new SolidColorBrush(Color.FromRgb(80, 80, 80))
            };
            panel.Children.Add(title);

            // Image Preview Border
            var border = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
                BorderThickness = new Thickness(2),
                Background = new SolidColorBrush(Color.FromRgb(245, 245, 245)),
                MinHeight = 400,
                Margin = new Thickness(0, 0, 0, 10)
            };

            _previewImage = new Image
            {
                Stretch = Stretch.Uniform,
                Margin = new Thickness(10)
            };

            // Placeholder text
            var placeholder = new TextBlock
            {
                Text = "Generated image will appear here",
                FontSize = 14,
                Foreground = Brushes.Gray,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(20)
            };

            var grid = new Grid();
            grid.Children.Add(placeholder);
            grid.Children.Add(_previewImage);

            border.Child = grid;
            panel.Children.Add(border);

            // Info text
            var infoText = new TextBlock
            {
                Text = "💡 Tip: Try different combinations of style and subject!\nImages are saved to VEO3_Output folder.",
                FontSize = 12,
                Foreground = Brushes.Gray,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 10, 0, 0)
            };
            panel.Children.Add(infoText);

            return panel;
        }

        private TextBlock CreateSectionTitle(string text)
        {
            return new TextBlock
            {
                Text = text,
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 10, 0, 5),
                Foreground = new SolidColorBrush(Color.FromRgb(60, 60, 60))
            };
        }

        private async void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Disable button during generation
                _generateButton.IsEnabled = false;
                _progressBar.Visibility = Visibility.Visible;
                _statusText.Text = "🎨 Generating image...";
                _statusText.Foreground = Brushes.Blue;

                // Build prompt
                string prompt = BuildPrompt();
                string negativePrompt = _negativePromptTextBox.Text.Trim();
                string aspectRatio = _aspectRatioComboBox.SelectedItem?.ToString() ?? "16:9";

                _context.Log($"Generating image: {prompt}", LogLevel.Info);

                // Generate image using VEO3 Service
                string imagePath = await _context.Veo3Service.GenerateImage(
                    prompt,
                    aspectRatio,
                    string.IsNullOrEmpty(negativePrompt) ? null : negativePrompt
                );

                // Display image
                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    LoadImageToPreview(imagePath);
                    _statusText.Text = $"✅ Image generated successfully!\nSaved to: {Path.GetFileName(imagePath)}";
                    _statusText.Foreground = Brushes.Green;
                    _context.ShowNotification("Image generated successfully!", NotificationType.Success);
                }
                else
                {
                    throw new Exception("Failed to generate image");
                }
            }
            catch (Exception ex)
            {
                _statusText.Text = $"❌ Error: {ex.Message}";
                _statusText.Foreground = Brushes.Red;
                _context.ShowNotification($"Error: {ex.Message}", NotificationType.Error);
                _context.Log($"Image generation error: {ex.Message}", LogLevel.Error);
            }
            finally
            {
                _generateButton.IsEnabled = true;
                _progressBar.Visibility = Visibility.Collapsed;
            }
        }

        private string BuildPrompt()
        {
            // If custom prompt is provided, use it
            if (!string.IsNullOrWhiteSpace(_promptTextBox.Text))
            {
                return _promptTextBox.Text.Trim();
            }

            // Build prompt from selections
            string style = _styleComboBox.SelectedItem?.ToString() ?? "Realistic Photo";
            string subject = _subjectComboBox.SelectedItem?.ToString() ?? "Landscape";

            // Create detailed prompt based on combinations
            string prompt = $"{subject} in {style} style, high quality, detailed, professional";

            // Add style-specific enhancements
            if (style.Contains("Realistic"))
                prompt += ", photorealistic, 8k, sharp focus";
            else if (style.Contains("Anime"))
                prompt += ", vibrant colors, smooth lines, studio quality";
            else if (style.Contains("Oil Painting"))
                prompt += ", textured canvas, classical art, masterpiece";
            else if (style.Contains("Watercolor"))
                prompt += ", soft colors, flowing, artistic, delicate";
            else if (style.Contains("3D Render"))
                prompt += ", octane render, cinematic lighting, ray tracing";
            else if (style.Contains("Cyberpunk"))
                prompt += ", neon lights, futuristic, dark atmosphere, sci-fi";
            else if (style.Contains("Ghibli"))
                prompt += ", studio ghibli style, beautiful scenery, anime aesthetic";
            else if (style.Contains("Pixel Art"))
                prompt += ", retro game style, 16-bit, pixel perfect";

            return prompt;
        }

        private void LoadImageToPreview(string imagePath)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmap.EndInit();
                bitmap.Freeze();

                _previewImage.Source = bitmap;
            }
            catch (Exception ex)
            {
                _context.Log($"Failed to load image preview: {ex.Message}", LogLevel.Error);
            }
        }
    }
}
