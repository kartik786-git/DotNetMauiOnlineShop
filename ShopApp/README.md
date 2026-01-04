# ShopApp - .NET MAUI E-Commerce Application

A modern, cross-platform online shopping application built with **.NET 10 MAUI** and the **CommunityToolkit.Mvvm**.

## 🚀 Features

- **Modern Home Page**: Two-column product grid with hero headers, pill-shaped category filters, and search functionality.
- **Rich Product Details**: Dynamic high-quality images, sticky "Add to Cart" footer, and quantity controls.
- **Smart Cart Management**: Card-based cart items, real-time total calculation, and streamlined checkout simulation.
- **Order Tracking**: Visual order history with color-coded status badges (Pending, Delivered, Cancelled) and timeline elements.
- **Robust Architecture**: Built on MVVM standards with Dependency Injection and resilient API handling (including `JsonSerializerOptions` for case-insensitivity).
- **Cross-Platform**: Single codebase running seamlessly on Windows, Android, iOS, and macOS.

---

## 🏗️ Architecture & Structure

The application strictly follows the **MVVM (Model-View-ViewModel)** pattern to ensure separation of concerns and testability.

### 📂 Project Structure

- **Models** (`/Models`)
  - `Product`: Data model for store items.
  - `Order`: Represents a customer order with `Items` and `Status`.
  - `CartItem`: Represents items added to the cart w/ Quantity.
  - `Category`: Product categorization.

- **ViewModels** (`/ViewModels`)
  - Inherit from `BaseViewModel` (handles `IsBusy`, `Title`).
  - `HomeViewModel`: Fetches categories and products. Handles navigation.
  - `ProductDetailsViewModel`: Manages product selection and "Add to Cart" logic.
  - `CartViewModel`: Loads cart items and handles checkout logic.
  - `OrdersViewModel`: Fetches order history and logs on-screen debug info (if enabled).
  - Uses `[ObservableProperty]` and `[RelayCommand]` for boilerplate reduction.

- **Views** (`/Views`)
  - `HomePage`: Main landing with `CollectionView` grid.
  - `ProductDetailsPage`: Detailed view with hero image parallax concepts.
  - `CartPage`: Cart summary and checkout.
  - `OrdersPage`: List of orders with custom XAML triggers for status colors.

- **Services** (`/Services`)
  - `ApiService`: Centralized HTTP client.
    - Handles endpoints: `/api/products`, `/api/cart`, `/api/orders`.
    - **Smart Image Assessment**: Automatically assigns Unsplash images based on product keywords (e.g., "iPhone" -> Phone Image) to prevent empty placeholders.

---

## 🔄 End-to-End User Flow

1. **Discovery**
   - User launches app to **HomePage**.
   - Browses "Popular Products" or selects a "Category" chip.
   - User taps a product card.

2. **Selection**
   - User lands on **ProductDetailsPage**.
   - Views large hero image and description.
   - Adjusts quantity using `(+)` and `(-)` buttons.
   - Clicks **"Add to Cart"** -> Item is posted to the backend session.

3. **Checkout**
   - User navigates to **Cart** tab.
   - Reviews items and total price.
   - Clicks **"Checkout"**. App simulates payment and creates an order via API.

4. **Tracking**
   - User switches to **Orders** tab.
   - The new order appears in the list.
   - Status badge (e.g., "Pending", "Delivered") reflects the current backend state.

---

## 🛠️ Setup & Running

**Prerequisites**:
- .NET 10 SDK
- Visual Studio 2022 (17.8+) or VS Code with MAUI extension.

**How to Run**:

1. **Clone & Restore**:
   ```powershell
   dotnet restore
   ```

2. **Run on Windows**:
   ```powershell
   dotnet run -f net10.0-windows10.0.19041.0
   ```

3. **Run on Android** (requires emulator/device):
   ```powershell
   dotnet run -f net10.0-android
   ```

## 🎨 UI Resources
- **Colors**: Defined in `Colors.xaml` (Primary Purple: `#512BD4`).
- **Styles**: Global styles for Buttons, Labels, and Borders in `Styles.xaml`.
- **Icons**: Uses Font/Glyph icons and FontImageSources.

---

## 💻 Using this Project from GitHub

If you are viewing this project on GitHub and want to run it locally, follow these steps:

1. **Clone the Repository**:
   ```powershell
   git clone <YOUR_GITHUB_REPO_URL>
   cd ShopApp
   ```

2. **Restore Dependencies**:
   Open a terminal in the project folder and run:
   ```powershell
   dotnet restore
   ```

3. **Build the Project**:
   ```powershell
   dotnet build
   ```

4. **Run the Application**:
   Select your target platform and run:
   - **Windows**: `dotnet run -f net10.0-windows10.0.19041.0`
   - **Android**: `dotnet run -f net10.0-android`
   - **iOS/Mac**: `dotnet run -f net10.0-maccatalyst` (Requires Mac)

> **Note**: Ensure you have the [.NET Multi-platform App UI development](https://dotnet.microsoft.com/en-us/apps/maui) workload installed in Visual Studio.

---

## 📦 Building Distribution Packages

### Building Android APK

To create an installable APK file for Android:

```powershell
dotnet publish -f net10.0-android -c Release -p:AndroidPackageFormat=apk
```

**Output Location:**
`bin\Release\net10.0-android\publish\com.companyname.shopapp-Signed.apk`

You can transfer this APK file to any Android device and install it.

---

### Building Windows Desktop Installer

#### Option 1: Professional Installer (Recommended)

This creates a traditional `.exe` installer using Inno Setup:

1. **Install Inno Setup** (one-time):
   ```powershell
   winget install JRSoftware.InnoSetup
   ```

2. **Build the Application**:
   ```powershell
   dotnet publish -f net10.0-windows10.0.19041.0 -c Release
   ```

3. **Compile the Installer**:
   ```powershell
   & "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" "ShopApp-Setup.iss"
   ```

**Output Location:**
`Installer\ShopApp-Setup.exe`

This installer:
- Works like professional software installers
- No certificates needed
- Creates Start Menu shortcuts
- Includes proper Uninstaller
- Just double-click to install!

#### Option 2: Portable Version

For a simple folder-based deployment without installation:

```powershell
dotnet publish -f net10.0-windows10.0.19041.0 -c Release
```

**Output Location:**
`bin\Release\net10.0-windows10.0.19041.0\win-x64\publish\`

Run `ShopApp.exe` directly from this folder, or use the included `Run-ShopApp.bat` script.

---

## 🔧 Troubleshooting

### Android Build Issues
- Ensure Android SDK is properly installed
- Check that `ANDROID_HOME` environment variable is set
- For signing errors, the APK is automatically signed with a debug certificate

### Windows Build Issues
- Ensure Windows SDK 10.0.19041.0 or higher is installed
- For MSIX certificate errors, use the Inno Setup installer method instead
- Run Visual Studio Installer to verify .NET MAUI workload is installed

