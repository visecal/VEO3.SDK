# VEO3 SDK - Complete User Guide

> **Hướng dẫn chi tiết** để tạo plugins cho VEO3 - Dành cho developers muốn mở rộng chức năng VEO3

---

## 📋 Mục lục

1. [Giới thiệu](#-giới-thiệu)
2. [Yêu cầu hệ thống](#-yêu-cầu-hệ-thống)
3. [Cài đặt SDK](#-cài-đặt-sdk)
4. [Tạo Plugin đầu tiên](#-tạo-plugin-đầu-tiên)
5. [API Reference](#-api-reference)
6. [Examples nâng cao](#-examples-nâng-cao)
7. [Build và Deploy](#-build-và-deploy)
8. [Troubleshooting](#-troubleshooting)
9. [AI Assistant Prompts](#-ai-assistant-prompts)

---

## 🌟 Giới thiệu

**VEO3 SDK** cho phép bạn tạo plugins mở rộng chức năng của VEO3

**Chỉ cần:**
- ✅ VEO3.SDK.dll (~20KB)
- ✅ .NET 8 SDK
- ✅ Code editor (VS Code, Visual Studio, Rider)

**Plugin có thể làm gì:**
- ✨ Thêm tabs mới vào VEO3 UI
- 🎨 Tạo custom UI
- 🤖 Sử dụng VEO3 services (Video Gen, AI Prompt, etc.)
- 💾 Lưu config riêng
- 🔔 Hiển thị notifications
- 📝 Log messages

---

## 💻 Yêu cầu hệ thống

### Bắt buộc
- **Operating System:** Windows 10/11 (64-bit)
- **.NET 8 SDK:** [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Editor:** Visual Studio 2022, VS Code, hoặc Rider

### Kiểm tra cài đặt
```bash
# Check .NET version
dotnet --version
# Expected: 8.0.x hoặc cao hơn
```

### Optional (Recommended)
- **Visual Studio 2022** (Community/Pro/Enterprise)
  - Workload: .NET desktop development
  - WPF designer support
- **Git** (for version control)

---

## 📦 Cài đặt SDK

### Option 1: Download từ GitHub Releases

1. **Download SDK:**
   - Truy cập: https://github.com/visecal/subphim/releases
   - Download `VEO3.SDK.dll` từ latest release

2. **Tạo folder structure:**
   ```
   MyPluginProject/
   ├── sdk/
   │   └── VEO3.SDK.dll          ← Place SDK here
   ├── MyPlugin/
   │   ├── MyPlugin.cs
   │   └── MyPlugin.csproj
   ```

## 🚀 Tạo Plugin đầu tiên

### Step 1: Tạo Project

```bash
# Create new class library project
dotnet new classlib -n MyFirstPlugin -f net8.0-windows
cd MyFirstPlugin
```

### Step 2: Configure Project File

Edit `MyFirstPlugin.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0-windows</TargetFramework>
    <UseWPF>true</UseWPF>
    <OutputType>Library</OutputType>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <!-- Reference to VEO3.SDK -->
    <Reference Include="VEO3.SDK">
      <HintPath>..\sdk\VEO3.SDK.dll</HintPath>
      <Private>false</Private>
    </Reference>
  </ItemGroup>
</Project>
```

**⚠️ Important:** `<Private>false</Private>` prevents SDK from being copied to output.

### Step 3: Tạo Plugin Class

Create `MyFirstPlugin.cs`:

```csharp
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VEO3.SDK;

namespace MyFirstPlugin
{
    public class MyFirstPlugin : PluginBase
    {
        // Plugin metadata
        public override string Name => "My First Plugin";
        public override string Version => "1.0.0";
        public override string Description => "A simple example plugin";
        public override string Author => "Your Name";
        public override string Icon => "🎉";

        // Called when plugin is loaded
        protected override async Task OnInitialize()
        {
            Log("Plugin initialized successfully!", LogLevel.Info);
            ShowNotification("Hello from My First Plugin!", NotificationType.Success);

            // You can access services here
            // Example: await Context.Veo3Service.TestConnection();
        }

        // Create UI for the plugin tab
        public override UserControl CreateUI()
        {
            return new MyFirstPluginControl(Context);
        }

        // Called when plugin is unloaded
        protected override async Task OnShutdown()
        {
            Log("Plugin shutting down...", LogLevel.Info);
        }
    }

    // Plugin UI
    public class MyFirstPluginControl : UserControl
    {
        private readonly IPluginContext _context;
        private TextBlock _statusText;
        private Button _testButton;

        public MyFirstPluginControl(IPluginContext context)
        {
            _context = context;
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Create simple UI
            var grid = new Grid
            {
                Margin = new Thickness(20)
            };

            // Define rows
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Title
            var title = new TextBlock
            {
                Text = "🎉 My First Plugin",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(title, 0);
            grid.Children.Add(title);

            // Status text
            _statusText = new TextBlock
            {
                Text = "Ready to test!",
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(_statusText, 2);
            grid.Children.Add(_statusText);

            // Test button
            _testButton = new Button
            {
                Content = "Test Plugin",
                Width = 150,
                Height = 40,
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 0)
            };
            _testButton.Click += TestButton_Click;
            Grid.SetRow(_testButton, 2);
            grid.Children.Add(_testButton);

            // Set content
            Content = grid;
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            _statusText.Text = $"Button clicked at {DateTime.Now:HH:mm:ss}";
            _context.Log("Test button clicked!", LogLevel.Info);
            _context.ShowNotification("Button clicked!", NotificationType.Success);
        }
    }
}
```

### Step 4: Build Plugin

```bash
dotnet build -c Release
```

**Output:** `bin/Release/net8.0-windows/MyFirstPlugin.dll`

### Step 5: Deploy to VEO3

```bash
# Copy plugin DLL to VEO3 Plugins folder
copy bin\Release\net8.0-windows\MyFirstPlugin.dll "C:\Path\To\VEO3\Plugins\"
```

### Step 6: Test Plugin

1. Launch VEO3 application
2. Check logs for plugin loading:
   ```
   [Info] Loaded plugin: My First Plugin v1.0.0 by Your Name
   ```
3. Look for new tab "🎉 My First Plugin"
4. Click "Test Plugin" button

**🎉 Congratulations!** Bạn đã tạo plugin đầu tiên!

---

## 📚 API Reference

### IVeo3Plugin Interface

**Main interface** mà tất cả plugins phải implement.

```csharp
public interface IVeo3Plugin
{
    // Metadata
    string Name { get; }        // Plugin name (displayed in tab)
    string Version { get; }     // Semantic version (e.g., "1.0.0")
    string Description { get; } // Short description
    string Author { get; }      // Your name/organization
    string Icon { get; }        // Emoji or icon (e.g., "🔥")

    // Lifecycle methods
    Task Initialize(IPluginContext context);
    UserControl CreateUI();
    Task Shutdown();
}
```

**Usage với PluginBase:**
```csharp
public class MyPlugin : PluginBase
{
    // Override properties
    public override string Name => "My Plugin";
    public override string Version => "1.0.0";

    // Override lifecycle methods
    protected override async Task OnInitialize()
    {
        // Your init code
    }

    protected override async Task OnShutdown()
    {
        // Your cleanup code
    }
}
```

---

### IPluginContext Interface

**Provides access** to VEO3 services and utilities.

```csharp
public interface IPluginContext
{
    // Services
    IVeo3Service Veo3Service { get; }           // Video generation
    IAIPromptService AIPromptService { get; }   // AI prompts
    HttpClient HttpClient { get; }              // HTTP client

    // Configuration
    string GetConfig(string key);
    void SetConfig(string key, string value);

    // UI Feedback
    void ShowNotification(string message, NotificationType type);
    void Log(string message, LogLevel level);

    // Utilities
    string GetOutputDirectory();
    void SubscribeToEvent(string eventName, Action<object> handler);
}
```

**Examples:**

#### Configuration
```csharp
// Save config
Context.SetConfig("my_api_key", "sk-1234567890");
Context.SetConfig("theme", "dark");

// Load config
var apiKey = Context.GetConfig("my_api_key");
var theme = Context.GetConfig("theme") ?? "light"; // Default value
```

#### Notifications
```csharp
// Success notification
Context.ShowNotification("Operation completed!", NotificationType.Success);

// Error notification
Context.ShowNotification("Failed to connect!", NotificationType.Error);

// Warning
Context.ShowNotification("API quota low!", NotificationType.Warning);

// Info
Context.ShowNotification("Processing...", NotificationType.Info);
```

#### Logging
```csharp
// Different log levels
Context.Log("Plugin started", LogLevel.Info);
Context.Log("Processing file...", LogLevel.Debug);
Context.Log("API rate limit approaching", LogLevel.Warning);
Context.Log("Connection failed!", LogLevel.Error);
```

#### Output Directory
```csharp
// Get output directory (for saving files)
var outputDir = Context.GetOutputDirectory();
// Returns: "C:\Path\To\VEO3\VEO3_Output"

// Save file
var filePath = Path.Combine(outputDir, "my_output.txt");
File.WriteAllText(filePath, "Hello World!");
```

---

### IVeo3Service Interface

**Video generation service** - access to VEO3 API.

```csharp
public interface IVeo3Service
{
    void Initialize(string apiKey, string cookieToken);
    Task<bool> TestConnection();
    Task<string> GenerateVideo(string prompt, string characterData, int durationSeconds = 8);
    Task<string> DownloadVideo(string videoUrl, string savePath);
    Task<string> GenerateImage(string prompt, string aspectRatio = "16:9", string negativePrompt = null);
}
```

**Example: Generate video**
```csharp
protected override async Task OnInitialize()
{
    var veo3 = Context.Veo3Service;

    // Test connection
    var isConnected = await veo3.TestConnection();
    if (!isConnected)
    {
        ShowNotification("VEO3 service not available", NotificationType.Warning);
        return;
    }

    // Generate video
    try
    {
        var characterData = "{}"; // Empty or provide character consistency JSON
        var videoUrl = await veo3.GenerateVideo(
            prompt: "A cat playing piano in a sunny room",
            characterData: characterData,
            durationSeconds: 8
        );

        Log($"Video generated: {videoUrl}", LogLevel.Info);

        // Download video
        var outputDir = Context.GetOutputDirectory();
        var savePath = Path.Combine(outputDir, "cat_piano.mp4");
        await veo3.DownloadVideo(videoUrl, savePath);

        ShowNotification($"Video saved to {savePath}", NotificationType.Success);
    }
    catch (Exception ex)
    {
        Log($"Error: {ex.Message}", LogLevel.Error);
        ShowNotification("Failed to generate video", NotificationType.Error);
    }
}
```

**Example: Generate image**
```csharp
private async void GenerateImageButton_Click(object sender, RoutedEventArgs e)
{
    var prompt = _promptTextBox.Text;
    var aspectRatio = "16:9"; // or "9:16", "1:1", "4:3", "21:9"

    try
    {
        var imagePath = await _context.Veo3Service.GenerateImage(
            prompt: prompt,
            aspectRatio: aspectRatio,
            negativePrompt: "blurry, low quality"
        );

        // Display image
        var bitmap = new BitmapImage(new Uri(imagePath));
        _imageControl.Source = bitmap;

        _context.ShowNotification("Image generated!", NotificationType.Success);
    }
    catch (Exception ex)
    {
        _context.Log($"Image generation failed: {ex.Message}", LogLevel.Error);
    }
}
```

---

### IAIPromptService Interface

**AI prompt generation** - enhance prompts with AI.

```csharp
public interface IAIPromptService
{
    void Initialize(string aiModel, string apiKey, string modelName = null);
    Task<string> GenerateCharacterPrompt(string description);
    Task<string> AnalyzeYoutubeVideo(string youtubeUrl);
    Task<string> AnalyzeImage(string imagePath);
    Task<string> AnalyzeImageForCharacter(string imagePath);
}
```

**Example: Generate character prompt**
```csharp
private async void GenerateCharacter_Click(object sender, RoutedEventArgs e)
{
    var description = "A young wizard with blue eyes and long silver hair";

    try
    {
        var characterJson = await _context.AIPromptService.GenerateCharacterPrompt(description);

        // characterJson contains detailed character description in JSON format
        _characterTextBox.Text = characterJson;

        _context.ShowNotification("Character generated!", NotificationType.Success);
    }
    catch (Exception ex)
    {
        _context.Log($"Character generation failed: {ex.Message}", LogLevel.Error);
    }
}
```

**Example: Analyze YouTube video**
```csharp
private async void AnalyzeYoutube_Click(object sender, RoutedEventArgs e)
{
    var youtubeUrl = _urlTextBox.Text;

    try
    {
        var analysis = await _context.AIPromptService.AnalyzeYoutubeVideo(youtubeUrl);

        // analysis contains video summary, key scenes, suggested prompts
        _analysisTextBox.Text = analysis;

        _context.ShowNotification("Analysis complete!", NotificationType.Success);
    }
    catch (Exception ex)
    {
        _context.Log($"Analysis failed: {ex.Message}", LogLevel.Error);
    }
}
```

---

### PluginBase Abstract Class

**Helper base class** with common functionality.

```csharp
public abstract class PluginBase : IVeo3Plugin
{
    // Properties you must override
    public abstract string Name { get; }
    public abstract string Version { get; }

    // Optional properties (have defaults)
    public virtual string Description => "No description provided";
    public virtual string Author => "Unknown";
    public virtual string Icon => "📦";

    // Context available after Initialize()
    protected IPluginContext Context { get; private set; }

    // Helper methods
    protected void ShowNotification(string message, NotificationType type = NotificationType.Info);
    protected void Log(string message, LogLevel level = LogLevel.Info);

    // Virtual methods to override
    protected virtual Task OnInitialize() => Task.CompletedTask;
    protected virtual Task OnShutdown() => Task.CompletedTask;

    // Abstract method you must implement
    public abstract UserControl CreateUI();
}
```

**Benefits:**
- ✅ Less boilerplate code
- ✅ Helper methods included
- ✅ Proper lifecycle management
- ✅ Default values for optional properties

---

## 🎨 Examples nâng cao

### Example 1: Plugin với API Integration

```csharp
public class WeatherPlugin : PluginBase
{
    public override string Name => "Weather Info";
    public override string Version => "1.0.0";
    public override string Icon => "🌤️";

    private const string API_KEY_CONFIG = "weather_api_key";

    protected override async Task OnInitialize()
    {
        // Check if API key is configured
        var apiKey = Context.GetConfig(API_KEY_CONFIG);
        if (string.IsNullOrEmpty(apiKey))
        {
            ShowNotification("Please configure API key in settings", NotificationType.Warning);
        }
    }

    public override UserControl CreateUI()
    {
        return new WeatherControl(Context);
    }
}

public class WeatherControl : UserControl
{
    private readonly IPluginContext _context;
    private TextBox _cityTextBox;
    private TextBlock _weatherResult;
    private TextBox _apiKeyTextBox;

    public WeatherControl(IPluginContext context)
    {
        _context = context;
        InitializeUI();
        LoadConfig();
    }

    private void InitializeUI()
    {
        var stack = new StackPanel { Margin = new Thickness(20) };

        // API Key section
        stack.Children.Add(new TextBlock { Text = "OpenWeather API Key:", FontWeight = FontWeights.Bold });
        _apiKeyTextBox = new TextBox { Margin = new Thickness(0, 5, 0, 10) };
        stack.Children.Add(_apiKeyTextBox);

        var saveKeyButton = new Button { Content = "Save API Key", Width = 120, HorizontalAlignment = HorizontalAlignment.Left };
        saveKeyButton.Click += SaveApiKey_Click;
        stack.Children.Add(saveKeyButton);

        // Weather query section
        stack.Children.Add(new TextBlock { Text = "City:", FontWeight = FontWeights.Bold, Margin = new Thickness(0, 20, 0, 0) });
        _cityTextBox = new TextBox { Margin = new Thickness(0, 5, 0, 10) };
        stack.Children.Add(_cityTextBox);

        var getWeatherButton = new Button { Content = "Get Weather", Width = 120, HorizontalAlignment = HorizontalAlignment.Left };
        getWeatherButton.Click += GetWeather_Click;
        stack.Children.Add(getWeatherButton);

        // Result
        _weatherResult = new TextBlock { Margin = new Thickness(0, 20, 0, 0), TextWrapping = TextWrapping.Wrap };
        stack.Children.Add(_weatherResult);

        Content = stack;
    }

    private void LoadConfig()
    {
        _apiKeyTextBox.Text = _context.GetConfig("weather_api_key") ?? "";
    }

    private void SaveApiKey_Click(object sender, RoutedEventArgs e)
    {
        _context.SetConfig("weather_api_key", _apiKeyTextBox.Text);
        _context.ShowNotification("API Key saved!", NotificationType.Success);
    }

    private async void GetWeather_Click(object sender, RoutedEventArgs e)
    {
        var apiKey = _context.GetConfig("weather_api_key");
        if (string.IsNullOrEmpty(apiKey))
        {
            _context.ShowNotification("Please configure API key first!", NotificationType.Warning);
            return;
        }

        var city = _cityTextBox.Text;
        if (string.IsNullOrEmpty(city))
        {
            _context.ShowNotification("Please enter a city name!", NotificationType.Warning);
            return;
        }

        try
        {
            _weatherResult.Text = "Loading...";

            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
            var response = await _context.HttpClient.GetStringAsync(url);

            // Parse JSON (you can use Newtonsoft.Json if referenced)
            dynamic weather = Newtonsoft.Json.JsonConvert.DeserializeObject(response);

            _weatherResult.Text = $"🌡️ Temperature: {weather.main.temp}°C\n" +
                                 $"💨 Wind: {weather.wind.speed} m/s\n" +
                                 $"☁️ Conditions: {weather.weather[0].description}";

            _context.Log($"Weather fetched for {city}", LogLevel.Info);
        }
        catch (Exception ex)
        {
            _weatherResult.Text = $"Error: {ex.Message}";
            _context.Log($"Weather fetch failed: {ex.Message}", LogLevel.Error);
        }
    }
}
```

### Example 2: Plugin với Custom Styles

```csharp
public class StyledPluginControl : UserControl
{
    public StyledPluginControl(IPluginContext context)
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        // Define resources
        var resources = new ResourceDictionary();

        // Button style
        var buttonStyle = new Style(typeof(Button));
        buttonStyle.Setters.Add(new Setter(BackgroundProperty, new SolidColorBrush(Color.FromRgb(41, 128, 185))));
        buttonStyle.Setters.Add(new Setter(ForegroundProperty, Brushes.White));
        buttonStyle.Setters.Add(new Setter(FontSizeProperty, 14.0));
        buttonStyle.Setters.Add(new Setter(PaddingProperty, new Thickness(15, 8, 15, 8)));
        buttonStyle.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(0)));
        buttonStyle.Setters.Add(new Setter(CursorProperty, Cursors.Hand));

        resources.Add("ModernButton", buttonStyle);

        // TextBox style
        var textBoxStyle = new Style(typeof(TextBox));
        textBoxStyle.Setters.Add(new Setter(FontSizeProperty, 14.0));
        textBoxStyle.Setters.Add(new Setter(PaddingProperty, new Thickness(8)));
        textBoxStyle.Setters.Add(new Setter(BorderBrushProperty, new SolidColorBrush(Color.FromRgb(200, 200, 200))));

        resources.Add("ModernTextBox", textBoxStyle);

        Resources = resources;

        // Create UI using styles
        var grid = new Grid { Margin = new Thickness(20) };

        var button = new Button
        {
            Content = "Styled Button",
            Style = (Style)Resources["ModernButton"],
            Width = 150,
            Height = 40
        };

        grid.Children.Add(button);
        Content = grid;
    }
}
```

### Example 3: Plugin với Data Binding

```csharp
public class DataBoundPlugin : PluginBase
{
    public override string Name => "Data Bound Example";
    public override string Version => "1.0.0";

    public override UserControl CreateUI()
    {
        return new DataBoundControl(Context);
    }
}

public class DataBoundControl : UserControl
{
    private ObservableCollection<TaskItem> _tasks;
    private ListBox _taskListBox;
    private TextBox _newTaskTextBox;

    public DataBoundControl(IPluginContext context)
    {
        _tasks = new ObservableCollection<TaskItem>();
        InitializeUI();
        LoadTasks(context);
    }

    private void InitializeUI()
    {
        var grid = new Grid { Margin = new Thickness(20) };
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        // Task list
        _taskListBox = new ListBox
        {
            ItemsSource = _tasks,
            DisplayMemberPath = "Name"
        };
        Grid.SetRow(_taskListBox, 0);
        grid.Children.Add(_taskListBox);

        // Add task panel
        var addPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 10, 0, 0) };

        _newTaskTextBox = new TextBox { Width = 200, Margin = new Thickness(0, 0, 10, 0) };
        addPanel.Children.Add(_newTaskTextBox);

        var addButton = new Button { Content = "Add Task", Width = 80 };
        addButton.Click += AddTask_Click;
        addPanel.Children.Add(addButton);

        Grid.SetRow(addPanel, 1);
        grid.Children.Add(addPanel);

        Content = grid;
    }

    private void LoadTasks(IPluginContext context)
    {
        // Load saved tasks from config
        var tasksJson = context.GetConfig("tasks");
        if (!string.IsNullOrEmpty(tasksJson))
        {
            var tasks = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TaskItem>>(tasksJson);
            foreach (var task in tasks)
            {
                _tasks.Add(task);
            }
        }
    }

    private void AddTask_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(_newTaskTextBox.Text))
        {
            _tasks.Add(new TaskItem { Name = _newTaskTextBox.Text });
            _newTaskTextBox.Clear();

            // Save tasks
            var tasksJson = Newtonsoft.Json.JsonConvert.SerializeObject(_tasks);
            // Note: You'd need to pass context through constructor
        }
    }
}

public class TaskItem
{
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
}
```

---

## 🔨 Build và Deploy

### Build Scripts

#### Windows (Build-Plugin.bat)
```batch
@echo off
setlocal

if "%1"=="" (
    echo Usage: Build-Plugin.bat ^<PluginName^>
    exit /b 1
)

set PLUGIN_NAME=%1

echo Building %PLUGIN_NAME%...
dotnet build "%PLUGIN_NAME%\%PLUGIN_NAME%.csproj" -c Release

if %ERRORLEVEL% NEQ 0 (
    echo Build failed!
    exit /b 1
)

set PLUGIN_DLL=%PLUGIN_NAME%\bin\Release\net8.0-windows\%PLUGIN_NAME%.dll
set OUTPUT_DIR=BuildOutput\Plugins

if not exist "%OUTPUT_DIR%" mkdir "%OUTPUT_DIR%"
copy "%PLUGIN_DLL%" "%OUTPUT_DIR%\"

echo.
echo ✅ Plugin built successfully!
echo Output: %OUTPUT_DIR%\%PLUGIN_NAME%.dll
echo.
echo Copy this file to your VEO3 installation Plugins folder:
echo copy "%OUTPUT_DIR%\%PLUGIN_NAME%.dll" "C:\Path\To\VEO3\bin\Debug\net8.0-windows\Plugins\"
```

#### Linux/Mac (Build-Plugin.sh)
```bash
#!/bin/bash

if [ -z "$1" ]; then
    echo "Usage: ./Build-Plugin.sh <PluginName>"
    exit 1
fi

PLUGIN_NAME=$1

echo "Building $PLUGIN_NAME..."
dotnet build "$PLUGIN_NAME/$PLUGIN_NAME.csproj" -c Release

if [ $? -ne 0 ]; then
    echo "Build failed!"
    exit 1
fi

PLUGIN_DLL="$PLUGIN_NAME/bin/Release/net8.0-windows/$PLUGIN_NAME.dll"
OUTPUT_DIR="BuildOutput/Plugins"

mkdir -p "$OUTPUT_DIR"
cp "$PLUGIN_DLL" "$OUTPUT_DIR/"

echo ""
echo "✅ Plugin built successfully!"
echo "Output: $OUTPUT_DIR/$PLUGIN_NAME.dll"
echo ""
echo "Copy this file to your VEO3 installation Plugins folder"
```

### Deploy to VEO3

**Manual deployment:**
```bash
# Copy plugin DLL to VEO3 Plugins folder
copy MyPlugin\bin\Release\net8.0-windows\MyPlugin.dll ^
     "C:\Path\To\VEO3\bin\Debug\net8.0-windows\Plugins\"
```

**Automatic deployment (add to .csproj):**
```xml
<Target Name="PostBuild" AfterTargets="PostBuildEvent">
  <Copy SourceFiles="$(TargetPath)"
        DestinationFolder="C:\Path\To\VEO3\bin\Debug\net8.0-windows\Plugins\" />
</Target>
```

---

## 🐛 Troubleshooting

### Plugin không load

**Symptoms:**
- Plugin DLL trong Plugins folder
- Không có tab xuất hiện
- Log: "Found 0 plugin types"

**Solutions:**

1. **Check VEO3.SDK.dll tồn tại:**
   ```bash
   # SDK phải ở cùng folder với AIOLauncher.exe
   dir "C:\Path\To\VEO3\bin\Debug\net8.0-windows\VEO3.SDK.dll"
   ```

2. **Verify Private=false trong plugin .csproj:**
   ```xml
   <Reference Include="VEO3.SDK">
     <HintPath>..\sdk\VEO3.SDK.dll</HintPath>
     <Private>false</Private>  <!-- MUST be false! -->
   </Reference>
   ```

3. **Check plugin inherits IVeo3Plugin:**
   ```csharp
   public class MyPlugin : PluginBase  // ✅ Correct
   public class MyPlugin : IVeo3Plugin // ✅ Also correct
   public class MyPlugin               // ❌ Wrong!
   ```

4. **Rebuild clean:**
   ```bash
   # Delete build cache
   rmdir /s /q bin
   rmdir /s /q obj

   # Rebuild
   dotnet build -c Release
   ```

### Assembly binding error

**Error:**
```
Could not load file or assembly 'VEO3.SDK, Version=1.0.0.0'
```

**Solutions:**

1. **Verify SDK version match:**
   - Check plugin references VEO3.SDK v1.0.0
   - Check VEO3 uses VEO3.SDK v1.0.0
   - Versions MUST match!

2. **Check SDK in VEO3 bin folder:**
   ```bash
   # SDK must be here
   C:\Path\To\VEO3\bin\Debug\net8.0-windows\VEO3.SDK.dll
   ```

### CreateUI() exception

**Error:**
```
Plugin loaded but UI creation failed
```

**Solutions:**

1. **Check constructor parameters:**
   ```csharp
   // ✅ Correct - receive IPluginContext
   public MyControl(IPluginContext context)

   // ❌ Wrong - receive plugin instance
   public MyControl(MyPlugin plugin)
   ```

2. **Check WPF elements properly initialized:**
   ```csharp
   public override UserControl CreateUI()
   {
       var control = new MyControl(Context);
       // Verify control.Content is set!
       return control;
   }
   ```

### Build errors

**Error: SDK not found**
```
The type or namespace 'VEO3' could not be found
```

**Solution:**
```xml
<!-- Check HintPath is correct -->
<Reference Include="VEO3.SDK">
  <HintPath>..\sdk\VEO3.SDK.dll</HintPath>  <!-- Verify path! -->
</Reference>
```

**Error: WPF not available**
```
The name 'UserControl' does not exist
```

**Solution:**
```xml
<!-- Add UseWPF in .csproj -->
<PropertyGroup>
  <UseWPF>true</UseWPF>
</PropertyGroup>
```

---

## 🤖 AI Assistant Prompts

Use these prompts để AI assistants (ChatGPT, Claude, etc.) hiểu và giúp bạn với VEO3 SDK:

### Prompt 1: Understand SDK Architecture

```
I'm working with VEO3 SDK - a plugin system for video generation application.

Here's the SDK structure:

**VEO3.SDK.dll** contains these interfaces:

1. IVeo3Plugin - main plugin interface with:
   - Properties: Name, Version, Description, Author, Icon
   - Methods: Initialize(IPluginContext), CreateUI(), Shutdown()

2. IPluginContext - provides access to:
   - Services: Veo3Service (video gen), AIPromptService (AI prompts), HttpClient
   - Config: GetConfig(key), SetConfig(key, value)
   - UI: ShowNotification(message, type), Log(message, level)
   - Utilities: GetOutputDirectory(), SubscribeToEvent()

3. IVeo3Service - video generation:
   - GenerateVideo(prompt, characterData, duration)
   - GenerateImage(prompt, aspectRatio, negativePrompt)
   - DownloadVideo(url, savePath)
   - TestConnection()

4. IAIPromptService - AI features:
   - GenerateCharacterPrompt(description)
   - AnalyzeYoutubeVideo(url)
   - AnalyzeImage(imagePath)

5. PluginBase - abstract helper class with:
   - Protected Context property
   - Helper methods: ShowNotification(), Log()
   - Virtual methods: OnInitialize(), OnShutdown()

Plugins are .NET 8 WPF class libraries that reference VEO3.SDK.dll (20KB, no dependencies).

Please help me create a plugin that [DESCRIBE YOUR PLUGIN IDEA].
```

### Prompt 2: Create New Plugin

```
Create a VEO3 plugin with these requirements:

**Plugin Info:**
- Name: [Your Plugin Name]
- Purpose: [What it does]
- Features: [List features]

**Technical Requirements:**
- Must inherit PluginBase
- Must create WPF UserControl UI
- Target framework: net8.0-windows
- Reference: VEO3.SDK.dll with Private=false

**Project Structure:**
- [PluginName].csproj - project file with WPF enabled
- [PluginName].cs - main plugin class + UI control

Please provide complete code for both files.
```

### Prompt 3: Debug Plugin Issue

```
I'm having issues with my VEO3 plugin:

**Problem:** [Describe problem]

**Current Code:**
```csharp
[Paste your plugin code]
```

**Error/Behavior:**
[Paste error message or describe unexpected behavior]

**VEO3 SDK Info:**
- IVeo3Plugin interface requires: Name, Version, Description, Author, Icon properties
- Must implement: Initialize(IPluginContext), CreateUI(), Shutdown()
- PluginBase provides: Context property, ShowNotification(), Log() helpers
- UI must be WPF UserControl

Please help me fix this issue.
```

### Prompt 4: Add Feature to Existing Plugin

```
I have a working VEO3 plugin and want to add a feature:

**Current Plugin:**
```csharp
[Paste current plugin code]
```

**Feature to Add:**
[Describe feature]

**Available Services (via Context):**
- Context.Veo3Service - video/image generation
- Context.AIPromptService - AI prompts and analysis
- Context.HttpClient - HTTP requests
- Context.GetConfig/SetConfig - persistent storage
- Context.ShowNotification/Log - user feedback

Please show me how to implement this feature.
```

### Prompt 5: Create UI Layout

```
Create a WPF UserControl UI for VEO3 plugin with this layout:

**Layout Requirements:**
[Describe UI layout - e.g., two columns, tabs, sections]

**Controls Needed:**
[List controls - TextBox, Button, ComboBox, etc.]

**Functionality:**
[Describe what each control does]

**Context:**
- Must receive IPluginContext in constructor
- Use _context for logging: _context.Log(message, LogLevel.Info)
- Use _context for notifications: _context.ShowNotification(msg, NotificationType.Success)

Please provide complete WPF UserControl code with proper layout and event handlers.
```

---

## 📖 Complete Example Templates

### Template 1: Simple Single-File Plugin

Save as `SimplePlugin.cs`:

```csharp
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VEO3.SDK;

namespace SimplePlugin
{
    // Main plugin class
    public class SimplePlugin : PluginBase
    {
        public override string Name => "Simple Plugin";
        public override string Version => "1.0.0";
        public override string Description => "A minimal example plugin";
        public override string Author => "Your Name";
        public override string Icon => "📦";

        protected override async Task OnInitialize()
        {
            Log("Simple plugin initialized!");
        }

        public override UserControl CreateUI()
        {
            return new SimplePluginControl(Context);
        }
    }

    // Plugin UI
    public class SimplePluginControl : UserControl
    {
        private readonly IPluginContext _context;

        public SimplePluginControl(IPluginContext context)
        {
            _context = context;

            var button = new Button
            {
                Content = "Click Me!",
                Width = 150,
                Height = 40,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            button.Click += (s, e) =>
            {
                _context.ShowNotification("Button clicked!", NotificationType.Success);
            };

            Content = button;
        }
    }
}
```

Project file `SimplePlugin.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0-windows</TargetFramework>
    <UseWPF>true</UseWPF>
    <OutputType>Library</OutputType>
  </PropertyGroup>

  <ItemGroup>
    <Reference Include="VEO3.SDK">
      <HintPath>..\sdk\VEO3.SDK.dll</HintPath>
      <Private>false</Private>
    </Reference>
  </ItemGroup>
</Project>
```

Build:
```bash
dotnet build SimplePlugin.csproj -c Release
```

---

## 🎓 Best Practices

### 1. Error Handling
```csharp
protected override async Task OnInitialize()
{
    try
    {
        // Your initialization code
        await SomeAsyncOperation();
        Log("Initialized successfully", LogLevel.Info);
    }
    catch (Exception ex)
    {
        Log($"Initialization failed: {ex.Message}", LogLevel.Error);
        ShowNotification("Plugin initialization failed", NotificationType.Error);
    }
}
```

### 2. Async/Await Best Practices
```csharp
// ✅ Good - properly awaited
private async void Button_Click(object sender, RoutedEventArgs e)
{
    try
    {
        await DoSomethingAsync();
    }
    catch (Exception ex)
    {
        _context.Log($"Error: {ex.Message}", LogLevel.Error);
    }
}

// ❌ Bad - fire-and-forget
private void Button_Click(object sender, RoutedEventArgs e)
{
    DoSomethingAsync(); // Missing await!
}
```

### 3. Resource Cleanup
```csharp
public class MyPlugin : PluginBase
{
    private HttpClient _customHttpClient;
    private Timer _timer;

    protected override async Task OnInitialize()
    {
        _customHttpClient = new HttpClient();
        _timer = new Timer(OnTimerElapsed, null, 0, 1000);
    }

    protected override async Task OnShutdown()
    {
        _timer?.Dispose();
        _customHttpClient?.Dispose();
        Log("Resources cleaned up", LogLevel.Info);
    }
}
```

### 4. Configuration Management
```csharp
private void SaveSettings()
{
    // Save multiple settings
    Context.SetConfig($"{Name}_api_key", _apiKeyTextBox.Text);
    Context.SetConfig($"{Name}_theme", _themeComboBox.SelectedValue.ToString());
    Context.SetConfig($"{Name}_auto_start", _autoStartCheckBox.IsChecked.ToString());
}

private void LoadSettings()
{
    // Load with defaults
    _apiKeyTextBox.Text = Context.GetConfig($"{Name}_api_key") ?? "";
    _themeComboBox.SelectedValue = Context.GetConfig($"{Name}_theme") ?? "Light";
    _autoStartCheckBox.IsChecked = bool.Parse(Context.GetConfig($"{Name}_auto_start") ?? "false");
}
```

### 5. Logging Strategy
```csharp
// Log levels usage:
Log("Plugin started", LogLevel.Info);              // Normal operations
Log("Processing file: {filename}", LogLevel.Debug); // Detailed info
Log("API quota at 80%", LogLevel.Warning);         // Warnings
Log("Connection failed", LogLevel.Error);          // Errors

// Don't log sensitive data!
// ❌ Log($"API Key: {apiKey}", LogLevel.Debug);
// ✅ Log("API Key configured", LogLevel.Debug);
```

---

## 📞 Support & Resources

### Documentation
- **SDK README:** `VEO3.SDK/README.md`
- **This Guide:** `VEO3_SDK_USER_GUIDE.md`
- **API Docs:** XML documentation trong VEO3.SDK.dll

### Example Projects
- `VEO3_SDK_Examples/ImageGenV2Plugin/` - Advanced plugin với đầy đủ UI
- `VEO3_SDK_Examples/ExamplePlugin/` - Simple example

### Community
- **GitHub Issues:** Report bugs, request features
- **GitHub Discussions:** Q&A, share plugins
- **Pull Requests:** Contribute improvements

### Getting Help

**When reporting issues, include:**
1. VEO3 version
2. SDK version
3. Plugin code (simplified)
4. Error messages / logs
5. Steps to reproduce

**Good issue report:**
```
**VEO3 Version:** 2.6.2.2
**SDK Version:** 1.0.0
**OS:** Windows 11

**Problem:** Plugin loads but CreateUI() throws NullReferenceException

**Code:**
[Minimal code that reproduces issue]

**Error:**
[Full error message and stack trace]

**Steps:**
1. Build plugin with dotnet build
2. Copy DLL to Plugins folder
3. Launch VEO3
4. Error appears in logs
```

---

## ✅ Checklist: Plugin Development

### Before Starting
- [ ] .NET 8 SDK installed
- [ ] VEO3.SDK.dll downloaded
- [ ] Code editor setup (VS Code/Visual Studio)
- [ ] VEO3 application available for testing

### During Development
- [ ] Plugin class inherits PluginBase or implements IVeo3Plugin
- [ ] All required properties implemented (Name, Version, etc.)
- [ ] CreateUI() returns valid UserControl
- [ ] Error handling in async methods
- [ ] Logging for important operations
- [ ] Configuration saved/loaded properly

### Before Building
- [ ] .csproj has UseWPF=true
- [ ] SDK reference has Private=false
- [ ] TargetFramework is net8.0-windows
- [ ] No hardcoded paths or credentials

### Testing
- [ ] Plugin builds without errors
- [ ] DLL copied to VEO3 Plugins folder
- [ ] Plugin appears as tab in VEO3
- [ ] All features work as expected
- [ ] No errors in VEO3 logs
- [ ] Notifications display correctly

### Deployment
- [ ] Build in Release configuration
- [ ] Test on clean machine if possible
- [ ] Documentation for users
- [ ] Version number updated
- [ ] CHANGELOG updated

---

## 🎉 Kết luận

Bạn đã có tất cả kiến thức cần thiết để tạo plugins cho VEO3!

**Next Steps:**
1. Create your first plugin với template Simple
2. Explore example plugins trong VEO3_SDK_Examples/
3. Experiment với VEO3 services (video gen, AI prompts)
4. Share your plugins với community!

**Happy Coding! 🚀**

---

*Last Updated: 2025-01-21*
*SDK Version: 1.0.0*
