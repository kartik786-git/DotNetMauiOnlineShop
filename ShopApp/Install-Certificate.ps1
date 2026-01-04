# Complete Certificate Installation for ShopApp MSIX
# This script ensures the certificate is properly installed for MSIX installation

Write-Host "=== ShopApp Certificate Installation ===" -ForegroundColor Cyan
Write-Host ""

$pfxPath = "ShopApp.pfx"
$password = "password"

# Check if PFX exists
if (-not (Test-Path $pfxPath)) {
    Write-Host "Error: ShopApp.pfx not found!" -ForegroundColor Red
    Write-Host "Please run this script from: d:\Project\DotNetMauiOnlineShop\ShopApp" -ForegroundColor Yellow
    exit 1
}

Write-Host "Step 1: Removing any existing ShopApp certificates..." -ForegroundColor Yellow
$existingCerts = Get-ChildItem Cert:\CurrentUser\Root | Where-Object { $_.Subject -like "*ShopApp*" }
foreach ($cert in $existingCerts) {
    Remove-Item -Path $cert.PSPath -Force
    Write-Host "  Removed certificate: $($cert.Thumbprint)" -ForegroundColor Gray
}

Write-Host "Step 2: Installing certificate to Trusted Root..." -ForegroundColor Yellow
try {
    $securePassword = ConvertTo-SecureString -String $password -AsPlainText -Force
    $cert = Import-PfxCertificate -FilePath $pfxPath -CertStoreLocation Cert:\CurrentUser\Root -Password $securePassword -Exportable
    Write-Host "  Certificate installed successfully!" -ForegroundColor Green
    Write-Host "  Thumbprint: $($cert.Thumbprint)" -ForegroundColor Gray
}
catch {
    Write-Host "  Error: $_" -ForegroundColor Red
    exit 1
}

Write-Host "Step 3: Installing certificate to TrustedPeople..." -ForegroundColor Yellow
try {
    $cert2 = Import-PfxCertificate -FilePath $pfxPath -CertStoreLocation Cert:\CurrentUser\TrustedPeople -Password $securePassword -Exportable
    Write-Host "  Certificate installed to TrustedPeople!" -ForegroundColor Green
}
catch {
    Write-Host "  Warning: Could not install to TrustedPeople (this is usually OK)" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "=== Installation Complete ===" -ForegroundColor Green
Write-Host ""
Write-Host "You can now install the MSIX package:" -ForegroundColor Cyan
Write-Host "bin\Release\net10.0-windows10.0.19041.0\win-x64\AppPackages\ShopApp_1.0.0.1_Test\ShopApp_1.0.0.1_x64.msix"
Write-Host ""
Write-Host "If you still get an error, try enabling Developer Mode:" -ForegroundColor Yellow
Write-Host "Settings > Privacy & Security > For Developers > Developer Mode ON"
