# 🎨 ImageGenV2Plugin

Advanced Image Generation Plugin cho VEO3 với nhiều preset và style.

## ✨ Features

### 🎭 10 Art Styles
- **Realistic Photo** - Ảnh chân thực, photorealistic
- **Anime Style** - Phong cách anime Nhật Bản
- **Oil Painting** - Tranh sơn dầu cổ điển
- **Watercolor** - Màu nước, mềm mại
- **3D Render** - Render 3D chuyên nghiệp
- **Cyberpunk** - Phong cách cyberpunk, neon
- **Studio Ghibli** - Phong cách Ghibli đẹp mắt
- **Pixel Art** - Pixel art retro 16-bit
- **Comic Book** - Phong cách truyện tranh
- **Minimalist** - Tối giản, đơn giản

### 📸 10 Subjects
- Portrait (Chân dung)
- Landscape (Phong cảnh)
- Animal (Động vật)
- Food (Đồ ăn)
- Architecture (Kiến trúc)
- Vehicle (Xe cộ)
- Fantasy Character (Nhân vật fantasy)
- Sci-Fi Scene (Cảnh khoa học viễn tưởng)
- Nature (Thiên nhiên)
- Abstract (Trừu tượng)

### ⚡ 6 Quick Presets
Click một cái để tạo ngay!

- **🌅 Sunset** - Hoàng hôn đẹp
- **😺 Cute Cat** - Mèo dễ thương anime
- **🏰 Fantasy Castle** - Lâu đài fantasy
- **🤖 Cyberpunk** - Cảnh cyberpunk futuristic
- **🌸 Watercolor** - Thiên nhiên màu nước
- **🎮 Pixel Game** - Nhân vật pixel art

### 📐 Aspect Ratios
- 1:1 - Vuông (Instagram)
- 16:9 - Ngang (YouTube thumbnail)
- 9:16 - Dọc (TikTok, Stories)
- 4:3 - Classic
- 3:4 - Portrait

### 🎨 Advanced Options
- **Custom Prompt** - Tự viết prompt của bạn
- **Negative Prompt** - Loại bỏ các yếu tố không mong muốn
- **Live Preview** - Xem ảnh ngay trong plugin
- **Auto Save** - Tự động lưu vào `VEO3_Output/`

## 🖼️ UI Layout

```
┌─────────────────────────────────────────────────┐
│  🎨 Image Generation V2                         │
├──────────────────┬──────────────────────────────┤
│ Controls Panel   │  Preview Panel               │
│                  │                              │
│ ⚡ Quick Presets │  🖼️ Preview                   │
│ [6 buttons]      │  ┌────────────────────────┐  │
│                  │  │                        │  │
│ 🎭 Art Style     │  │   Generated Image      │  │
│ [Dropdown]       │  │   Appears Here         │  │
│                  │  │                        │  │
│ 📸 Subject       │  └────────────────────────┘  │
│ [Dropdown]       │                              │
│                  │  💡 Tips & Info              │
│ 📐 Aspect Ratio  │                              │
│ [Dropdown]       │                              │
│                  │                              │
│ ✏️ Custom Prompt │                              │
│ [TextBox]        │                              │
│                  │                              │
│ 🚫 Negative      │                              │
│ [TextBox]        │                              │
│                  │                              │
│ [Generate Button]│                              │
│ [Progress Bar]   │                              │
│ Status Text      │                              │
└──────────────────┴──────────────────────────────┘
```

## 🚀 Cách sử dụng

### 1. Build Plugin

**Windows:**
```bash
Build-Plugin.bat ImageGenV2Plugin
```

**Mac/Linux:**
```bash
./Build-Plugin.sh ImageGenV2Plugin
```

### 2. Chạy VEO3

Plugin sẽ tự động load và xuất hiện tab "🎨 Image Gen V2"

### 3. Tạo hình ảnh

#### Cách 1: Quick Preset (Nhanh nhất!)
1. Click vào một trong 6 preset buttons
2. Click "🎨 Generate Image"
3. Đợi vài giây
4. Xem ảnh trong Preview panel!

#### Cách 2: Custom Combo
1. Chọn **Art Style** (vd: Anime Style)
2. Chọn **Subject** (vd: Animal)
3. Chọn **Aspect Ratio** (vd: 1:1)
4. Click "🎨 Generate Image"

#### Cách 3: Full Control
1. Điền **Custom Prompt** của bạn
2. Điền **Negative Prompt** (loại bỏ gì)
3. Chọn **Aspect Ratio**
4. Click "🎨 Generate Image"

## 💡 Tips & Tricks

### Combine Styles
Thử kết hợp khác nhau:
- **Anime + Landscape** = Cảnh anime đẹp như Ghibli
- **Cyberpunk + Vehicle** = Xe futuristic neon
- **Watercolor + Nature** = Thiên nhiên mềm mại
- **Pixel Art + Fantasy** = Game character retro

### Good Prompts
- Càng chi tiết càng tốt: "A red dragon flying over mountains at sunset"
- Thêm chất lượng: "high quality, detailed, professional"
- Thêm lighting: "cinematic lighting, golden hour, soft shadows"

### Negative Prompts
Loại bỏ:
- `blurry, low quality` - Ảnh mờ, chất lượng thấp
- `distorted, ugly` - Biến dạng, xấu
- `text, watermark` - Chữ, watermark
- `extra limbs, bad anatomy` - Dư tay chân, giải phẫu sai

## 📂 Output

Ảnh được lưu tự động vào:
```
subphimv1/bin/Debug/net8.0-windows/VEO3_Output/
```

File name format: `image_20250121_143052.png`

## 🎓 Code Demo

Plugin này demo các khả năng của VEO3.SDK:

### 1. Inherit từ PluginBase
```csharp
public class ImageGenV2Plugin : PluginBase
{
    public override string Name => "Image Gen V2";
    public override string Icon => "🎨";
    // ...
}
```

### 2. Sử dụng VEO3 Service
```csharp
string imagePath = await _plugin.Context.Veo3Service.GenerateImage(
    prompt,
    aspectRatio,
    negativePrompt
);
```

### 3. Helper Methods
```csharp
_plugin.ShowNotification("Success!", NotificationType.Success);
_plugin.Log("Generated image", LogLevel.Info);
```

### 4. WPF UI
- Grid layout với 2 columns
- ComboBox, TextBox, Button, ProgressBar
- Image preview với BitmapImage
- Event handlers

## 🔧 Customization

### Thêm Preset Mới

Trong `CreatePresetsPanel()`:
```csharp
new { Name = "🌟 Your Preset", Style = "Art Style", Subject = "Subject" }
```

### Thêm Art Style Mới

Trong `_artStyles`:
```csharp
"Your New Style"
```

Trong `BuildPrompt()`:
```csharp
else if (style.Contains("Your New Style"))
    prompt += ", your custom enhancements";
```

### Thay đổi UI Colors

```csharp
Background = new SolidColorBrush(Color.FromRgb(R, G, B))
```

## 📊 Technical Details

- **Framework:** .NET 8 + WPF
- **SDK:** VEO3.SDK v1.0.0
- **Architecture:** MVVM-inspired pattern
- **Code Lines:** ~518 lines
- **File Size:** ~20KB compiled

## 🎯 Learning Points

Plugin này minh họa:
1. ✅ Cách sử dụng VEO3.SDK interfaces
2. ✅ Cách tạo UI phức tạp với WPF
3. ✅ Async/await pattern cho AI generation
4. ✅ Error handling và user feedback
5. ✅ Image loading và preview
6. ✅ Dynamic prompt building
7. ✅ Preset system architecture

## 🎉 Conclusion

**ImageGenV2Plugin** là ví dụ hoàn chỉnh về cách tạo plugin professional cho VEO3:
- 🎨 Feature-rich
- 💎 Beautiful UI
- 🚀 Easy to use
- 🔧 Easy to extend

Perfect starting point để học VEO3 SDK!

---

**Author:** VEO3 Team
**Version:** 1.0.0
**License:** MIT
